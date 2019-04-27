using System.Collections.Generic;
using System.Linq;
using k8s;
using k8s.Models;
using KubeManage.Api;
using KubeManage.Entity.Docker;
using KubeManage.Request.Docker;
using KubeManage.Request.K8s;
using KubeManage.Response.K8s;
using KubeManage.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace KubeManage.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/kubemanage/v{version:apiVersion}/k8s/deployment/[action]")]
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

        [HttpPost]
        public ApiResult UpdateImageVersion([FromBody] UpdateImageVersionRequest request)
        {
            var arr = request.Image.Split(':');

            DockerImage dockerImage = new DockerImage()
            {
                Image = arr[0].Replace("registry-vpc.cn-hangzhou.aliyuncs.com", "registry.cn-hangzhou.aliyuncs.com"),
                Version = arr[1]
            };

            var deployments = KubeHelper.Client.ListNamespacedDeployment(request.NS).Items;

            foreach (var v1Deployment in deployments)
            {
                var deployMentImage = v1Deployment.Spec.Template.Spec.Containers[0].Image.Split(':')[0]
                    .Replace("registry-vpc.cn-hangzhou.aliyuncs.com", "registry.cn-hangzhou.aliyuncs.com");

                if (deployMentImage == dockerImage.Image)
                {
                    var patch = new JsonPatchDocument<V1Deployment>();

                    patch.Replace(t => t.Spec.Template.Spec.Containers[0].Image, request.Image);

                    KubeHelper.Client.PatchNamespacedDeployment(new V1Patch(patch), v1Deployment.Metadata.Name,
                        request.NS);
                }
            }

            return new ApiResult();
        }
    }
}