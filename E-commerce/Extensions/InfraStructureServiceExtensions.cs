using Domain.Contracts;
using Persistance.Repositories;
using Persistance;
using Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Extensions
{
    public static class InfraStructureServiceExtensions
    {
        public static IServiceCollection AddInfraStructureServics(this IServiceCollection Services,IConfiguration Configuration)
        {
            Services.AddScoped<IDbInitializer, DbInitializer>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            //return
            return Services;

        }
    }
}
