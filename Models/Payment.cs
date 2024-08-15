using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace GiveAID.Models
{
    [Table("payment")]
    [PrimaryKey("id")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength]
        public string? description { get; set; }

        [Required]
        public int payout { get; set; }

        [Required]
        [ForeignKey("program_id")]
        public int program_id { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; }
    }
}
