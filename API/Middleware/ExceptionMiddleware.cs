using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    /** Note: **/
    /* The custom exception middleware is responsible for catching and handling exceptions that occur during the processing of an HTTP request. 
    By adding this middleware to the pipeline, you can centrally handle exceptions and customize the behavior of the application when an exception occurs. 
    The implementation of the middleware's InvokeAsync or Invoke method will contain the actual exception handling logic.*/
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        /*
        1. next (of type RequestDelegate): This parameter represents the next middleware component in the pipeline.
        2. logger (of type ILogger<ExceptionMiddleware>): This parameter is an instance of a logger, which is used to log exceptions or other relevant information during the exception handling process.
        3. env (of type IHostEnvironment): This parameter represents the hosting environment in which the application is running.
        */
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;  
        }

        /** 
        * This method is responsible for catching and handling exceptions that occur during the processing of an HTTP request.
        */
        public async Task InvokeAsync(HttpContext context){
            try
            {
                // execution of the next middleware component in the pipeline.
                await _next(context);
            }
            catch (Exception ex)
            {
                //LogError method is used to log the exception along with its associated message.
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError; //500 (Internal Server Error)

                // current hosting environment (_env).
                var response = _env.IsDevelopment()
                ? new ApiExceptions(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) //true = 'Development'
                : new ApiExceptions(context.Response.StatusCode,  "Internal Server Error"); //false = 'Production'

                // policy specifies that property names should be converted to camel case. 
                // Camel case means that the first letter of the property name is lowercase, and the first letter of each subsequent concatenated word is capitalized. 
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                // Next, the response object is serialized to JSON using the following
                var json = JsonSerializer.Serialize(response, options);

                //Finally, the JSON response is written to the HTTP response using the following
                await context.Response.WriteAsync(json);
            }
        }
    
    }
}