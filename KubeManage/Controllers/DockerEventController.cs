using System;
using System.Linq;
using KubeManage.Api;
using KubeManage.Entity.Docker;
using KubeManage.Request.Docker;
using KubeManage.Util;
using Microsoft.AspNetCore.Mvc;

namespace KubeManage.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/kubemanage/v{version:apiVersion}/dockerevent/[action]")]
    public class DockerEventController : ControllerBase
    {
        [HttpPost]
        public ApiResult ImagePushed([FromBody] ImagePushedRequest request)
        {
            using (var dbcontext = new DataContext())
            {
                var arr = request.Image.Split(':');

                DockerImage dockerImage = new DockerImage()
                {
                    Image = arr[0],
                    Version = arr[1]
                };

                dbcontext.DockerImages.Add(dockerImage);

                dbcontext.SaveChanges();
            }

            return new ApiResult();
        }
    }
}