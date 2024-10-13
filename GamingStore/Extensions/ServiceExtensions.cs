using FluentValidation;
using FluentValidation.AspNetCore;
using GamingStore.AutoMapper;
using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.BL.Services;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.DL.Repositories;
using GamingStore.GamingStore.Models.Configurations.Identity;

namespace GamingStore.Extensions
{
    public static class ServiceExtensions
    {
           
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IGamesRepository, SQLGamesRepository>();       
            services.AddSingleton<IOrdersRepository, OrdersRepository>();
            

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            var jwtSettings = new JwtSettings();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<IGamesService, GamesService>();
            services.AddSingleton<IOrdersService, OrdersService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddSingleton(jwtSettings);            
            services.AddAutoMapper(typeof(AutoMapping));
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining(typeof(Program));            
            return services;
        }

        
    }
}
