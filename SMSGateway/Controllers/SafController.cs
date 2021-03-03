using SMSGateway.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using SMSGateway.Hubs;

namespace SMSGateway.Controllers
{
    public class SafController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
        DropdownUserLevel userLevel = new DropdownUserLevel();
        // GET: Saf
        public ActionResult _UserList()
        {
            var model = db.Saf.ToList();
            foreach (var item in model)
            {
                item.FullName = db.Clients.Where(x => x.ClientCode == item.Username).Select(x => x.Name).SingleOrDefault();
            }
            ViewBag.Session = (string)Session["level"];
            return PartialView(model);
        }

        public ActionResult _AddUser()
        {
            ViewBag.Level = new SelectList(userLevel.dpUserlevel.Select(x => new { Value = x.Level, Text = x.Desc }), "Value", "Text");
            var model = new Saf()
            {
                _Status = true
            };
            return PartialView(model);
        }

        public ActionResult _EditUser(string username)
        {
            var model = db.Saf.Where(x => x.Username == username).SingleOrDefault();
            ViewBag.Level = new SelectList(userLevel.dpUserlevel.Select(x => new { Value = x.Level, Text = x.Desc }), "Value", "Text", model.Level);
            return PartialView(model);
        }

        public ActionResult _ChangePassword()
        {
            return PartialView();
        }

        public ActionResult SendEmail(string email, string username, string password)
        {
            try
            {
                string sender = "noreply@mbnepal.com";
                string key = "ubH8U5rNb4";
                string alias = "MicroBanker Nepal Pvt. Ltd.";

                SmtpClient smtp = new SmtpClient("smtpout.asia.secureserver.net");
                smtp.EnableSsl = false;
                smtp.Port = 80;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(sender, key);
                MailMessage message = new MailMessage();
                message.Sender = new MailAddress(sender, alias);
                message.From = new MailAddress(sender, alias);
                message.To.Add(new MailAddress(email, ""));

                message.Subject = "User created";
                message.Body = "Welcome to MicroBanker SMS Gateway Client Portal. Your credentials are: <br /> Username: <b>" + username + "</b> <br /> Password: <b>" + password + "</b><br /> Click <a href=" + "http://sms.microbankernepal.com.np:81/" + ">here</a> to login to our client portal.";
                message.IsBodyHtml = true;
                smtp.Send(message);


                return Json(new { message = "<text class='text-success'>Mail has been sent successfully to " + email + "</text>" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = "<text class='text-danger'>" + ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message + "</text>" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddUser(Saf saf)
        {
            saf.Status = saf._Status == true ? 1 : 0;
            db.Saf.Add(saf);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json("<text class='text-danger'>" + ex.Message + "</text>", JsonRequestBehavior.AllowGet);
            }
            ViewBag.Email = saf.EmailId;
            ViewBag.Message = "User has been created successfully!";
            return PartialView(saf);
        }

        [HttpPost]
        public ActionResult EditUser(Saf saf)
        {
            if (saf._Status == true)
            {
                saf.Status = 1;
            }
            else
            {
                saf.Status = 0;
            }
            db.Entry(saf).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json("<text class='text-danger'>" + ex.Message == "" ? ex.Message : ex.InnerException.Message + "</text>", JsonRequestBehavior.AllowGet);
            }
            return Json("<text class='text-success'>User has been edited!</text>", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangePassword(Password model)
        {
            var username = (string)Session["username"];
            var clientCode = System.Configuration.ConfigurationManager.AppSettings["ClientCode"].ToString();
            Saf row = new Saf();
            if (clientCode == "")
            {
                row = db.Saf.AsNoTracking().Where(x => x.Username == username || x.EmailId == username).SingleOrDefault();
            }
            else
            {
                var email = db.Clients.AsNoTracking().Where(x => x.ClientCode == clientCode).Select(x => x.EmailId).SingleOrDefault();
                row = db.Saf.AsNoTracking().Where(x => x.EmailId == email).SingleOrDefault();
            }
            if (row.Password != model.CurrentPassword)
            {
                return Json(new {status = false, message = "<text class='text-danger'>Your current password is incorrect. Please check your password once again.</text>" }, JsonRequestBehavior.AllowGet);
            }
            row.Password = model.ConfirmPassword;
            db.Entry(row).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new {status = false, message = "<text class='text-danger'>" + ex.Message + "</text>" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new {status = true, message = "<text class='text-success'>Your password has been changed. Try re-logging in to your account with your new password.</text>" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddApprovedUser(string clientID)
        {
            var model = db.Saf.Where(x => x.Username == clientID).SingleOrDefault();
            ViewBag.Email = model.EmailId;
            ViewBag.CreatorId = db.Clients.Where(x => x.ClientCode == clientID).Select(x => x.CreatedBy).SingleOrDefault();
            return PartialView(model);
        }

        public string SendMail(string recipient, string subject, string body)
        {
            try
            {
                
                string sender = "noreply@mbnepal.com";
                string password = "ubH8U5rNb4";
                string alias = "MicroBanker Nepal Pvt. Ltd.";

                SmtpClient smtp = new SmtpClient("smtpout.secureserver.net");
                smtp.EnableSsl = false;
                smtp.Port = 80;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(sender, password);
                MailMessage message = new MailMessage();
                message.Sender = new MailAddress(sender, alias);
                message.From = new MailAddress(sender, alias);
                message.To.Add(new MailAddress(recipient, ""));

               

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                smtp.Send(message);



                return "<text class='text-success'>Mail has been sent successfully to " + recipient +"</text>";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
            }
        }

        [ValidateInput(false)]
        public ActionResult ApproveUser(string recipient, string subject, string body, string isChecked)
        {
            var model = db.Clients.Where(x => x.EmailId == recipient).SingleOrDefault();
            model.Status = 1;
            db.Entry(model).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
                if (isChecked == "true")
                {
                    SendMail(recipient, subject, body);
                }

                UpdateApprovalCount hub = new UpdateApprovalCount();
                hub.Update(db.Clients.Where(x => x.Status == 2).Count());

            }
            catch(Exception ex)
            {
                return Json(new { status = false, message = "<text class='text-danger'>" + ex.Message + "</text>" }, JsonRequestBehavior.AllowGet);
            }
            return Json("<text class='text-success'>User has been approved!</text>", JsonRequestBehavior.AllowGet);
        }
    }
}