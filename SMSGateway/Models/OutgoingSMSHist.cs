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
    using System.Collections.Generic;
    
    public partial class OutgoingSMSHist
    {
        public int MsgId { get; set; }
        public System.DateTime MsgDateTime { get; set; }
        public string ClientCode { get; set; }
        public string Recipients { get; set; }
        public Nullable<int> RecipientCount { get; set; }
        public string MsgText { get; set; }
        public int Status { get; set; }
        public Nullable<int> Parts { get; set; }
        public string RequestCode { get; set; }
        public string RequestIP { get; set; }
        public string RegId { get; set; }
        public Nullable<int> CreditsUsed { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Balance { get; set; }
    }
}
