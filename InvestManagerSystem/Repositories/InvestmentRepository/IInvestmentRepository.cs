using InvestManagerSystem.Enums;
using InvestManagerSystem.Models;

namespace InvestManagerSystem.Repositories.InvestmentRepository
{
    public interface IInvestmentRepository
    {
        void Save(Investment entity);
        IList<Investment> FindByClientId(int clientId);
        Investment? FindById(int id);
        Investment? FindByFinancerProduct(string financerProduct);
        void Update(Investment entity);
        void Delete(Investment entity);
    }
}
