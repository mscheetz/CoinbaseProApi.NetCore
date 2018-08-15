using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class TradeParams
    {
        public string type { get; set; }
        public decimal size { get; set; }
        public decimal price { get; set; }
        public string side { get; set; }
        public string product_id { get; set; }
        public bool post_only { get; set; }
    }
}
