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

            if (!Directory.Exists(Path.Combine(dir, "db")))
            {
                Directory.CreateDirectory(Path.Combine(dir, "db"));
            }
        }

        public static LiteDatabase Db()
        {
            return new LiteDatabase(Path.Combine(dir, "db", "MyData.db"));
        }
    }
}