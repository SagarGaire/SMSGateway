using SMSGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSGateway.Controllers
{
    public class DashboardController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();

        public ActionResult _Dashboard()
        {
            if ((string)Session["code"] == "Client")
            {
                return RedirectToAction("_Dashboard", "ClientPortal", new { clientCode = System.Configuration.ConfigurationManager.AppSettings["ClientCode"].ToString()});
            }
            var balanceInfo = db.spDashBoardView1();
            var model = db.vwClients.Where(x => x.Balamt < (x.AlertLimit.Value == 0 ? 1: x.AlertLimit) && x.Status == 1).OrderBy(x=> ((x.Balamt/ (x.AlertLimit.Value == 0 ? 1 : x.AlertLimit)) * 100));

            
            foreach (var item in balanceInfo)
            {
                ViewBag.ExpectedBalance = item.ExpectedBalance;
                ViewBag.TodayOutGoing = item.TodayOutGoing;
                ViewBag.TotalOutgoing = item.TotalOutGoing;
                ViewBag.TotalSMSPurchases = item.TotalSMSPurchases;
                decimal clientBalance = item.ClientBalance ?? 0;
                ViewBag.ClientBalance = clientBalance.ToString("0");

                
            }

            var coopName = new List<string>();
            var data = new List<int>();
            var color = new List<string>();

            var barData = db.vwClientStatics.OrderByDescending(x=>x.Cnt).ToList().Take(10);
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

            return PartialView(model.ToList());
        }
        public ActionResult _ExpectedBalance()
        {
            var model = db.vwExpectedBalance.ToList();
            return PartialView(model);
        }
    }
}