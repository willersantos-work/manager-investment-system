using InvestManagerSystem.Enums;
using InvestManagerSystem.Models;

namespace InvestManagerSystem.Repositories.TransactionRepository
{
    public interface ITransactionRepository
    {
        void Save(Transaction entity);
        IList<Transaction> FindByInvestmentId(int investmentId);
        IList<Transaction> FindByTransactionInRange(int clientId, int amount, string financerProduct, TransactionTypeEnum type, DateTime start);
    }
}
