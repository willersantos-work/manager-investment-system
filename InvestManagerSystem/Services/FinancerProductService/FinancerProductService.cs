using InvestManagerSystem.Enums;
using InvestManagerSystem.Global.Helpers.CustomException;
using InvestManagerSystem.Global.Mapper;
using InvestManagerSystem.Interfaces.Email;
using InvestManagerSystem.Interfaces.FinancerProduct;
using InvestManagerSystem.Models;
using InvestManagerSystem.Repositories.FinancerProductRepository;
using InvestManagerSystem.Services.EmailService;
using InvestManagerSystem.Services.UserService;
using System.Net;
using System.Text.Json;

namespace InvestManagerSystem.Services.FinancerProductService
{
    public class FinancerProductService : IFinancerProductService
    {
        private readonly ILogger<FinancerProductService> _logger;
        private readonly IFinancerProductRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly int _quantityDaysToWarnMaturity = 3;

        public FinancerProductService(
            ILogger<FinancerProductService> logger,
            IFinancerProductRepository repository,
            IEmailService emailService,
            IUserService userService
        )
        {
            _logger = logger;
            _repository = repository;
            _emailService = emailService;
            _userService = userService;
        }

        public FinancerProduct Create(FinancerProductCreateDto data)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(Create)} - Request - {JsonSerializer.Serialize(data)}");
                var nameAlreadyExist = _repository.HasName(data.Name);

                if (nameAlreadyExist)
                {
                    _logger.LogWarning($"Financer Product with name {data.Name} already exist.");
                    throw new CustomException(HttpStatusCode.Conflict, $"Financer Product with name {data.Name} already exist.");
                }

                var entity = new FinancerProduct
                {
                    Name = data.Name,
                    Description = data.Description,
                    InterestRate = data.InterestRate,
                    MaturityDate = DateTime.SpecifyKind(data.MaturityDate, DateTimeKind.Local).ToUniversalTime(),
                    Type = data.Type,
                    Price = data.Price,
                    Quantity = data.Quantity,
                    QuantityBought = data.QuantityBought
                };
                _repository.Save(entity);
                _logger.LogInformation($"end service {nameof(Create)} - Response - {JsonSerializer.Serialize(entity)}");
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(Create)} - Error - {JsonSerializer.Serialize(data)}");
                throw;
            }
        }

        public IList<FinancerProductListDto> GetAll()
        {
            try
            {
                _logger.LogInformation($"start service {nameof(GetAll)}");
                IList<FinancerProduct> financerProducts = _repository.FindAll();
                IList<FinancerProductListDto> financerProductsDto = Mapper.Map<IList<FinancerProduct>, IList<FinancerProductListDto>>(financerProducts); 
                _logger.LogInformation($"end service {nameof(GetAll)} - Response - {JsonSerializer.Serialize(financerProductsDto)}");
                return financerProductsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(GetAll)} - Error");
                throw;
            }
        }

        public FinancerProductDto GetById(int id)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(GetById)} - Request - {id}");
                var entity = _repository.FindById(id);
                
                if (entity is null)
                {
                    _logger.LogWarning($"Financer Product with id {id} not found.");
                    throw new CustomException(HttpStatusCode.NotFound, $"Financer Product with id {id} not found.");
                }

                FinancerProductDto response = Mapper.Map<FinancerProduct, FinancerProductDto>(entity);
                _logger.LogInformation($"end service {nameof(GetById)} - Response - {JsonSerializer.Serialize(response)}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(GetById)} - Error - {id}");
                throw;
            }
        }

        public FinancerProductDto GetByName(string name)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(GetByName)} - Request - {name}");
                var entity = _repository.FindByName(name);
                
                if (entity is null)
                {
                    _logger.LogWarning($"Financer Product with name {name} not found.");
                    throw new CustomException(HttpStatusCode.NotFound, $"Financer Product with name {name} not found.");
                }

                FinancerProductDto response = Mapper.Map<FinancerProduct, FinancerProductDto>(entity);
                _logger.LogInformation($"end service {nameof(GetByName)} - Response - {JsonSerializer.Serialize(response)}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(GetByName)} - Error - {name}");
                throw;
            }
        }

        public void Update(FinancerProductUpdateDto data, int id)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(Update)} - Request - {JsonSerializer.Serialize(data)} {id}");
                var financerProduct = _repository.FindById(id);
                
                if (financerProduct is null)
                {
                    _logger.LogWarning($"Financer Product with id {id} not found.");
                    throw new CustomException(HttpStatusCode.NotFound, $"Financer Product with id {id} not found.");
                }

                financerProduct.Name = data.Name ?? financerProduct.Name;
                financerProduct.Description = data.Description ?? financerProduct.Description;
                financerProduct.InterestRate = data.InterestRate ?? financerProduct.InterestRate;
                financerProduct.MaturityDate = data.MaturityDate is not null ? DateTime.SpecifyKind((DateTime)data.MaturityDate, DateTimeKind.Local).ToUniversalTime() : financerProduct.MaturityDate;
                financerProduct.Type = data.Type ?? financerProduct.Type;
                financerProduct.Price = data.Price ?? financerProduct.Price;
                financerProduct.Quantity = data.Quantity ?? financerProduct.Quantity;
                financerProduct.QuantityBought = data.QuantityBought ?? financerProduct.QuantityBought;
                financerProduct.UpdatedDate = DateTime.UtcNow;
                
                _repository.Update(financerProduct);
                _logger.LogInformation($"end service {nameof(Update)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(Update)} - Error - {JsonSerializer.Serialize(data)} {id}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(Delete)} - Request - {id}");
                var financerProduct = _repository.FindById(id);
                
                if (financerProduct is null)
                {
                    _logger.LogWarning($"Financer Product with id {id} not found.");
                    throw new CustomException(HttpStatusCode.NotFound, $"Financer Product with id {id} not found.");
                }

                _repository.Delete(financerProduct);
                _logger.LogInformation($"end service {nameof(Delete)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(Delete)} - Error - {id}");
                throw;
            }
        }

        public async Task NotifyAfterMaturityDate()
        {
            try
            {
                _logger.LogInformation($"start service {nameof(NotifyAfterMaturityDate)}");
                var expectedMaturityDate = DateTime.UtcNow.AddDays(_quantityDaysToWarnMaturity);
                var productsNearMaturity = _repository.FindAfterMaturity(expectedMaturityDate);

                var productNames = productsNearMaturity.Select(p => p.Name);
                var productNamesString = string.Join("<br>", productNames);

                var adminEmails = _userService.GetByType(UserTypeEnum.Admin);

                foreach (var adminEmail in adminEmails) { 
                    var message = new EmailDto
                    {
                        To = adminEmail.Email,
                        Subject = "teste",
                        Message = 
                            "<h4>Produtos com data de vencimento próxima</h4>" +
                            "<p>Segue a lista de produtos com data de vencimento próxima</p>" +
                            $"<p> {productNamesString} </p>"
                    };
                    await _emailService.SendEmail(message);
                }
                _logger.LogInformation($"end service {nameof(NotifyAfterMaturityDate)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(NotifyAfterMaturityDate)} - Error");
                throw;
            }
        }
    }
}