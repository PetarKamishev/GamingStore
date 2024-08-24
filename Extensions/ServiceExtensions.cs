using GamingStore.AutoMapper;
using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.BL.Services;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.DL.Repositories;

namespace GamingStore.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddControllers();      
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();           
            services.AddSingleton<IGamesService, GamesService>();
            services.AddAutoMapper(typeof(AutoMapping));
            return services;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IGamesRepository, SQLGamesRepository>();
            return services;
        }
    }
}
