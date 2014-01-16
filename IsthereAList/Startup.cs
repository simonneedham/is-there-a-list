using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IsThereAList.Startup))]
namespace IsThereAList
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
