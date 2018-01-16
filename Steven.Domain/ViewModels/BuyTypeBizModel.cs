using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Steven.Core.Extensions;
using Steven.Domain.Enums;

namespace Steven.Domain.ViewModels
{
    [DataContract]
    public class BuyTypeBizModel
    {
        [DataMember]
        public BuyType Type { get; set; }

        [DataMember]
        public string Name
        {
            get
            {
                switch (Type)
                {
                    case BuyType.Delivery:
                        return "商家配送";
                    case BuyType.Arrival:
                        return "到店消费";
                }
                return Type.GetDescriotion();
            }
            set { }
        }
    }
}
