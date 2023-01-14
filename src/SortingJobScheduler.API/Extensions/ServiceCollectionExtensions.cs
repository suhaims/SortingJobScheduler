using SortingJobScheduler.Interfaces.Services;
using SortingJobScheduler.Services;

namespace SortingJobScheduler.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ISortingService, SortingService>();
            return services;
        }
    }
}
