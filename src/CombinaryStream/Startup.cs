using System;
using CombinaryStream.Models;
using CombinaryStream.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CombinaryStream
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews().AddJsonOptions(o => {
                o.JsonSerializerOptions.WriteIndented = true;
                o.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddTransient<YoutubeRepository>();
            services.AddTransient<TwitterRepository>();
            services.AddTransient<FacebookRepository>();
            services.AddTransient<RssParser>();
            services.AddTransient<MergeService>();
            services.AddTransient<CachedMergeService>();
            services.AddMemoryCache();
            services.AddHostedService<CacheRefreshService>();

            var settings = Configuration.Get<AppSettings>();
            services.AddSingleton(settings);

            Console.WriteLine(JToken.FromObject(settings).ToString(Formatting.Indented));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
