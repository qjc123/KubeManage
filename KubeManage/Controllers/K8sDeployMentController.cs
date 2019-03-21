using System.Collections.Generic;
using k8s;
using KubeManage.Api;
using KubeManage.Response.K8s;
using KubeManage.Util;
using Microsoft.AspNetCore.Mvc;

namespace KubeManage.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/k8s/deployment/[action]")]
    public class K8sDeployMentController : ControllerBase
    {
        public ApiResult<List<DeployMentItem>> List(string @namespace)
        {
            var deployments = KubeHelper.Client.ListNamespacedDeployment(@namespace).Items;

            List<DeployMentItem> list = new List<DeployMentItem>();

            foreach (var v1Deployment in deployments)
            {
                list.Add(new DeployMentItem()
                {
                    Image = v1Deployment.Spec.Template.Spec.Containers[0].Image
                });
            }

            return new ApiResult<List<DeployMentItem>>() {Data = list};
        }
    }
}