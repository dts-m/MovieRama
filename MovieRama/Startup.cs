using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieRama.Startup))]
namespace MovieRama
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
