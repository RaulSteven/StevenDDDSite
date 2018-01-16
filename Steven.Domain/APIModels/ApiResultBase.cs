using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.APIModels
{
    [DataContract]
    public class ApiResultBase
    {
        #region properties
        [DataMember]
        public ApiResultCode Code { get; set; }
        [DataMember]
        public string Msg { get; set; }

        /// <summary>
        /// 缓存时间，单位秒
        /// </summary>
        [DataMember]
        public int CacheTimeOut { get; set; }

        #endregion

        public ApiResultBase()
        {
            this.Code = ApiResultCode.Failed;
            this.Msg = "";
        }

        public void SetData(string msg)
        {
            this.Code = ApiResultCode.Ok;
            this.Msg = msg;
        }
    }
}
