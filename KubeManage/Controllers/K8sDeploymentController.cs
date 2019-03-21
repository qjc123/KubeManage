using System.Collections.Generic;
using System.Linq;
using k8s;
using KubeManage.Api;
using KubeManage.Entity.Docker;
using KubeManage.Response.K8s;
using KubeManage.Util;
using Microsoft.AspNetCore.Mvc;

namespace KubeManage.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/k8s/deployment/[action]")]
    public class K8sDeploymentController : ControllerBase
    {
        [HttpGet]
        public ApiResult<List<DeployMentItem>> List(string @namespace)
        {
            using (var dbcontext = new DataContext())
            {
                var deployments = KubeHelper.Client.ListNamespacedDeployment(@namespace).Items;

                List<DeployMentItem> list = new List<DeployMentItem>();

                foreach (var v1Deployment in deployments)
                {
                    var arr = v1Deployment.Spec.Template.Spec.Containers[0].Image.Split(':');

                    list.Add(new DeployMentItem
                    {
                        Name = v1Deployment.Metadata.Name,
                        CurrentImage = new Image()
                        {
                            Name = arr[0],
                            Version = arr[1]
                        },
                        RegistryVersions = dbcontext.DockerImages
                            .Where(t => t.Image == arr[0])
                            .OrderByDescending(t => t.Time)
                            .Take(10).ToList()
                    });
                }


                return new ApiResult<List<DeployMentItem>> {Data = list};
            }
        }
    }
}