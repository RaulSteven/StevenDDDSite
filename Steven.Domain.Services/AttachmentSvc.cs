using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Steven.Core.Utilities;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using ImageProcessor;
using System.Drawing;
using System.Net;
using ImageProcessor.Imaging;
using Steven.Domain.Enums;

namespace Steven.Domain.Services
{
    public class AttachmentSvc : BaseSvc, IAttachmentSvc
    {
        private const string UploadPathLocal = "Local";
        public IAttachmentRepository AttachmentRepository { get; set; }
        public ISysConfigRepository SysConfigRepository { get; set; }

        public bool Save(HttpPostedFileBase postedFile, Attachment oAtt, string fCats = null, string fileName = null)
        {
            if (postedFile == null || postedFile.ContentLength <= 0)
            {
                return false;
            }
            var refFileName = "";
            try
            {
                oAtt.Name = Path.GetFileNameWithoutExtension(postedFile.FileName);
                if (oAtt.Name != null && oAtt.Name.Length > 50)
                {
                    oAtt.Name = oAtt.Name.Substring(0, 50);
                }
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = string.Format("{0}{1}", DateTime.Now.ToString("HHmmss"), Math.Abs((oAtt.Name + Guid.NewGuid()).GetHashCode()));
                }
                refFileName = SaveFile(postedFile, fCats, fileName);
                oAtt.FilePath = refFileName;
                oAtt.FileSize = postedFile.ContentLength;
                oAtt.FileExt = Path.GetExtension(postedFile.FileName);
                oAtt.ViewCount = 0;
                oAtt.SortIndex = 0;
                AttachmentRepository.Save(oAtt);
            }
            catch (Exception ex)
            {
                var f = GetFullPath(refFileName);
                if (File.Exists(f)) File.Delete(f);
                Log.Error("保存附件报错！", ex);
                return false;
            }
            return true;
        }

