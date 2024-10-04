using System.Text.Json;

namespace Reddit.Middlewares
{
    public class ExceptionHandlingMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                // Log the error
                _logger.LogError(ex, "Developer, go fix it up!");

                // Create the error response model
                var errorResponseModel = new ErrorResponse
                {
                    Message = "Sorry, an error occurred.",
                    Details = ex.Message
                };

                // Set the response status code and content type
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                // Serialize the error response to JSON
                var jsonResponse = JsonSerializer.Serialize(errorResponseModel);

                // Write the response body
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
