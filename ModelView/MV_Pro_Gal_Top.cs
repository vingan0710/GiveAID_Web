using System.ComponentModel.DataAnnotations;

namespace GiveAID.ModelView
{
    public class MV_Pro_Gal_Top
    {
        public int id_program { get; set; }
        public string? o_program_name { get; set; }
        public DateTime o_program_date { get; set; }
        public string? about_detail { get; set; }
        public int target { get; set; }
        public int current { get; set; }
        public Int16 status { get; set; }
        public bool outstanding { get; set; }
        public int topic_id { get; set; }
        public DateTime created_at { get; set; }

        public int id_gallery { get; set; }
        public string? image_name { get; set; }

        public int id_topic { get; set; }

        public string? topic_name { get; set; }
    }
}
