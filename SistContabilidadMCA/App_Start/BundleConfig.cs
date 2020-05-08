using System.Web;
using System.Web.Optimization;

namespace SistContabilidadMCA
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/jqwidgetsjs").Include(
              "~/Scripts/plugins/jqwidgets/jqxcore.js",
              "~/Scripts/plugins/jqwidgets/jqxdata.js",
              "~/Scripts/plugins/jqwidgets/jqxgrid.js",
              "~/Scripts/plugins/jqwidgets/jqxgrid.selection.js",
              "~/Scripts/plugins/jqwidgets/jqxgrid.pager.js",
              "~/Scripts/plugins/jqwidgets/jqxlistbox.js",
              "~/Scripts/plugins/jqwidgets/jqxbuttons.js",
              "~/Scripts/plugins/jqwidgets/jqxscrollbar.js",
              "~/Scripts/plugins/jqwidgets/jqxdatatable.js",
              "~/Scripts/plugins/jqwidgets/jqxtreegrid.js",
              "~/Scripts/plugins/jqwidgets/jqxmenu.js",
              "~/Scripts/plugins/jqwidgets/jqxcalendar.js",
              "~/Scripts/plugins/jqwidgets/jqxgrid.sort.js",
              "~/Scripts/plugins/jqwidgets/jqxgrid.filter.js",
              "~/Scripts/plugins/jqwidgets/jqxdatetimeinput.js",
              "~/Scripts/plugins/jqwidgets/jqxdropdownlist.js",
              "~/Scripts/plugins/jqwidgets/jqxslider.js",
              "~/Scripts/plugins/jqwidgets/jqxeditor.js",
              "~/Scripts/plugins/jqwidgets/jqxinput.js",
              "~/Scripts/plugins/jqwidgets/jqxdraw.js",
              "~/Scripts/plugins/jqwidgets/jqxchart.core.js",
              "~/Scripts/plugins/jqwidgets/jqxchart.rangeselector.js",
              "~/Scripts/plugins/jqwidgets/jqxtree.js",
              "~/Scripts/plugins/jqwidgets/globalize.js",
              "~/Scripts/plugins/jqwidgets/jqxbulletchart.js",
              "~/Scripts/plugins/jqwidgets/jqxcheckbox.js",
              "~/Scripts/plugins/jqwidgets/jqxradiobutton.js",
              "~/Scripts/plugins/jqwidgets/jqxvalidator.js",
              "~/Scripts/plugins/jqwidgets/jqxpanel.js",
              "~/Scripts/plugins/jqwidgets/jqxpasswordinput.js",
              "~/Scripts/plugins/jqwidgets/jqxnumberinput.js",
              "~/Scripts/plugins/jqwidgets/jqxcombobox.js",
              "~/Scripts/plugins/jqwidgets/jqx-all.js"
              ));
            bundles.Add(new StyleBundle("~/jqwidgetscss").Include(
            "~/Scripts/plugins/jqwidgets/styles/jqx.base.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.arctic.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.black.css",
            "~/Scripts/plugins/jqwidgets/styles/jqx.bootstrap.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.classic.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.darkblue.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.energyblue.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.fresh.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.highcontrast.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.metro.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.metrodark.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.office.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.orange.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.shinyblack.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.summer.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.web.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.ui-darkness.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.ui-lightness.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.ui-le-frog.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.ui-overcast.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.ui-redmond.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.ui-smoothness.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.ui-start.css",
            //"~/Scripts/plugins/jqwidgets/styles/jqx.ui-sunny.css",
            // "~/Scripts/plugins/jqwidgets/styles/jqx.darkblue.css"
            "~/Scripts/plugins/jqwidgets/styles/theme.css"
            ));

            //bundles.Add(new ScriptBundle("~/jqwidgetsjs").Include(
            //  "~/Scripts/plugins/jqwidgets/jqxcore.js",
            //  "~/Scripts/plugins/jqwidgets/jqxdata.js",
            //  "~/Scripts/plugins/jqwidgets/jqxgrid.js",
            //  "~/Scripts/plugins/jqwidgets/jqxgrid.selection.js",
            //  "~/Scripts/plugins/jqwidgets/jqxgrid.pager.js",
            //  "~/Scripts/plugins/jqwidgets/jqxlistbox.js",
            //  "~/Scripts/plugins/jqwidgets/jqxbuttons.js",
            //  "~/Scripts/plugins/jqwidgets/jqxscrollbar.js",
            //  "~/Scripts/plugins/jqwidgets/jqxdatatable.js",
            //  "~/Scripts/plugins/jqwidgets/jqxtreegrid.js",
            //  "~/Scripts/plugins/jqwidgets/jqxmenu.js",
            //  "~/Scripts/plugins/jqwidgets/jqxcalendar.js",
            //  "~/Scripts/plugins/jqwidgets/jqxgrid.sort.js",
            //  "~/Scripts/plugins/jqwidgets/jqxgrid.filter.js",
            //  "~/Scripts/plugins/jqwidgets/jqxdatetimeinput.js",
            //  "~/Scripts/plugins/jqwidgets/jqxdropdownlist.js",
            //  "~/Scripts/plugins/jqwidgets/jqxslider.js",
            //  "~/Scripts/plugins/jqwidgets/jqxeditor.js",
            //  "~/Scripts/plugins/jqwidgets/jqxinput.js",
            //  "~/Scripts/plugins/jqwidgets/jqxdraw.js",
            //  "~/Scripts/plugins/jqwidgets/jqxchart.core.js",
            //  "~/Scripts/plugins/jqwidgets/jqxchart.rangeselector.js",
            //  "~/Scripts/plugins/jqwidgets/jqxtree.js",
            //  "~/Scripts/plugins/jqwidgets/globalize.js",
            //  "~/Scripts/plugins/jqwidgets/jqxbulletchart.js",
            //  "~/Scripts/plugins/jqwidgets/jqxcheckbox.js",
            //  "~/Scripts/plugins/jqwidgets/jqxradiobutton.js",
            //  "~/Scripts/plugins/jqwidgets/jqxvalidator.js",
            //  "~/Scripts/plugins/jqwidgets/jqxpanel.js",
            //  "~/Scripts/plugins/jqwidgets/jqxpasswordinput.js",
            //  "~/Scripts/plugins/jqwidgets/jqxnumberinput.js",
            //  "~/Scripts/plugins/jqwidgets/jqxcombobox.js",
            //  "~/Scripts/plugins/jqwidgets/jqx-all.js"
            //  ));

            //bundles.Add(new StyleBundle("~/jqwidgetscss").Include(
            //  "~/Scripts/plugins/jqwidgets/styles/jqx.base.css",
            //  "~/Scripts/plugins/jqwidgets/styles/jqx.bootstrap.css",
            //  "~/Scripts/plugins/jqwidgets/styles/theme.css"
            //  ));

        }
    }
}
