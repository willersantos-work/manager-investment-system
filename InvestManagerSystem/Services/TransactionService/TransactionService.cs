using InvestManagerSystem.Enums;
using InvestManagerSystem.Global.Helpers.CustomException;
using InvestManagerSystem.Global.Mapper;
using InvestManagerSystem.Interfaces.FinancerProduct;
using InvestManagerSystem.Interfaces.Transaction;
using InvestManagerSystem.Models;
using InvestManagerSystem.Repositories.TransactionRepository;
using System.Net;
using System.Text.Json;

namespace InvestManagerSystem.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionRepository _repository;

        public TransactionService(ILogger<TransactionService> logger, ITransactionRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }


        public void Create(TransactionCreateDto data)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(Create)} - Request - {JsonSerializer.Serialize(data)}");
                var entity = new Transaction
                {
                    InvestmentId = data.InvestmentId,
                    Type = data.Type,
                    Amount = data.Amount,
                    Price = data.Price,
                    Total = data.Price * data.Amount
                };
                _repository.Save(entity);
                _logger.LogInformation($"end service {nameof(Create)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(Create)} - Error - {JsonSerializer.Serialize(data)}");
                throw;
            }
        }

        public IList<TransactionDto> GetByInvestmentId(int investmentId)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(GetByInvestmentId)} - Request - {investmentId}");
                var entities = _repository.FindByInvestmentId(investmentId);
                IList<TransactionDto> transactions = Mapper.Map<IList<Transaction>, IList<TransactionDto>>(entities);
                _logger.LogInformation($"end service {nameof(GetByInvestmentId)} - Response - {JsonSerializer.Serialize(transactions)}");
                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(GetByInvestmentId)} - Error - {investmentId}");
                throw;
            }
        }

        public IList<TransactionDto> GetByTransactionInRange(
                    int clientId,
                    string financerProduct,
                    int amount,
                    TransactionTypeEnum type,
                    DateTime start
        )
        {
            try
            {
                _logger.LogInformation($"start service {nameof(GetByTransactionInRange)} - Request - {clientId} {financerProduct} {amount} {type} {start}");
                var entities = _repository.FindByTransactionInRange(clientId, amount, financerProduct, type, start);
                IList<TransactionDto> transactions = Mapper.Map<IList<Transaction>, IList<TransactionDto>>(entities);
                _logger.LogInformation($"end service {nameof(GetByTransactionInRange)} - Response - {JsonSerializer.Serialize(transactions)}");
                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(GetByTransactionInRange)} - Error - {clientId} {financerProduct} {amount} {type} {start}");
                throw;
            }
        }
    }
}