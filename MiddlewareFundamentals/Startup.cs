using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MiddlewareFundamentals
{
    public class Startup
    {

        private readonly ILogger _logger;

        public Startup(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        private static void HandleMapTest1(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 1");
            });
        }

        private static void HandleMapTest2(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 2");
            });
        }

        private static void HandleMultiSeg(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test Multi");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use((context, next) =>
            {
                _logger.LogInformation("IP: " + context.Connection.RemoteIpAddress.ToString());

                // 调用下一个中间件
                return next();
            });

            app.UseRequestIP();

            app.Map("/map1", HandleMapTest1);

            app.Map("/map2", HandleMapTest2);

            app.Map("/level1/level2", HandleMultiSeg);

            app.Map("/level1", level1App =>
            {
                level1App.Map("/level2a", level2AApp =>
                {
                    // "/level1/level2a"
                    //...
                    level2AApp.Run(async context =>
                    {
                        await context.Response.WriteAsync("Map Test level2a");
                    });
                });
                level1App.Map("/level2b", level2BApp =>
                {
                    // "/level1/level2b"
                    //...
                    level2BApp.Run(async context =>
                    {
                        await context.Response.WriteAsync("Map Test level2b");
                    });
                });
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
