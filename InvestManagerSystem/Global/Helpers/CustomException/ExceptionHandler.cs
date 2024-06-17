using System.Net;
using System.Text.Json;

namespace InvestManagerSystem.Global.Helpers.CustomException
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                await HandleHttpExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleInternalServerErrorAsync(context, ex);
            }
        }

        private static Task HandleHttpExceptionAsync(HttpContext context, CustomException ex)
        {
            context.Response.StatusCode = (int)ex.StatusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                error = new
                {
                    message = ex.Message,
                    statusCode = ex.StatusCode
                }
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private static Task HandleInternalServerErrorAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                error = new
                {
                    message = "Internal Server Error",
                    statusCode = HttpStatusCode.InternalServerError
                }
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
