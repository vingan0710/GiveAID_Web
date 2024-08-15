using GiveAID.Models;
using GiveAID.ModelView;

namespace GiveAID.Dao
{
    public class Dao_Pro_Gal_Top
    {
        private static Dao_Pro_Gal_Top? instance = null;
        private static AIDContext? ct;

        private Dao_Pro_Gal_Top()
        {
            ct = new();
        }

        public static Dao_Pro_Gal_Top Instance()
        {
            instance ??= new Dao_Pro_Gal_Top();
            return instance;
        }

        public List<MV_Pro_Gal_Top> Get_Program_Galleries()
        {
            var zx = new AIDContext();
            var pro = (from p in zx?.Organization_Programs
                       join g in zx?.Galleries
                       on p.id equals g.program_id
                       join t in zx?.Topics
                       on p.topic_id equals t.id
                       where p.status == 1 && p.outstanding == false
                       select new MV_Pro_Gal_Top
                       {
                           id_program = p.id,
                           id_gallery = g.id,
                           id_topic = t.id,
                           topic_name = t.topic_name,
                           about_detail = p.about_detail,
                           current = p.current,
                           created_at = p.created_at,
                           image_name = g.image_name,
                           outstanding = p.outstanding,
                           o_program_date = p.o_program_date,
                           o_program_name = p.o_program_name,
                           target = p.target,
                           topic_id = p.topic_id,
                       }).ToList();
            return pro;
        }

        public List<MV_Pro_Gal_Top> Get_Program_GalleriesHaveOuts()
        {
            var zx = new AIDContext();
            var pro = (from p in zx?.Organization_Programs
                       join g in zx?.Galleries
                       on p.id equals g.program_id
                       join t in zx?.Topics
                       on p.topic_id equals t.id
                       where p.status == 1
                       select new MV_Pro_Gal_Top
                       {
                           id_program = p.id,
                           id_gallery = g.id,
                           id_topic = t.id,
                           topic_name = t.topic_name,
                           about_detail = p.about_detail,
                           current = p.current,
                           created_at = p.created_at,
                           image_name = g.image_name,
                           outstanding = p.outstanding,
                           o_program_date = p.o_program_date,
                           o_program_name = p.o_program_name,
                           target = p.target,
                           topic_id = p.topic_id,
                       }).ToList();
            return pro;
        }

        public List<MV_Pro_Gal_Top> Get_Program_Galleries__IdTopic(int id)
        {
            var pro = (from p in ct?.Organization_Programs
                       join g in ct?.Galleries
                       on p.id equals g.program_id
                       join t in ct.Topics
                       on p.topic_id equals t.id
                       where p.status == 1 && p.outstanding == false && t.id == id
                       select new MV_Pro_Gal_Top
                       {
                           id_program = p.id,
                           id_gallery = g.id,
                           id_topic = t.id,
                           topic_name = t.topic_name,
                           about_detail = p.about_detail,
                           current = p.current,
                           created_at = p.created_at,
                           image_name = g.image_name,
                           outstanding = p.outstanding,
                           o_program_date = p.o_program_date,
                           o_program_name = p.o_program_name,
                           target = p.target,
                           topic_id = p.topic_id,
                       }).ToList();
            return pro;
        }

        public List<MV_Pro_Gal_Top> Get_Program_GalleriesWithTopicId(int id)
        {
            var pro = (from p in ct?.Organization_Programs
                       join g in ct?.Galleries
                       on p.id equals g.program_id
                       join t in ct.Topics
                       on p.topic_id equals t.id
                       where p.status == 1 && p.topic_id == id
                       select new MV_Pro_Gal_Top
                       {
                           id_program = p.id,
                           id_gallery = g.id,
                           id_topic = t.id,
                           topic_name = t.topic_name,
                           about_detail = p.about_detail,
                           current = p.current,
                           created_at = p.created_at,
                           image_name = g.image_name,
                           outstanding = p.outstanding,
                           o_program_date = p.o_program_date,
                           o_program_name = p.o_program_name,
                           target = p.target,
                           topic_id = p.topic_id,
                       }).ToList();
            return pro;
        }

        public MV_Pro_Gal_Top Get_Program_Galleries__IdProgram(int id)
        {
            var pro = (from p in ct?.Organization_Programs
                       join g in ct?.Galleries
                       on p.id equals g.program_id
                       join t in ct.Topics
                       on p.topic_id equals t.id
                       where p.status == 1 && p.id == id
                       select new MV_Pro_Gal_Top
                       {
                           id_program = p.id,
                           id_gallery = g.id,
                           id_topic = t.id,
                           topic_name = t.topic_name,
                           about_detail = p.about_detail,
                           current = p.current,
                           created_at = p.created_at,
                           image_name = g.image_name,
                           outstanding = p.outstanding,
                           o_program_date = p.o_program_date,
                           o_program_name = p.o_program_name,
                           target = p.target,
                           topic_id = p.topic_id,
                       }).FirstOrDefault();
            return pro;
        }

        public MV_Pro_Gal_Top GetOutstanding()
        {
            var pro = (from p in ct.Organization_Programs join g in ct?.Galleries
                       on p.id equals g.program_id
                       join t in ct.Topics
                       on p.topic_id equals t.id
                       where p.outstanding == true && p.status == 1
                       select new MV_Pro_Gal_Top
                       {
                           id_program = p.id,
                           id_gallery = g.id,
                           id_topic = t.id,
                           topic_name = t.topic_name,
                           about_detail = p.about_detail,
                           current = p.current,
                           created_at = p.created_at,
                           image_name = g.image_name,
                           outstanding = p.outstanding,
                           o_program_date = p.o_program_date,
                           o_program_name = p.o_program_name,
                           target = p.target,
                           topic_id = p.topic_id,
                       }).FirstOrDefault();
            return pro;
        }

    }
}
