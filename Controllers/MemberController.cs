using GiveAID.Dao;
using GiveAID.Models;
using GiveAID.Models.RazorPay;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Newtonsoft.Json;
using Razorpay.Api;
using System.Diagnostics.Metrics;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
using Payment = Razorpay.Api.Payment;
using GiveAID.ModelView;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using GiveAID.Areas.Admin.Dao;

namespace GiveAID.Controllers
{
    public class MemberController : Controller
    {
        private readonly AIDContext ct = new();

        public IActionResult Login(int id)
        {
            if (Request.Cookies["member"] != null)
            {
                return RedirectToAction("Index");
            }
            TempData["DetailProgramId"] = id;
            return View();
        }

        [HttpPost]
        public IActionResult CheckLogin(Member mem)
        {
            var id = Request.Form["DetailProgramId"];
            int DetailProgramId = 0;
			if (id.Count != 0){
				DetailProgramId = int.Parse(Request.Form["DetailProgramId"].ToString());
			}
			Member member = Dao_Member.Instance().Check_Uname_Pass(mem.mem_username ?? "", mem.mem_password ?? "");
            if (member != null)
            {
                string m = JsonConvert.SerializeObject(member);
                Response.Cookies.Append("member", m, new CookieOptions { Expires = DateTimeOffset.MaxValue });
                if (DetailProgramId == 0){
					return RedirectToAction("Index");
                }else{
                    return RedirectToAction("Program", new {id = DetailProgramId });
				}
            }
            else
            {
                TempData["Message"] = "Username Or Password Is Incorrect";
                return RedirectToAction("Login");
            }
        }

        public IActionResult Register()     
        {
            return View();
        }

		[HttpPost]
		public IActionResult CheckRegister(Member mem)
		{
			bool check = Dao_Member.Instance().Check_Uname(mem.mem_username ?? "");
			if (check == false)
			{
                Member member = Dao_Member.Instance().InsertMember(mem.mem_username, mem.mem_password, mem.mem_name, mem.email, mem.phone_number);
                Response.Cookies.Delete("member");
                string m = JsonConvert.SerializeObject(member);
                Response.Cookies.Append("member", m, new CookieOptions { Expires = DateTimeOffset.MaxValue });
                return RedirectToAction("Index");
			}
			else
			{
				TempData["ErrorRegister"] = "Username Is Invalid";
				return RedirectToAction("Register");
			}
		}

		[HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("member");
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            TempData["Outstanding"] = Dao_Pro_Gal_Top.Instance().GetOutstanding();
            TempData["Programs"] = Dao_Pro_Gal_Top.Instance().Get_Program_Galleries();

            List<MV_Topic> list = new();
            var topics = Dao_Topic.Instance().GetTopics();
            foreach (MV_Topic item in topics)
            {
                var ob = Dao_Pro_Gal_Top.Instance().Get_Program_Galleries__IdTopic(item.id);
                var json = JsonConvert.SerializeObject(ob);
                item.pro_gal = json;
                list.Add(item);
            }
            TempData["Result"] = JsonConvert.SerializeObject(list);
            return View();
        }

        public IActionResult TopicDetail(int id)
        {
            TempData["Programs"] = Dao_Pro_Gal_Top.Instance().Get_Program_GalleriesWithTopicId(id);
            return View();
        }

        public IActionResult Program(int id)
        {
            TempData["Topic"] = Dao_Topic.Instance().GetTopics();
            TempData["DetailProgram"] = Dao_Pro_Gal_Top.Instance().Get_Program_Galleries__IdProgram(id);
            try
            {
                TempData["Member"] = JsonConvert.DeserializeObject<Member>(Request?.Cookies["member"]);
            }
            catch (Exception)
            {
                TempData["Member"] = new Member();
            }
            return View();
        }

