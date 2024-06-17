using InvestManagerSystem.Enums;
using InvestManagerSystem.Interfaces.Transaction;

namespace InvestManagerSystem.Services.TransactionService
{
    public interface ITransactionService
    {
        void Create(TransactionCreateDto data);
        IList<TransactionDto> GetByInvestmentId(int investmentId);
        IList<TransactionDto> GetByTransactionInRange(
                    int clientId,
                    string financerProduct,
                    int amount,
                    TransactionTypeEnum type,
                    DateTime start
        );
    }
}
