using System.Threading.Tasks;
using System.Web;
using Steven.Domain.Models;
using Steven.Domain.Enums;

namespace Steven.Domain.Services
{
    public interface IAttachmentSvc
    {
        bool Save(HttpPostedFileBase postedFile, Attachment oAtt, string fCats = null,string fileName = null);
        string GetFullPath(string path);
        string GetFileContentType(string ext);

        string GetAttachmentUrl(long attaId);

        string GetPicUrl(long attaId);
        string GetPicUrl(long attaId, int width = 100, int height = 100, ThumMode mode = ThumMode.Crop, int quality = 100, WaterMarkingPosition? waterPosition = null);

        long DownLoadRemoteImage(string imgUrl, TableSource source, long sourceId, string rootDir = "");
    }
}
