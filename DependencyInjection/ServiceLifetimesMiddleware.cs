using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjection
{
    public class ServiceLifetimesMiddleware
    {
        private readonly RequestDelegate _next;

        public ServiceLifetimesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,
            OperationService2 operationService2,
            IOperationTransient transientOperation,
            IOperationScoped scopedOperation,
            IOperationSingleton singletonOperation,
            IOperationSingletonInstance singletonInstanceOperation)
        {
            await context.Response.WriteAsync($"Operations：\r\n");
            await context.Response.WriteAsync($"Transient:{transientOperation.OperationId}\r\n");
            await context.Response.WriteAsync($"Scoped:{scopedOperation.OperationId}\r\n");
            await context.Response.WriteAsync($"Singleton:{singletonOperation.OperationId}\r\n");
            await context.Response.WriteAsync($"SingletonInstance:{singletonInstanceOperation.OperationId}\r\n");
            await context.Response.WriteAsync("\r\n");
            await context.Response.WriteAsync($"Serivce Operations：\r\n");
            await context.Response.WriteAsync($"Transient:{operationService2.TransientOperation.OperationId}\r\n");
            await context.Response.WriteAsync($"Scoped:{operationService2.ScopedOperation.OperationId}\r\n");
            await context.Response.WriteAsync($"Singleton:{operationService2.SingletonOperation.OperationId}\r\n");
            await context.Response.WriteAsync($"SingletonInstance:{operationService2.SingletonInstanceOperation.OperationId}\r\n");
        }
    }

    public static class ServiceLifetimesMiddlewareExtensions
    {
        public static IApplicationBuilder UseServiceLifetimesMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ServiceLifetimesMiddleware>();
        }
    }
}
