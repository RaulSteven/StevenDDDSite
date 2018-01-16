using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.APIModels
{
    public enum ApiResultCode
    {
        Ok = 0,
        Failed = 1,
        NotFound = 2,
        NoAuth = 3
    }
}
