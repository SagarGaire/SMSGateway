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
    
    public partial class IncomingSMS
    {
        public int MsgId { get; set; }
        public System.DateTime MsgDateTime { get; set; }
        public string MsgFrom { get; set; }
        public string MsgTo { get; set; }
        public string KeyWord { get; set; }
        public string MsgText { get; set; }
    }
}
