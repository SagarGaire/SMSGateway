using PagedList;
using SMSGateway.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace SMSGateway.Controllers
{
    public class RefillReportController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();

        public ActionResult _Index()
        {

            ViewBag.Status = new SelectList(db.StatusCodes, "Code", "Description");
            if ((string)Session["code"] == "User")
            {
                ViewBag.Client = new SelectList(db.Clients, "ClientCode", "Name");
            }
            ViewBag.Code = (string)Session["code"];
            return PartialView();

        }
     
        public ActionResult _RefillReport(DateTime? fromDate, DateTime? toDate, string client = null, int pageNumber = 1, int pageSize = 20)
        {
            ViewBag.Client = client;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.PageNumber = pageNumber;
            ViewBag.SN = ((pageNumber - 1) * pageSize) + 1;

            ObjectParameter totalPages = new ObjectParameter("pageCount", typeof(int));

            var model = db.spRefillReport(client, fromDate, toDate, pageNumber, totalPages).OrderBy(x => x.CancelledBy).ToList();
            
            
            
            ViewBag.PageCount = totalPages.Value;
            return PartialView(model.ToPagedList(1, pageSize));

        }

        public ActionResult ExportToExcel(Refill_Report formRefill)
        {

            ObjectParameter totalPages = new ObjectParameter("pageCount", typeof(int));
            var pageNumber = 0;
            string clientfilter = null;
            if (formRefill.client != null)
            {
                clientfilter = (formRefill.client.Count() == 0 || formRefill.client == null ? "" : string.Join(",", formRefill.client));
            }
            else
            {
                clientfilter = "";
            }

            var model = db.spRefillReport(clientfilter, formRefill.fromDate, formRefill.toDate, pageNumber, totalPages).OrderBy(x => x.CancelledBy).ToList();

            if (model.Count == 0)
            {
                return PartialView("_NoRecords");

            }
            else
            {

                GridView gv = new GridView() { AutoGenerateColumns = true };


                gv.DataSource = model;

                gv.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Refill_Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter swriter = new StringWriter();
                HtmlTextWriter htwriter = new HtmlTextWriter(swriter);

                gv.RenderControl(htwriter);

                Response.Output.Write(swriter.ToString());
                Response.Flush();
                Response.End();


                return View();
            }
        }

    }
}