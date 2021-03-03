using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SMSGateway.Models
{
    public class Metadata
    {
    }

    public class LoginMetadata
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class ClientMetadata
    {
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }
        [Required]
        [Display(Name = "Client Name")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Passkey")]
        public string PassKey { get; set; }
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string EmailId { get; set; }
        [Display(Name = "SMS Notification")]
        public string SMSNotification { get; set; }
        [Display(Name = "Email Notification")]
        public string EmailNotification { get; set; }
        [Display(Name = "Post Billing")]
        public string PostBilling { get; set; }
        [Display(Name = "Monthly Limit")]
        public string MonthlyLimit { get; set; }
    }

    public class SafMetadata
    {
        [Display(Name = "Status")]
        public string _Status { get; set; }
        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; }
        [Display(Name = "Email")]
        public string EmailId { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }

    public class PasswordMetadata
    {
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class spReportOutgoingSMS_Result1MetaData
    {

    }
}