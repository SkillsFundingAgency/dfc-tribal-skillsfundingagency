using IMS.NCS.CourseSearchService.DatabaseContext;
using System;
using System.Linq;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    [PermissionAuthorize(Permission.PermissionName.CanEditStopWords)]
    [ContextAuthorize(UserContext.UserContextName.Administration)]
    public class StopWordController : BaseController
    {
        // GET: StopWord
        public ActionResult Index()
        {
            ListStopWordsModel model = new ListStopWordsModel();
            SFA_SearchAPIEntities searchAPIEntities = new SFA_SearchAPIEntities();
            foreach (String stopWord in searchAPIEntities.GetStopWords())
            {
                model.Items.Add(new ListStopWordsItemModel(stopWord));
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddStopWordModel model = new AddStopWordModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(AddStopWordModel model)
        {
            SFA_SearchAPIEntities searchAPIEntities = new SFA_SearchAPIEntities();
            if (searchAPIEntities.GetStopWords().Any(x => x == model.StopWord))
            {
                ModelState.AddModelError("StopWord", AppGlobal.Language.GetText(this, "StopWordAlreadyExists", "Stop word already exists"));
            }

            if (ModelState.IsValid)
            {
                searchAPIEntities.Configuration.EnsureTransactionsForFunctionsAndCommands = false; // Cannot be in a transaction
                searchAPIEntities.AddStopWord(model.StopWord.Trim());
                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(String stopword)
        {
            SFA_SearchAPIEntities searchAPIEntities = new SFA_SearchAPIEntities();
            searchAPIEntities.Configuration.EnsureTransactionsForFunctionsAndCommands = false; // Cannot be in a transaction
            searchAPIEntities.DropStopWord(stopword);

            ShowGenericSavedMessage();

            return RedirectToAction("Index");
        }
    }
}