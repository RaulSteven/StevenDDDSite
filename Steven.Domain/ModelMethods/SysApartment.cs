using Dapper.Contrib.Extensions;
using System.Collections.Generic;
namespace Steven.Domain.Models
{
    public partial class SysApartment
    {
        /// <summary>
        /// 在同辈中的位置，从0开始算
        /// </summary>
        [Write(false)]
        public int IndexOfParent { get; set; }

        /// <summary>
        /// 角色id列表
        /// </summary>
        [Write(false)]
        public IEnumerable<long> LstRoleIds { get; set; }
    }
}
