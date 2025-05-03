using System.Net;
using Domain.Exceptions;
using Shared.ErrorModels;

namespace E_commerce.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next,ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    await HandleNotFoundEndPointException(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went Wrong{ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleNotFoundEndPointException(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "applictaion/json";
            var response = new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                ErrorMessage = $"The EndPoint {httpContext.Request.Path} Not Found"
            }.ToString();
            await httpContext.Response.WriteAsync(response);
        }

        // Handle Exceptions
        public async Task HandleExceptionAsync(HttpContext httpContext,Exception exception)
        {
            // Set Status Code To 500
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // Set Content Type "application/json"
            httpContext.Response.ContentType = "application/json";
            var response = new ErrorDetails
            {
                ErrorMessage = exception.Message,
            };
            // C#09:
            httpContext.Response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                RegisterValidationExceptions validationExceptions=> HandleValidationException(validationExceptions,response),
                _ => (int)HttpStatusCode.InternalServerError,
            };
            // return Standerd Response
            response.StatusCode = httpContext.Response.StatusCode;
            await httpContext.Response.WriteAsync(response.ToString());
        }

        private int HandleValidationException(RegisterValidationExceptions validationExceptions, ErrorDetails response)
        {
            response.Errors= validationExceptions.Errors;
            return (int)HttpStatusCode.BadRequest;
        }
    }
}
