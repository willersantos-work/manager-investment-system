using InvestManagerSystem.Interfaces.FinancerProduct;
using InvestManagerSystem.Models;

namespace InvestManagerSystem.Services.FinancerProductService
{
    public interface IFinancerProductService
    {
        FinancerProduct Create(FinancerProductCreateDto data);
        IList<FinancerProductDto> GetAll();
        FinancerProductDto GetById(int id);
        FinancerProductDto GetByName(string name);
        void Update(FinancerProductUpdateDto data, int id);
        void Delete(int id);
        Task NotifyAfterMaturityDate();
    }
}
