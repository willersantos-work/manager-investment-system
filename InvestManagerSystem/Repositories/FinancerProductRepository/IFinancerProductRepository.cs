using InvestManagerSystem.Models;

namespace InvestManagerSystem.Repositories.FinancerProductRepository
{
    public interface IFinancerProductRepository
    {
        void Save(FinancerProduct entity);
        IList<FinancerProduct> FindAll();
        FinancerProduct? FindById(int id);
        FinancerProduct? FindByName(string name);
        IList<FinancerProduct> FindAfterMaturity(DateTime expectedMaturityDate);
        bool HasName(string name);
        void Update(FinancerProduct entity);
        void Delete(FinancerProduct entity);
    }
}
