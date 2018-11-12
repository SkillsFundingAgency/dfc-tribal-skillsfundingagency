using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Web;
using TribalTechnology.InformationManagement.Net.Mail;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public class Examples
    {

        public static void SendingEmailExample()
        { 
            // ***************************************************************************************************
            // Create a normal email object or better use the Tribal one that allows parameter replacements
            //****************************************************************************************************
            
            // Creating a Tribal Email message object. Omit the 'from address' which will then default to the site standard from address (set in ConfigurationSettings table)
            EmailMessage email = new EmailMessage("leigh.carpenter@tribalgroup.com");
            email.Subject = "A subject with a %NAME% place holder";
            email.Body = "Hi %NAME% this is a test email.";

            // Use the AddEmailParameter method to set the replacement text, example
            email.AddEmailParameter("%NAME%", "Peter Jones");  // %NAME% in the subject and body will be replaced by Peter Jones.

            // If required add attachments or make other changes as normal as the Tribal email object inherits the standard one so all options are available
            email.Priority = System.Net.Mail.MailPriority.High;
                                 
            // Send the email, the email is added to the queue and processed asynchronously so control is returned immediately to the caller, and the event log gets a log of the email sent with the parameters           
            // The email is sent immediately
            AppGlobal.EmailQueue.AddToSendQueue(email);    
        }

        public static void SendTemplatedEmailExample()
        {
            // Creating a Tribal Email message object from a template.
            // You can specify a user ID or a MailAddress to send the email to
            // Optionally specify a from address, if none is specified the default site from name and email will be used
            // Optionally specify a list of parameters
            var userId = Permission.GetCurrentUserId();
            EmailMessage email = TemplatedEmail.EmailMessage(userId, Constants.EmailTemplates.Base);

            // Use the AddEmailParameter method to set the replacement text, example
            email.AddEmailParameter("%HTMLBODY%", "Hello world");

            // Send the email, the email is added to the queue and processed asynchronously so control is returned immediately to the caller, and the event log gets a log of the email sent with the parameters           
            // The email is sent immediately
            AppGlobal.EmailQueue.AddToSendQueue(email);

            // Or all-in-one
            AppGlobal.EmailQueue.AddToSendQueue(
                TemplatedEmail.EmailMessage(
                    userId,
                    Constants.EmailTemplates.Base,
                    new List<EmailParameter>
                    {
                        new EmailParameter("%HTMLBODY%", "Hello World")
                    }));
        }

        public static void ResponsiveDataTable()
        {
            // No JavasScript is required, the table will automatically wire itself up
            // <table class="dataTable">
            //   <thead>
            //     <tr><th>...table headers...</th></tr>
            //   </thead>
            //   <tbody>
            //     <tr><td>...table cells</td></tr>
            //   </tbody>
            //  ...optionally <tfoot/> repeating the contents of <thead/>
            // </table>
        }

        public static void EventLogging()
        { 
            // To write an event
            AppGlobal.Log.WriteDebug("A debug message, only shown if logging level is debugging");
            AppGlobal.Log.WriteLog("A standard information message, logged on levels Informational and below");
            AppGlobal.Log.WriteWarning("A warning message, logged on levels Warning and below");
            AppGlobal.Log.WriteError("An error log, logged on levels Error and below");

            // Auditing events
            AppGlobal.WriteAudit("This is an audit success message", true);
            AppGlobal.WriteAudit("This is an audit failure message", false);        
        }

        public static void PermissionCheckExample()
        { 
            // Permissions are checked using the Permission object, permissions are managed and cached to avoid repeated requests back to the
            // database.

            // Check the permission and throw an exception if the user doesn't have the permission
            Permission.HasPermission(true, false, Permission.PermissionName.CanViewHomePage);

            // Check a permission without throwing an exception, this returns true if the user has one or both of the requested permissions
            bool showHomePageLink = Permission.HasPermission(false, false, Permission.PermissionName.CanViewHomePage, Permission.PermissionName.CanViewAdministratorHomePage);

            // Get the current users identity
            var userIdentity = Permission.GetCurrentUserId();
        }

        public static void LanguageSystem()
        {
            string textFromTheLanguageSystem = AppGlobal.Language.GetText("Controller_View_FieldName", "Default text is added if key doesn't exist, then it will be read from the database.");

            // Use Razor with
            //  @AppGlobal.Language.GetText(this, "Heading", "My heading")

        }

    }
}