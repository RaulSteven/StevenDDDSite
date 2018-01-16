using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Core.Utilities
{
    public class ProductImg
    {
        public static long GetCoverImgId(string picAttIds)
        {
            if (!string.IsNullOrEmpty(picAttIds))
            {
                string[] attIds = picAttIds.Split(',');
                if (attIds.Any() && attIds[0] != null)
                {
                    long attId = long.Parse(attIds[0]);
                    return attId;
                }
            }
            return 0;
        }
    }
}
