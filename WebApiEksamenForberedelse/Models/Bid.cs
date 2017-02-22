using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApiEksamenForberedelse.Models
{
    [DataContract]
    public class Bid
    {
        [DataMember]
        public int Price { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public DateTime Time { get; set; }
    }
}