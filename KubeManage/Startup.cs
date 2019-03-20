using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KubeManage.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace KubeManage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions()
                .AddSingleton(Configuration)
                .AddApiVersioning(option =>
                {
                    option.ReportApiVersions = true;
                    option.AssumeDefaultVersionWhenUnspecified = true;
                    option.DefaultApiVersion = new ApiVersion(1, 0);
                })
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver =
                        new Newtonsoft.Json.Serialization.DefaultContractResolver();

                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;

                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseErrorHandling();

            app.UseCors(builder => builder
                .SetIsOriginAllowed(host => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromHours(12))
            );

            app.UseMvc();
        }
    }
}