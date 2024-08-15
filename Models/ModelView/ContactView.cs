using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveAID.Models.ModelView
{
    public class ContactView
    {

        public int id { get; set; }

        public int status { get; set; }

        public int mem_id { get; set; }

        public int program_id { get; set; }
        public string? fullname { get; set; }
        public string? body { get; set; }

        public DateTime created_at { get; set; }

        public string? email { get; set; }
        public string? phone_number { get; set; }
    }
}
