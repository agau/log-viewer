using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LogViewer.Startup))]
namespace LogViewer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
