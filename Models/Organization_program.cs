using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace GiveAID.Models
{
    [Table("organization_program")]
    [PrimaryKey("id")]
    public class Organization_program
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string? o_program_name { get; set; }

        [Required]
        public DateTime o_program_date { get; set; }

        [Required]
        [MaxLength]
        public string? about_detail { get; set; }

        public int target { get; set; }
        public int current { get; set; }
        public int status { get; set; }
        public bool outstanding { get; set; }

        [Required]
        [ForeignKey("topic_id")]
        public int topic_id { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; }

        public Topic? Topic;
    }
}
