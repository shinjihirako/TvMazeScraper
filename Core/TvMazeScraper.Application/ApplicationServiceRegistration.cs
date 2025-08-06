using Microsoft.Extensions.DependencyInjection;

namespace TvMazeScraper.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();    
            services.AddAutoMapper(assemblies);
        
            return services;
        }
    }
}
