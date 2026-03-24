using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DiaCare.Domain.Entities;
using DiaCare.Infrastructure.Data;


namespace DiaCare.Infrastructure.Extensions
{
    public static class ServiceRegistration
    {
        //Extension Method => Encapsulation
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(); 
                }
               ));


            services.AddIdentity<ApplicationUser, IdentityRole>()
     .AddEntityFrameworkStores<AppDbContext>()
     .AddDefaultTokenProviders();

            return services;
        }
    }
}
