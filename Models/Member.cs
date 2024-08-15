using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace GiveAID.Models
{
    [Table("member")]
    [PrimaryKey("mem_id")]
    [Index(nameof(mem_username), IsUnique = true)]
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mem_id { get; set; }

        [Required]
        [StringLength(50)]
        public string? mem_username { get; set; }

        [Required]
        [StringLength(100)]
        public string? mem_password { get; set; }

        [Required]
        [StringLength(50)]
        public string? mem_name { get; set; }

        [Required]
        [StringLength(50)]
        public string? email { get; set; }

        [Required]
        [StringLength(50)]
        public string? phone_number { get; set; }

        [Required]
        public int status { get; set; } = 1;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; }
    }
}
