using System;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 广告
    /// </summary>
    [Table("Advert")]
    public partial class Advert : AggregateRoot
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 广告位主键
        /// </summary>
        public long AdPosId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 小组
        /// </summary>
        public short AdGroup { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public short Seat { get; set; }

        /// <summary>
        /// 状态：草稿、发布、回收站
        /// </summary>
        public AdvertStatus AdvertStatus { get; set; }

        /// <summary>
        /// 类型：图片、文字、富文本
        /// </summary>
        public AdvertType AdType { get; set; }

        /// <summary>
        /// 链接跳转方式：源窗口、新窗口
        /// </summary>
        public Target Target { get; set; }

        /// <summary>
        /// 富文本内容
        /// </summary>
        public string MetaContent { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 宽高：500x500
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public long ImageId { get; set; }

        /// <summary>
        /// 排序值，倒序
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        /// 数据Id
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 通用状态
        /// </summary>
        public CommonStatus CommonStatus { get; set; }

    }
}
