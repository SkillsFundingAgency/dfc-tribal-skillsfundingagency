using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using IMS.NCS.CourseSearchService.TestHarness.Models;
using Ims.Schemas.Alse.CourseSearch.Contract;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace IMS.NCS.CourseSearchService.TestHarness
{
    public partial class ProviderDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // check for query string
                string providerId = Page.Request.QueryString["providerid"];
                String APIKey = Page.Request.QueryString["APIKey"];
                if (!string.IsNullOrEmpty(providerId))
                {
                    PopulateData(providerId, APIKey);
                }
            }
            else
            {
                // need to remove the viewstate for cross page redirects, otherwise we'll get a MAC validation error
                NameValueCollection newQueryString = Utilities.RemoveViewState(Page.Request.QueryString);
                Response.Redirect("ProviderSearch.aspx" + Utilities.CreateQueryString(newQueryString));
            }
        }


        /// <summary>
        /// Call the search and populate the table with the (hopefully) found Provider detail.
        /// </summary>
        /// <param name="providerID">The id of the Provider to display.</param>
        private void PopulateData(string providerID, String APIKey)
        {
            ServiceInterface client = new ServiceInterfaceClient("CourseSearchService");
            ProviderDetailsInput request = new ProviderDetailsInput(providerID, APIKey);

            try
            {
                ProviderDetailsOutput response = client.ProviderDetails(request);

                // make sure we start with a clear table
                ProviderDetailTable.Rows.Clear();

                if (response.ProviderDetails != null)
                {
                    //ResultsOverviewLabel.Text = response.ProviderDetails.ProviderName;

                    // add header
                    TableHeaderCell headerCell = new TableHeaderCell();
                    headerCell.Text = response.ProviderDetails.ProviderName;
                    headerCell.ColumnSpan = 2;
                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(headerCell);
                    ProviderDetailTable.Rows.Add(headerRow);

                    // add the rows of provider data
                    ProviderDetailTable.Rows.Add(CreateTableRow("Provider name:", response.ProviderDetails.ProviderName));
                    ProviderDetailTable.Rows.Add(CreateTableRow("Provider ID:", response.ProviderDetails.ProviderID));
                    ProviderDetailTable.Rows.Add(CreateTableRow("UKPRN:", response.ProviderDetails.UKPRN));
                    ProviderDetailTable.Rows.Add(CreateTableRow("UPIN:", response.ProviderDetails.UPIN));
                    ProviderDetailTable.Rows.Add(CreateTableRow("Address Line 1:", response.ProviderDetails.ProviderAddress.Address_line_1));
                    ProviderDetailTable.Rows.Add(CreateTableRow("Address Line 2:", response.ProviderDetails.ProviderAddress.Address_line_2));
                    ProviderDetailTable.Rows.Add(CreateTableRow("Town:", response.ProviderDetails.ProviderAddress.Town));
                    ProviderDetailTable.Rows.Add(CreateTableRow("County:", response.ProviderDetails.ProviderAddress.County));
                    ProviderDetailTable.Rows.Add(CreateTableRow("Postcode:", response.ProviderDetails.ProviderAddress.PostCode));
                    ProviderDetailTable.Rows.Add(CreateTableRow("Phone:", response.ProviderDetails.Phone));
                    ProviderDetailTable.Rows.Add(CreateTableRow("Fax:", response.ProviderDetails.Fax));
                    ProviderDetailTable.Rows.Add(CreateTableRow("Email:", response.ProviderDetails.Email));
                    ProviderDetailTable.Rows.Add(CreateTableRow("Website:", response.ProviderDetails.Website));
                    ProviderDetailTable.Rows.Add(CreateTableRow("24+ Loans:", response.ProviderDetails.TFPlusLoans.ToString()));
                    ProviderDetailTable.Rows.Add(CreateTableRow("DFE Provider:", response.ProviderDetails.DFE1619Funded.ToString()));
                }
            }
            catch (Exception ex)
            {
                ResultsOverviewLabel.Text = ex.Message + "/n/n" + ex.StackTrace;
            }
        }


        /// <summary>
        /// Helper method to create a row of data to display.
        /// </summary>
        /// <param name="title">The title of the row.</param>
        /// <param name="value">The value of the data.</param>
        /// <returns>A populated TableRow.</returns>
        private TableRow CreateTableRow(string title, string value)
        {
            TableRow providerRow = new TableRow();
            TableCell titleCell = new TableCell();
            TableCell valueCell = new TableCell();

            titleCell.Text = title;
            valueCell.Text = value;

            providerRow.Cells.Add(titleCell);
            providerRow.Cells.Add(valueCell);

            return providerRow;
        }
    }
}
