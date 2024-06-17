using InvestManagerSystem.Enums;
using System.Text.Json.Serialization;

namespace InvestManagerSystem.Interfaces.FinancerProduct
{
    public class FinancerProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProductTypeEnum Type { get; set; }

        public DateTime MaturityDate { get; set; }

        public int Quantity { get; set; }

        public int QuantityBought { get; set; }

        public decimal Price { get; set; }

        public decimal? InterestRate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
