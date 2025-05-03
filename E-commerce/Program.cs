
using Domain.Contracts;
using E_commerce.Extensions;
using E_commerce.Factories;
using E_commerce.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Persistance.Data;
using Persistance.Repositories;
using Services;
using Services.Abstractions;

namespace E_commerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            #region Configure Services
            builder.Services.AddInfraStructureServics(builder.Configuration);
            builder.Services.AddCoreServices(builder.Configuration);
            builder.Services.AddPersentationsServices();
            #endregion
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #region Build
            var app = builder.Build();
            #endregion
            #region MiddleWares
            app.UseCustomMiddlewareExceptions();
            await app.SeedDbAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            
            #endregion


        }
    }
}
