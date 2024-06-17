using Coravel.Invocable;
using InvestManagerSystem.Services.FinancerProductService;

namespace InvestManagerSystem.TaskScheduler.Tasks
{
    public class NotifyTask : IInvocable
    {
        private readonly ILogger<NotifyTask> _logger;
        private readonly IFinancerProductService _financerProductService;

        public NotifyTask(ILogger<NotifyTask> logger, IFinancerProductService financerProductService)
        {
            _logger = logger;
            _financerProductService = financerProductService;
        }

        public async Task Invoke()
        {
            _logger.LogInformation($"start task {nameof(NotifyTask)}");
            await _financerProductService.NotifyAfterMaturityDate();
            _logger.LogInformation($"end task {nameof(NotifyTask)}");
        }
    }
}
