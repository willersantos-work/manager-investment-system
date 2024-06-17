﻿using InvestManagerSystem.Enums;

namespace InvestManagerSystem.Interfaces.Transaction
{
    public class TransactionDto
    {
        public string FinancerProductName { get; set; } = null!;
        public TransactionTypeEnum Type { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }
    }
}
