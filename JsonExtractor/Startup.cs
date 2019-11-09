using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JsonExtractor.Startup))]
namespace JsonExtractor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          //  ConfigureAuth(app);
        }
    }
}