        [HttpPost]
        public IActionResult PreviousPost(MV_Pro_Gal_Top pro)
        {
            int ProgramDetailId = 0;
            List<MV_Pro_Gal_Top> list = Dao_Pro_Gal_Top.Instance().Get_Program_GalleriesHaveOuts();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    if (list[i].id_program == pro.id_program)
                    {
                        ProgramDetailId = list[i].id_program;
                        break;
                    }
                }
                if (list[i].id_program == pro.id_program)
                {
                    ProgramDetailId = list[i - 1].id_program;
                    break;
                }
            }
            return RedirectToAction("Program", new {id = ProgramDetailId});
        }

        [HttpPost]
        public IActionResult NextPost(MV_Pro_Gal_Top pro)
        {
            int ProgramDetailId = 0;
            List<MV_Pro_Gal_Top> list = Dao_Pro_Gal_Top.Instance().Get_Program_GalleriesHaveOuts();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (i == list.Count - 1)
                {
                    if (list[i].id_program == pro.id_program)
                    {
                        ProgramDetailId = list[i].id_program;
                        break;
                    }
                }
                if (list[i].id_program == pro.id_program)
                {
                    ProgramDetailId = list[i + 1].id_program;
                    break;
                }
            }
            return RedirectToAction("Program", new { id = ProgramDetailId });
        }
       
        public IActionResult Donate(int id)
        {
            if (Request.Cookies["member"] == null)
            {
                return RedirectToAction("Login");
            }
            Member member = JsonConvert.DeserializeObject<Member>(Request?.Cookies["member"]);
            TempData["Pro"] = new List<Organization_program>(Dao_Organization_program.Instance().GetProsInProgress());
            TempData["Member"] = member;
            TempData["IdChoosePro"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult CreateOrder(PaymentRequest _requestData)
        {
            // Generate random receipt number for order
            Random randomObj = new();
            string transactionId = randomObj.Next(10000000, 100000000).ToString();
            RazorpayClient client = new("rzp_test_unAtgRERC0DP5v", "KGPEzBN6fuQfVvaMkh8oJVwh");
            Dictionary<string, object> options = new()
            {
                { "amount", _requestData.Amount * 100 },  // Amount will in paise
                { "receipt", transactionId },
                { "currency", "INR" },
                { "payment_capture", "0" } // 1 - automatic  , 0 - manual
            };
            //options.Add("notes", "-- You can put any notes here --");
            Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();
            // Create order model for return on view
            RazorPayOrder orderModel = new()
            {
                OrderId = orderResponse.Attributes["id"],
                RazorPayAPIKey = "rzp_test_unAtgRERC0DP5v",
                Amount = _requestData.Amount * 100,
                Currency = "INR",
                Name = _requestData.Name,
                Email = _requestData.Email,
                mem_id = _requestData.mem_id,
                pro_id = _requestData.pro_id
            };
            Member member = JsonConvert.DeserializeObject<Member>(Request?.Cookies["member"]);
            TempData["Member"] = member;
            TempData["Pro"] = new List<Organization_program>(Dao_Organization_program.Instance().GetProsInProgress());
            // Return on PaymentPage with Order data
            return View("CreateOrder", orderModel);
        }

        [HttpPost]
        public ActionResult Complete()
        {
            // Payment data comes in url so we have to get it from url
            // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
            string paymentId = HttpContext.Request.Form["rzp_paymentid"].ToString();
            // This is orderId
            string orderId = HttpContext.Request.Form["rzp_orderid"].ToString();
            RazorpayClient client = new RazorpayClient("rzp_test_unAtgRERC0DP5v", "KGPEzBN6fuQfVvaMkh8oJVwh");
            Payment payment = client.Payment.Fetch(paymentId);
            // This code is for capture the payment 
            Dictionary<string, object> options = new();
            options.Add("amount", payment.Attributes["amount"]);
            Payment paymentCaptured = payment.Capture(options); 

            int amt = paymentCaptured.Attributes["amount"];// amt/100 
            string pay_id = paymentCaptured.Attributes["id"];
            int cr = paymentCaptured.Attributes["created_at"];
            int mem_id = int.Parse(Request.Form["mem_id"][0].ToString());
            int pro_id = int.Parse(Request.Form["pro_id"][0].ToString());
            DateTime donation_date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(cr).ToLocalTime();
            //// Check payment made successfully
            if (paymentCaptured.Attributes["status"] == "captured")
            {
                // Create these action method
                Dao_Donation.Instance().InsertDonate(amt/100, donation_date, pay_id, mem_id, pro_id);
                Dao_Organization_program.Instance().UpdateProgram(pro_id, amt/100);
                TempData["Message"] = "Paid Successfully";
                return RedirectToAction("Donate");
            }
            else
            {
                TempData["Message"] = "Payment failed, something went wrong";
                return RedirectToAction("Donate");
            }
        }

        [HttpGet]
        public IActionResult Contact()
        {
            if (Request.Cookies["member"] == null)
            {
                return RedirectToAction("Login");
            }
            TempData["Member"] = JsonConvert.DeserializeObject<Member>(Request?.Cookies["member"]);
            return View();
        }

        [HttpPost]
        public IActionResult Contact(int program_id)
        {
            Member member = JsonConvert.DeserializeObject<Member>(Request?.Cookies["member"]);
            string? fullname = Request.Form["fullname"];
            string? phone_number = Request.Form["phone_number"];
            string? email = Request.Form["email"];
            string? body = Request.Form["body"];
            Contact contact = new()
            {
                fullname = fullname,
                phone_number = phone_number,
                email = email,
                body = body,
                mem_id = (int)(member?.mem_id),
                program_id = program_id,
                status = 0
            };

            Dao_Contact.Instance.SendContact(contact);

            //Delay(Dao_Contact.Instance.SendContact(contact));

            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("GiveAID", "ntnhan0812a@gmail.com"));
            mailMessage.To.Add(new MailboxAddress(contact.fullname, contact.email));

            mailMessage.Subject = "Thank you for sending your information!";
            mailMessage.Body = new TextPart("plain")
            {
                Text = "Dear" + "," + " " + contact.fullname + "\r\n" +
                "Thank you for being so interested in our website\r\n " +
                "Our team will contact you soon"
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, false);
                smtpClient.Authenticate("ntnhan0812a@gmail.com", "bttcvsqkzoamxobb");
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
            TempData["ContactMessage"] = "Sending Contact Is Success";
            if (program_id != 0){
                return RedirectToAction("Program",  new{ id = program_id });
            }else{
                return RedirectToAction("Contact");
            }
        }

        public IActionResult Profile()
        {
            if (Request.Cookies["member"] == null)
            {
                return RedirectToAction("Login");
            }
            Member member = JsonConvert.DeserializeObject<Member>(Request?.Cookies["member"]);
            TempData["Member"] = member;
            TempData["Topic"] = Dao_Topic.Instance().GetTopics();
            TempData["Donate"] = Dao_Donate_Pro_Topic.Instance.GetDonate_Pro_Topics(member.mem_id);
            return View();
        }

        public string DetailPayment(int id)
        {
            int i = 1;
            string result = "";
            List<Models.Payment> payments = Dao_Payment.Instance.GetPaymentWithIdProgram(id);
            if (payments.Count == 0){
                return "<tr></tr>";
            }else {
                foreach (Models.Payment payment in payments)
                {
                    result += "<tr>" +
                                "<th scope='row'>" + i++ + "</th>" +
                                "<td>" + payment.payout + "</td>" +
                                "<td>" + payment.description + "</td>" +
                                "<td>" + payment.created_at.ToString("dd MMMM, yyyy") + "</td>" +
                              "</tr>";
                }
            }
            return result;
        }

        [HttpPost]
        public IActionResult UpdateProfile(Member mem)
        {
            Member member = Dao_Member.Instance().EditProfile(mem);
            string m = JsonConvert.SerializeObject(member);
            TempData["UpdateProfileMessage"] = "Update Profile Successfully";
            Response.Cookies.Append("member", m, new CookieOptions { Expires = DateTimeOffset.MaxValue });
            return RedirectToAction("Profile");
        }
        
        [HttpPost]
        public IActionResult UpdatePassword()
        {
            string? mem_password = Request.Form["password"];
            int mem_id = int.Parse(Request.Form["mem_id"].ToString());

            Dao_Member.Instance().UpdateMemberPassword(mem_id, mem_password);
            return RedirectToAction("Profile");
        }

        private async Task Delay(Contact contact)
        {
            if (DateTime.Now.Hour - contact.created_at.Hour < 24)
            {
                await Task.Delay(86400000);
            }
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Partner()
        {
            return View(PartnerDao.Instance.Partners);
        }
    }
}
