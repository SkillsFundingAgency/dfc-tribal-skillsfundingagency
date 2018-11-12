using System;
using System.Configuration;
using System.Web.UI;
using Ims.Schemas.Alse.CourseSearch.Contract;
using System.Text;

namespace IMS.NCS.CourseSearchService.TestHarness
{
    public partial class ProviderSearchCriteriaCtrl : UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            phAPIKey.Visible = !ConfigurationManager.AppSettings[Constants.ConfigurationKeys.Environment].Contains("NCS Search");
        }

        /// <summary>
        /// Populates the page controls with the values from the search criteria.
        /// </summary>
        /// <param name="criteria">The populated search criteria.</param>
        public void PopulatePage(SearchCriteriaStructure criteria)
        {
            const string scriptName = "PopulateCriteria";
            Type thisType = this.GetType();

            ClientScriptManager mgr = Page.ClientScript;
            if (!mgr.IsStartupScriptRegistered(thisType, scriptName))
            {
                StringBuilder scriptText = new StringBuilder();
                scriptText.Append("<script type=text/javascript>" + Environment.NewLine);

                // API Key
                if (phAPIKey.Visible)
                {
                    scriptText.Append(Utilities.GetJavascriptSetValueSnippet("APIKey", criteria.APIKey));
                }

                // PROVIDER
                scriptText.Append(Utilities.GetJavascriptSetValueSnippet("Provider", criteria.ProviderKeyword));

                // end
                scriptText.Append("</script>");

                mgr.RegisterStartupScript(thisType, scriptName, scriptText.ToString());
            }
        }
    }
}