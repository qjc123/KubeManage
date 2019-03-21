using k8s;

namespace KubeManage.Util
{
    public class KubeHelper
    {
        private static Kubernetes _kubernetes;

        static KubeHelper()
        {
            var config = new KubernetesClientConfiguration {Host = "http://127.0.0.1:8001"};
            _kubernetes = new Kubernetes(config);
        }

        public static Kubernetes Client => _kubernetes;
    }
}