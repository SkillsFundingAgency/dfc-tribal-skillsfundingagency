using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ims.Schemas.Alse.CourseSearch.Contract;

namespace IMS.NCS.CourseSearchService.TestHarness
{
    public partial class CategoryList : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                // get the search criteria so we know what we're trying to find
                SubjectBrowseInput criteria = CreateSearchCriteria(Page.Request.QueryString);

                // call the search and populate our results table
                PopulateData(criteria);

                // once we've displayed our results we need to set the display back to the
                // criteria values
                SearchControl.PopulatePage(criteria);
            }
        }

        private static SubjectBrowseInput CreateSearchCriteria(NameValueCollection requestData)
        {
            SubjectBrowseInput criteria = new SubjectBrowseInput
            {
                APIKey = Utilities.GetQueryStringValue(requestData, "APIKey") ?? String.Empty
            };

            return criteria;
        }

        private void PopulateData(SubjectBrowseInput request)
        { 
            ServiceInterface client = new ServiceInterfaceClient("CourseSearchService");

            try
            {
                SubjectBrowseOutput response = client.GetCategories(request);

                foreach (Level1 level1 in response.Level1)
                {
                    // start Level 1
                    string level1Text = CreateLevelText(level1.LDCS.LDCSCode, level1.LDCS.LDCSDesc, level1.LDCS.Searchable.ToString(), level1.CourseCounts);
                    TreeNode level1TreeNode = new TreeNode(level1Text);
                    level1TreeNode.SelectAction = TreeNodeSelectAction.None;
                            
                    if (level1.Level2 != null)
                    {
                        foreach (Level2 level2 in level1.Level2)
                        {
                            // start Level 2
                            string level2Text = CreateLevelText(level2.LDCS.LDCSCode, level2.LDCS.LDCSDesc, level2.LDCS.Searchable.ToString(), level2.CourseCounts);
                            TreeNode level2TreeNode = new TreeNode(level2Text);
                            level2TreeNode.SelectAction = TreeNodeSelectAction.None;
                            if (level2.Level3 != null)
                            {
                                foreach (CategoryInfo level3 in level2.Level3)
                                {
                                    string level3Text = CreateLevelText(level3.LDCS.LDCSCode, level3.LDCS.LDCSDesc, level3.LDCS.Searchable.ToString(), level3.CourseCounts);
                                    TreeNode level3TreeNode = new TreeNode(level3Text);
                                    level3TreeNode.NavigateUrl="~\\CourseSearch.aspx?ldcscategorycodes=" + level3.LDCS.LDCSCode + "&APIKey=" + request.APIKey + "&action=search&recordsperpage=10";
                                    // add the level 3 node to the level 2 node
                                    level2TreeNode.ChildNodes.Add(level3TreeNode);
                                }
                            }

                            // add level 2 node to the level 1 node
                            level1TreeNode.ChildNodes.Add(level2TreeNode);
                        }
                    }

                    // add the level 1 node
                    TreeView1.Nodes.Add(level1TreeNode);
                }
                   
                TreeView1.DataBind();
            }
            catch (Exception ex)
            {
                ResultsOverviewLabel.Text = ex.Message + "/n/n" + ex.StackTrace;
            }
        }


        private string CreateLevelText(string code, string description, string searchable, string count)
        {
            return code + " - " + description + " (" + count + ")";
        }
    }
}
