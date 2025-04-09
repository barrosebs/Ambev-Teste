using System.Net;

namespace Ambev.Application.DTOs
{
    public class ApiErrorResponse
    {
        public string Type { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }
        public Dictionary<string, string[]> ValidationErrors { get; set; }

        public ApiErrorResponse()
        {
            ValidationErrors = new Dictionary<string, string[]>();
        }

        public static ApiErrorResponse CreateValidationError(Dictionary<string, string[]> validationErrors)
        {
            return new ApiErrorResponse
            {
                Type = "ValidationError",
                Error = "Invalid input data",
                Detail = "One or more validation errors occurred",
                StatusCode = HttpStatusCode.BadRequest,
                ValidationErrors = validationErrors
            };
        }

        public static ApiErrorResponse CreateNotFoundError(string detail)
        {
            return new ApiErrorResponse
            {
                Type = "ResourceNotFound",
                Error = "Resource not found",
                Detail = detail,
                StatusCode = HttpStatusCode.NotFound
            };
        }

        public static ApiErrorResponse CreateUnauthorizedError(string detail)
        {
            return new ApiErrorResponse
            {
                Type = "Unauthorized",
                Error = "Unauthorized access",
                Detail = detail,
                StatusCode = HttpStatusCode.Unauthorized
            };
        }

        public static ApiErrorResponse CreateForbiddenError(string detail)
        {
            return new ApiErrorResponse
            {
                Type = "Forbidden",
                Error = "Access forbidden",
                Detail = detail,
                StatusCode = HttpStatusCode.Forbidden
            };
        }

        public static ApiErrorResponse CreateInternalServerError(string detail)
        {
            return new ApiErrorResponse
            {
                Type = "InternalServerError",
                Error = "An error occurred while processing your request",
                Detail = detail,
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }
} 