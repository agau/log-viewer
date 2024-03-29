﻿using System.Web;
using System.Web.Optimization;

namespace LogViewer
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
						"~/Scripts/jquery.dataTables.js",
						"~/Scripts/moment.min.js",
						"~/Scripts/daterangepicker.js",					
						"~/Scripts/dataTables.bootstrap.js"));

			//bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
			//            "~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			//bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
			//            "~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap-multiselect.js",
					  "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
					  //"~/Content/dataTables.bootstrap.min.css",
					  "~/Content/bootstrap.min.css",
					  "~/Content/bootstrap-multiselect.css",
					  "~/Content/daterangepicker.css",
					  "~/Content/jquery.dataTables.min.css",
					  "~/Content/font-awesome.min.css"));
        }
    }
}
