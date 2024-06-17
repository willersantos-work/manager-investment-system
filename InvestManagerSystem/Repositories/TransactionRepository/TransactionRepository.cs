using InvestManagerSystem.Enums;
using InvestManagerSystem.Global.Database;
using InvestManagerSystem.Models;

namespace InvestManagerSystem.Repositories.TransactionRepository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DatabaseContext _context;

        public TransactionRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Save(Transaction entity)
        {
            _context.Transaction.Add(entity);
            _context.SaveChanges();
        }

        public IList<Transaction> FindByInvestmentId(int investmentId)
        {
            return 
                _context
                .Transaction
                .Where(i => i.InvestmentId == investmentId)
                .ToList();
        }

        public IList<Transaction> FindByTransactionInRange(int clientId, int amount, string financerProduct, TransactionTypeEnum type, DateTime start)
        {
            return
                _context
                .Transaction
                .Where(
                    p => p.Investment.Quantity == amount &&
                    p.Investment.ClientId == clientId &&
                    p.Investment.FinancerProduct.Name == financerProduct &&
                    p.Type == type &&
                    p.CreatedDate > start
                ).ToList();
        }
    }
}
