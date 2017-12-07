using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace DependencyInjection
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add Repository
            services.AddTransient<IRepository, EFRepository>();
            //services.AddTransient<IRepository, DapperRepository>();
            // Add Service
            services.AddTransient<OperationService, OperationService>();

            // Add Test Timelife
            services.AddTransient<IOperationTransient, Operation>();
            services.AddScoped<IOperationScoped, Operation>();
            services.AddSingleton<IOperationSingleton, Operation>();
            services.AddSingleton<IOperationSingletonInstance>(new Operation(Guid.Empty));
            services.AddTransient<OperationService2, OperationService2>();

            //Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.RegisterModule<AutofacModule>();
            Startup.AutofacContainer = containerBuilder.Build();
            return new AutofacServiceProvider(Startup.AutofacContainer);
        }

        public static IContainer AutofacContainer { get; set; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/ef", efApp => {
                efApp.Run(async (context) => {
                    OperationService service = new OperationService(new EFRepository());
                    await context.Response.WriteAsync(service.GetList());
                });
            });

            app.Map("/dapper", dapperApp => {
                dapperApp.Run(async (context) => {
                    OperationService service = new OperationService(new DapperRepository());
                    await context.Response.WriteAsync(service.GetList());
                });
            });

            app.Map("/autofac", autofacApp => {
                autofacApp.Run(async (context) => {
                    var info = Startup.AutofacContainer.Resolve<IRepository>().GetInfo();
                    await context.Response.WriteAsync(info);
                });
            });

            //app.UseRepositoryMiddleware();

            app.UseServiceLifetimesMiddleware();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
