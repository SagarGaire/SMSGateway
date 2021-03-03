using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSGateway.Models
{
    public partial class Report_OutgoingSMS
    {        
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Recipients { get; set; }
        public List<string> Status { get; set; }
        public List<string> Clients { get; set; }
    }
}