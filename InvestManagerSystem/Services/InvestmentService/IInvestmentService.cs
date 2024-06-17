using InvestManagerSystem.Interfaces.Investment;
using InvestManagerSystem.Interfaces.Transaction;

namespace InvestManagerSystem.Services.InvestmentService
{
    public interface IInvestmentService
    {
        void Buy(InvestmentTransactionDto investmentTransaction, int clientId);
        void Sell(InvestmentTransactionDto investmentTransaction, int clientId);
        IList<InvestmentDto> GetByClientId(int clientId);
        InvestmentDto GetById(int id, int clientId);
        IList<TransactionDto> GetStatementById(int id, int clientId);
    }
}
