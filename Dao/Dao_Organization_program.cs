using GiveAID.Models;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Dao
{
    public class Dao_Organization_program
    {
        private static Dao_Organization_program? instance = null;
        private static AIDContext? ct;

        private Dao_Organization_program()
        {
            ct = new();
        }

        public static Dao_Organization_program Instance()
        {
            instance ??= new Dao_Organization_program();
            return instance;
        }

        public List<Organization_program> GetPros()
        {
            var pro = (from p in ct?.Organization_Programs
                       where p.status == 1
            select new Organization_program
            {
                id = p.id,
                o_program_name = p.o_program_name,
                o_program_date = p.o_program_date,
                about_detail = p.about_detail,
                target = p.target,
                current = p.current,
                topic_id = p.topic_id,
                created_at = p.created_at,
            }).ToList();
            return pro;
        }

        public List<Organization_program> GetProsWithId(int id)
        {
            var pro = (from p in ct?.Organization_Programs
                       where p.status == 1 && p.id == id
                       select new Organization_program
                       {
                           id = p.id,
                           o_program_name = p.o_program_name,
                           o_program_date = p.o_program_date,
                           about_detail = p.about_detail,
                           target = p.target,
                           current = p.current,
                           topic_id = p.topic_id,
                           created_at = p.created_at,
                       }).ToList();
            return pro;
        }

        public List<Organization_program> GetProsWithTopicId(int id)
        {
            var pro = (from p in ct?.Organization_Programs
                       where p.status == 1 && p.topic_id == id
                       select new Organization_program
                       {
                           id = p.id,
                           o_program_name = p.o_program_name,
                           o_program_date = p.o_program_date,
                           about_detail = p.about_detail,
                           target = p.target,
                           current = p.current,
                           topic_id = p.topic_id,
                           created_at = p.created_at,
                       }).ToList();
            return pro;
        }

        public List<Organization_program> GetProsInProgress()
        {
            var pro = (from p in ct?.Organization_Programs
                       where p.status == 1 && (p.target - p.current > 0)
                       select new Organization_program
                       {
                           id = p.id,
                           o_program_name = p.o_program_name,
                           o_program_date = p.o_program_date,
                           about_detail = p.about_detail,
                           target = p.target,
                           current = p.current,
                           topic_id = p.topic_id,
                           created_at = p.created_at,
                       }).ToList();
            return pro;
        }

        public Organization_program GetOutstanding()
        {
            var pro = (from p in ct.Organization_Programs
                       where p.outstanding == true && p.status == 1
                       select new Organization_program
                       {
                           id = p.id,
                           o_program_name = p.o_program_name,
                           o_program_date = p.o_program_date,
                           about_detail = p.about_detail,
                           target = p.target,
                           current = p.current,
                           topic_id = p.topic_id,
                           outstanding = p.outstanding,
                           created_at = p.created_at,
                       }).FirstOrDefault();
            return pro;
        }

        public void UpdateProgram(int id, int amount)
        {
            Organization_program pro = (from p in ct?.Organization_Programs
                                        where p.id == id
                                        select p).FirstOrDefault();
            pro.current += amount;
            ct.SaveChanges();
        }
    }
}
