using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GiveAID.Models.ModelView
{
    public class GalleryView
    {
        public int id { get; set; }
        public string? image_name { get; set; }
        public int program_id { get; set; }
        public DateTime created_at { get; set; }

        public string? o_program_name { get; set; }
        //upload images
        public IFormFile? myImgUpload { get; set; }
    }
}
