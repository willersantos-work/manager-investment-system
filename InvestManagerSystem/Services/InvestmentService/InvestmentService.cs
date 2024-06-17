using InvestManagerSystem.Enums;
using InvestManagerSystem.Global.Helpers.CustomException;
using InvestManagerSystem.Global.Mapper;
using InvestManagerSystem.Interfaces.FinancerProduct;
using InvestManagerSystem.Interfaces.Investment;
using InvestManagerSystem.Interfaces.Transaction;
using InvestManagerSystem.Models;
using InvestManagerSystem.Repositories.InvestmentRepository;
using InvestManagerSystem.Services.FinancerProductService;
using InvestManagerSystem.Services.TransactionService;
using System.Net;
using System.Text.Json;

namespace InvestManagerSystem.Services.InvestmentService
{
    public class InvestmentService : IInvestmentService
    {
        private readonly ILogger<InvestmentService> _logger;
        private readonly IInvestmentRepository _repository;
        private readonly ITransactionService _transactionService;
        private readonly IFinancerProductService _financerProductService;

        public InvestmentService(
            ILogger<InvestmentService> logger,
            IInvestmentRepository repository,
            ITransactionService transactionService,
            IFinancerProductService financerProductService
        )
        {
            _logger = logger;
            _repository = repository;
            _transactionService = transactionService;
            _financerProductService = financerProductService;
        }

        public void Buy(InvestmentTransactionDto transaction, int clientId)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(Buy)} - Request - {JsonSerializer.Serialize(transaction)} {clientId}");
                var financerProduct = VerifyFinancerProduct(transaction.FinancerProductName);
                VerifyDuplication(clientId, transaction.Amount, transaction.FinancerProductName, TransactionTypeEnum.Bought);

                var quantityRest = financerProduct.Quantity - financerProduct.QuantityBought;
                if (quantityRest < transaction.Amount)
                {
                    _logger.LogWarning($"Amount for this financer product is not enought for your bought, it just rest {quantityRest} in this product.");
                    throw new CustomException(HttpStatusCode.PreconditionFailed, $"Amount for this financer product is not enought for your bought, it just rest {quantityRest} in this product.");
                }


                var investment = _repository.FindByFinancerProduct(transaction.FinancerProductName);

                if (investment is not null)
                {
                    investment.UpdatedDate = DateTime.UtcNow;
                    investment.PurchaseDate = DateTime.UtcNow;
                    investment.Quantity += transaction.Amount;
                    investment.PurchasePrice = financerProduct.Price;
                    _repository.Update(investment);
                } else
                {
                    investment = new Investment
                    {
                        ClientId = clientId,
                        FinancerProductId = financerProduct.Id,
                        PurchaseDate = DateTime.UtcNow,
                        PurchasePrice = financerProduct.Price,
                        Quantity = transaction.Amount,
                    };
                    _repository.Save(investment);
                }

                // TODO: Verificar se conseguirá pegar id na primeira criação
                _transactionService.Create(new TransactionCreateDto{
                    Amount = transaction.Amount,
                    InvestmentId = investment.Id,
                    Type = TransactionTypeEnum.Bought,
                    Price = financerProduct.Price
                });

