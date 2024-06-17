using InvestManagerSystem.Enums;
using InvestManagerSystem.Global.Database;
using InvestManagerSystem.Models;

namespace InvestManagerSystem.Repositories.InvestmentRepository
{
    public class InvestmentRepository : IInvestmentRepository
    {
        private readonly DatabaseContext _context;

        public InvestmentRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Save(Investment entity)
        {
            _context.Investment.Add(entity);
            _context.SaveChanges();
        }

        public IList<Investment> FindByClientId(int clientId)
        {
            return _context.Investment.Where(i => i.ClientId == clientId).ToList();
        }

        public Investment? FindById(int id)
        {
            return _context.Investment.FirstOrDefault(i => i.Id == id);
        }
        
        public Investment? FindByFinancerProduct(string financerProduct)
        {
            return _context.Investment.FirstOrDefault(i => i.FinancerProduct.Name == financerProduct);
        }

        public void Update(Investment entity)
        {
            _context.Investment.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(Investment entity)
        {
            _context.Investment.Remove(entity);
            _context.SaveChanges();
        }
    }
}
