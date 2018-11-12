// ReSharper disable once CheckNamespace

using System.Web.Http;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api
{
    /// <summary>
    ///     The base API controller.
    /// </summary>
    public class BaseApiController : ApiController
    {
        /// <summary>
        ///     The db context.
        /// </summary>
        protected readonly ProviderPortalEntities db = new ProviderPortalEntities();

        /// <summary>
        ///     The dispose API controller.
        /// </summary>
        /// <param name="disposing">
        ///     True when disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}