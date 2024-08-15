using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GiveAID.Models.ModelView
{
    public class ProgramView
    {
        public int id { get; set; }
        public string? o_program_name { get; set; }
        public DateTime o_program_date { get; set; }
        public string? about_detail { get; set; }
        public int target { get; set; }
        public int current { get; set; }
        public int status { get; set; }
        public bool outstanding { get; set; }

        public int topic_id { get; set; }
        public string? topic_name { get; set; }
        public List<Topic>? topics { get; set; }

    }
    
}
