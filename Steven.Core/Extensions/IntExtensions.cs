using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Core.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// 显示为美金
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string ToDollar(this int money)
        {
            return money.ToString("#,#");
        }
    }
}
