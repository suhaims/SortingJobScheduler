using SortingJobScheduler.Interfaces.Queue;
using SortingJobScheduler.Interfaces.Services;
using SortingJobScheduler.Queue;
using SortingJobScheduler.Services;

namespace SortingJobScheduler.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ISortingJobService, SortingJobService>();
            services.AddSingleton<ISortingService, SortingService>();
            services.AddSingleton<ISortingJobQueue, SortingJobQueue>();
            return services;
        }
    }
}
