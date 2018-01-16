using Steven.Core.Extensions;
using Steven.Domain.Enums;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Text;

namespace Steven.Domain.Models
{
    public partial class SysMenu
    {
        [Write(false)]
        public List<SysMenu> Children { get; set; }

        [Write(false)]
        public List<string> ButtonList
        {
            get
            {
                var lstBtn = new List<string>();
                foreach (var btn in Buttons.GetDescriptDict())
                {
                    var button = (SysButton)btn.Key;
                    if (Buttons.HasFlag(button))
                    {
                        lstBtn.Add(button.ToString());
                    }
                }
                return lstBtn;
            }
        }

        /// <summary>
        /// 在同辈中的位置，从0开始算
        /// </summary>
        [Write(false)]
        public int IndexOfParent { get; set; }
    }
}
