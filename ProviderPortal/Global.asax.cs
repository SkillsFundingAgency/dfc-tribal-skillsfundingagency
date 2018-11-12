using System;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public class MvcApplication : HttpApplication
    {
        #region KeepAlive

        // IIS has a worker idle timeout which defaults to 20 minutes.
        // If no requests are made on the server for 20 minutes the thread is shut down
        // This closes the thread running the timer(s) and the automated tasks stop running
        // To prevent this we create a background thread which requests the home page every 15 minutes
        static readonly Thread keepAliveThread = new Thread(KeepAlive);
        private static String homePageUrl = "";

        static void KeepAlive()
        {
            while (true)
            {
                if (String.IsNullOrWhiteSpace(homePageUrl))
                {
                    Thread.Sleep(60000);
                }
                else
                {
                    try
                    {
                        WebRequest req = WebRequest.Create(homePageUrl);
                        req.GetResponse();
                        Thread.Sleep(60000 * 15); // 15 minutes
                    }
                    catch (ThreadAbortException)
                    {
                        break;
                    }
                }
            }
        }

        #endregion

        void Application_BeginRequest(Object source, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(homePageUrl))
            {
                homePageUrl = String.Format("{0}://{1}/", Context.Request.Url.Scheme, Context.Request.Url.Authority);
                keepAliveThread.Start();
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;

            // Register Client Side Validation for Custom Regular Expression Attributes
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(ProviderPortalUrl), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(ProviderPortalPostcode), typeof(RegularExpressionAttributeAdapter));

            // Initialise other site tasks
            SiteStart.Initialise();
        }

        private void Application_End(object sender, EventArgs e)
        {
            // Shut down the Log cleanly and log reason for site shutdown
            AppGlobal.Log.WriteWarning(string.Concat("Site unloading. Reason: ",
                HostingEnvironment.ShutdownReason.ToString()));
            if (SiteStart.larsTimer != null)
            {
                SiteStart.larsTimer.Dispose();
            }
            AppGlobal.Log.Dispose();

            if (keepAliveThread.IsAlive)
            {
                keepAliveThread.Abort();
            }
        }

        private void Application_AcquireRequestState()
        {
            SessionManager.DetectNew();
        }

        private void Session_Start()
        {
            SessionManager.Start();
        }

        private void Session_End()
        {
            SessionManager.End();
        }
    }
}