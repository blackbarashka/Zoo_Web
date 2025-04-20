using Microsoft.Extensions.DependencyInjection;
using ZooHSE.WebApi.Application.Interfaces;
using ZooHSE.WebApi.Application.Services;
using ZooHSE.WebApi.Infrastructure.Repositories;

namespace ZooHSE.WebApi.Infrastructure
{
    /// <summary>
    /// Класс для регистрации зависимостей в контейнере внедрения зависимостей.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Регистрация репозиториев
            services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
            services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
            services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();

            // Регистрация сервисов
            services.AddSingleton<AnimalTransferService>();
            services.AddSingleton<FeedingOrganizationService>();
            services.AddSingleton<ZooStatisticsService>();

            return services;
        }
    }
}
