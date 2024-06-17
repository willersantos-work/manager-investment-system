using InvestManagerSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestManagerSystem.Models
{
    [Table("financer_product")]
    public class FinancerProduct : BaseModel
    {
        [MaxLength(100)]
        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Required]
        [Column("description")]
        public string Description { get; set; } = null!;

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("quantity_bought")]
        public int QuantityBought { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column("price")]
        public decimal Price { get; set; }

        [Required]
        [Column("type")]
        public ProductTypeEnum Type { get; set; }

        [Required]
        [Column("maturity_date")]
        public DateTime MaturityDate { get; set; }

        [Column("interest_rate")]
        public decimal? InterestRate { get; set; }
    }
}
