using GiveAID.Models;

namespace GiveAID.Dao
{
    public class Dao_Payment
    {
        private static Dao_Payment? instance = null;
        private static AIDContext? ct;
        private Dao_Payment()
        {
            ct = new AIDContext();
        }
        internal static Dao_Payment Instance
        {
            get
            {
                instance ??= new Dao_Payment();
                return instance;
            }
        }

        public List<Payment> GetPaymentWithIdProgram(int id)
        {
            List<Payment> list = (from p in ct?.Payment
                                  where p.program_id == id
                                  select p).ToList();
            return list;
        }
    }
}
