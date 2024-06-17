using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestManagerSystem.Models
{
    [Table("investment")]
    public class Investment : BaseModel
    {
        [Required]
        [Column("financer_product_id")]
        public int FinancerProductId { get; set; }

        [ForeignKey("FinancerProductId" )]
        public FinancerProduct FinancerProduct { get; set; } = null!;

        [Required]
        [Column("client_id")]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public User Client { get; set; } = null!;

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("purchase_date")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column("purchase_price")]
        public decimal PurchasePrice { get; set; }

        [Column("sales_date")]
        public DateTime? SalesDate { get; set; }

        [DataType(DataType.Currency)]
        [Column("sales_price")]
        public decimal? SalesPrice { get; set; }
    }
}
