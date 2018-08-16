using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NFCZavrsniWeb.Startup))]
namespace NFCZavrsniWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
