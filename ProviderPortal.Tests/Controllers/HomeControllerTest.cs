using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tribal.SkillsFundingAgency.ProviderPortal;
using Tribal.SkillsFundingAgency.ProviderPortal.Controllers;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Tests.Controllers
{
    using System.Security.Principal;
    using System.Threading;

    [TestClass]
    public class HomeControllerTest
    {
        /// <summary>
        /// The set user identity name.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        public static void SetUserIdentityName(string userId)
        {
            IPrincipal principal = null;
            principal = new GenericPrincipal(
            new GenericIdentity(userId),
            new string[0]);
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        //[TestMethod]
        //public void About()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.About() as ViewResult;

        //    // Assert
        //    Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        //}

        //[TestMethod]
        //public void Contact()
        //{
        //    var httpRequest = new HttpRequest(string.Empty, "http://mySomething/", string.Empty);
        //    var stringWriter = new StringWriter();
        //    var httpResponce = new HttpResponse(stringWriter);
        //    var httpContext = new HttpContext(httpRequest, httpResponce);

        //    var sessionContainer = new HttpSessionStateContainer(
        //        "id",
        //        new SessionStateItemCollection(),
        //        new HttpStaticObjectsCollection(),
        //        20,
        //        true,
        //        HttpCookieMode.AutoDetect,
        //        SessionStateMode.InProc,
        //        false);

        //    httpContext.Items["AspSession"] =
        //        typeof(HttpSessionState).GetConstructor(
        //            BindingFlags.NonPublic | BindingFlags.Instance,
        //            null,
        //            CallingConventions.Standard,
        //            new[] { typeof(HttpSessionStateContainer) },
        //            null).Invoke(new object[] { sessionContainer });

        //    httpContext.Session.Add(Constants.SessionFieldNames.UserName, "leigh.carpenter@tribalgroup.com");
        //    httpContext.Session.Add(Constants.SessionFieldNames.UserId, "694b963b-d200-4742-a0f5-2fbdd5168ea1");
        //    httpContext.Session.Add(
        //        Constants.SessionFieldNames.Permissions,
        //        new System.Collections.Generic.List<int>() { 1 });

        //    HttpContext.Current = httpContext;
        //    SetUserIdentityName("leigh.carpenter@tribalgroup.com");

        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.Contact() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);

        //    if (result != null)
        //    {
        //        // Check if no permission that request is refused, should get a security exception
        //        httpContext.Session.Remove(Constants.SessionFieldNames.Permissions);
        //        httpContext.Session.Add(
        //            Constants.SessionFieldNames.Permissions,
        //            new System.Collections.Generic.List<int> { 2 });  // 2 isn't a permission allowed to access this controller

        //        System.Exception ex = null;
        //        try
        //        {
        //            result = controller.Contact() as ViewResult;
        //        }
        //        catch (System.Exception thrownEx)
        //        {
        //            ex = thrownEx as System.Exception;
        //        }
        //        Assert.IsNotNull(ex);
        //    }
        //}
    }
}
