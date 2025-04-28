using Services.Abstractions;
using Services;

namespace E_commerce.Extensions
{
    public static class CoreServicesExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection Services)
        {
            Services.AddScoped<IServiceManager, ServiceManager>();
            Services.AddAutoMapper(typeof(Services.AssemblyRefernce).Assembly);
            return Services;
        }
    }
}
