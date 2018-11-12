using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    [GoHomeOnInvalidUserContext]
    [SessionAuthorize]
    public class BaseController : Controller
    {
        protected readonly ProviderPortalEntities db = new ProviderPortalEntities();
        protected readonly UserContext.UserContextInfo userContext = UserContext.GetUserContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Message helpers

        /// <summary>
        /// Shows the generic data saved message on the next page load.
        /// </summary>
        public void ShowGenericSavedMessage()
        {
            ShowGenericSavedMessage(false);
        }

        /// <summary>
        /// Shows the generic data saved message on the next page load, surviving a redirect.
        /// </summary>
        public void ShowGenericSavedMessage(bool surviveRedirect)
        {
            var message = AppGlobal.Language.GetText(this, "SaveSuccessful", "Your changes were saved successfully.");
            SessionMessage.SetMessage(message, SessionMessageType.Success, surviveRedirect ? 2 : 1);
        }

        #endregion

        #region Overridden Json methods

        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.JsonResult"/> object that serializes the specified object to JavaScript Object Notation (JSON).
        /// </summary>
        /// 
        /// <returns>
        /// The JSON result object that serializes the specified object to JSON format. The result object that is prepared by this method is written to the response by the ASP.NET MVC framework when the object is executed.
        /// </returns>
        /// <param name="data">The JavaScript object graph to serialize.</param>
        protected new NewtonsoftJsonResult Json(object data)
        {
            return this.Json(data, (string)null, (Encoding)null, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.JsonResult"/> object that serializes the specified object to JavaScript Object Notation (JSON) format.
        /// </summary>
        /// 
        /// <returns>
        /// The JSON result object that serializes the specified object to JSON format.
        /// </returns>
        /// <param name="data">The JavaScript object graph to serialize.</param><param name="contentType">The content type (MIME type).</param>
        protected new NewtonsoftJsonResult Json(object data, string contentType)
        {
            return this.Json(data, contentType, (Encoding)null, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.JsonResult"/> object that serializes the specified object to JavaScript Object Notation (JSON) format.
        /// </summary>
        /// 
        /// <returns>
        /// The JSON result object that serializes the specified object to JSON format.
        /// </returns>
        /// <param name="data">The JavaScript object graph to serialize.</param><param name="contentType">The content type (MIME type).</param><param name="contentEncoding">The content encoding.</param>
        protected new virtual NewtonsoftJsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return this.Json(data, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Creates a JsonResult object that serializes the specified object to JavaScript Object Notation (JSON) format using the specified JSON request behavior.
        /// </summary>
        /// 
        /// <returns>
        /// The result object that serializes the specified object to JSON format.
        /// </returns>
        /// <param name="data">The JavaScript object graph to serialize.</param><param name="behavior">The JSON request behavior.</param>
        protected new NewtonsoftJsonResult Json(object data, JsonRequestBehavior behavior)
        {
            return this.Json(data, (string)null, (Encoding)null, behavior);
        }

        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.JsonResult"/> object that serializes the specified object to JavaScript Object Notation (JSON) format using the specified content type and JSON request behavior.
        /// </summary>
        /// 
        /// <returns>
        /// The result object that serializes the specified object to JSON format.
        /// </returns>
        /// <param name="data">The JavaScript object graph to serialize.</param><param name="contentType">The content type (MIME type).</param><param name="behavior">The JSON request behavior</param>
        protected new NewtonsoftJsonResult Json(object data, string contentType, JsonRequestBehavior behavior)
        {
            return this.Json(data, contentType, (Encoding)null, behavior);
        }

        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.JsonResult"/> object that serializes the specified object to JavaScript Object Notation (JSON) format using the content type, content encoding, and the JSON request behavior.
        /// </summary>
        /// 
        /// <returns>
        /// The result object that serializes the specified object to JSON format.
        /// </returns>
        /// <param name="data">The JavaScript object graph to serialize.</param><param name="contentType">The content type (MIME type).</param><param name="contentEncoding">The content encoding.</param><param name="behavior">The JSON request behavior</param>
        protected new virtual NewtonsoftJsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new NewtonsoftJsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        #endregion

    }
}