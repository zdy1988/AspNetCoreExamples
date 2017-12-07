using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Services;
using Data.Models;

namespace EF2MySql
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IRepository repository)
        {
            if (httpContext.Request.Path == "/")
            {
                await repository.InsertAsync<Account>(new Account
                {
                    UserName = "123456",
                    Password = "123456"
                });
                await repository.SaveChangesAsync();
                var accounts = await repository.FindAsync<Account>(t => true);
                await httpContext.Response.WriteAsync(accounts.Count().ToString());
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TestMiddleware>();
        }
    }
}
