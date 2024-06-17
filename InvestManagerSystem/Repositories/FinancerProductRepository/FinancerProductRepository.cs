using InvestManagerSystem.Global.Database;
using InvestManagerSystem.Models;

namespace InvestManagerSystem.Repositories.FinancerProductRepository
{
    public class FinancerProductRepository : IFinancerProductRepository
    {
        private readonly DatabaseContext _context;

        public FinancerProductRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Save(FinancerProduct entity)
        {
            _context.FinancerProduct.Add(entity);
            _context.SaveChanges();
        }

        public IList<FinancerProduct> FindAll()
        {
            return _context.FinancerProduct.ToList();
        }

        public FinancerProduct? FindById(int id)
        {
            return _context.FinancerProduct.Find(id);
        }
        
        public FinancerProduct? FindByName(string name)
        {
            return _context.FinancerProduct.Where(p => p.Name == name).FirstOrDefault();
        }
        
        public IList<FinancerProduct> FindAfterMaturity(DateTime expectedMaturityDate)
        {
            return _context.FinancerProduct.Where(p => p.MaturityDate < expectedMaturityDate).ToList();
        }

        public bool HasName(string name)
        {
            return _context.FinancerProduct.Any(p => p.Name == name);
        }

        public void Update(FinancerProduct entity)
        {
            _context.FinancerProduct.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(FinancerProduct entity)
        {
            _context.FinancerProduct.Remove(entity);
            _context.SaveChanges();
        }
    }
}
