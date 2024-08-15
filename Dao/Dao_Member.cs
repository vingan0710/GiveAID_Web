using GiveAID.Models;

namespace GiveAID.Dao
{
	public class Dao_Member
	{
		private static Dao_Member? instance = null;
		private static AIDContext? ct;

		private Dao_Member() 
		{
			ct = new();
		}

		public static Dao_Member Instance()
		{
			instance ??= new Dao_Member();
			return instance;
		}

		public Member? Check_Uname_Pass(string name, string pass) 
		{
			var member = (from m in ct.Members
						  where m.mem_username == name && m.mem_password == pass
						  select new Member
						  {
							  mem_id = m.mem_id,
							  mem_username = m.mem_username,
							  mem_password = m.mem_password,
							  mem_name = m.mem_name,
							  email = m.email,
							  phone_number = m.phone_number,
							  created_at = m.created_at
						  }).FirstOrDefault();
			return member;
		}

		public bool Check_Uname(string name)
		{
			var member = (from m in ct.Members
						  where m.mem_username == name
						  select new Member
						  {
							  mem_id = m.mem_id,
							  mem_username = m.mem_username,
							  mem_password = m.mem_password,
							  mem_name = m.mem_name,
							  email = m.email,
							  phone_number = m.phone_number,
							  created_at = m.created_at
						  }).FirstOrDefault();
			if (member == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public Member? InsertMember(string uname, string pass, string name, string email, string phone)
		{
			Member member = new Member
			{
				mem_username = uname,
				mem_password = pass,
				mem_name = name,
				email = email,
				phone_number = phone,
				status = 1
			};
			ct.Add(member);
			ct.SaveChanges();
			return member;
		}

        public Member EditProfile(Member mem)
        {
			Member? member = (from m in ct.Members
							 where m.mem_id == mem.mem_id
							 select m).FirstOrDefault();
			member.mem_name = mem.mem_name;
			member.email = mem.email;
			member.phone_number = mem.phone_number;
			ct.Update(member);
			ct.SaveChanges();
			return member;
        }

        public void UpdateMemberProfile(int id, string mem_name, string phone_number, string email)
        {
            AIDContext context = new AIDContext();
            Member? member = context.Members.Find(id);
            member.mem_name = mem_name;
            member.phone_number = phone_number;
            member.email = email;
            context.SaveChanges();
        }

        public void UpdateMemberPassword(int id, string mem_password)
        {
			Member? member = (from m in ct.Members
							  where m.mem_id == id
							  select m).FirstOrDefault();
            member.mem_password = mem_password;
            ct.SaveChanges();
        }



        //public List<Donation> checkDonationID(int id)
        //{
        //    var do = (from d in ct?.Donations
        //               where d.member_id == id
        //join p in ct?.Organization_Programs
        //on d.program_id equals id
        //               select new Donation
        //               {
        //                   id = d.id,
        //                   money = d.money,
        //                   donation_date = d.donation_date,
        //                   program_id = d.program_id,
        //                   created_at = d.created_at,
        //               }).ToList();
        //    return do;
        //}
    }
}
