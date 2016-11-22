using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarPool.Models
{
    public class Pool
    {
        public string poolName { get; set; }
        public string hostName { get; set; }
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public string fromPin { get; set; }
        public string toPin { get; set; }
        public string carType { get; set; }
        public string carNumber { get; set; }
        public int seatsToOffer{get;set;}
        public DateTime date { get; set; }
        public DateTime startTime { get; set; }

        public List<PoolMember> members;
    }
}