using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GiveAID.ModelView
{
    public class MV_Topic
    {
        public int id { get; set; }
        public string? topic_name { get; set; }
        public DateTime created_at { get; set; }
        public string pro_gal { get; set; }
    }
}
