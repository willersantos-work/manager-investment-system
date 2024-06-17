using Microsoft.AspNetCore.Mvc;
using InvestManagerSystem.Interfaces.FinancerProduct;
using InvestManagerSystem.Services.FinancerProductService;
using InvestManagerSystem.Models;
using InvestManagerSystem.Global.Helpers.ApiPrefix;

namespace InvestManagerSystem.Controllers.Admin
{
    [Route(ApiPrefix.Admin + "financer-product")]
    [ApiController]
    public class FinancerProductController : ControllerBase
    {
        private readonly ILogger<FinancerProductController> _logger;
        private readonly IFinancerProductService _financerProductService;

        public FinancerProductController(ILogger<FinancerProductController> logger, IFinancerProductService financerProductService)
        {
            _logger = logger;
            _financerProductService = financerProductService;
        }

        // POST /api/admin/financer-product
        [HttpPost]
        public ActionResult<FinancerProduct> Create([FromBody] FinancerProductCreateDto newFinancerProduct)
        {
            _logger.LogInformation($"start method {nameof(Create)}");
            var response = _financerProductService.Create(newFinancerProduct);
            return CreatedAtAction(nameof(Create), response);
        }

        // GET /api/admin/financer-product
        [HttpGet]
        public ActionResult<IList<FinancerProductListDto>> GetAll()
        {
            _logger.LogInformation($"start method {nameof(GetAll)}");
            var response = _financerProductService.GetAll();
            return Ok(response);
        }

        // GET /api/admin/financer-product/{id}
        [HttpGet("{id}")]
        public ActionResult<FinancerProductDto> GetById(int id)
        {
            _logger.LogInformation($"start method {nameof(GetById)}");
            var response = _financerProductService.GetById(id);
            return Ok(response);
        }

        // PUT /api/admin/financer-product/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] FinancerProductUpdateDto financerProduct)
        {
            _logger.LogInformation($"start method {nameof(Update)}");
            _financerProductService.Update(financerProduct, id);
            return Ok();
        }

        // DELETE /api/admin/financer-product/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"start method {nameof(Delete)}");
            _financerProductService.Delete(id);
            return NoContent();
        }
    }
}
