using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMSGateway.Models
{
    public class PartialClass
    {
    }

    [MetadataType(typeof(LoginMetadata))]
    public partial class Login
    {
    }

    [MetadataType(typeof(ClientMetadata))]
    public partial class Clients
    {
        [NotMapped]
        public bool isActive { get; set; }
        [NotMapped]
        public string Username { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        public bool isUser { get; set; }
        
   
    }
    public partial class SMSPurchases
    {
        [NotMapped]
        public DateTime CancelledDateFormatted { get; set; }
    }

    [MetadataType(typeof(SafMetadata))]
    public partial class Saf
    {
        [NotMapped]
        public bool _Status { get; set; }
        [NotMapped]
        public string FullName { get; set; }
       
    }
    [MetadataType(typeof(Suppliers))]
    public partial class Suppliers
    {
        [NotMapped]
        public bool _Status { get; set; }
       
    }

    [MetadataType(typeof(PasswordMetadata))]
    public partial class Password
    {

    }

    //public partial class spReportOutgoingSMS_Result
    //{
    //    [NotMapped]
    //    public DateTime FormattedDateTime { get; set; }
    //}

    public partial class spReportSMSStmt_Result
    {
        [NotMapped]
        public DateTime FormattedDateTime { get; set; }
        [NotMapped]
        public string FormattedDebitSMS { get; set; }
        [NotMapped]
        public string FormattedCreditSMS { get; set; }
        [NotMapped]
        public string FormattedDebitAmt { get; set; }
        [NotMapped]
        public string FormattedCreditAmt { get; set; }
        [NotMapped]
        public string FormattedSMSBal { get; set; }
        [NotMapped]
        public string FormattedBalance { get; set; }
    }

    public partial class Refills
    {
        [NotMapped]
        public decimal Amount { get; set; }
    }

    public partial class spRefillReport_Result
    {
   
       

       
       
    }

    public partial class OutgoingSMSReport
    {
        [NotMapped]
        public DateTime FormattedDateTime { get; set; }
    
    }

   
}