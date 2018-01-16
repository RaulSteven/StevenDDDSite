using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Senparc.Weixin;

namespace Steven.Domain.Models
{
    [Table("WeixinNotifies")]
    public partial class WeixinNotify:AggregateRoot
    {
        public string UserOpenId { get; set; }

        public long WeixinNotifyTemplateId { get; set; }
        
        public string NotifyData { get; set; }
        
        public string NotifyUrl { get; set; }

        public ReturnCode Result { get; set; }

        public TableSource Source { get; set; }

        public long SourceId { get; set; }

        public CommonStatus Status { get; set; }
    }
}