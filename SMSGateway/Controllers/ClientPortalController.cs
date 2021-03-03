using SMSGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSGateway.Controllers
{
    public class ClientPortalController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
      
        public ActionResult _Dashboard()
        {
            var clientCode = System.Configuration.ConfigurationManager.AppSettings["ClientCode"].ToString();
            var model = db.vwClients.Where(x => x.ClientCode.ToString() == clientCode).SingleOrDefault();           
            ViewBag.Balance = model.Balamt.ToString();
            decimal rate = model.Rate ?? 0;
            ViewBag.Rate = rate.ToString("0.00");
            if (model.AlertLimit.HasValue)
            {
                ViewBag.AlertLimit = model.AlertLimit.Value.ToString("0");
            }
            return PartialView(model);
        }

        public ActionResult _EditClientInfo(string clientCode, string contact, string email, string alertLimit)
        {
            var row = db.Clients.Where(x => x.ClientCode == clientCode).SingleOrDefault();
            row.MobileNumber = contact;
            row.EmailId = email;
            row.AlertLimit = decimal.Parse(alertLimit);

            db.Entry(row).State = System.Data.Entity.EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch
            {
                return Json(new { status = false, message = "<label class='text-danger'>There seems to be an error while updating your information. Please check your fields and try again, or contact your software provider.</label>" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = true, message = "<label class='text-success'>Your information has been updated.</label>" }, JsonRequestBehavior.AllowGet);
        }
    }
}