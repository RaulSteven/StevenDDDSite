using System;
using Steven.Domain.Enums;

namespace Steven.Domain.ViewModels
{
    public class AdvertBizModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; } 
        /// <summary>
        /// 状态
        /// </summary>
        public AdvertStatus AdvertStatus { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public AdvertType AdType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; } 
        /// <summary>
        /// 排序
        /// </summary>
        public int SortIndex { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string ClassifyName { get; set; }
    }
}
