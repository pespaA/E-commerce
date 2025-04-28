using System.Runtime.CompilerServices;
using Domain.Contracts;
using E_commerce.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace E_commerce.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            //Create Object From Type That Implements IDbintializer
            using var Scope = app.Services.CreateScope();
            var dbinitailizer = Scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbinitailizer.InitializeAsync();
            return app;
        }
        public static WebApplication UseCustomMiddlewareExceptions(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            return app;
        }
        
    }
}
