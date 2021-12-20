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
        public int Recid { get; set; }
        public int Supplier { get; set; }
        public string SupplierName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string EntryBy { get; set; }
        public string BillNo { get; set; }
        public string Remarks { get; set; }
    }
}