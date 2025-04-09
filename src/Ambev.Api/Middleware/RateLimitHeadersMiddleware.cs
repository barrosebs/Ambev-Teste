using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Ambev.API.Middleware
{
    public class RateLimitHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public RateLimitHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                if (context.Response.Headers.ContainsKey("X-Rate-Limit-Limit"))
                {
                    context.Response.Headers.Append("X-Rate-Limit-Remaining", context.Response.Headers["X-Rate-Limit-Remaining"]);
                    context.Response.Headers.Append("X-Rate-Limit-Reset", context.Response.Headers["X-Rate-Limit-Reset"]);
                }
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
} 