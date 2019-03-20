using System;
using KubeManage.Api;
using KubeManage.Request.Docker;
using Microsoft.AspNetCore.Mvc;

namespace KubeManage.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/dockerevent/[action]")]
    public class DockerEventController : ControllerBase
    {
        [HttpPost]
        public ApiResult ImagePushed([FromBody] ImagePushedRequest request)
        {
            Console.WriteLine(request.ToJson());
            return new ApiResult();
        }
    }
}