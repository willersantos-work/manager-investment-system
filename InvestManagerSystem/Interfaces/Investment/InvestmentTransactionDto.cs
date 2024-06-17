namespace InvestManagerSystem.Interfaces.Investment
{
    public class InvestmentTransactionDto
    {
        public string FinancerProductName { get; set; } = null!;

        public int Amount { get; set; }
    }
}
