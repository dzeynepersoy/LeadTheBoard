using LeadTheBoard.Application.Repositories;
using LeadTheBoard.Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LeadTheBoard.Application.Utilities.Extensions
{
    public static class ApplicationDependencyConfig
    {
        /// <summary>
        /// Application katmanında kullanılan dependency ler
        /// </summary>
        public static IServiceCollection AddApplicationDependencyConfig(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Transient: Her sorgulama veya talep için yeni bir örneğin oluşturulduğu lifetime. Herhangi bir yerden yapılan her çağrıda farklı örnekler döner.
            //Scoped: Her HTTP isteği için bir örneğin oluşturulduğu lifetime. Aynı HTTP isteği içinde yapılan tüm çağrılarda aynı örnek döner.
            //Singleton: Uygulama ömrü boyunca yalnızca bir örneğin oluşturulduğu lifetime. İlk talepte oluşturulur ve sonraki tüm taleplerde aynı örnek döner.

            return services;
        }
    }
}
