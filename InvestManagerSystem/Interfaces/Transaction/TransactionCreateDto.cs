using InvestManagerSystem.Enums;
using System.Text.Json.Serialization;

namespace InvestManagerSystem.Interfaces.Transaction
{
    public class TransactionCreateDto
    {
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int InvestmentId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionTypeEnum Type {  get; set; }
    }
}
