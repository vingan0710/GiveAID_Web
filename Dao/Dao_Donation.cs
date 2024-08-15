using GiveAID.Models;

namespace GiveAID.Dao
{
    public class Dao_Donation
    {
        private static Dao_Donation? instance = null;
        private static AIDContext? ct;

        private Dao_Donation()
        {
            ct = new();
        }

        public static Dao_Donation Instance()
        {
            instance ??= new Dao_Donation();
            return instance;
        }

        public void InsertDonate(int money, DateTime donation_date, string id_pay, int member_id, int program_id)
        {
            Donation donation = new Donation { 
                money = money,
                donation_date = donation_date,
                id_pay = id_pay,
                member_id = member_id,
                program_id = program_id 
            };
            ct.Donations?.Add(donation);
            ct.SaveChanges();
        }
    }
}
