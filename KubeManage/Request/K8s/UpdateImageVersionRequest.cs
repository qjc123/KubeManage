using Newtonsoft.Json;

namespace KubeManage.Request.K8s
{
    public class UpdateImageVersionRequest
    {
        [JsonProperty("image")]
        public string Image { get; set; }
        
        [JsonProperty("namespace")]
        public string NS { get; set; }
    }
}