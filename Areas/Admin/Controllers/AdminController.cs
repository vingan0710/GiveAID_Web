#pragma warning disable CS8600
#pragma warning disable CS8604
using GiveAID.Areas.Admin.Data;
using GiveAID.Models;
using GiveAID.Models.ModelView;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Security.Cryptography;

namespace GiveAID.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {

            if (Request.Cookies["LoginCookie"] == null)
            {
                return Redirect("/Admin/Admin/Login");
            }

            return View(AdminDao.Instance.ContactViews());
        }

        #region LOGIN
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    
        //    return RedirectToAction("Index", "Home", new { area = "" });
        //}
        [HttpGet]
        //[Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Login(AdminModel ad)
        {
            string? name = HttpContext.Request.Cookies["Name"];
            string? username = Request.Form["username"];
            string? password = Request.Form["password"];
            AIDContext context = new AIDContext();
            if (ModelState.IsValid)
            {
                var result = context.Admins?.Where(x => x.ad_username == username && x.ad_password == password).FirstOrDefault();
                if (result != null)
                {
                    try
                    {
                        //HttpContext.Session.SetString("LoginSession", "nyan");
                        HttpContext.Response.Cookies.Append("LoginCookie", "Nyan");
                        //EncodePassword(password);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    return Redirect("Index");
                }
                else
                {
                    ViewBag.Message = "Username or password is incorrect, please try again";
                    return Redirect("Login");
                }
            }
            return View(ad);
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("LoginCookie");
            return Redirect("Login");
        }



        #endregion

        #region TOPIC 
        public IActionResult Topic()
        {
            if (Request.Cookies["LoginCookie"] == null)
            {
                return Redirect("Login");
            }
            //ViewBag.tops = AdminDao.Instance.TopicViews;
            return View(AdminDao.Instance.TopicViews);
        }
        public IActionResult _InsertTopic()
        {
            if (Request.Cookies["LoginCookie"] == null)
            {
                return Redirect("Login");
            }
            return PartialView();
        }
        public IActionResult NewTopic()
        {
            string Name = Request.Form["topic_name"];
            AdminDao.Instance.InsertTopic(Name);
            return Redirect("topic");
        }
        public IActionResult ViewEditTopic(int id)
        {
            if (Request.Cookies["LoginCookie"] == null)
            {
                return Redirect("Login");
            }
            ViewBag.topic = AdminDao.Instance.FindID(id);
            return PartialView("EditTopic");
        }
        [HttpPost]
        public IActionResult EditTopic(int id, Topic tp)
        {
            var q = AdminDao.Instance.FindID(id);
            ViewBag.idTop = id;
            AdminDao.Instance.EditTopic(q.id, tp.topic_name);

            return Redirect("Topic");
        }
        #endregion

        #region PROGRAM
        public IActionResult Program()
        {
            AdminDao.Instance.CheckCurrentandTarget();
            return View(AdminDao.Instance.ProgramViews);
        }

        //public PartialViewResult SearchProgram(string name)
        //{
        //    var pro = AdminDao.Instance.ProgramViews;
        //    if (name.Equals("*") == false)
        //    {
        //        pro = pro.Where(x => x.o_program_name.ToLower().Contains(name)).ToList();
        //    }

        //    return PartialView("_ProgramView", pro);

        //}
        public IActionResult InsertProgram()
        {
            return View(AdminDao.Instance.TopicViews);

        }
        public IActionResult NewProgram(int id, int amount)
        {
            int TopicID = Convert.ToInt32(Request.Form["topic_id"]);
            string Name = Request.Form["o_program_name"];
            string Desc = Request.Form["about_detail"];
            DateTime Date = Convert.ToDateTime(Request.Form["o_program_date"]);
            int Target = Convert.ToInt32(Request.Form["target"]);
            bool Type = Request.Form.ContainsKey("type");

            AdminDao.Instance.InsertProgram(TopicID, Name, Desc, Date, Target, Type);
            //AdminDao.Instance.UpdateProgram(id, amount);

            return Redirect("Program");
        }
        public IActionResult EditProgram(int id)
        {

            var q = AdminDao.Instance.CheckID(id);
            ProgramView model = new ProgramView() { id = q.id, topic_id = q.topic_id, o_program_name = q.o_program_name, about_detail = q.about_detail, o_program_date = q.o_program_date, target = q.target, outstanding = q.outstanding };
            var lstopic = AdminDao.Instance.TopicViews;
            model.topics = lstopic;
            return View(model);
        }
        public IActionResult UpdateProgram(ProgramView model)
        {

            if (model != null)
            {
                var type = Request.Form["type"];
                if (type == "on") { model.outstanding = true; } else { model.outstanding = false; }
                AdminDao.Instance.EditProgram(model.id, model.topic_id, model.o_program_name, model.about_detail, model.o_program_date, model.target, model.outstanding);
            }
            return Redirect("Program");
        }
        #endregion

        #region PAYMENT
        public IActionResult InsertPayment(int ProID)
        {
            var q = AdminDao.Instance.CheckID(ProID);
            return PartialView("InsertPayment", ProID);
        }
        [HttpPost]
        public IActionResult NewPayment(Payment pay)
        {
            AdminDao.Instance.InsertPaymet(pay.program_id, pay.description, pay.payout);

            return Redirect("program");
        }
        public IActionResult _PaymentView(int idPro)
        {
            var q = AdminDao.Instance.CheckID(idPro);
            ViewBag.pays = AdminDao.Instance.PaymentView(idPro);
            ViewBag.idPro = idPro;
            return PartialView("_PaymentView");
        }
        #endregion

        #region GALLERY
        public IActionResult Gallery()
        {
            var q = AdminDao.Instance.ProgramViews;
            ViewBag.pros = q;
            return View();
        }
        [HttpPost]
        public IActionResult InsertGallery(IFormFile formfile)
        {
            string Design = Uploads.Instance.UploadImage(formfile);
            AdminDao.Instance.InsertGallery(int.Parse(Request.Form["pid"].ToString()), Design);
            return Json("success");
        }
        public IActionResult DeleteGallery(int id)
        {
            var q = AdminDao.Instance.IDGall(id);
            var deleteImg = Path.Combine(Directory.GetCurrentDirectory(), "C:\\Exercise\\GiveAID\\wwwroot\\Uploads", q.image_name);
            FileInfo fileInfo = new FileInfo(deleteImg);
            fileInfo.Delete();
            AdminDao.Instance.DeleteGallery(id);
            return Redirect("/Admin/Admin/Gallery");

        }

        #endregion

        #region CONTACT
        public IActionResult Contact()
        {
            return View(AdminDao.Instance.ListContact);
        }
        public int CountContact()
        {
            return AdminDao.Instance.CountContact();
        }
        public IActionResult _Contact()
        {
            List<Contact> q = AdminDao.Instance.ContactViews();
            ViewBag.cons = q;
            return PartialView();
        }
        public IActionResult ReplyContact(int id)
        {
            return View(AdminDao.Instance.ReplyContact(id));
        }
        public IActionResult SendMail()
        {
            int idContact = int.Parse(Request.Form["id"]);
            string Email = Request.Form["email"];
            string Reply = Request.Form["reply"];
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Sender Name", "ngan2003.vn@gmail.com"));
            email.To.Add(new MailboxAddress("Receiver Name", Email));

            email.Subject = "Reply Contact";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = Reply
            };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("ngan2003.vn@gmail.com", "bdeydodsvcdkggcq");

                smtp.Send(email);
                smtp.Disconnect(true);

                //goi ham doi trang thai
                //dk:truyen id qua dao
                AdminDao.Instance.CheckStatusContact(idContact);
            }
            return Redirect("Contact");
        }
        #endregion

        #region PARTNER
        public IActionResult MgrPartner()
        {
            return View(AdminDao.Instance.Partners);
        }
        public IActionResult AddPartner()
        {
            return View();
        }
        [HttpPost]
        public IActionResult InsertPartner(Partnership pn)
        {
            //string? part_name = Request.Form["name_part"];
            //string? Description = Request.Form["Description"];
            string logo = Uploads.Instance.UploadImage(pn.myUploads);
            AdminDao.Instance.InsertPartner(pn.partner_name, pn.description, logo);
            return Redirect("MgrPartner");
        }
        public IActionResult EditPartner(int ID)
        {

            return View(AdminDao.Instance.EditPartner(ID));
        }

        public IActionResult UpdatePartner(int ID, Partnership pn)
        {
            string? part_name = Request.Form["name_part"];
            string? Description = Request.Form["Description"];
            string logo = Uploads.Instance.UploadImage(pn.myUploads);
            AdminDao.Instance.UpdatePartner(ID, part_name, Description, logo);
            return Redirect("MgrPartner");
        }
        public IActionResult DeletePartner(int ID)
        {
            var q = AdminDao.Instance.EditPartner(ID);
            var deleteImg = Path.Combine(Directory.GetCurrentDirectory(), "C:\\Exercise\\GiveAID_final\\GiveAID\\wwwroot\\Uploads", q.logo);
            FileInfo fileInfo = new FileInfo(deleteImg);
            fileInfo.Delete();
            AdminDao.Instance.DeletePartner(ID);
            return Redirect("MgrPartner");
        }
        #endregion

        #region DONATION
        public IActionResult TableDonate()
        {
            return View(AdminDao.Instance.Get_DonateTable);
        }
        #endregion

        #region MEMBER
        public IActionResult TableMember()
        {
            return View(AdminDao.Instance.Members);
        }
        public IActionResult DisactiveStatus(int id, Member mem)
        {

            var q = AdminDao.Instance.IDMem(id);
            ViewBag.idMem = id;
            AdminDao.Instance.DisactiveMember(q.mem_id, mem.status);

            return Redirect("/Admin/Admin/TableMember");
        }
        public IActionResult ActiveStatus(int id, Member mem)
        {
            var q = AdminDao.Instance.IDMem(id);
            ViewBag.idMem = id;
            AdminDao.Instance.ActiveMember(q.mem_id, mem.status);

            return Redirect("/Admin/Admin/TableMember");
        }
        #endregion
    }



}

