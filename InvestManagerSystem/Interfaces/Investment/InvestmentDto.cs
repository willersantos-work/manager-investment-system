using InvestManagerSystem.Interfaces.FinancerProduct;

namespace InvestManagerSystem.Interfaces.Investment
{
    public class InvestmentDto
    {
        public int FinancerProductId { get; set; }

        public FinancerProductDto FinancerProduct { get; set; } = null!;

        public int Quantity { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal PurchasePrice { get; set; }

        public DateTime? SalesDate { get; set; }

        public decimal? SalesPrice { get; set; }
    }
}
