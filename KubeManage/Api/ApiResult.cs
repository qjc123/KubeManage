using Newtonsoft.Json;

namespace KubeManage.Api
{
    public class ApiResult<T> where T : class, new()
    {
        [JsonProperty("code")] public int Code { get; set; } = 200;

        [JsonProperty("msg")] public string Message { get; set; } = "";

        [JsonProperty("data")] public T Data { get; set; }
    }

    public class ApiResult : ApiResult<object>
    {
    }
}