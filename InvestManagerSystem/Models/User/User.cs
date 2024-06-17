using InvestManagerSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestManagerSystem.Models
{
    [Table("user")]
    public class User : BaseModel
    {
        [Required]
        [Column("type")]
        public UserTypeEnum Type { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("fullname")]
        public string Fullname { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; } = null!;

        [Required]
        [Column("hash_password")]
        public string HashPassword { get; set; } = null!;
    }
}
