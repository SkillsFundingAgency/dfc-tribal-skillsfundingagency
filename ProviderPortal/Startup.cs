using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tribal.SkillsFundingAgency.ProviderPortal.Startup))]
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