        public string GetFullPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            if (string.IsNullOrEmpty(SysConfigRepository.UploadPath) || SysConfigRepository.UploadPath.Equals(UploadPathLocal))
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SysConfigRepository.UploadRootDirectory, path);
            }
            else
            {
                return Path.Combine(SysConfigRepository.UploadPath, path);
            }
        }

        public string SaveFile(HttpPostedFileBase fileBase, string fCats, string name)
        {
            var fileExt = Path.GetExtension(fileBase.FileName);
            var relatePath = BuildRelatePath(fCats, name, fileExt);
            var fullPath = GetFullPath(relatePath);
            var fDir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(fDir)) Directory.CreateDirectory(fDir);
            fileBase.SaveAs(fullPath);
            return relatePath;
        }

        private string BuildRelatePath(string fileCatName, string name, string ext)
        {
            if (String.IsNullOrEmpty(fileCatName)) fileCatName = "default";
            DateTime dt = DateTime.Now;
            if (!ext.StartsWith(".")) ext = "." + ext;
            return String.Format(@"{0}\{1}_{2}\{3}\{4}{5}", fileCatName, dt.Year, dt.Month.ToString("00"), dt.Day, name, ext);
        }

        public string GetFileContentType(string ext)
        {
            string contentType = "text/plain"; // text/html
            switch (ext.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".bmp":
                    contentType = "image/bmp";
                    break;
                case ".tif":
                    contentType = "image/tiff";
                    break;
                case ".swf":
                case ".flv":
                    contentType = "application/x-shockwave-flash";
                    break;
                case ".ico":
                    contentType = "image/x-icon";
                    break;
                case ".js":
                    contentType = "application/javascript";
                    break;
                case ".css":
                    contentType = "text/css";
                    break;
                case ".html":
                case ".htm":
                    contentType = "text/html";
                    break;
                case ".xml":
                    contentType = "text/xml";
                    break;
                case ".apk":
                    contentType = "application/vnd.android.package-archive";
                    break;
                case ".rar":
                    contentType = "application/x-rar";
                    break;
                case ".zip":
                    contentType = "application/zip";
                    break;
                case ".arj":
                    contentType = "application/x-arj";
                    break;
                case ".gz":
                    contentType = "application/x-gzip";
                    break;
                case ".z":
                    contentType = "application/x-compress";
                    break;
                case ".7z":
                    contentType = "application/x-7z-compressed";
                    break;
                case ".doc":
                    contentType = "application/msword";
                    break;
                case ".docx":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".dotx":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                    break;
                case ".dot":
                    contentType = "text/vnd.graphviz";
                    break;
                case ".pdf":
                    contentType = "application/pdf";
                    break;
            }
            return contentType;

        }

        public string GetAttachmentUrl(long attaId)
        {
            var atta = AttachmentRepository.Get(attaId);
            if (atta == null)
            {
                return null;
            }
            var attaPath = Path.Combine(SysConfigRepository.UploadRootDirectory, atta.FilePath);
            return GetFileUrl(attaPath);
        }

        private string GetFileUrl(string path)
        {
            string attaSite = GetAttaSite(path);
            return $"{attaSite}/{path}".Replace('\\', '/');
        }

        private string GetAttaSite(string path)
        {
            var index = Math.Abs(path.GetHashCode() % SysConfigRepository.ImgSites.Length);
            var imgSite = SysConfigRepository.ImgSites[index];
            return imgSite;
        }

        public string GetPicUrl(long attaId)
        {
            return GetAttachmentUrl(attaId);
        }


        private void AddWatermark(WaterMarkingPosition? waterPosition, ImageFactory originalImage)
        {
            if (!waterPosition.HasValue)
            {
                return;
            }
            using (var watermarkImage = new ImageFactory(true))
            {
                watermarkImage.Load(SysConfigRepository.WaterMarkingPath);
                var originalWidth = originalImage.Image.Width;
                var originalHeight = originalImage.Image.Height;
                var markWidth = originalWidth / 5;
                var markHeight = watermarkImage.Image.Height * markWidth / watermarkImage.Image.Width;

                Point point;
                const int edge = 20;
                switch (waterPosition.Value)
                {
                    case WaterMarkingPosition.LeftTop:
                        point = new Point(edge, edge);
                        break;
                    case WaterMarkingPosition.LeftBottom:
                        point = new Point(edge, originalHeight - markHeight - edge);
                        break;
                    case WaterMarkingPosition.RightTop:
                        point = new Point(originalWidth - markWidth - edge, edge);
                        break;
                    case WaterMarkingPosition.RightBottom:
                        point = new Point(originalWidth - markWidth - edge, originalHeight - markHeight - edge);
                        break;
                    case WaterMarkingPosition.Center:
                        point = new Point((originalWidth - markWidth) / 2, (originalHeight - markHeight) / 2);
                        break;
                    default:
                        point = new Point(edge, edge);
                        break;
                }
                var imageLayer = new ImageLayer()
                {
                    Image = watermarkImage.Image,
                    Position = point,
                    Size = new Size(markWidth, markHeight)
                };
                originalImage.Overlay(imageLayer);
            }
        }

        private ResizeMode GetRezieMode(ThumMode mode)
        {
            var resizeMode = ResizeMode.Crop;
            switch (mode)
            {
                case ThumMode.Crop:
                    resizeMode = ResizeMode.Crop;
                    break;
                case ThumMode.Fit:
                    resizeMode = ResizeMode.Max;
                    break;
                case ThumMode.Pad:
                    resizeMode = ResizeMode.Pad;
                    break;
                default:
                    break;
            }

            return resizeMode;
        }

        public string GetPicUrl(long attaId, int width = 100, int height = 100, ThumMode mode = ThumMode.Crop, int quality = 100, WaterMarkingPosition? waterPosition = null)
        {
            var atta = AttachmentRepository.Get(attaId);
            if (atta == null)
            {
                return null;
            }
            var thumbPath = Path.Combine(SysConfigRepository.UploadThumbDirectory,
                $"{mode}_{width}x{height}",
                atta.FilePath);
            var thumbFullPath = GetFullPath(thumbPath);
            if (!File.Exists(thumbFullPath))
            {
                var originalFullPath = GetFullPath(atta.FilePath);
                if (!File.Exists(originalFullPath))
                {
                    return null;
                }
                using (var originalImage = new ImageFactory(true))
                {
                    ResizeMode resizeMode = GetRezieMode(mode);
                    var resizeLayer = new ResizeLayer(new Size(width, height), resizeMode);
                    originalImage.Load(originalFullPath)
                        .Resize(resizeLayer);
                    AddWatermark(waterPosition, originalImage);
                    originalImage.BackgroundColor(Color.White)
                        .Quality(quality)
                        .Save(thumbFullPath);
                }
            }


            return GetFileUrl(Path.Combine(SysConfigRepository.UploadRootDirectory, thumbPath));
        }

        public long DownLoadRemoteImage(string imgUrl, TableSource source,long sourceId, string rootDir = "")
        {
            if (string.IsNullOrEmpty(imgUrl)) return 0;

            try
            {
                var fExt = Path.GetExtension(imgUrl);
                if (string.IsNullOrEmpty(fExt))
                {
                    fExt = ".jpg";
                }
                var fReleative = BuildRelatePath(rootDir, Guid.NewGuid().ToString(), fExt);
                var fAbs = Path.Combine(SysConfigRepository.UploadPath, fReleative);
                var fDir = Path.GetDirectoryName(fAbs);

                if (string.IsNullOrEmpty(fDir)) return 0;
                if (!Directory.Exists(fDir)) Directory.CreateDirectory(fDir);

                using (var wcClient = new WebClient())
                {
                    wcClient.DownloadFile(imgUrl, fAbs);
                }
                Attachment oAtt=new Attachment();
                oAtt.Name =string.Format("{0}{1}", DateTime.Now.ToString("HHmmss"), Math.Abs((oAtt.Name + Guid.NewGuid()).GetHashCode()));               
                oAtt.FilePath = fReleative;
                HttpWebRequest hwreq = (HttpWebRequest)WebRequest.Create(imgUrl);
                HttpWebResponse hwrep = (HttpWebResponse) hwreq.GetResponse();     
                oAtt.FileSize = hwrep.ContentLength;                
                oAtt.FileExt = fExt;
                oAtt.ViewCount = 0;
                oAtt.SortIndex = 0;
                oAtt.Source = source;
                oAtt.SourceId = sourceId;
                AttachmentRepository.Save(oAtt);
                return oAtt.Id;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("下载图片失败，图片路径：{0},失败原因：{1}", imgUrl, ex);
                return 0;
            }

        }
    }
}
