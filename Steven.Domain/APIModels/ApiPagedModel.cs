using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.APIModels
{
    [DataContract]
    public class ApiPagedModel:ApiResultBase
    {
        [DataMember]
        public int CurrentPageIndex { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int TotalItemCount { get; set; }

        [DataMember]
        public int TotalPageCount{ get; set; }
    }
}
