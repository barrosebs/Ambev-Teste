using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ambev.Infrastructure.Repositories.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, Dictionary<string, string>? filters)
        {
            if (filters == null || !filters.Any())
            {
                return query;
            }

            var parameter = Expression.Parameter(typeof(T), "e"); // "e" for entity

            foreach (var filter in filters)
            {
                var filterKey = filter.Key;
                var filterValue = filter.Value;
                bool isMin = false;
                bool isMax = false;
                string propertyName;

                if (string.IsNullOrWhiteSpace(filterValue)) continue;

                if (filterKey.StartsWith("_min"))
                {
                    isMin = true;
                    propertyName = filterKey.Substring(4);
                }
                else if (filterKey.StartsWith("_max"))
                {
                    isMax = true;
                    propertyName = filterKey.Substring(4);
                }
                else
                {
                    propertyName = filterKey;
                }

                var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null) continue;

                try
                {
                    var member = Expression.Property(parameter, property);
                    Expression? comparison = null;
                    var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    if (targetType == typeof(string))
                    {
                        var constant = Expression.Constant(filterValue);
                        MethodInfo? stringMethod;

                        if (filterValue.StartsWith("*") && filterValue.EndsWith("*"))
                        {
                            stringMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                            comparison = Expression.Call(member, stringMethod!, Expression.Constant(filterValue.Trim('*')));
                        }
                        else if (filterValue.StartsWith("*"))
                        {
                            stringMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                            comparison = Expression.Call(member, stringMethod!, Expression.Constant(filterValue.TrimStart('*')));
                        }
                        else if (filterValue.EndsWith("*"))
                        {
                            stringMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                            comparison = Expression.Call(member, stringMethod!, Expression.Constant(filterValue.TrimEnd('*')));
                        }
                        else
                        {
                            comparison = Expression.Equal(member, constant);
                        }
                    }
                    else
                    {
                        var convertedValue = Convert.ChangeType(filterValue, targetType);
                        var constant = Expression.Constant(convertedValue, property.PropertyType); // Use property.PropertyType for potential nullables

                        if (isMin) { comparison = Expression.GreaterThanOrEqual(member, constant); }
                        else if (isMax) { comparison = Expression.LessThanOrEqual(member, constant); }
                        else { comparison = Expression.Equal(member, constant); }
                    }

                    if (comparison != null)
                    {
                        var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
                        query = query.Where(lambda);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao aplicar filtro {filterKey}={filterValue}: {ex.Message}");
                    continue;
                }
            }
            return query;
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? order)
        {
            if (string.IsNullOrWhiteSpace(order))
            {
                // Try to find an 'Id' property for default sorting
                var idProperty = typeof(T).GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (idProperty != null)
                {
                    var entityParameter = Expression.Parameter(typeof(T), "x");
                    var propertyAccess = Expression.MakeMemberAccess(entityParameter, idProperty);
                    var orderByExpression = Expression.Lambda(propertyAccess, entityParameter);
                    var resultExpression = Expression.Call(typeof(Queryable), "OrderBy",
                        new Type[] { typeof(T), idProperty.PropertyType },
                        query.Expression, Expression.Quote(orderByExpression));
                    return query.Provider.CreateQuery<T>(resultExpression);
                }
                else
                {
                    return query; // No default sorting possible
                }
            }

            var orderParams = order.Split(',');
            var command = "OrderBy";
            var sortParameter = Expression.Parameter(typeof(T), "x");

            foreach (var param in orderParams)
            {
                var trimmedParam = param.Trim();
                if (string.IsNullOrWhiteSpace(trimmedParam)) continue;

                var parts = trimmedParam.Split(' ');
                var propertyName = parts[0];
                var direction = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase) ? "Descending" : "";

                var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null) continue;

                var propertyAccess = Expression.MakeMemberAccess(sortParameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, sortParameter);

                var methodName = command + direction;
                var resultExpression = Expression.Call(typeof(Queryable), methodName,
                    new Type[] { typeof(T), property.PropertyType },
                    query.Expression, Expression.Quote(orderByExpression));

                query = query.Provider.CreateQuery<T>(resultExpression);
                command = "ThenBy";
            }
            return query;
        }
    }
} 