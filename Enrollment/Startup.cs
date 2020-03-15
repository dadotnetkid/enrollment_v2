using Microsoft.Owin;
using Models.Startups;
using Owin;

[assembly: OwinStartupAttribute(typeof(Enrollment.Startup))]
namespace Enrollment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Authentication.ConfigureAuth(app);
        }
    }
}
