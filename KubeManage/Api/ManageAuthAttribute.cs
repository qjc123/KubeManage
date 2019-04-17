using System;
using System.Linq;
using KubeManage.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace KubeManage.Api
{
    public class ManageAuthAttribute : TypeFilterAttribute
    {
        public ManageAuthAttribute() : base(typeof(ManageAuthFilter))
        {
            Order = 0;
        }
    }


    public class ManageAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            string tokenText = "";

            if (context.HttpContext.Request.Headers.ContainsKey("token"))
            {
                tokenText = context.HttpContext.Request.Headers["token"];

                try
                {
                    tokenText = SecurityHelper.AESDecrypt(tokenText, LocalConfig.Instance.AesKey);

                    Token token = JsonConvert.DeserializeObject<Token>(tokenText);

                    if (token.CreateTime < DateTime.Now.AddDays(1))
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }
                catch
                {
                    Console.WriteLine("token解析失败");
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                Console.WriteLine("token不存在");
                context.Result= new UnauthorizedResult();
            }
        }
    }

    public class Token
    {
        public string Phone { get; set; }
        
        public DateTime CreateTime { get; set; }
    }
}