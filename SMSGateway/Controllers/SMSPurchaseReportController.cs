using SMSGateway.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;
using System.Data.Entity.Core.Objects;

namespace SMSGateway.Controllers
{
    public class SMSPurchaseReportController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ADO_connection"].ConnectionString);

        public ActionResult _Index()
        {
            var supplier = db.Suppliers.ToList();
            List<SelectListItem> s_list = new List<SelectListItem>();
            s_list.Add(new SelectListItem { Text = "All", Value = "0" });
            s_list.Add(new SelectListItem { Text = "Sparrow Main", Value = "1" });
            s_list.Add(new SelectListItem { Text = "Sparrow NCBL", Value = "2" });
            ViewBag.Supplier = s_list;
            ViewBag.Code = (string)Session["code"];
            return PartialView();
        }

        public ActionResult _SMSPurchaseReport(DateTime? fromDate, DateTime? toDate, int supplier)
        {
            ViewBag.SupplierId = supplier;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            var totalQuantity = new SqlParameter { ParameterName = "@TotalQuantity", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            var totalAmount = new SqlParameter { ParameterName = "@TotalAmount", SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Output };

            var model = db.spSMSPurchaseReport(fromDate, toDate, supplier, totalQuantity, totalAmount).ToList();

            int tq = 0;
            double ta = 0.0;
            foreach (var item in model)
            {
                tq += item.Quantity;
                ta += (double)item.Amount;
            }

            ViewBag.TotalQuantity = tq;
            ViewBag.TotalAmount = ta;

            return PartialView(model);
        }

        public ActionResult ExportToExcel(Report_SMSPurchase formSMSPurchase)
        {
            string supplierfilter = null;
            if (formSMSPurchase.supplier != null)
            {
                supplierfilter = (formSMSPurchase.supplier.Count() == 0 || formSMSPurchase.supplier == null ? "" : string.Join(",", formSMSPurchase.supplier));
            }
            else
            {
                supplierfilter = "";
            }

            var totalQuantity = new SqlParameter { ParameterName = "@TotalQuantity", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            var totalAmount = new SqlParameter { ParameterName = "@TotalAmount", SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Output };
            var model = db.spSMSPurchaseReport(formSMSPurchase.FromDate, formSMSPurchase.ToDate, Convert.ToInt32(supplierfilter), totalQuantity, totalAmount).ToList();

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
                Response.AddHeader("content-disposition", "attachment; filename=SMSPurchase_Report.xls");
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

