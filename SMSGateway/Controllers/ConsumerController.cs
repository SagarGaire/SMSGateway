using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using SMSGateway.Models;

namespace SMSGateway.Controllers
{
    public class ConsumerController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ADO_connection"].ConnectionString);

        public ActionResult _Index()
        {
            var clients = db.Clients.ToList();
            SelectList clt = new SelectList(clients, "ClientCode", "Name");
            ViewBag.Client = clt;
            //ViewBag.Client = new SelectList(db.Clients, "ClientCode", "Name");
            ViewBag.Code = (string)Session["code"];
            return PartialView();
        }

        public ActionResult _ConsumerReport(string clientCode, string clientType, DateTime? fromDate, DateTime? toDate, int rows = 50)
        {
            SqlCommand command = new SqlCommand("spSMSClientConsumptionAll", connection);
            command.CommandTimeout = 600;
            connection.Open();
    
            command.CommandType = CommandType.StoredProcedure;
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

            command.Parameters.AddWithValue("@ClientCode", clientCode);
            command.Parameters.AddWithValue("@ClientType", clientType);

            command.CommandText = "spSMSClientConsumptionAll"; //store procedure name

            command.Parameters.Add("@TotalOpeningBalanceCredit", SqlDbType.Int);
            command.Parameters["@TotalOpeningBalanceCredit"].Direction = ParameterDirection.Output;

            command.Parameters.Add("@TotalOpeningBalanceAmt", SqlDbType.Int);
            command.Parameters["@TotalOpeningBalanceAmt"].Direction = ParameterDirection.Output;

            command.Parameters.Add("@TotalPurchaseCredit", SqlDbType.Int);
            command.Parameters["@TotalPurchaseCredit"].Direction = ParameterDirection.Output;

            command.Parameters.Add("@TotalPurchaseAmt", SqlDbType.Int);
            command.Parameters["@TotalPurchaseAmt"].Direction = ParameterDirection.Output;

            command.Parameters.Add("@TotalConsumedCredit", SqlDbType.Int);
            command.Parameters["@TotalConsumedCredit"].Direction = ParameterDirection.Output;

            command.Parameters.Add("@TotalConsumedAmt", SqlDbType.Int);
            command.Parameters["@TotalConsumedAmt"].Direction = ParameterDirection.Output;

            command.Parameters.Add("@TotalClosingBalanceCredit", SqlDbType.Int);
            command.Parameters["@TotalClosingBalanceCredit"].Direction = ParameterDirection.Output;

            command.Parameters.Add("@TotalClosingBalanceAmt", SqlDbType.Int);
            command.Parameters["@TotalClosingBalanceAmt"].Direction = ParameterDirection.Output;


            SqlDataAdapter da = new SqlDataAdapter(command);
            da.SelectCommand = command;
            DataSet ds = new DataSet();
            da.Fill(ds);
            command.ExecuteNonQuery();
            connection.Close();

            List<SMSClientAll> consumerlist = new List<SMSClientAll>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    SMSClientAll c = new SMSClientAll();
                    c.ClientCode = item["ClientCode"].ToString();
                    c.ClientName = item["ClientName"].ToString();
                    c.OpeningBalanceCredit = Convert.ToDecimal(item["OpeningBalanceCredit"]);
                    c.OpeningBalanceAmt = Convert.ToDecimal(item["OpeningBalanceAmt"]);
                    c.PurchaseCredit = Convert.ToDecimal(item["PurchaseCredit"]);
                    c.PurchaseAmount = Convert.ToDecimal(item["PurchaseAmount"]);
                    c.ConsumedCredit = Convert.ToDecimal(item["ConsumedCredit"]);
                    c.ConsumedAmt = Convert.ToDecimal(item["ConsumedAmt"]);
                    c.ClosingBalanceCredit = Convert.ToDecimal(item["ClosingBalanceCredit"]);
                    c.ClosingBalanceAmt = Convert.ToDecimal(item["ClosingBalanceAmt"]);

                    consumerlist.Add(c);
                };
            }
            try
            {
                connection.Open();
                command.ExecuteNonQuery();

                var openingbalancecredit = Convert.ToDecimal(command.Parameters["@TotalOpeningBalanceCredit"].Value);
                ViewBag.TotalOpeningBalanceCredit = openingbalancecredit;

                var openingbalanceamount = Convert.ToDecimal(command.Parameters["@TotalOpeningBalanceAmt"].Value);
                ViewBag.TotalOpeningBalanceAmt = openingbalanceamount;

                var purchasecredit = Convert.ToDecimal(command.Parameters["@TotalPurchaseCredit"].Value);
                ViewBag.TotalPurchaseCredit = purchasecredit;

                var purchaseamount = Convert.ToDecimal(command.Parameters["@TotalPurchaseAmt"].Value);
                ViewBag.TotalPurchaseAmount = purchaseamount;

                var consumedcredit = Convert.ToDecimal(command.Parameters["@TotalConsumedCredit"].Value);
                ViewBag.TotalConsumedCredit = consumedcredit;

                var consumedamt = Convert.ToDecimal(command.Parameters["@TotalConsumedAmt"].Value);
                ViewBag.TotalConsumedAmt = consumedamt;

                var closingbalancecredit = Convert.ToDecimal(command.Parameters["@TotalOpeningBalanceCredit"].Value);
                ViewBag.TotalClosingBalanceCredit = closingbalancecredit;

                var closingbalanceamount = Convert.ToDecimal(command.Parameters["@TotalOpeningBalanceAmt"].Value);
                ViewBag.TotalClosingBalanceAmt = closingbalanceamount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            ViewBag.SelectedRows = rows;

            return PartialView(consumerlist);
        }

    }
}
