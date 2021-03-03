using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSGateway.Models
{
    public partial class Refill_Report
    {

        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

        public List<string> client { get; set; }

    }
}