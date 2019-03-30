using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KubeManage.Util
{
    public class DingTalkHelper
    {
        public static async Task<bool> SendCode(IHttpClientFactory httpClientFactory, string phoneNumber, int code)
        {
            var res = await httpClientFactory.CreateClient().PostAsJsonAsync(
                LocalConfig.Instance.DingTalKubeMangeUrl
                , new
                {
                    msgtype = "text",
                    text = new
                    {
                        content = "验证码：" + code
                    },
                    at = new
                    {
                        atMobiles = new[] {phoneNumber}
                    },
                    isAtAll = false
                });

            return await CheckSuccess(res);
        }

        private static async Task<bool> CheckSuccess(HttpResponseMessage res)
        {
            var content = await res.Content.ReadAsStringAsync();

            JObject jObject = JsonConvert.DeserializeObject<JObject>(content);

            if (jObject["errmsg"].ToString() == "ok" && Convert.ToInt32(jObject["errcode"]) != 0)
            {
                return false;
            }

            return true;
        }
    }
}