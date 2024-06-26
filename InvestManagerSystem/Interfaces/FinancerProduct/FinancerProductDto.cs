﻿using InvestManagerSystem.Enums;

namespace InvestManagerSystem.Interfaces.FinancerProduct
{
    public class FinancerProductListDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime MaturityDate { get; set; }

        public int Quantity { get; set; }

        public int QuantityBought { get; set; }

        public decimal Price { get; set; }
    }
}
