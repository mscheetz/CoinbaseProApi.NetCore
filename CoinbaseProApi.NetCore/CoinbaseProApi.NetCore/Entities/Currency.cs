using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class Currency
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal min_size { get; set; }
    }
}
