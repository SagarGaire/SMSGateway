using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSGateway.Models
{
    public class ReportSMSClientConsumptionAll
    {
		public string ClientCode { get; set; }
		public string ClientName { get; set; }
		public decimal OpeningBalanceCredit { get; set; }
		public decimal OpeningBalanceAmt { get; set; }
		public decimal PurchaseCredit { get; set; }
		public decimal PurchaseAmount { get; set; }
		public decimal ConsumedCredit { get; set; }
		public decimal ConsumedAmt { get; set; }
		public decimal ClosingBalanceCredit { get; set; }
		public decimal ClosingBalanceAmt { get; set; }


		public Nullable<decimal> TotalOpeningBalanceCredit { get; set; }
		public Nullable<decimal> TotalOpeningBalanceAmt { get; set; }
		public Nullable<decimal> TotalPurchaseCredit { get; set; }
		public Nullable<decimal> TotalPurchaseAmt { get; set; }
		public Nullable<decimal> TotalConsumedCredit { get; set; }
		public Nullable<decimal> TotalConsumedAmt { get; set; }
		public Nullable<decimal> TotalClosingBalanceCredit { get; set; }
		public Nullable<decimal> TotalClosingBalanceAmt { get; set; }
	}
}