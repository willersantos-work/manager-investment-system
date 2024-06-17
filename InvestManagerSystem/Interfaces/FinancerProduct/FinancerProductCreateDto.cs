using InvestManagerSystem.Enums;

namespace InvestManagerSystem.Interfaces.FinancerProduct
{
    public class FinancerProductCreateDto
    {
        public string Name { get; set; } = null!;
        
        public string Description { get; set; } = null!;

        public ProductTypeEnum Type { get; set; }
        
        public int Quantity { get; set; }
        
        public int QuantityBought { get; set; }
        
        public decimal Price { get; set; }

        public DateTime MaturityDate { get; set; }

        public decimal? InterestRate { get; set; }
    }
}
