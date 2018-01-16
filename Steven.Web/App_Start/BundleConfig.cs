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