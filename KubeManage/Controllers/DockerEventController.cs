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
    [Route("api/v{version:apiVersion}/dockerevent/[action]")]
    public class DockerEventController : ControllerBase
    {
        [HttpPost]
        public ApiResult ImagePushed([FromBody] ImagePushedRequest request)
        {
            using (var db = LiteDbHelper.Db())
            {
                var col = db.GetCollection<DockerImage>();

                var arr = request.Image.Split(':');

                DockerImage dockerImage = new DockerImage()
                {
                    Image = arr[0],
                    Version = arr[1]
                };

                var items = col.FindAll().ToList();
                
                Console.WriteLine(items.ToJson());

                col.Insert(dockerImage);
            }

            return new ApiResult();
        }
    }
}