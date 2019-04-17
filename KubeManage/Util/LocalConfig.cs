using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace KubeManage.Util
{
    public class LocalConfig
    {
        public string DingTalKubeMangeUrl { get; set; }

        public List<string> ManagerPhoneNumbers { get; set; }

        private static LocalConfig _instance = null;

        public  string AesKey { get; set; }

        public static LocalConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    string json = File.ReadAllText(Path.Combine(PathHelper.BinPath, "config", "config.json"));

                    Console.WriteLine("发现配置文件：" + json);

                    _instance = JsonConvert.DeserializeObject<LocalConfig>(json);
                }

                return _instance;
            }
        }
    }
}