using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApiEksamenForberedelse.Models
{
    [DataContract]
    public class Auction
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Rating { get; set; }
        [DataMember]
        public Bid Bid { get; set; }

        public Auction(int startPrice)
        {
            this.Bid = new Bid() { Price = startPrice};
        }
    }
}