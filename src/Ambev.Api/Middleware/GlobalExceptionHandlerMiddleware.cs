using Ambev.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Ambev.API.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            ApiErrorResponse errorResponse = exception switch
            {
                ValidationException validationEx => ApiErrorResponse.CreateValidationError(
                    validationEx.Errors.GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())),
                NotFoundException notFoundEx => ApiErrorResponse.CreateNotFoundError(notFoundEx.Message),
                UnauthorizedAccessException => ApiErrorResponse.CreateUnauthorizedError("Access denied"),
                ForbiddenAccessException => ApiErrorResponse.CreateForbiddenError("You don't have permission to perform this action"),
                _ => ApiErrorResponse.CreateInternalServerError(exception.Message)
            };

            context.Response.StatusCode = (int)errorResponse.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }

    public class ValidationException : Exception
    {
        public IEnumerable<ValidationError> Errors { get; }

        public ValidationException(IEnumerable<ValidationError> errors)
            : base("Validation failed")
        {
            Errors = errors;
        }
    }

    public class ValidationError
    {
        public string PropertyName { get; }
        public string ErrorMessage { get; }

        public ValidationError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message) : base(message) { }
    }
} 