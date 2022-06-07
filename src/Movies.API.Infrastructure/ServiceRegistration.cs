using Microsoft.Extensions.DependencyInjection;
using Movies.API.Application.Interfaces;
using Movies.API.Infrastructure.Repositories;

namespace Movies.API.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IMovieRepository, MovieRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
