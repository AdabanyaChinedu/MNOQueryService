using MNOQueryService.SharedLibrary.Exceptions;
using System.Net;
using System.Text.Json;

namespace MNOQueryService.API.Extensions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var errorMessage = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)code;
                    errorMessage = JsonSerializer.Serialize(new { ErrorMessage = validationException.Failures.FirstOrDefault().Value });
                    break;
                case EntityNotFoundException entityNotFoundException:
                    code = HttpStatusCode.NotFound;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)code;
                    errorMessage = !string.IsNullOrEmpty(entityNotFoundException.Message) ? entityNotFoundException.Message : "Entity not found.Please provide a valid search criteria and try again.";
                    errorMessage = JsonSerializer.Serialize(new { ErrorMessage = errorMessage });
                    break;                
                default:
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)code;
                    errorMessage = !string.IsNullOrEmpty(exception.Message) ? exception.Message : "An error occured on the payload";
                    errorMessage = JsonSerializer.Serialize(new { ErrorMessage = errorMessage });
                    break;
            }

            return context.Response.WriteAsync(errorMessage);
        }
    }
}
