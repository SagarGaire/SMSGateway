using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSGateway.Controllers
{
    public class RedirectionController : Controller
    {
        public ActionResult Redirection(string redirectTo)
        {
            if (redirectTo == " Dashboard")
            {
                return RedirectToAction("_Dashboard", "Dashboard");
            }
            else if (redirectTo == " Outgoing")
            {
                return RedirectToAction("_Index", "OutgoingSMS");
            }
            else if (redirectTo == " SMS Purchases")
            {
                return RedirectToAction("_SMSPurchaseList", "SMSPurchases");
            }
            else if (redirectTo == " User List")
            {
                return RedirectToAction("_UserList", "Saf");
            }
            else if (redirectTo == " Applications")
            {
                return RedirectToAction("_ApplicationsList", "Applications");
            }
            else if (redirectTo == " Statement")
            {
                return RedirectToAction("_Index", "Statement");
            }
            else if (redirectTo == " Consumer Report")
            {
                return RedirectToAction("_Index", "Consumer");
            }
            else if (redirectTo == " Refill")
            {
                return RedirectToAction("_Index", "RefillReport");
            }
            else if (redirectTo.Substring(0, 8) == " Pending")
            {
                return RedirectToAction("_PendingApproval", "Client");
            }
            else if (redirectTo == " Supplier List")
            {
                return RedirectToAction("_SupplierList", "Supplier");
            }
            else if (redirectTo == " Clients")
            {
                return RedirectToAction("_Index", "Client");
            }
            else
            {
                return RedirectToAction("_Index", "SMSPurchaseReport");
            }          
        }
    }
}