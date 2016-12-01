using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WGHotel.Areas.Backend
{
    internal static class BundleConfig
    {
        internal static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/jquery-localize/dist/jquery.localize.js",
                        "~/Scripts/jquery.cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));



            bundles.Add(new ScriptBundle("~/Admin/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Content/Admin/assets/jquery-knob/js/jquery.knob.js",
                      "~/Content/Admin/js/owl.carousel.js",
                      "~/Content/Admin/js/jquery.customSelect.min.js",
                      "~/Content/Admin/js/jquery-jvectormap-1.2.2.min.js",
                      "~/Content/Admin/js/jquery.scrollTo.min.js",
                      "~/Content/Admin/js/jquery.nicescroll.js",
                      "~/Content/Admin/js/scripts.js"

                      ));

            bundles.Add(new StyleBundle("~/Admin/Content/css").Include(
                    "~/Content/bootstrap.css",
                //"~/Content/Site.css",
                    "~/Content/Admin/css/bootstrap.min.css",
                    "~/Content/Admin/css/bootstrap-theme.css",
                    "~/Content/Admin/css/elegant-icons-style.css",
                    "~/Content/Admin/css/font-awesome.min.css",
                    "~/Content/Admin/css/owl.carousel.css",
                    "~/Content/Admin/css/jquery-jvectormap-1.2.2.css",
                    "~/Content/Admin/css/widgets.css",
                    "~/Content/Admin/css/style.css",
                    "~/Content/Admin/css/style-responsive.css",
                    "~/Content/Admin/css/jquery-ui-1.10.4.min.css"
                    ));
        }
    }
}