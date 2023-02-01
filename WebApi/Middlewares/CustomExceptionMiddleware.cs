using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using WebApi.Services;

namespace WebApi.Middlewares
{
    public class CustomExeptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;
        public CustomExeptionMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var watch = Stopwatch.StartNew();
            try
            {
                string message = "[Request] HTTP " + context.Request.Method + " - " + context.Request.Path;
                _logger.Write(message);

                await _next(context);
                watch.Stop();

                message = "[Response] HTTP " + context.Request.Method + " - " + context.Request.Path + " responded" + context.Response.StatusCode + " in " + watch.ElapsedMilliseconds + "ms";
                _logger.Write(message);
            }
            catch (Exception ex)
            {
                watch.Stop();
                await HandleException(context, ex, watch);
            }
        }

        private Task HandleException(HttpContext context, Exception ex, Stopwatch watch)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "[Error] HTTP " + context.Request.Method + " - " + context.Response.StatusCode + " Error Message " + ex.Message + " in " + watch.ElapsedMilliseconds + "ms";
            _logger.Write(message);


            var result = JsonConvert.SerializeObject(new { error = ex.Message }, Formatting.None);

            return context.Response.WriteAsync(result);
        }
    }

    static public class CustomExeptionMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExeptionMiddle(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExeptionMiddleware>();
        }
    }
}