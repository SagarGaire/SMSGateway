using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSGateway.Models;

namespace SMSGateway.Controllers
{
    public class HomeController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                return RedirectToAction("Index", "Main");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login login)
        {
            try
            {
                var row = db.Saf.Where(x => (x.Username == login.Username && x.Password == login.Password) || (x.EmailId == login.Username && x.Password == login.Password)).SingleOrDefault();
                var accessLogs = new AccessLogs();
                if (row != null)
                {
                    if (row.Status != 0)
                    {
                        Clients client = new Clients();
                        accessLogs.LoggedFrom = Request.UserHostAddress;
                        accessLogs.UserName = row.Username;
                        accessLogs.LogDateTime = DateTime.Now;
                        db.AccessLogs.Add(accessLogs);
                        db.SaveChanges();
                        Session["username"] = row.Username;
                        Session["level"] = row.Level.ToString();
                        Session["code"] = "User";
                        if (row.Level == 5)
                        {
                            client = db.Clients.Where(x => x.ClientCode == row.Username).SingleOrDefault();
                            Session["code"] = "Client";
                            Session["username"] = client.Name;
                            System.Configuration.ConfigurationManager.AppSettings["ClientCode"] = client.ClientCode;
                        }
                        return RedirectToAction("Index", "Main");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your login credentials have been disabled. Contact your service provider for more information.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View();
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return RedirectToAction("Index", "Error", new { errorMessage = error });
            }
        }

        public ActionResult Logout()
        {
            Session["username"] = null;
            Session["level"] = null;
            Session["code"] = null;
            return RedirectToAction("Index");
        }
    }
}