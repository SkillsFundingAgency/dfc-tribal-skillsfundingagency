using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;
using TribalTechnology.InformationManagement.Net.Mail;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    [PermissionAuthorize(Permission.PermissionName.CanEditEmailTemplates)]
    [ContextAuthorize(UserContext.UserContextName.Administration)]
    public class EmailTemplateController : BaseController
    {
        // GET: /TribalAdmin/EmailTemplate/
        public ActionResult Index()
        {
            var emailtemplates = db.EmailTemplates.Include(e => e.EmailTemplateGroup);
            return View(emailtemplates.ToList());
        }

        // GET: /TribalAdmin/EmailTemplate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailTemplate emailtemplate = db.EmailTemplates.Find(id);
            if (emailtemplate == null)
            {
                return HttpNotFound();
            }
            var model = new EmailTemplateViewModel(emailtemplate);
            return View(model);
        }

        // GET: /TribalAdmin/EmailTemplate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailTemplate emailtemplate = db.EmailTemplates.Find(id);
            if (emailtemplate == null)
            {
                return HttpNotFound();
            }
            var model = new EmailTemplateViewModel(emailtemplate);
            return View(model);
        }

        // POST: /TribalAdmin/EmailTemplate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(EmailTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var emailTemplate = model.ToEntity(db);
                db.SaveChanges();
                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendTestEmail(int id)
        {
            EmailTemplate emailtemplate = db.EmailTemplates.Find(id);
            if (emailtemplate == null)
            {
                return HttpNotFound();
            }

            //AppGlobal.EmailQueue.AddToSendQueue(
            //    TemplatedEmail.EmailMessage(
            //        Permission.GetCurrentUserId(),
            //        (Constants.EmailTemplates)id,
            //        new List<EmailParameter>()));

            var emailMessage = TemplatedEmail.EmailMessage(
                    Permission.GetCurrentUserId(),
                    (Constants.EmailTemplates)id,
                    new List<EmailParameter>());
            var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null);

            return RedirectToAction("Edit", new {id = id});
        }
    }
}
