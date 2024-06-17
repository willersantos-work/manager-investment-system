using Coravel.Scheduling.Schedule.Interfaces;
using InvestManagerSystem.TaskScheduler.Tasks;

namespace InvestManagerSystem.TaskScheduler
{
    public class SchedulerConfig
    {
        public static void ConfigureScheduler(IScheduler scheduler)
        {
            // TODO: Remover após testes de envio de email
            scheduler.Schedule<NotifyTask>().EveryTenSeconds();

            scheduler.Schedule<NotifyTask>().DailyAt(07, 00).Zoned(TimeZoneInfo.Utc).Once();
        }
    }
}
