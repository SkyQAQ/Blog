using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Blog.Core.Model;
using Newtonsoft.Json;

namespace Blog.Core.Common.MiddleWare
{
    public class AuthorizeHandlerMiddleWare
    {
        private readonly RequestDelegate _next;

        public AuthorizeHandlerMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await this._next(context);
        }
    }
}
