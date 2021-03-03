using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSGateway.Models;

namespace SMSGateway.Controllers
{
    public class StatementController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
        // GET: Statement
        public ActionResult _Index()
        {
            ViewBag.Client = new SelectList(db.Clients, "ClientCode", "Name");
            ViewBag.Code = (string)Session["code"];
            return PartialView();
        }

        public ActionResult _GenerateStatement(string clientCode, DateTime? fromDateTime, DateTime? toDateTime, int rows = 50)
        {
            if ((string)Session["level"] == "5")
            {
                clientCode = System.Configuration.ConfigurationManager.AppSettings["ClientCode"].ToString();
            }
            var model = db.spReportSMSStmt(clientCode, fromDateTime, toDateTime).ToList();
            var showLessModel = model.Take(rows).ToList();

            decimal totalDebitAmount = 0;
            decimal totalCreditAmount = 0;
            decimal totalDebitSMS = 0;
            decimal totalCreditSMS = 0;

            foreach (var item in model)
            {
                item.FormattedDebitSMS = (item.Debit ?? 0).ToString("0");
                item.FormattedCreditSMS = (item.Credit ?? 0).ToString("0");
                item.FormattedDebitAmt = (item.DebitAmt ?? 0).ToString("0");
                item.FormattedCreditAmt = (item.CreditAmt ?? 0).ToString("0");
                item.FormattedSMSBal = (item.SMSBal ?? 0).ToString("0");
                item.FormattedBalance = (item.Balance ?? 0).ToString("0");

                totalDebitAmount += item.DebitAmt ?? 0;
                totalCreditAmount += item.CreditAmt ?? 0;
                totalDebitSMS += item.Debit ?? 0;
                totalCreditSMS += item.Credit ?? 0;
                if (item.TrnDate != null)
                {
                    item.FormattedDateTime = (DateTime)item.TrnDate;
                }
            }
            ViewBag.TotalDebitAmount = totalDebitAmount.ToString("0");
            ViewBag.TotalCreditAmount = totalCreditAmount.ToString("0");
            ViewBag.totalDebitSMS = totalDebitSMS.ToString("0");
            ViewBag.totalCreditSMS = totalCreditSMS.ToString("0");

            ViewBag.TotalRows = model.Count();
            ViewBag.SelectedRows = rows;
            return PartialView(showLessModel);
        }
    }
}