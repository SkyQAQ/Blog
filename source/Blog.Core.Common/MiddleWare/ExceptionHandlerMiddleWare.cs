using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Blog.Core.Model;
using Newtonsoft.Json;
using System.IO;

namespace Blog.Core.Common.MiddleWare
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.code = Constants.Result_Failure;
                result.mssg = ex.Message;
                if (ex is UnauthorizedAccessException)
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                else if (ex is TimeoutException)
                    context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                else if (ex is MethodAccessException)
                    context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                else if (ex is FileNotFoundException || ex is FileFormatException || ex is FileLoadException)
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;               
                context.Response.ContentType = Constants.ContentType2;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(result)).ConfigureAwait(false);
            } 
        }
    }
}
