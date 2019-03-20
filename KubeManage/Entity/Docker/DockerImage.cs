using System;

namespace KubeManage.Entity.Docker
{
    public class DockerImage
    {
        public int Id { get; set; }
        public string Image { get; set; }

        public string Version { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;
    }
}