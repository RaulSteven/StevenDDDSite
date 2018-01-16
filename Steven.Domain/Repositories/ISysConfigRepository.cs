using Steven.Domain.Models;
using Steven.Domain.Infrastructure;
using System.Threading.Tasks;
using Steven.Domain.Enums;

namespace Steven.Domain.Repositories
{
    public interface ISysConfigRepository : IRepository<SysConfig>
    {
        #region 网站配置
        string WebSiteName { get; }
        string WebSiteCopyRight { get; }
        string WebSiteICP { get; }
        string WebSiteUrl { get; }

        string SitemapPath { get; }

        //公司名称"
        string CompanyName { get; }

        // 公司地址
        string CompanyAddr { get; }

        //公司电话"
        string CompanyTel { get; }

        /// <summary>
        /// 日志记录路径
        /// </summary>
        string LogFilePath { get; }

        #endregion

        #region 文件上传配置
        string UploadPath { get; }
        string[] UploadFileTypes { get; }
        int UploadFileSizes { get; }

        /// <summary>
        /// 水印图片位置
        /// </summary>
        string WaterMarkingPath { get; }

        string UploadRootDirectory { get; }

        string UploadThumbDirectory { get; }

        #endregion

        #region 缩略图设置
        string[] ImgSites { get; }
        #endregion

        #region 统计
        string StatisticsJS { get; }
        #endregion

        #region icon 列表
        string[] Icons { get; }

        #endregion

        #region 接口

        Pager<SysConfig> GetPager(string keyword, SysConfigClassify? clz, PageSearchModel search);

        #endregion

        #region 商户系统
        long ArticleProductDynamicId { get; }
        #endregion

        #region // 微信公众号配置
        string WxAppId { get; }
        string WxAppSecret { get; }
        #endregion

        #region 商户通知模板消息
        string ShopUserDownTemplateId { get; }
        string ShopUserPayTemplateId { get; }
        string ShopUserTakeTemplateId { get; }
        #endregion
    }
}
