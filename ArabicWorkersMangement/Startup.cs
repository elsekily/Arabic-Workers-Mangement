using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArabicWorkersMangement.Startup))]
namespace ArabicWorkersMangement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
