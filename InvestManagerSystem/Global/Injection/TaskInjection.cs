using Coravel;
using InvestManagerSystem.TaskScheduler.Tasks;

namespace InvestManagerSystem.Global.Injection
{
    public static class TaskInjection
    {
        public static IServiceCollection TaskLayerInjection(this IServiceCollection services)
        {
            services.AddScheduler();

            services.AddTransient<NotifyTask>();

            return services;
        }
    }
}
