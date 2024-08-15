using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveAID.Models
{
    [Table("partnership")]
    [PrimaryKey("id")]
    public class Partnership
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string? partner_name { get; set; }

        [Required]
        [StringLength(50)]
        public string? logo { get; set; }

        [StringLength(20)]
        public string? description { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; }

        [NotMapped]
        public IFormFile? myUploads { get; set; }

    }
}
