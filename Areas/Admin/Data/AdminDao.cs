#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
using GiveAID.Models;
using GiveAID.Models.ModelView;

namespace GiveAID.Areas.Admin.Data
{
    internal class AdminDao
    {
        private static AdminDao? instance = null;
        public AdminDao()
        {

        }
        internal static AdminDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdminDao();
                }
                return instance;
            }
        }
        #region TOPIC
        public void InsertTopic(string? topic_name)
        {
            Topic tp = new Topic();
            tp.topic_name = topic_name;

            AIDContext aid = new AIDContext();
            aid.Topics.Add(tp);
            aid.SaveChanges();
        }
        public List<Topic> TopicViews
        {
            get
            {
                var aid = new AIDContext();
                var result = (from tp in aid.Topics
                              select new Topic()
                              {
                                  id = tp.id,
                                  topic_name = tp.topic_name,
                              }).ToList();
                return result;

            }
        }
        public Topic FindID(int id)
        {
            AIDContext aid = new AIDContext();
            Topic tp = aid.Topics.Find(id);
            return tp;
        }
        public void EditTopic(int id, string? topic_name)
        {
            AIDContext aid = new AIDContext();
            Topic tp = aid.Topics.Find(id);
            tp.topic_name = topic_name;

            aid.SaveChanges();
        }
        #endregion

        #region PROGRAM
        public void InsertProgram(int topic_id, string? name, string? desc, DateTime date, int target, bool type)
        {
            AIDContext aid = new AIDContext();
            Organization_program og = new Organization_program();
            og.topic_id = topic_id;
            og.o_program_name = name;
            og.about_detail = desc;
            og.o_program_date = date;
            og.target = target;
            og.current = 0;
            og.outstanding = type;
            if (og.o_program_date > DateTime.Now)
            {
                og.status = 0;
            }
            else if (og.o_program_date < DateTime.Now)
            {
                og.status = 1;
            }
            if (og.outstanding == true)
            {
                var q = aid.Organization_Programs.Where(x => x.outstanding == true).FirstOrDefault();
                if (q != null)
                {
                    q.outstanding = false;
                    aid.SaveChanges();
                }
            }

            aid.Organization_Programs.Add(og);
            aid.SaveChanges();
        }

        public List<ProgramView> ProgramViews
        {
            get
            {
                var aid = new AIDContext();
                var result = (from og in aid.Organization_Programs
                              join tp in aid.Topics on og.topic_id equals tp.id
                              select new ProgramView()
                              {
                                  id = og.id,
                                  topic_id = og.topic_id,
                                  topic_name = tp.topic_name,
                                  o_program_name = og.o_program_name,
                                  about_detail = og.about_detail,
                                  o_program_date = og.o_program_date,
                                  target = og.target,
                                  current = og.current,
                                  status = og.status,
                                  outstanding = og.outstanding,
                              }).OrderByDescending(x=>x.id).ToList();
                return result;
            }
        }
        public Organization_program CheckID(int id)
        {
            AIDContext aid = new AIDContext();
            Organization_program og = aid.Organization_Programs.Find(id);
            return og;
        }
        public void EditProgram(int id, int topic_id, string? name, string? desc, DateTime date, int target, bool outstanding)
        {
            AIDContext aid = new AIDContext();
            Organization_program og = aid.Organization_Programs.Find(id);
            og.topic_id = topic_id;
            og.o_program_name = name;
            og.about_detail = desc;
            og.o_program_date = date;
            og.target = target;
            og.outstanding = outstanding;
            if (og.o_program_date > DateTime.Now)
            {
                og.status = 0;
            }
            else if (og.o_program_date < DateTime.Now)
            {
                og.status = 1;
            }
            if (og.outstanding == true)
            {
                var q = aid.Organization_Programs.Where(x => x.outstanding == true).FirstOrDefault();
                if (q != null)
                {
                    q.outstanding = false;
                    aid.SaveChanges();
                }

            }
            aid.SaveChanges();
        }
        public void CheckCurrentandTarget()
        {
            AIDContext aIDContext = new AIDContext();
            foreach (var item in ProgramViews)
            {
                if (item.target == item.current)
                {
                    var q = aIDContext.Organization_Programs.Where(x => x.id == item.id).FirstOrDefault();
                    q.status = 2;
                    aIDContext.SaveChanges();
                }
                else if (item.o_program_date < DateTime.Now)
                {
                    var q = aIDContext.Organization_Programs.Where(x => x.id == item.id).FirstOrDefault();
                    q.status = 1;
                    aIDContext.SaveChanges();
                }
                else if (item.o_program_date >= DateTime.Now)
                {
                    var q = aIDContext.Organization_Programs.Where(x => x.id == item.id).FirstOrDefault();
                    q.status = 0;
                    aIDContext.SaveChanges();
                }

            }
        }
        //public void UpdateProgram(int id, int amount)
        //{
        //    AIDContext aid = new AIDContext();
        //    Organization_program pro = (from p in aid.Organization_Programs
        //                                where p.id == id
        //                                select p).FirstOrDefault();
        //    pro.current += amount;
        //    aid.SaveChanges();
        //}
        #endregion

        #region PAYMENT
        public void InsertPaymet(int program_id, string? description, int payout)
        {
            Payment pay = new Payment();
            pay.program_id = program_id;
            pay.description = description;
            pay.payout = payout;

            AIDContext aid = new AIDContext();
            aid.Payment.Add(pay);
            aid.SaveChanges();
        }
        public List<Payment> PaymentView(int id)
        {
            var aid = new AIDContext();
            var result = (from pay in aid.Payment
                          where pay.program_id == id
                          select new Payment()
                          {
                              id = pay.id,
                              program_id = pay.program_id,
                              description = pay.description,
                              payout = pay.payout,
                              created_at = pay.created_at,

                          }).ToList();
            return result;
        }
        #endregion

        #region GALLERY
        public List<GalleryView> GalleryView(int id)
        {
            var aid = new AIDContext();
            var result = (from ga in aid.Galleries
                          join og in aid.Organization_Programs on ga.program_id equals og.id
                          where ga.program_id == id
                          select new GalleryView()
                          {
                              id = ga.id,
                              program_id = ga.program_id,
                              o_program_name = og.o_program_name,
                              image_name = ga.image_name,
                              created_at = ga.created_at,

                          }).OrderByDescending(x=>x.id).ToList();
            return result;
        }

        public void InsertGallery(int program_id, string? image_name)
        {
            Gallery gal = new Gallery();
            gal.program_id = program_id;
            gal.image_name = image_name;

            AIDContext aid = new AIDContext();
            aid.Galleries.Add(gal);
            aid.SaveChanges();
        }
        public Gallery IDGall(int id)
        {
            AIDContext aid = new AIDContext();
            Gallery gal = aid.Galleries.Find(id);
            return gal;
        }
        public void DeleteGallery(int id)
        {
            AIDContext aid = new AIDContext();
            Gallery gal = aid.Galleries.Find(id);
            aid.Galleries.Remove(gal);
            aid.SaveChanges();
        }
        #endregion

        #region CONTACT
        public int CountContact()
        {
            var aid = new AIDContext();
            var result = (from con in aid.Contacts
                          where con.status == 0
                          select con
                         ).Count();
            return result;
        }

        public List<Contact> ContactViews()
        {
            var aid = new AIDContext();
            var result = (from con in aid.Contacts
                          where con.status == 0
                          select con).OrderByDescending(x=>x.id).ToList();
            return result;
        }

        public List<ContactView> ReplyContact(int id)
        {
            var aid = new AIDContext();
            var result = (from con in aid.Contacts
                          join mem in aid.Members on con.mem_id equals mem.mem_id
                          where con.id == id
                          select new ContactView()
                          {
                              id = con.id,
                              mem_id = con.mem_id,
                              body = con.body,
                              email = mem.email,
                              fullname = mem.mem_name,
                              phone_number = mem.phone_number,
                              status = con.status,
                              created_at = con.created_at,

                          }).ToList();
            return result;
        }
        public List<ContactView> ListContact
        {
            get
            {
                var aid = new AIDContext();
                var result = (from con in aid.Contacts
                              join mem in aid.Members on con.mem_id equals mem.mem_id
                              select new ContactView()
                              {
                                  id = con.id,
                                  mem_id = con.mem_id,
                                  body = con.body,
                                  fullname = mem.mem_name,
                                  email = mem.email,
                                  phone_number = mem.phone_number,
                                  status = con.status,
                                  created_at = con.created_at,

                              }).ToList();
                return result;
            }
        }
        public void CheckStatusContact(int idContact)
        {
            AIDContext aid = new AIDContext();
            var q = aid.Contacts.Where(d => d.id == idContact).FirstOrDefault();
            q.status = 1;
            aid.SaveChanges();
        }
        #endregion

        #region DONATION
        public List<DonationView> Get_DonateTable
        {
            get
            {
                AIDContext aid = new AIDContext();
                var pro = (from d in aid.Donations
                           join m in aid.Members
                           on d.member_id equals m.mem_id
                           join op in aid.Organization_Programs
                           on d.program_id equals op.id
                           select new DonationView
                           {
                               id = d.id,
                               money = d.money,
                               donation_date = d.donation_date,
                               mem_name = m.mem_name,
                               program_name = op.o_program_name
                           }).ToList();
                return pro;
            }
        }
        #endregion

        #region MEMBER
        public List<Member> Members
        {
            get
            {
                AIDContext f = new AIDContext();
                var result = f.Members.Select(x => new Member()
                {
                    mem_id = x.mem_id,
                    mem_username = x.mem_username,
                    mem_password = x.mem_password,
                    mem_name = x.mem_name,
                    email = x.email,
                    phone_number = x.phone_number,
                    created_at = x.created_at,
                    status = x.status,
                }).ToList();
                return result;
            }
        }
        public Member IDMem(int id)
        {
            AIDContext aid = new AIDContext();
            Member mem = aid.Members.Find(id);
            return mem;
        }
        public void DisactiveMember(int id, int status)
        {
            AIDContext aid = new AIDContext();
            Member mem = aid.Members.Find(id);
            mem.status = 0;
            aid.SaveChanges();
        }
        public void ActiveMember(int id, int status)
        {
            AIDContext aid = new AIDContext();
            Member mem = aid.Members.Find(id);
            mem.status = 1;
            aid.SaveChanges();
        }
        #endregion

        #region PARTNERSHIP
        public void InsertPartner(string part_name, string description, string logo)
        {
            AIDContext ct = new AIDContext();
            Partnership part = new Partnership();
            part.partner_name = part_name;
            part.logo = logo;
            part.description = description;
            ct.Partnerships.Add(part);
            ct.SaveChanges();

        }
        public List<Partnership> Partners
        {
            get
            {
                AIDContext f = new AIDContext();
                var result = f.Partnerships.Select(x => new Partnership()
                {
                    id = x.id,
                    partner_name = x.partner_name,
                    description = x.description,
                    logo = x.logo
                }).ToList();
                return result;
            }
        }
        public Partnership EditPartner(int ID)
        {
            AIDContext f = new AIDContext();
            Partnership st = f.Partnerships.Find(ID);
            return st;
        }
        public void UpdatePartner(int ID, string part_name, string Description, string logo)
        {
            AIDContext f = new AIDContext();
            Partnership part = f.Partnerships.Find(ID);
            part.partner_name = part_name;
            part.logo = logo;
            part.description = Description;
            f.SaveChanges();
        }
        public void DeletePartner(int ID)
        {
            AIDContext f = new AIDContext();
            Partnership part = f.Partnerships.Find(ID);
            f.Partnerships.Remove(part);
            f.SaveChanges();
        }
        #endregion

    }
}

