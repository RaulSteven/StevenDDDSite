using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.APIModels
{
    [DataContract]
    public class ShipingAddressModel:ApiResultBase
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Receiver { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string ShippingAddress { get; set; }

        [DataMember]
        public bool IsDefault { get; set; }
    }
}
