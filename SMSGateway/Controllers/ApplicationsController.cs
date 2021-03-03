using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSGateway.Models;

namespace SMSGateway.Controllers
{
    public class ApplicationsController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
        // GET: Applications
        public ActionResult _ApplicationsList()
        {
            var model = db.vwClients.Where(x => x.ClientCode.CompareTo("0000") < 0 || x.ClientCode.CompareTo("9999") > 0).ToList();
            ViewBag.Session = (string)Session["level"];
            return PartialView(model);
        }
    }
}