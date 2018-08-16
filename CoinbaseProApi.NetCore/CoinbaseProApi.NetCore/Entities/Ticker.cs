using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class Ticker
    {
        public long trade_id { get; set; }
        public decimal price { get; set; }
        public decimal size { get; set; }
        public decimal bid { get; set; }
        public decimal ask { get; set; }
        public decimal volume { get; set; }
        public DateTimeOffset time { get; set; }
    }
}
