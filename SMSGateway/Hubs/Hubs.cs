using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSGateway.Hubs
{

    [HubName("UpdateApprovalCount")]
    
   public class UpdateApprovalCount : Hub

    {
        [HubMethodName("SendData")]
        public void Update(int count)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<UpdateApprovalCount>();
            context.Clients.All.sendData(count);
        }
    }
}