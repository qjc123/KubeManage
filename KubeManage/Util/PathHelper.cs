using System.IO;
using System.Reflection;

namespace KubeManage.Util
{
    public class PathHelper
    {
        public static string BinPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    }
}