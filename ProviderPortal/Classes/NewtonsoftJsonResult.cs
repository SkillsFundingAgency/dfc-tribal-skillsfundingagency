using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Properties;
using System.Web.Script.Serialization;

// Based on System.Web.Mvc.JsonResult
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    /// <summary>
    /// Represents a class that is used to send JSON-formatted content to the response.
    /// </summary>
    public class NewtonsoftJsonResult : ActionResult
    {
        /// <summary>
        /// Gets or sets the content encoding.
        /// </summary>
        /// 
        /// <returns>
        /// The content encoding.
        /// </returns>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// 
        /// <returns>
        /// The type of the content.
        /// </returns>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// 
        /// <returns>
        /// The data.
        /// </returns>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether HTTP GET requests from the client are allowed.
        /// </summary>
        /// 
        /// <returns>
        /// A value that indicates whether HTTP GET requests from the client are allowed.
        /// </returns>
        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        ///// <summary>
        ///// Gets or sets the recursion limit.
        ///// </summary>
        ///// 
        ///// <returns>
        ///// The recursion limit.
        ///// </returns>
        //public int? RecursionLimit { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.JsonResult"/> class.
        /// </summary>
        public NewtonsoftJsonResult()
        {
            this.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context within which the result is executed.</param><exception cref="T:System.ArgumentNullException">The <paramref name="context"/> parameter is null.</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Bad Method - GET is not permitted for this operation");
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;
            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            response.Write(JsonConvert.SerializeObject(this.Data, Formatting.None));
        }

        public NewtonsoftJsonResult NewtonsoftJson(object data)
        {
            return new NewtonsoftJsonResult
            {
                 JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                 Data = new { data = data }
            };
        }
    }
}