                _financerProductService.Update(new FinancerProductUpdateDto
                {
                    QuantityBought = financerProduct.QuantityBought + transaction.Amount
                }, financerProduct.Id);
                _logger.LogInformation($"end service {nameof(Buy)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(Buy)} - Error - {JsonSerializer.Serialize(transaction)} {clientId}");
                throw;
            }
        }

        public void Sell(InvestmentTransactionDto transaction, int clientId)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(Sell)} - Request - {JsonSerializer.Serialize(transaction)}");
                var financerProduct = VerifyFinancerProduct(transaction.FinancerProductName);
                VerifyDuplication(clientId, transaction.Amount, transaction.FinancerProductName, TransactionTypeEnum.Sales);

                var investment = _repository.FindByFinancerProduct(transaction.FinancerProductName);

                if (investment is null)
                {
                    _logger.LogWarning($"None investment with this financer product {transaction.FinancerProductName}.");
                    throw new CustomException(HttpStatusCode.NotFound, $"None investment with this financer product {transaction.FinancerProductName}.");
                } else if (investment.Quantity < transaction.Amount)
                {
                    _logger.LogWarning($"There is not quantity enough to sell, quantity in your investment is {investment.Quantity}.");
                    throw new CustomException(HttpStatusCode.PreconditionFailed, $"There is not quantity enough to sell, quantity in your investment is {investment.Quantity}.");
                } else
                {
                    investment.UpdatedDate = DateTime.UtcNow;
                    investment.SalesDate = DateTime.UtcNow;
                    investment.Quantity -= transaction.Amount;
                    investment.SalesPrice = financerProduct.Price;
                    _repository.Update(investment);

                    _transactionService.Create(new TransactionCreateDto
                    {
                        Amount = transaction.Amount,
                        InvestmentId = investment.Id,
                        Type = TransactionTypeEnum.Sales,
                        Price = financerProduct.Price
                    });

                    if (investment.Quantity == 0) _repository.Delete(investment);

                    _financerProductService.Update(new FinancerProductUpdateDto
                    {
                        QuantityBought = financerProduct.QuantityBought - transaction.Amount
                    }, financerProduct.Id);
                }
                _logger.LogInformation($"end service {nameof(Sell)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(Sell)} - Error - {JsonSerializer.Serialize(transaction)}");
                throw;
            }
        }

        public IList<InvestmentDto> GetByClientId(int clientId)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(GetByClientId)} - Request - {clientId}");
                var entities = _repository.FindByClientId(clientId);
                IList<InvestmentDto> investments = Mapper.Map<IList<Investment>, IList<InvestmentDto>>(entities);
                _logger.LogInformation($"end service {nameof(GetByClientId)} - Response - {JsonSerializer.Serialize(investments)}");
                return investments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(GetByClientId)} - Error - {clientId}");
                throw;
            }
        }

        public InvestmentDto GetById(int id, int clientId)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(GetById)} - Request - {id} {clientId}");
                var entity = _repository.FindById(id);

                if (entity is null)
                {
                    _logger.LogWarning($"Investment with id {id} not found.");
                    throw new CustomException(HttpStatusCode.NotFound, $"Investment with id {id} not found.");
                }

                if(entity.ClientId != clientId)
                {
                    _logger.LogWarning($"Investment does not belong for this client.");
                    throw new CustomException(HttpStatusCode.Forbidden, $"Investment does not belong for this client.");
                }
                
                InvestmentDto investment = Mapper.Map<Investment, InvestmentDto>(entity);
                _logger.LogInformation($"end service {nameof(GetById)} - Response - {JsonSerializer.Serialize(investment)}");
                return investment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(GetById)} - Error - {id} {clientId}");
                throw;
            }
        }

        public IList<TransactionDto> GetStatementById(int id, int clientId)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(GetStatementById)} - Request - {id} {clientId}");
                GetById(id, clientId);

                var transactions = _transactionService.GetByInvestmentId(id);
                _logger.LogInformation($"end service {nameof(GetStatementById)} - Response - {JsonSerializer.Serialize(transactions)}");
                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(GetStatementById)} - Error - {id} {clientId}");
                throw;
            }
        }

        private FinancerProductDto VerifyFinancerProduct(string financerProductName)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(VerifyFinancerProduct)} - Request - {financerProductName}");
                var financerProduct = _financerProductService.GetByName(financerProductName);

                if (financerProduct is null)
                {
                    _logger.LogWarning($"Financer Product with name {financerProductName} not found.");
                    throw new CustomException(HttpStatusCode.NotFound, $"Financer Product with name {financerProductName} not found.");
                }

                if (financerProduct.MaturityDate < DateTime.UtcNow)
                {
                    _logger.LogWarning($"Financer Product is out of maturity date, maturity date of this product is {financerProduct.MaturityDate.Date}.");
                    throw new CustomException(HttpStatusCode.PreconditionFailed, $"Financer Product is out of maturity date, maturity date of this product is {financerProduct.MaturityDate.Date}.");
                }
                _logger.LogInformation($"end service {nameof(VerifyFinancerProduct)} - Response - {JsonSerializer.Serialize(financerProduct)}");
                return financerProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(VerifyFinancerProduct)} - Error - {financerProductName}");
                throw;
            }
        }

        private void VerifyDuplication(int clientId, int amount, string financerProduct, TransactionTypeEnum type)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(VerifyDuplication)} - Request - {clientId} {financerProduct} {amount} {type}");
                var transactions = _transactionService.GetByTransactionInRange(
                    clientId,
                    financerProduct,
                    amount,
                    type,
                    DateTime.UtcNow.AddMinutes(-1)
                );

                if (transactions.Count > 0)
                {
                    _logger.LogWarning($"Transaction is duplicated.");
                    throw new CustomException(HttpStatusCode.PreconditionFailed, $"Transaction is duplicated.");
                }
                _logger.LogInformation($"end service {nameof(VerifyDuplication)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(VerifyDuplication)} - Error - {clientId} {financerProduct} {amount} {type}");
                throw;
            }
        }
    }
}