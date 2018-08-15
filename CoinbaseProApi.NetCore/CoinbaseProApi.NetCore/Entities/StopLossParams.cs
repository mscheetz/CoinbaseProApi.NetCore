using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class StopLossParams : TradeParams
    {
        public string stop { get; set; }
        public decimal stop_price { get; set; }
    }
}
