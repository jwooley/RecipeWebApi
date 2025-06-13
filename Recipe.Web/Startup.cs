using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Recipe.Web.Startup))]

namespace Recipe.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
    }
}
