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

        private static async Task HandleHttpExceptionAsync(HttpContext context, CustomException ex)
        {
            if (context.Response.HasStarted)
            {
                Console.WriteLine("The response has already started, the http status code middleware will not be executed.");
                return;
            }

            try
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

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
            catch (Exception writeException)
            {
                Console.WriteLine($"An error occurred while writing the response: {writeException}");
            }
        }

        private static async Task HandleInternalServerErrorAsync(HttpContext context, Exception ex)
        {
            if (context.Response.HasStarted)
            {
                Console.WriteLine("The response has already started, the http status code middleware will not be executed.");
                return;
            }

            try
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

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
            catch (Exception writeException)
            {
                Console.WriteLine($"An error occurred while writing the response: {writeException}");
            }
        }
    }
}
