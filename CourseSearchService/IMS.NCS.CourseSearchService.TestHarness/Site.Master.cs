using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.ServiceModel.Configuration;
using System.Web.Configuration;


namespace IMS.NCS.CourseSearchService.TestHarness
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblEnvironment.Text = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.Environment];
            lblUrl.Text = GetUrl();

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            lblVersion.Text = currentAssembly.GetName().Version.ToString();
        }

        private static string GetUrl()
        {
            ClientSection client = (ClientSection)WebConfigurationManager.GetSection("system.serviceModel/client");
            return client == null ? "nvc Is Null" : client.Endpoints != null ? client.Endpoints[0].Address.ToString() : "";
        }
    }
}
