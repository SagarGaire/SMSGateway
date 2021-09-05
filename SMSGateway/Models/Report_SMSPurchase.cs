using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSGateway.Models
{
    public class Report_SMSPurchase
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<string> supplier { get; set; }
    }
}