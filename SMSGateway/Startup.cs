using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SMSGateway.Startup))]
namespace SMSGateway
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);           
        }
    }
}
