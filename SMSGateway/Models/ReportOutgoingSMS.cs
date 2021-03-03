using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSGateway.Models
{
    public class ReportOutgoingSMS
    {
        public long MsgId { get; set; }
        public Nullable<System.DateTime> MsgDateTime { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string Recipients { get; set; }
        public string RecipientCount { get; set; }
        public string MsgText { get; set; }
        public Nullable<int> Status { get; set; }
        public string StatusName { get; set; }
        public Nullable<int> Part { get; set; }
        public Nullable<decimal> SMSCost { get; set; }
        public Nullable<int> SMSCount { get; set; }
        public string RequestCode { get; set; }
        public Nullable<int> TotalPageCount { get; set; }
        public Nullable<int> TotalSMSCount { get; set; }
        public Nullable<decimal> TotalSMSCost { get; set; }
    }
}