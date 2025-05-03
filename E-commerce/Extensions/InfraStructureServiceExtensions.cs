using Domain.Contracts;
using Persistance.Repositories;
using Persistance;
using Persistance.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Shared.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            Services.AddDbContext<StoreIdentityContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });
            Services.AddSingleton<IConnectionMultiplexer>(
                _=> ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));
            Services.ConfigureIdentityService();
            Services.ConfigureJwt(Configuration);
            //return
            return Services;

        }
        public static IServiceCollection ConfigureIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(option =>
            {
                option.Password.RequireDigit = true;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<StoreIdentityContext>();
            return services;
        }
        public static IServiceCollection ConfigureJwt(this IServiceCollection services,IConfiguration configuration)
        {
            var jwtoption = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option=>
            option.TokenValidationParameters= new TokenValidationParameters
            {
                ValidateIssuer=true,
                ValidateAudience=true,
                ValidateLifetime=true,
                ValidateIssuerSigningKey=true,
                ValidIssuer=jwtoption.Issure,
                ValidAudience=jwtoption.Audience,
                IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoption.SecretKey))
            });
            services.AddAuthorization();
            return services;  
        }
    }
}
