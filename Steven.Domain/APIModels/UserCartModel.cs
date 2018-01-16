using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.APIModels
{
    [DataContract]
    public class UserCartModel : ApiResultBase
    {
        public UserCartModel()
        {
            List = new List<UserCartProModel>();
        }

        [DataMember]
        public decimal TotalPrice { get; set; }

        [DataMember]
        public int TotalProNum { get; set; }

        [DataMember]
        public bool IsChooseAll { get; set; }

        [DataMember]
        public List<UserCartProModel> List { get; set; }
    }
    [DataContract]
    public class UserCartProModel
    {
        [DataMember]
        public long CartId { get; set; }

        [DataMember]
        public long ProductId { get; set; }

        [DataMember]
        public string Img { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public bool IsChoose { get; set; }
    }
}
