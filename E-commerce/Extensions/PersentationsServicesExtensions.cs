using Services.Abstractions;
using Services;
using E_commerce.Factories;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Extensions
{
    public static class PersentationsServicesExtensions
    {
        public static IServiceCollection AddPersentationsServices(this IServiceCollection Services)
        {
            Services.AddControllers().AddApplicationPart(typeof(Persentation.AssemblyReference).Assembly);
            Services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrors;
            });
            return Services;
        }
    }
}
