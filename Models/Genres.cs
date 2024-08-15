using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveAID.Models
{
    [Table("genres")]
    public class Genres
    {
        [Key]
        public int gen_id { get; set; }
        public string?  gen_title { get; set; }
    }
}
