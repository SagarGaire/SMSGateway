using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSGateway.Models;

namespace SMSGateway.Controllers
{
    public class ConsumerAllController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ADO_connection"].ConnectionString);
        // GET: ConsumerAll
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _consumerAll(string clientCode, string clientType, DateTime? fromDateTime, DateTime? toDateTime)
        {
            SqlCommand command = new SqlCommand("spSMSClientConsumptionAll", connection);
            connection.Open();

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@fromdate", "2020-07-16");
            command.Parameters.AddWithValue("@todate", "2021-01-13");
            command.Parameters.AddWithValue("@ClientCode", "");
            command.Parameters.AddWithValue("@ClientType", "");

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

            List<ReportSMSClientConsumptionAll> consumeralllist = new List<ReportSMSClientConsumptionAll>();
            ReportSMSClientConsumptionAll rep = null;

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    rep = new ReportSMSClientConsumptionAll();
                    rep.ClientCode = item["ClientCode"].ToString();
                    rep.ClientName = item["ClientName"].ToString();
                    rep.OpeningBalanceCredit = Convert.ToDecimal(item["OpeningBalanceCredit"]);
                    rep.OpeningBalanceAmt = Convert.ToDecimal(item["OpeningBalanceAmt"]);
                    rep.PurchaseCredit = Convert.ToDecimal(item["PurchaseCredit"]);
                    rep.PurchaseAmount = Convert.ToDecimal(item["PurchaseAmount"]);
                    rep.ConsumedCredit = Convert.ToDecimal(item["ConsumedCredit"]);
                    rep.ConsumedAmt = Convert.ToDecimal(item["ConsumedAmt"]);
                    rep.ClosingBalanceCredit = Convert.ToDecimal(item["ClosingBalanceCredit"]);
                    rep.ClosingBalanceAmt = Convert.ToDecimal(item["ClosingBalanceAmt"]);

                    consumeralllist.Add(rep);
                }
            }
            try
            {
                connection.Open();
                command.ExecuteNonQuery();

                var openingbalancecredit = Convert.ToDecimal(command.Parameters["@TotalOpeningBalanceCredit"].Value);
                ViewBag.openingbalance = openingbalancecredit;

                var openingbalanceamount = Convert.ToDecimal(command.Parameters["@TotalOpeningBalanceAmt"].Value);
                ViewBag.openingbalanceamoint = openingbalanceamount;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //var totalOpeningCreditAmount = Convert.ToDecimal(command.Parameters["TotalOpeningBalanceCredit"].Value);
            //ViewBag.Test = totalOpeningCreditAmount;

            return View(consumeralllist);
        }
    }
}