using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSGateway.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace SMSGateway.Controllers
{
    public class RevenueReportController : Controller
    {
        // GET: RevenueReport
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ADO_connection"].ConnectionString);
        public ActionResult _Index()
        {
            return PartialView();
        }

        public ActionResult _RevenueReport(DateTime? fromDate, DateTime? toDate)
        {
            SqlCommand command = new SqlCommand("spRevenueReport", connection);
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

            command.CommandText = "spRevenueReport";
            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(command);
            da.SelectCommand = command;
            DataSet ds = new DataSet();
            da.Fill(ds);
            command.ExecuteNonQuery();
            connection.Close();

            List<Report_Revenue> list = new List<Report_Revenue>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Report_Revenue r = new Report_Revenue();
                    r.Supplier = (int)item["SupplierId"];
                    r.SupplierName = item["SupplierName"].ToString();
                    r.SubType = Convert.ToString(item["SubType"]);                    
                    r.OpeningCredits = (Convert.IsDBNull(item["OpeningCredits"]) ? 0 : Convert.ToInt32(item["OpeningCredits"]));
                    r.OpeningAmount = (Convert.IsDBNull(item["OpeningAmount"]) ? 0 : Convert.ToDecimal(item["OpeningAmount"]));
                    r.CreditsPurchases = (Convert.IsDBNull(item["CreditPurchases"]) ? 0 : Convert.ToInt32(item["CreditPurchases"]));
                    r.AmountPurchases = (Convert.IsDBNull(item["AmountPurchases"]) ? 0 : Convert.ToDecimal(item["AmountPurchases"]));
                    r.CreditsConsumed = (Convert.IsDBNull(item["CreditsConsumed"]) ? 0 : Convert.ToInt32(item["CreditsConsumed"]));
                    r.AmountConsumed = (Convert.IsDBNull(item["AmountConsumed"]) ? 0 : Convert.ToDecimal(item["AmountConsumed"]));
                    r.ClosingCredit = (Convert.IsDBNull(item["ClosingCredit"]) ? 0 : Convert.ToInt32(item["ClosingCredit"]));
                    r.ClosingAmount = (Convert.IsDBNull(item["ClosingAmount"]) ? 0 : Convert.ToDecimal(item["ClosingAmount"]));
                    r.CostOfCredits = (Convert.IsDBNull(item["CostOfCredits"]) ? 0 : Convert.ToDecimal(item["CostOfCredits"]));
                    r.Revenue = (Convert.IsDBNull(item["Revenue"]) ? 0 : Convert.ToDecimal(item["Revenue"]));

                    list.Add(r);
                };
            }         
            return PartialView(list);
        }

        public ActionResult ExportToExcel(Report_Revenue formData)
        {

            SqlCommand command = new SqlCommand("spRevenueReport", connection);
            connection.Open();

            command.Parameters.AddWithValue("@FromDate", formData.FromDate);
            command.Parameters.AddWithValue("@ToDate", formData.ToDate);

            command.CommandText = "spRevenueReport";
            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(command);
            da.SelectCommand = command;
            DataSet ds = new DataSet();
            da.Fill(ds);
            command.ExecuteNonQuery();
            connection.Close();

            List<Report_Revenue> list = new List<Report_Revenue>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {


                    Report_Revenue r = new Report_Revenue();
                    r.Supplier = (int)item["SupplierId"];
                    r.SupplierName = item["SupplierName"].ToString();
                    r.SubType = Convert.ToString(item["SubType"]);
                    r.OpeningAmount = (Convert.IsDBNull(item["OpeningAmount"]) ? 0 : Convert.ToDecimal(item["OpeningAmount"]));
                    r.CreditsPurchases = (Convert.IsDBNull(item["CreditPurchases"]) ? 0 : Convert.ToInt32(item["CreditPurchases"]));
                    r.AmountPurchases = (Convert.IsDBNull(item["AmountPurchases"]) ? 0 : Convert.ToDecimal(item["AmountPurchases"]));
                    r.CreditsConsumed = (Convert.IsDBNull(item["CreditsConsumed"]) ? 0 : Convert.ToInt32(item["CreditsConsumed"]));
                    r.OpeningCredits = (Convert.IsDBNull(item["OpeningCredits"]) ? 0 : Convert.ToInt32(item["OpeningCredits"]));
                    r.AmountConsumed = (Convert.IsDBNull(item["AmountConsumed"]) ? 0 : Convert.ToDecimal(item["AmountConsumed"]));
                    r.ClosingCredit = (Convert.IsDBNull(item["ClosingCredit"]) ? 0 : Convert.ToInt32(item["ClosingCredit"]));
                    r.ClosingAmount = (Convert.IsDBNull(item["ClosingAmount"]) ? 0 : Convert.ToDecimal(item["ClosingAmount"]));
                    r.CostOfCredits = (Convert.IsDBNull(item["CostOfCredits"]) ? 0 : Convert.ToDecimal(item["CostOfCredits"]));
                    r.Revenue = (Convert.IsDBNull(item["Revenue"]) ? 0 : Convert.ToDecimal(item["Revenue"]));

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
                Response.AddHeader("content-disposition", "attachment; filename=Revenue_Report.xls");
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