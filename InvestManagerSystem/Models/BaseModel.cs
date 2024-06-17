using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestManagerSystem.Models
{
    public abstract class BaseModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("created_rate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("updated_rate")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
