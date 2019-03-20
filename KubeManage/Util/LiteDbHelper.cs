using System.IO;
using System.Reflection;
using LiteDB;

namespace KubeManage.Util
{
    public class LiteDbHelper
    {
        private static string dir = "";

        static LiteDbHelper()
        {
            dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        public static LiteDatabase Db()
        {
            return new LiteDatabase(Path.Combine(dir, "MyData.db"));
        }
    }
}