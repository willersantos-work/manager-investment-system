using Microsoft.AspNetCore.Mvc;
using InvestManagerSystem.Interfaces.Investment;
using InvestManagerSystem.Interfaces.Transaction;
using InvestManagerSystem.Services.InvestmentService;
using InvestManagerSystem.Auth.Context;
using InvestManagerSystem.Global.Helpers.ApiPrefix;

namespace InvestManagerSystem.Controllers.Client
{
    [ApiController]
    [Route(ApiPrefix.Client + "investment")]
    public class ClientInvestmentController : ControllerBase
    {
        private readonly ILogger<ClientInvestmentController> _logger;
        private readonly IInvestmentService _investmentService;

        public ClientInvestmentController(ILogger<ClientInvestmentController> logger, IInvestmentService investmentService)
        {
            _logger = logger;
            _investmentService = investmentService;
        }

        // POST /api/client/investments/buy
        [HttpPost("buy")]
        public IActionResult Buy([FromBody] InvestmentTransactionDto investmentTransaction)
        {
            _logger.LogInformation($"start method {nameof(Buy)}");
            var clientId = HttpContext.GetId();
            _investmentService.Buy(investmentTransaction, clientId);
            return Ok();
        }

        // POST /api/client/investments/sell
        [HttpPost("sell")]
        public IActionResult Sell([FromBody] InvestmentTransactionDto investmentTransaction)
        {
            _logger.LogInformation($"start method {nameof(Sell)}");
            var clientId = HttpContext.GetId();
            _investmentService.Sell(investmentTransaction, clientId);
            return Ok();
        }

        // GET /api/client/investments
        [HttpGet]
        public ActionResult<IList<InvestmentDto>> GetAll()
        {
            _logger.LogInformation($"start method {nameof(GetAll)}");
            var clientId = HttpContext.GetId();
            var response = _investmentService.GetByClientId(clientId);
            return Ok(response);
        }

        // GET /api/client/investments/{id}
        [HttpGet("{id}")]
        public ActionResult<InvestmentDto> GetById(int id)
        {
            _logger.LogInformation($"start method {nameof(GetById)}");
            var clientId = HttpContext.GetId();
            var response = _investmentService.GetById(id, clientId);
            return Ok(response);
        }

        // GET /api/client/investments/{id}/statement
        [HttpGet("{id}/statement")]
        public ActionResult<IList<TransactionDto>> GetStatement(int id)
        {
            _logger.LogInformation($"start method {nameof(GetStatement)}");
            var clientId = HttpContext.GetId();
            var response = _investmentService.GetStatementById(id, clientId);
            return Ok(response);
        }
    }
}
