//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMSGateway.Models
{
    using System;
    
    public partial class spRefillReport_Result
    {
        public Nullable<long> PageNo { get; set; }
        public Nullable<int> RefillId { get; set; }
        public string ClientCode { get; set; }
        public Nullable<decimal> RefillValue { get; set; }
        public string RefillBy { get; set; }
        public string Remarks { get; set; }
        public string CancelledBy { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
        public Nullable<System.DateTime> RefillDate { get; set; }
        public Nullable<decimal> RefillRate { get; set; }
        public string BillNo { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> RefillAmount { get; set; }
    }
}
