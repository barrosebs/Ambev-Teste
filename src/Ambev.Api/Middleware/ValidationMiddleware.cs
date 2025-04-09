using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Ambev.API.Middleware;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors.Select(e => new
            {
                Property = e.PropertyName,
                Message = e.ErrorMessage
            });

            var response = new
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Erro de validação",
                Errors = errors
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
} 