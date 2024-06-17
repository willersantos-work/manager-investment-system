﻿using InvestManagerSystem.Enums;
using System.Text.Json.Serialization;

namespace InvestManagerSystem.Interfaces.FinancerProduct
{
    public class FinancerProductUpdateDto
    {
        public string? Name { get; set; }
        
        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProductTypeEnum? Type { get; set; }

        public int? Quantity { get; set; }

        public int? QuantityBought { get; set; }

        public decimal? Price { get; set; }

        public DateTime? MaturityDate { get; set; }

        public decimal? InterestRate { get; set; }
    }
}
