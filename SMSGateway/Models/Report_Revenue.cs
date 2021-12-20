using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSGateway.Models
{
    public class Report_Revenue
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Supplier { get; set; }
        public string SupplierName { get; set; }
        public string SubType { get; set; }
        public decimal OpeningCredits { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal CreditsPurchases { get; set; }
        public decimal AmountPurchases { get; set; }
        public decimal CreditsConsumed { get; set; }
        public decimal AmountConsumed { get; set; }
        public decimal ClosingCredit { get; set; }
        public decimal ClosingAmount { get; set; }
        public decimal CostOfCredits { get; set; }
        public decimal Revenue { get; set; }
    }
}