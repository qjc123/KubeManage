using Newtonsoft.Json;

namespace KubeManage.Api
{
    public class ApiResult<T> where T : class, new()
    {
        public int Code { get; set; } = 200;

        public string Message { get; set; } = "";

        public T Data { get; set; }
    }

    public class ApiResult : ApiResult<object>
    {
    }
}