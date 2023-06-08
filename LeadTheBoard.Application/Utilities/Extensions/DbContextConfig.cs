using LeadTheBoard.Application.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeadTheBoard.Application.Utilities.Extensions
{
    public static class DbContextConfig
    {
        /// <summary>
        /// DbContext ayarları ve veritabanı bağlantısı
        /// </summary>
        public static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AppDbConnection"));
            });

            return services;
        }
    }
}
