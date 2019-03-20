using Newtonsoft.Json;

namespace KubeManage.Request.Docker
{
    public class ImagePushedRequest
    {
        [JsonProperty("image")]
        public string Image { get; set; }
    }
}