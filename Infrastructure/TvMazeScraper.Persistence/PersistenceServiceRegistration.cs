using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Persistence.Interfaces;
using TvMazeScraper.Persistence.Repositories;

namespace TvMazeScraper.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<TvMazeScraperDbContext>(options =>
                options.UseInMemoryDatabase("TvMazeScraper"));

            services.AddScoped<IShowRepository, ShowRepository>();

            return services;
        }
    }
}
