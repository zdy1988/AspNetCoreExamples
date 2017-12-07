using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjection
{
    public class RepositoryMiddleware
    {
        private readonly RequestDelegate _next;

        public RepositoryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,
            OperationService operationService)
        {
            await context.Response.WriteAsync(operationService.GetList());
        }
    }

    public static class RepositoryMiddlewareExtensions
    {
        public static IApplicationBuilder UseRepositoryMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RepositoryMiddleware>();
        }
    }
}
