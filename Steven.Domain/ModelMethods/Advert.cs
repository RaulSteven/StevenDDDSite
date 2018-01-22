using System;
using Steven.Domain.Enums;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class Advert
    {
        #region methods

        /// <summary>
        /// 是否正常广告，时间 & 状态 一起判断
        /// </summary>
        /// <returns></returns>
        public bool IsNormal()
        {
            var isNormal = CommonStatus == CommonStatus.Enabled
                           && StartTime <= DateTime.Now
                           && (EndTime == null || EndTime >= DateTime.Now);
            return isNormal;
        }

        /// <summary>
        /// a标签的target
        /// </summary>
        /// <returns></returns>
        public string GetTarget()
        {
            return "_" + Target.ToString().ToLower();
        }

        #endregion

        /// <summary>
        /// 图片链接url
        /// </summary>
        [Write(false)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 广告位标注的宽高
        /// </summary>
        [Write(false)]
        public string AdPosSize { get; set; }

        /// <summary>
        /// 广告位置
        /// </summary>
        [Write(false)]
        public AdPosKey Code { get; set; }

        /// <summary>
        /// 广告位名称
        /// </summary>
        [Write(false)]
        public string AdPosName { get; set; }
    }
}
