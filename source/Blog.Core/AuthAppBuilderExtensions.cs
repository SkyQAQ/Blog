using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core
{
    /// <summary>
    /// AuthAppBuilderExtensions
    /// </summary>
    public static class AuthAppBuilderExtensions
    {
        /// <summary>
        /// 用户验证
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthorize(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    await next();
                }
                else
                {
                    var user = context.User;
                    if (user?.Identity?.IsAuthenticated ?? false)
                    {
                        await next();
                    }
                    else
                    {
                        await context.ChallengeAsync();
                    }
                }
            });
        }
    }
}
