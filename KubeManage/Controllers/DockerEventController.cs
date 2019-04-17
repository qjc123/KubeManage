using System;
using System.Linq;
using KubeManage.Api;
using KubeManage.Entity.Docker;
using KubeManage.Request.Docker;
using KubeManage.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KubeManage.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/kubemanage/v{version:apiVersion}/dockerevent/[action]")]
    public class DockerEventController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public ApiResult ImagePushed([FromBody] ImagePushedRequest request)
        {
            using (var dbcontext = new DataContext())
            {
                var arr = request.Image.Split(':');

                DockerImage dockerImage = new DockerImage()
                {
                    Image = arr[0].Replace("registry-vpc.cn-hangzhou.aliyuncs.com","registry.cn-hangzhou.aliyuncs.com"),
                    Version = arr[1]
                };

                dbcontext.DockerImages.Add(dockerImage);

                dbcontext.SaveChanges();
            }

            return new ApiResult();
        }
    }
}