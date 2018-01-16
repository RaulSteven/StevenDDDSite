using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Steven.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region upload
            bundles.Add(new ScriptBundle("~/bundles/jQuery-File-Upload").Include(
                 "~/Res/global/js/lib/jquery-file-upload/js/vendor/jquery.ui.widget.js",
                 "~/Res/global/js/lib/jquery-file-upload/js/jquery.fileupload.js",
                 "~/Res/global/js/lib/jquery-file-upload/js/jquery.fileupload-process.js",
                 "~/Res/global/js/lib/jquery-file-upload/js/jquery.iframe-transport.js",
                 "~/Res/global/js/lib/jquery-file-upload/js/jquery.fileupload-image.js"));

            bundles.Add(new StyleBundle("~/bundles/jQuery-File-Upload-css")
                .Include("~/Res/global/js/lib/jquery-file-upload/css/jquery.fileupload.css",
                "~/Res/global/js/lib/jquery-file-upload/css/jquery.fileupload-ui.css"));
            #endregion

            #region icheck
            bundles.Add(new StyleBundle("~/Res/admin/css/plugins/iCheck/skins/beilin/css-bundle")
                .Include("~/Res/admin/css/plugins/iCheck/skins/beilin/blue.css"));
            #endregion

            #region 店铺
            bundles.Add(new StyleBundle("~/Res/shop/css/shop-layout-css")
                .Include("~/Res/shop/css/style.css",
                "~/Res/global/style/global.css",
                "~/Res/shop/css/admin.css",
                "~/Res/admin/css/plugins/sweetalert/sweetalert.css"));

            bundles.Add(new ScriptBundle("~/bundles/shop-layout-js")
                .Include("~/Res/admin/js/plugins/metisMenu/jquery.metisMenu.js",
                "~/Res/admin/js/plugins/slimscroll/jquery.slimscroll.min.js",
                "~/Res/admin/js/inspinia.js",
                "~/Res/admin/js/plugins/pace/pace.min.js",
                "~/Res/admin/js/plugins/sweetalert/sweetalert.min.js"));

            bundles.Add(new ScriptBundle("~/shop/pagination")
                .Include("~/Res/shop/js/pagination.js"));

            bundles.Add(new ScriptBundle("~/shop/chart")
                .Include("~/Res/admin/js/plugins/chartJs/Chart.bundle.js"));

            //<link rel="stylesheet" type="text/css" href="/Res/admin/font-awesome/css/font-awesome.css">
            bundles.Add(new StyleBundle("~/Res/admin/font-awesome/css/font-bundle")
                .Include("~/Res/admin/font-awesome/css/font-awesome.css"));

            bundles.Add(new ScriptBundle("~/shop/clipboard")
                .Include("~/Res/shop/js/clipboard.min.js"));
            #endregion

            #region ladda
            bundles.Add(new StyleBundle("~/bundles/ladda-css")
                .Include("~/Res/admin/css/plugins/ladda/ladda-themeless.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/ladda-js")
                .Include("~/Res/admin/js/plugins/ladda/spin.min.js",
                "~/Res/admin/js/plugins/ladda/ladda.min.js",
                "~/Res/admin/js/plugins/ladda/ladda.jquery.min.js"));
            #endregion

            #region summernote

            bundles.Add(new StyleBundle("~/Res/admin/css/plugins/summernote/summernote-css")
                .Include("~/Res/admin/css/plugins/summernote/summernote.css",
                "~/Res/admin/css/plugins/summernote/summernote-bs3.css"));
            bundles.Add(new ScriptBundle("~/bundles/summernote-js")
                .Include("~/Res/admin/js/plugins/ladda/spin.min.js",
                "~/Res/admin/js/plugins/summernote/summernote.min.js",
                "~/Res/admin/js/plugins/summernote/local/summernote-zh-CN.js"));
            #endregion

            #region submit
            bundles.Add(new ScriptBundle("~/Res/admin/js/plugins/validate/validate-bundle")
                .Include("~/Res/admin/js/plugins/validate/jquery.validate.min.js",
                "~/Res/admin/js/plugins/validate/messages_cn.js"));

            bundles.Add(new ScriptBundle("~/bundles/submit-js")
                        .Include("~/Res/admin/js/CommonSubmit.js"));
            #endregion

            #region jquery.common
            bundles.Add(new ScriptBundle("~/bundles/common")
                .Include("~/Res/global/js/jquery.common.js"));
            #endregion



            #region datepicker
            //        @*<script src="/Res/admin/js/plugins/datapicker/bootstrap-datepicker.js"></script>

            bundles.Add(new ScriptBundle("~/bundles/datepiker")
                .Include("~/Res/admin/js/plugins/datapicker/bootstrap-datepicker.js"));
            #endregion

            #region footable
            //<script src="/Res/admin/js/plugins/footable/footable.all.min.js"></script>*@
            bundles.Add(new ScriptBundle("~/bundles/footable")
                .Include("~/Res/admin/js/plugins/footable/footable.all.min.js"));
            #endregion
        }
    }
}