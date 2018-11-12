using System.Web.Optimization;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Content/jquery-ui-1.11.2/jquery-ui.min.js",
                "~/Scripts/jquery-ui.multidatespicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/bootstrap-dialog.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                //// "~/Scripts/DataTables-1.10.2/jquery.dataTables.js",
                //// "~/Scripts/DataTables-1.10.2/dataTables.*",
                //// "~/Scripts/Datatables-Bootstrap3/datatables.*"
                ////----
                "~/Scripts/DataTables-1.10.2/jquery.dataTables.js",
                "~/Scripts/DataTables-1.10.2/dataTables.autoFill.js",
                "~/Scripts/DataTables-1.10.2/dataTables.bootstrap.js",
                "~/Scripts/DataTables-1.10.2/dataTables.colReorder.js",
                "~/Scripts/DataTables-1.10.2/dataTables.colVis.js",
                "~/Scripts/DataTables-1.10.2/dataTables.fixedColumns.js",
                "~/Scripts/DataTables-1.10.2/dataTables.fixedHeader.js",
                "~/Scripts/DataTables-1.10.2/dataTables.foundation.js",
                ////"~/Scripts/DataTables-1.10.2/dataTables.jqueryui.js",
                "~/Scripts/DataTables-1.10.2/dataTables.keyTable.js",
                "~/Scripts/DataTables-1.10.2/dataTables.responsive.js",
                "~/Scripts/DataTables-1.10.2/dataTables.scroller.js",
                "~/Scripts/DataTables-1.10.2/dataTables.tableTools.js",
                "~/Scripts/Datatables-Bootstrap3/datatables.*",
                "~/Scripts/Datatables-Plugins/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include(
                "~/Scripts/twitter-typeahead/typeahead.bundle.js"));

            // Load TinyMCE directly as it does not survive the bundling process
            //bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
            //    "~/Scripts/tinymce/tinymce.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                //// This should be the last script
                "~/Scripts/Site.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/DataTables-1.10.2/css/dataTables.bootstrap.css",
                "~/Content/DataTables-1.10.2/css/dataTables.responsive.css",
                "~/Content/DataTables-1.10.2/css/dataTables.tableTools.css",
                "~/Content/DataTables-1.10.2/css/dataTables.colVis.css",
                "~/Content/Datatables-Bootstrap3/css/datatables.css",
                "~/Content/bootstrap-dialog.css",
                "~/Content/twitter-typeahead/typeahead.css",
                "~/Content/jquery-ui-1.11.2/jquery-ui.min.css",
                "~/Content/Site.css",
                "~/Content/UserWizard/css/modal.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}