using GiveAID.Models;
using GiveAID.ModelView;

namespace GiveAID.Dao
{
    public class Dao_Donate_Pro_Topic
    {
        private static Dao_Donate_Pro_Topic? instance = null;
        private static AIDContext? ct;

        private Dao_Donate_Pro_Topic()
        {
            ct = new();
        }
        internal static Dao_Donate_Pro_Topic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Dao_Donate_Pro_Topic();
                }
                return instance;
            }
        }

        public List<MV_Donate_Pro_Topic> GetDonate_Pro_Topics(int id)
        {
            List<MV_Donate_Pro_Topic> dpt = (from d in ct?.Donations
                                             join p in ct?.Organization_Programs
                                             on d.program_id equals p.id
                                             join t in ct?.Topics
                                             on p.topic_id equals t.id
                                             where d.member_id == id
                                             orderby d.id descending
                                             select new MV_Donate_Pro_Topic
                                             {
                                                 program_id = p.id,
                                                 o_program_name = p.o_program_name,
                                                 topic_name = t.topic_name,
                                                 money = d.money,
                                                 current = p.current,
                                                 target = p.target,
                                                 donation_date = d.donation_date,
                                             }).ToList();
            return dpt;
        }
    }
}
