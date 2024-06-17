using InvestManagerSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestManagerSystem.Models
{
    [Table("transaction")]
    public class Transaction : BaseModel
    {
        [Required]
        [Column("investment_id")]
        public int InvestmentId { get; set; }

        [ForeignKey("InvestmentId")]
        public Investment Investment { get; set; } = null!;

        [Required]
        [Column("type")]
        public TransactionTypeEnum Type { get; set; }

        [Required]
        [Column("amount")]
        public int Amount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column("price")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column("total")]
        public decimal Total { get; set; }
    }
}
