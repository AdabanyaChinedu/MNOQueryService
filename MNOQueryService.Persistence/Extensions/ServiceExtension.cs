using MNOQueryService.Domain.Interfaces;
using MNOQueryService.Persistence.DatabaseContext;
using MNOQueryService.Persistence.RedisContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MNOQueryService.Persistence.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<MNODbContext>(options =>
                            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")!));
            services.AddScoped<IMNODbContext>(provider => provider.GetRequiredService<MNODbContext>());

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
            });
            services.AddSingleton<IRedisService, RedisService>();
            return services;
        }
    }
}
