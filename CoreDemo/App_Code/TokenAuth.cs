using CoreDemo.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDemo.App_Code
{
    /// <summary>
    /// Token验证授权中间件
    /// </summary>
    public class TokenAuth
    {
        private readonly RequestDelegate _next;

        public TokenAuth(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var headers = httpContext.Request.Headers;
            if (!headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }
            var tokenStr = headers["Authorization"];
            try
            {
                string JwtStr = tokenStr.ToString().Substring("Bearer ".Length).Trim();
                if (!MyMemoryCache.Exists(JwtStr))
                {
                    return httpContext.Response.WriteAsync("非法请求");
                }
                TokenModel model = (TokenModel)MyMemoryCache.Get(JwtStr);

                List<Claim> lc = new List<Claim>();
                Claim c = new Claim(model.Sub + "Type", model.Sub);
                lc.Add(c);
                ClaimsIdentity identity = new ClaimsIdentity(lc);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                httpContext.User = principal;
                return _next(httpContext);
            }
            catch (Exception)
            {
                return httpContext.Response.WriteAsync("token验证异常");
                //throw;
            }
        }

    }
}
