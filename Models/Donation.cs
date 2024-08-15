using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace GiveAID.Models
{
    [Table("donation")]
    [PrimaryKey("id")]
    public class Donation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int money { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime donation_date { get; set; }

        [Required]
        [ForeignKey("member_id")]
        public int member_id { get; set; }

        [Required]
        [ForeignKey("program_id")]
        public int program_id { get; set; }

        public string? id_pay { get; set; } 

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; }

        //public Member? Member { get; set; }
        //public Organization_program? Organization_Program { get; set; }
    }
}
