using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSGateway.Models;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;
using System.Text;
using PagedList;
using System.Data.Entity.Validation;
using SMSGateway.Hubs;
using System.Configuration;

namespace SMSGateway.Controllers
{
    public class ClientController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();

        public ActionResult _Index()
        {
            ViewBag.Session = (string)Session["level"];
            return PartialView();
        }

        public ActionResult _ClientList(string search, int pageNumber=1, int pageSize = 10)
        {
            ViewBag.SN = ((pageNumber - 1) * pageSize) + 1;
            ViewBag.Search = search;
            
            var model = db.vwClients.Where(x => x.ClientCode.CompareTo("0000") >= 0 && x.ClientCode.CompareTo("9999") <= 0).ToList();
           
            if (search != null && search != "")
            {
                model = model.Where(x => x.Name.ToLower().Contains(search.ToLower()) || x.ClientCode == search).ToList();
            }
            ViewBag.Session = (string)Session["level"];
            return PartialView(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult _AddClient()
        {
            ViewBag.creator = (string)Session["Username"];
            return PartialView();
        }

        public ActionResult _PendingApproval()
        {
            var model = db.Clients.Where(x => x.Status == 2).ToList();
            
            return PartialView(model);
        }
        public ActionResult _InfoClient( string id)
        {
            {
                var model = (from m in db.Clients.Where(x => x.ClientCode == id) select m).Single();
                ViewBag.Code = model.ClientCode;
                ViewBag.Name = model.Name;
                ViewBag.Password = model.PassKey;
            }
            return PartialView();
        }
        public ActionResult _EditClient(string id)
        {  
            var model = (from m in db.Clients.Where(x => x.ClientCode == id) select m).Single();
            var saf = db.Saf.Where(x => x.Username == model.ClientCode).SingleOrDefault();
            if (saf != null)
            {
                if (saf.Status == 1)
                {
                    model.isUser = true;
                }
                else
                {
                    model.isUser = false;
                }
                model.Username = saf.Username;
                model.Password = saf.Password;

            }
            else
            {
                model.Username = "";
                model.Password = "";
                model.isUser = false;
            }
            if (model.Status == 1)
            {
                model.isActive = true;
            }
            else
            {
                model.isActive = false;
            }
            return PartialView(model);
        }
        public ActionResult _RefillBalance(string clientCode, string clientName, string balance, string newBalance = "", string refillAmount = "", string remarks = "", string billNo = "")
        {
            ViewBag.Balance = balance;
            ViewBag.ClientCode = clientCode;
            ViewBag.ClientName = clientName;
            ViewBag.NewBalance = newBalance;
            if (ViewBag.NewBalance == "")
            {
                ViewBag.NewBalance = balance;
            }
            ViewBag.RefillAmount = refillAmount;
            ViewBag.Remarks = remarks;
            ViewBag.BillNo = billNo;
            return PartialView();
        }

        public ActionResult _ConfirmRefill(string clientCode, string clientName, string currentBalance, string newBalance, int refillAmount, string remarks, string billno)
        {
            ViewBag.ClientName = clientName;
            ViewBag.CurrentBalance = currentBalance;
            ViewBag.NewBalance = newBalance;
            ViewBag.ClientCode = clientCode;
            ViewBag.RefillAmount = refillAmount;
            ViewBag.Remarks = remarks;
            ViewBag.BillNo = billno;
            return PartialView();
        }

        public ActionResult _ConfirmRemove(string id)
        {
            ViewBag.ClientCode = id;
            return PartialView();
        }

        public string _RemoveClient(string id)
        {
            var row = (from m in db.Clients.Where(x => x.ClientCode == id) select m).Single();
            db.Clients.Remove(row);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.InnerException.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return "<text class=" + "text-success" + ">The selected client has been removed successfully</text>";
        }

        public ActionResult _GetUUID()
        {
            Guid guid = Guid.NewGuid();
            var id = guid.ToString().Replace("-", "");
            return Json(id.Substring(0, 10), JsonRequestBehavior.AllowGet);
        }
        public ActionResult _ClientInfo(string clientCode)
        {
            var model = db.vwClients.Where(x => x.ClientCode == clientCode).Single();            
            ViewBag.Balance = model.Balance.ToString("0");
            if (model.AlertLimit.HasValue)
            {
                ViewBag.AlertLimit = model.AlertLimit.Value.ToString("0");
            }
            ViewBag.Level = (string)Session["level"];
            return PartialView(model);
        }

        public ActionResult _RefillHistory(string clientCode, string errorMsg = "", bool canceled = false)
        {
            ViewBag.Error = errorMsg;
            ViewBag.CanceledRefills = canceled;
            ViewBag.Level = (string)Session["level"];
            
            var model = db.Refills.ToList();
            if (canceled == false)
            {
                model = db.Refills.Where(x => x.ClientCode == clientCode && x.CancelledDate == null).OrderByDescending(x => x.RefillId).ToList();
            }
            else
            {
                model = db.Refills.Where(x => x.ClientCode == clientCode).OrderByDescending(x => x.RefillId).ToList();
            }
            
            decimal totalRefill = 0;
            decimal totalAmount = 0;
            
            foreach (var item in model)
            {
                if (item.CancelledDate == null)
                {
                    if (item.RefillRate.HasValue)
                    {
                        item.Amount = item.RefillRate.Value * item.RefillValue;
                        
                    }

                    totalRefill += item.RefillValue;
                    totalAmount += item.Amount;
                }
                
            }
            ViewBag.TotalRefill = totalRefill.ToString("0");

            ViewBag.TotalAmount = totalAmount.ToString("0");

            
            return PartialView(model);
        }

        public ActionResult _UndoRefill()
        {
            return PartialView();
        }

        public ActionResult UndoRefill(string refillId, string clientCode, bool canceled)
        {
            var errorMessage = "";
            var refId = int.Parse(refillId);
            var row = db.Refills.Where(x => x.RefillId == refId).Single();
            row.CancelledBy = Session["username"].ToString();
            row.CancelledDate = DateTime.Now;

            db.Entry(row).State = EntityState.Modified;

            var client = db.Clients.Where(x => x.ClientCode == clientCode).Single();
           // client.Balance = client.Balance - row.RefillValue;

            db.Entry(client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return RedirectToAction("_RefillHistory", new { clientCode = clientCode, errorMsg = errorMessage, canceled = canceled });
        }

        public ActionResult EditRefill(string refillId, string remarks, string billno)
        {
            var message = "";
            var editSuccess = false;
            var refId = int.Parse(refillId);
            var row = db.Refills.AsNoTracking().Where(x => x.RefillId == refId).Single();
            row.Remarks = remarks;
            row.BillNo = billno;
            db.Entry(row).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
                editSuccess = true;
                message = "Refill has been edited";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { editSuccess = editSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public string SendMail(string recipient, string subject, string body, string CC = "")
        {
            try
            {
                var trimmedCC = CC.Replace(" ", "");
                var CCs = trimmedCC.Split(',');
                string sender = ConfigurationManager.AppSettings["SendMail"];
                string password = ConfigurationManager.AppSettings["Password"];
                string alias = ConfigurationManager.AppSettings["Alias"];

                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTP"]);
                smtp.EnableSsl = false;
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["PORT"]);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(sender, password);
                MailMessage message = new MailMessage();
                message.Sender = new MailAddress(sender, alias);
                message.From = new MailAddress(sender, alias);
                message.To.Add(new MailAddress(recipient, ""));

                foreach (var id in CCs)
                {
                    if (id != "")
                    {
                        message.CC.Add(id);
                    }
                }

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                smtp.Send(message);



                if (CC == "")
                {
                    return "<text class='text-success'>Mail has been sent successfully to " + recipient + "</text>";
                }
                return "<text class='text-success'>Mail has been sent successfully to " + recipient + "," + CC + "</text>";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
            }
        }

        public string RefillBalance(string clientCode, string refillAmount, string remarks, string billno)
        {
            //var row = db.Clients.Where(x => x.ClientCode == clientCode).Single();
            //row.Balance = row.Balance + int.Parse(refillAmount);

            //db.Entry(row).State = EntityState.Modified;
            
            var refill = new Refills();
            refill.ClientCode = clientCode;
            refill.RefillBy = Session["username"].ToString();
            refill.RefillValue = decimal.Parse(refillAmount);
            refill.Remarks = remarks;
            refill.RefillDate = DateTime.Now;
            refill.BillNo = billno;
            db.Refills.Add(refill);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return "<text class='text-danger'>" + ex.Message == null ? ex.InnerException.Message : ex.Message + "</text>";
            }

            return "<text class='text-success'>Balance of NPR " + refillAmount + " has been successfully refilled.</text>";
        }

        public ActionResult Statistics()
        {
            var coopName = new List<string>();
            var data = new List<int>();
            var color = new List<string>();

            var barData = db.vwClientStatics.OrderByDescending(x => x.Cnt).ToList();
            foreach (var row in barData)
            {
                coopName.Add(row.Name);
                if(row.Cnt.HasValue)
                {
                    data.Add(row.Cnt.Value);
                }
                
                color.Add(row.ColorVal);
            }

            ViewBag.CoopName = coopName;
            ViewBag.Data = data;
            ViewBag.Color = color;
            ViewBag.TotalHeight = barData.Count() * 30;

            return View();
        }

        public string GeneratePassword()
        {
            Guid guid = Guid.NewGuid();
            var id = guid.ToString().Replace("-", "");
            return id.Substring(0, 10);
        }

        [HttpPost]
        public ActionResult AddClient(Clients clients)
        {
            clients.ClientCode = "void";            
            clients.ColorVal = "void";
            clients.PassKey = GeneratePassword();
            clients.isUser = true;
            clients.Status = 2;
          
            db.Clients.Add(clients);
            
            try
            {
                db.SaveChanges();
                var clientCode = db.Clients.Where(x => x.EmailId == clients.EmailId && x.MobileNumber == clients.MobileNumber && x.PassKey == clients.PassKey).Select(x => x.ClientCode).SingleOrDefault();
                if (clients.isUser == true)
                {
                    Saf credentials = new Saf()
                    {
                        Username = clientCode,
                        Password = clients.PassKey,
                        EmailId = clients.EmailId,
                        MobileNo = clients.MobileNumber,
                        Status = 1,
                        Level = 5
                    };
                    db.Saf.Add(credentials);
                    db.SaveChanges();

                    UpdateApprovalCount hub = new UpdateApprovalCount();
                    hub.Update(db.Clients.Where(x => x.Status == 2).Count());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Status = false;
                return Json("<text class='text-danger'>" + ex.Message + "</text>", JsonRequestBehavior.AllowGet);
            }
            ViewBag.Status = true;
            //ViewBag.Message = clients.Name + " has been successfully added";

            // var model = db.Clients.Where(x => x.EmailId == clients.EmailId).SingleOrDefault();
            //return PartialView();
            return Json("<text class='text-success'>Client has been added successfully!</text>", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string EditClient(Clients clients)
        {
            if (clients.isActive == true)
            {
                clients.Status = 1;
            }
            else
            {
                clients.Status = 0;
            }
            if (clients.isUser == true)
            {
                var row = db.Saf.AsNoTracking().Where(x => x.Username == clients.ClientCode).SingleOrDefault();
                if (row == null)
                {
                    clients.Status = 2;
                    Saf saf = new Saf
                    {
                        Username = clients.ClientCode,
                        Password = clients.PassKey,
                        EmailId = clients.EmailId,
                        MobileNo = clients.MobileNumber,
                        Status = 1,
                        Level = 5
                    };
                    db.Saf.Add(saf);
                }
                else
                {
                    row.Status = 1;
                    row.Username = clients.Username;
                    row.Password = clients.Password;
                    db.Entry(row).State = EntityState.Modified;
                }
            }
            else
            {
                var saf = db.Saf.AsNoTracking().Where(x => x.Username == clients.ClientCode).SingleOrDefault();
                if (saf != null)
                {
                    saf.Status = 0;
                    db.Entry(saf).State = EntityState.Modified;
                }
            }
            db.Entry(clients).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

                UpdateApprovalCount hub = new UpdateApprovalCount();
                hub.Update(db.Clients.Where(x => x.Status == 2).Count());
            }



            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
            catch (Exception ex)
            {
                return "<text class='text-danger'>" + ex.Message == null ? ex.InnerException.Message : ex.Message;
            }
            return "<text class='text-success'>" + clients.Name + " has been successfully edited!";

        }

        public ActionResult Notification()
        {
            var notificationcount = db.Clients.Where(x => x.Status == 2).Count();
            return Json(notificationcount,JsonRequestBehavior.AllowGet);
        }


    }
}