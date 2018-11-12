using System;
using System.Configuration;
using System.IO;
using System.Web.UI;
using Ims.Schemas.Alse.CourseSearch.Contract;

namespace IMS.NCS.CourseSearchService.Monitoring
{
    public partial class LoadTest : Page
    {
        DateTime lastRequest = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(String.Format("Started at {0}<br />", DateTime.Now.ToString("dd/MM/yyyy hh:mm")));
            String[] searchTerms = { "Plumbing", "Hairdressing", "Maths", "English", "Computing", "Welding", "Driving", "Religious", "CAD", "Gaelic", "Spanish", "French", "Italian", "German", "Introduction", "GCSE", "Advanced", "Level", "Admin" };
            String[] locations = { "Sheffield", "London", "Doncaster", "Nottingham", "Birmingham", "Swansea", "Manchester", "Newcastle", "York", "Glasgow", "Watford", "Derby", "Leicester", "Leeds", "Norwich", "Cardiff", "Bristol", "Coventry", "Luton", "Southampton", "Brighton", "Plymouth", "Devon", "Yorkshire", "Derbyshire", "Kent", "Essex" };
            foreach (String searchTerm in searchTerms)
            {
                foreach (String location in locations)
                {
                    Int32 pn = ProcessRequest(searchTerm, location, 1);
                    for (Int32 pageNumber = 2; pageNumber <= pn; pageNumber++)
                    {
                        ProcessRequest(searchTerm, location, pageNumber);
                    }
                }
            }
            Response.Write(String.Format("Ended at {0}<br />", DateTime.Now.ToString("dd/MM/yyyy hh:mm")));
        }

        protected Int32 ProcessRequest(String searchTerm, String location, Int32 pageNumber)
        {
            //while (lastRequest.AddTicks(100000000 * 2) <= DateTime.Now)
            //{
            //    // Wait at least 2/10th of a second
            //}
            //lastRequest = DateTime.Now;

            ServiceInterface client = new ServiceInterfaceClient("CourseSearchService");

            CourseListRequestStructure listRequestStructure = new CourseListRequestStructure
            {
                CourseSearchCriteria = new SearchCriteriaStructure
                {
                    //APIKey
                    APIKey = ConfigurationManager.AppSettings["APIKey"],
                    // SUBJECT
                    SubjectKeyword = searchTerm,
                    Location = location
                },
                RecordsPerPage = "10",
                PageNo = pageNumber.ToString(),
                SortBy = SortType.D,
                SortBySpecified = true
            };

            CourseListInput request = new CourseListInput(listRequestStructure);
            try
            {
                CourseListOutput output = client.CourseList(request);
                WriteLog(String.Format("Term:{0} Location:{1} PageNum:{2} Results:{3}", searchTerm, location, pageNumber, output.CourseListResponse.ResultInfo.NoOfRecords));
                return String.IsNullOrEmpty(output.CourseListResponse.ResultInfo.NoOfPages) ? 0 : Convert.ToInt32(output.CourseListResponse.ResultInfo.NoOfPages);
            }
            catch (Exception ex)
            {
                WriteLog(String.Format("Term:{0} Location:{1} PageNum:{2} Error:{3}", searchTerm, location, pageNumber, ex.Message));
            }

            return 0;
        }

        private void WriteLog(String msg)
        {
            try
            {
                File.AppendAllText("LoadTest.txt", msg + "\r\n");
            }
            catch { }
            Response.Write(msg + "<br />");
        }
    }
}