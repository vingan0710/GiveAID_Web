using GiveAID.Models;
using GiveAID.ModelView;

namespace GiveAID.Dao
{
    public class Dao_Topic
    {
        private static Dao_Topic? instance = null;
        private static AIDContext? ct;

        private Dao_Topic()
        {
            ct = new();
        }

        public static Dao_Topic Instance()
        {
            instance ??= new Dao_Topic();
            return instance;
        }

        public List<MV_Topic> GetTopics()
        {
            var topics = (from t in ct.Topics select new MV_Topic
            {
                id = t.id,
                topic_name = t.topic_name,
                created_at = t.created_at,
            }).ToList();
            return topics;
        }
    }
}
