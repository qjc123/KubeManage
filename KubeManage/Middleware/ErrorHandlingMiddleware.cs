using System;
using System.Threading.Tasks;
using KubeManage.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace KubeManage.Middleware
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
                int statusCode;
                string msg;

                statusCode = 500;
                msg = ex.Message;

                await HandleExceptionAsync(context, statusCode, msg);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            var data = new {Code = statusCode, Message = msg};

            var result = data.ToJson();

            context.Response.ContentType = "application/json;charset=utf-8";

            return context.Response.WriteAsync(result);
        }
    }

    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}