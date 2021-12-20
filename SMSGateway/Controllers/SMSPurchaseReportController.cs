using SMSGateway.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;
using System.Data.Entity.Core.Objects;
using System.Linq;

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
            SqlCommand command = new SqlCommand("spSMSPurchaseReport", connection);
            command.CommandTimeout = 600;
            connection.Open();

            if (fromDate == null)
            {
                command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            }
            else
            {
                command.Parameters.AddWithValue("@FromDate", fromDate);
            }

            if (toDate == null)
            {
                command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = DBNull.Value;
            }
            else
            {
                command.Parameters.AddWithValue("@ToDate", toDate);
            }
            command.Parameters.AddWithValue("@SupplierId", supplier);

            command.CommandText = "spSMSPurchaseReport";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@TotalQuantity", SqlDbType.Int);
            command.Parameters["@TotalQuantity"].Direction = ParameterDirection.Output;
            command.Parameters.Add("@TotalAmount", SqlDbType.Decimal);
            command.Parameters["@TotalAmount"].Direction = ParameterDirection.Output;


            SqlDataAdapter da = new SqlDataAdapter(command);
            da.SelectCommand = command;
            DataSet ds = new DataSet();
            da.Fill(ds);
            command.ExecuteNonQuery();
            connection.Close();

            List<Report_SMSPurchase> list = new List<Report_SMSPurchase>();

            int tq = 0;
            double ta = 0.0;

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Report_SMSPurchase r = new Report_SMSPurchase();
                    r.Supplier = (int)item["Supplier"];
                    r.SupplierName = item["SupplierName"].ToString();
                    r.PurchaseDate = Convert.ToDateTime(item["PurchaseDate"]);
                    r.Quantity = Convert.ToInt32(item["Quantity"]);
                    r.Rate = Convert.ToDecimal(item["Rate"]);
                    r.Amount = Convert.ToDecimal(item["Amount"]);
                    r.EntryBy = Convert.ToString(item["EntryBy"]);
                    r.BillNo = Convert.ToString(item["BillNo"]);
                    r.Remarks = Convert.ToString(item["Remarks"]);

                    tq += r.Quantity;
                    ta += (double)r.Amount;

                    list.Add(r);
                };

                ViewBag.TotalQuantity = tq;
                ViewBag.TotalAmount = ta;
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
            }
            return PartialView(list);
        }


        public ActionResult ExportToExcel(Report_SMSPurchase formData)
        {

            SqlCommand command = new SqlCommand("spSMSPurchaseReport", connection);
            connection.Open();

            command.Parameters.AddWithValue("@FromDate", formData.FromDate);
            command.Parameters.AddWithValue("@ToDate", formData.ToDate);
            command.Parameters.AddWithValue("@SupplierId", formData.Supplier);

            command.CommandText = "spSMSPurchaseReport";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@TotalQuantity", SqlDbType.Int);
            command.Parameters["@TotalQuantity"].Direction = ParameterDirection.Output;
            command.Parameters.Add("@TotalAmount", SqlDbType.Decimal);
            command.Parameters["@TotalAmount"].Direction = ParameterDirection.Output;

            SqlDataAdapter da = new SqlDataAdapter(command);
            da.SelectCommand = command;
            DataSet ds = new DataSet();
            da.Fill(ds);
            command.ExecuteNonQuery();
            connection.Close();

            List<Report_SMSPurchase> list = new List<Report_SMSPurchase>();

            Report_SMSPurchase r = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {


                    r = new Report_SMSPurchase();
                    r.Supplier = (int)item["Supplier"];
                    r.SupplierName = (string)item["SupplierName"];
                    r.PurchaseDate = (DateTime)item["PurchaseDate"];
                    r.Quantity = Convert.ToInt32(item["Quantity"]);
                    r.Rate = Convert.ToDecimal(item["Rate"]);
                    r.Amount = Convert.ToDecimal(item["Amount"]);
                    r.EntryBy = Convert.ToString(item["EntryBy"]);
                    r.BillNo = Convert.ToString(item["BillNo"]);
                    r.Remarks = Convert.ToString(item["Remarks"]);

                    list.Add(r);
                }
            }

            if (list.Count == 0)
            {
                return PartialView("_NoRecords");
            }
            else
            {
                GridView gv = new GridView() { AutoGenerateColumns = true };


                gv.DataSource = ds;

                gv.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=SMS_Purchase.xls");
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

