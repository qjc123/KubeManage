using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KubeManage.Api;
using KubeManage.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace KubeManage.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/kubemanage/v{version:apiVersion}/account/[action]")]
    public class AccountController : ControllerBase
    {
        private IMemoryCache _cache;
        private IHttpClientFactory _httpClientFactory;

        public AccountController(IMemoryCache cache, IHttpClientFactory httpClientFactory)
        {
            _cache = cache;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResult> SendCode(string phoneNumber)
        {
            using (var dbcontext = new DataContext())
            {
                if (!dbcontext.Managers.Any(t => t.PhoneNumber == phoneNumber))
                {
                    throw new Exception("手机号不存在");
                }
            }

            string sended = _cache.Get<string>("LoginCodeSended" + phoneNumber);

            if (!string.IsNullOrEmpty(sended))
            {
                throw new Exception("验证码已发送，稍后再试");
            }

            int code = new Random().Next(100000, 999999);

            var sendRes = await DingTalkHelper.SendCode(_httpClientFactory, phoneNumber, code);

            if (!sendRes)
            {
                throw new Exception("验证码发送失败");
            }

            _cache.Set("LoginCodeSended" + phoneNumber, phoneNumber, DateTime.Now.AddMinutes(1));
            _cache.Set("LoginCode" + phoneNumber, code, DateTime.Now.AddMinutes(5));

            return new ApiResult() {Message = "验证码已发送"};
        }

        [HttpGet]
        [AllowAnonymous]
        public ApiResult Login(string phoneNumber, int code)
        {
            var codeCache = _cache.Get<int>("LoginCode" + phoneNumber);

            if (code <= 0 || codeCache <= 0 || codeCache != code)
            {
                throw new Exception("验证码错误");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(new {Phone = phoneNumber})),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Exp,
                    $"{new DateTimeOffset(DateTime.Now.AddHours(30)).ToUnixTimeSeconds()}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AA5CDEA5AEEF4641932668D523AAFE17"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "ipis.etor.top",
                audience: "ipis.etor.top",
                claims: claims,
                expires: DateTime.Now.AddHours(30),
                signingCredentials: creds);

            return new ApiResult() {Message = new JwtSecurityTokenHandler().WriteToken(token)};
        }
    }
}