using Domain.Contracts;
using Persistance.Repositories;
using Persistance;
using Persistance.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace E_commerce.Extensions
{
    public static class InfraStructureServiceExtensions
    {
        public static IServiceCollection AddInfraStructureServics(this IServiceCollection Services,IConfiguration Configuration)
        {
            Services.AddScoped<IDbInitializer, DbInitializer>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IBasketRepository, BasketRepository>();
            Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            Services.AddSingleton<IConnectionMultiplexer>(
                _=> ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));
            //return
            return Services;

        }
    }
}
