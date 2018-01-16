using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.APIModels
{
    [DataContract]
    public class ApiResultBoolModel : ApiResultBase
    {
        [DataMember]
        public bool Data { get; set; }
    }
    [DataContract]
    public class ApiResultIntModel : ApiResultBase
    {
        [DataMember]
        public int Data { get; set; }
    }
    [DataContract]
    public class ApiResultStringModel :ApiResultBase
    {
        [DataMember]
        public string Data { get; set; } 
    }
    [DataContract]
    public class ApiResultLongModel : ApiResultBase
    {
        [DataMember]
        public long Data { get; set; }
    }
}
