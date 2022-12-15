using Imagination.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Imagination.Middlewares
{
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next,
            IWebHostEnvironment env,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            int code;
            try
            {
                await _next(context);
            }

            catch (Exception exception)
            {
                Dictionary<string, object> errorResponse = new();


                string message = null;
                if (_env.IsDevelopment())
                {
                    message = exception.Message;
                    errorResponse.Add("StackTrace", exception.StackTrace);

                    if (exception.InnerException != null)
                    {
                        errorResponse.Add("InnerException.Exception", exception.InnerException.Message);
                        errorResponse.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
                    }

                }
                else
                    message = "UnHandled Exception";


                if (exception is ImageConverterException imageConverterException)
                {
                    if (!_env.IsDevelopment())
                        message = imageConverterException.GetErrorTypeDescription();

                    code = (int)imageConverterException.Type;
                    errorResponse.Add("ErrorCode", code);
                    errorResponse.Add("ErrorType", nameof(ImageConverterException));
                }

                errorResponse.Add("ErrorMessage", message);

                _logger.LogError(exception, message);

                await WriteToResponseAsync(errorResponse);
            }

            async Task WriteToResponseAsync(object returnObject)
            {
                if (context.Response.HasStarted)
                    throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

                var json = JsonSerializer.Serialize(returnObject);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }
    }
}
