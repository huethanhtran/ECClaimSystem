﻿using System.Web;
using System.Web.Optimization;

namespace ECClaimSystem
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
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/Admin/css").Include(
                "~/Content/Css/bootstrap.min.css",
                "~/Content/Css/custom.min.css",
                "~/Content/Css/daterangepicker.css",
                "~/Content/Css/font-awesome.min.css",
                "~/Content/Css/green.css",
                 "~/Content/Css/npprogess.css",
                 "~/Content/Css/animate.min.css",
                 "~/Content/Css/dataTables.bootstrap.min.css"
                ));
            bundles.Add(new StyleBundle("~/Content/Admin/Scripts").Include(
              "~/Content/Scripts/bootstrap.js",
              "~/Content/Scripts/custom.js",
              "~/Content/Scripts/daterangepicker.js",
              "~/Content/Scripts/iCheck.js",
              "~/Content/Scripts/jquery.js"

              ));
        }
    }
}
