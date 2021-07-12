using FilmSearchClasses.ServiceInterfaces;
using FilmSearchClasses.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FilmSearchClasses.Extensions
{
    public static class FilmSearchServiceExtension
    {
        public static void AddFilmSearchService(this IServiceCollection services)
        {
            services.AddScoped<IFilmSearchService, FilmSearchService>();
        }
    }
}
