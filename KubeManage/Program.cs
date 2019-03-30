using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KubeManage.Entity;
using KubeManage.Util;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KubeManage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var dbcontext = new DataContext())
            {
                dbcontext.Database.Migrate();
                
                LocalConfig.Instance.ManagerPhoneNumbers.ForEach(number =>
                {
                    if (!dbcontext.Managers.Any(t => t.PhoneNumber == number))
                    {
                        dbcontext.Managers.Add(new Manager() {PhoneNumber = number});
                    }
                });

                dbcontext.SaveChanges();
            }
            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls("http://*:60000")
                .UseStartup<Startup>();
    }
}