using System.Linq;
using System.Web.Mvc;
using SMSGateway.Models;
using PagedList;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Data.Entity.Core.Objects;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace SMSGateway.Controllers
{
    public class OutgoingSMSController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ADO_connection"].ConnectionString);

        public ActionResult _Index()
        {
            ViewBag.Status = new SelectList(db.StatusCodes, "Code", "Description");
            if ((string)Session["code"] == "User")
            {
                ViewBag.Clients = new SelectList(db.Clients, "ClientCode", "Name");
            }
            ViewBag.Code = (string)Session["code"];
            return PartialView();
        }

        public ActionResult _OutgoingSMSList(DateTime? fromDate, DateTime? toDate, string recipients = null, string status = null, string client = null, int pageNumber = 1, int pageSize = 20)
        {
            if (client == "")
            {
                client = System.Configuration.ConfigurationManager.AppSettings["ClientCode"].ToString();
            }

            SqlCommand command = new SqlCommand("spReportOutgoingSMS", connection);
            connection.Open();

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@ClientCode", client);
            command.Parameters.AddWithValue("@Recipients", recipients);
            command.Parameters.AddWithValue("@FromDate", fromDate);
            command.Parameters.AddWithValue("@ToDate", toDate);
            command.Parameters.AddWithValue("@PageNo", pageNumber);

            command.CommandText = "spReportOutgoingSMS"; //store procedure name

            command.Parameters.Add("@PageCount", SqlDbType.Int);
            command.Parameters["@PageCount"].Direction = ParameterDirection.Output;
            command.Parameters.Add("@SMSCount", SqlDbType.Int);
            command.Parameters["@SMSCount"].Direction = ParameterDirection.Output;
            command.Parameters.Add("@SMSCost", SqlDbType.Decimal);
            command.Parameters["@SMSCost"].Direction = ParameterDirection.Output;


            SqlDataAdapter da = new SqlDataAdapter(command);
            da.SelectCommand = command;
            DataSet ds = new DataSet();
            da.Fill(ds);
            command.ExecuteNonQuery();
            connection.Close();

            List<ReportOutgoingSMS> outgoingSMSList = new List<ReportOutgoingSMS>();
            ReportOutgoingSMS p = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {


                    p = new ReportOutgoingSMS();
                    p.MsgId = Convert.ToInt64(item["MsgId"]);
                    if (item["MsgDateTime"].ToString() != null)
                    {

                        p.MsgDateTime = (DateTime)item["MsgDateTime"];
                    }

                    p.ClientCode = item["ClientCode"].ToString();
                    p.ClientName = item["ClientName"].ToString();
                    p.Recipients = item["Recipients"].ToString();
                    p.RecipientCount = item["RecipientCount"].ToString();
                    p.MsgText = item["MsgText"].ToString();
                    p.Status = (int)item["Status"];
                    p.StatusName = item["StatusName"].ToString();
                    p.Part = item["Part"] == DBNull.Value ? 0 : Convert.ToInt32(item["Part"]);
                    p.SMSCount = (int)item["SMSCount"];
                    p.SMSCost = (decimal)item["SMSCost"];
                    p.RequestCode = item["RequestCode"].ToString();

                    outgoingSMSList.Add(p);


                }
            }

            if ((string)Session["level"] == "5")
            {
                client = System.Configuration.ConfigurationManager.AppSettings["ClientCode"].ToString();

            }
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Recipients = recipients;
            ViewBag.Status = status;
            ViewBag.Clients = client;
            ViewBag.PageNumber = pageNumber;
            ViewBag.SN = ((pageNumber - 1) * pageSize) + 1;
            var outgoingSMS = new OutgoingSMS();

            //var a = command.Parameters["@PageCount"];



            ViewBag.PageCount = command.Parameters["@PageCount"].Value == DBNull.Value || command.Parameters["@PageCount"].SqlValue == null ? 0 : (int)(command.Parameters["@PageCount"].Value);
            ViewBag.TotalSMS = command.Parameters["@SMSCount"].Value == DBNull.Value || command.Parameters["@SMSCount"].SqlValue == null ? 0 : (int)(command.Parameters["@SMSCount"].Value);
            ViewBag.TotalCost = command.Parameters["@SMSCost"].Value == DBNull.Value || command.Parameters["@SMSCost"].SqlValue == null ? 0 : (decimal)(command.Parameters["@SMSCost"].Value);



            return PartialView(outgoingSMSList.ToPagedList(1, pageSize));

        }

        public ActionResult ExportToExcel(Report_OutgoingSMS formData)
        {

            SqlCommand command = new SqlCommand("spReportOutgoingSMS", connection);
            connection.Open();

            command.CommandType = CommandType.StoredProcedure;
            if (formData.Status != null)
            {

                command.Parameters.AddWithValue("@Status", formData.Status.Count() == 0 || formData == null ? "" : string.Join(",", formData.Status));
            }
            else
            {
                command.Parameters.AddWithValue("@Status", "");
            }
            if (formData.Clients != null)
            {
                command.Parameters.AddWithValue("@ClientCode", formData.Clients.Count() == 0 || formData == null ? "" : string.Join(",", formData.Clients));
            }
            else
            {
                command.Parameters.AddWithValue("@ClientCode", "");
            }
            command.Parameters.AddWithValue("@Recipients", formData.Recipients);//.Replace(" ", ""));
            command.Parameters.AddWithValue("@FromDate", formData.FromDate);
            command.Parameters.AddWithValue("@ToDate", formData.ToDate);
            command.Parameters.AddWithValue("@PageNo", 0);

            command.CommandText = "spReportOutgoingSMS";

            command.Parameters.Add("@PageCount", SqlDbType.Int);
            command.Parameters["@PageCount"].Direction = ParameterDirection.Output;
            command.Parameters.Add("@SMSCount", SqlDbType.Int);
            command.Parameters["@SMSCount"].Direction = ParameterDirection.Output;
            command.Parameters.Add("@SMSCost", SqlDbType.Decimal);
            command.Parameters["@SMSCost"].Direction = ParameterDirection.Output;

            SqlDataAdapter da = new SqlDataAdapter(command);
            da.SelectCommand = command;
            DataSet ds = new DataSet();
            da.Fill(ds);
            command.ExecuteNonQuery();
            connection.Close();

            List<ReportOutgoingSMS> outgoingSMSList = new List<ReportOutgoingSMS>();
            ReportOutgoingSMS p = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {


                    p = new ReportOutgoingSMS();
                    p.MsgId = Convert.ToInt64(item["MsgId"]);
                    if (item["MsgDateTime"].ToString() != null)
                    {

                        p.MsgDateTime = (DateTime)item["MsgDateTime"];
                    }

                    p.ClientCode = item["ClientCode"].ToString();
                    p.ClientName = item["ClientName"].ToString();
                    p.Recipients = item["Recipients"].ToString();
                    p.RecipientCount = item["RecipientCount"].ToString();
                    p.MsgText = item["MsgText"].ToString();
                    p.Status = (int)item["Status"];
                    p.StatusName = item["StatusName"].ToString();
                    p.Part = item["Part"] == DBNull.Value ? 0 : Convert.ToInt32(item["Part"]);
                    p.SMSCount = (int)item["SMSCount"];
                    p.SMSCost = (decimal)item["SMSCost"];
                    p.RequestCode = item["RequestCode"].ToString();

                    outgoingSMSList.Add(p);

                }
            }

            if (outgoingSMSList.Count == 0)
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
                Response.AddHeader("content-disposition", "attachment; filename=Outgoing_SMS.xls");
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