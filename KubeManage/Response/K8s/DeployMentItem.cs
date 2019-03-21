using System.Collections.Generic;
using KubeManage.Entity.Docker;

namespace KubeManage.Response.K8s
{
    public class DeployMentItem
    {
        public string Name { get; set; }
        public Image CurrentImage { get; set; }
        
        public List<DockerImage> RegistryVersions { get; set; }
        
        public string Description { get; set; }
    }

    public class Image
    {
        public string Name { get; set; }
        
        public string Version { get; set; }
    }
}