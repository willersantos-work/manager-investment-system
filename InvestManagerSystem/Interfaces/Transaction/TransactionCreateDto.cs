using InvestManagerSystem.Enums;

namespace InvestManagerSystem.Interfaces.Transaction
{
    public class TransactionCreateDto
    {
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int InvestmentId { get; set; }
        public TransactionTypeEnum Type {  get; set; }
    }
}
