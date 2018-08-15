using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class OrderBookResponse
    {
        public long sequence { get; set; }
        [JsonProperty(PropertyName = "asks")]
        public OrderBook[] sells { get; set; }
        [JsonProperty(PropertyName = "bids")]
        public OrderBook[] buys { get; set; }
    }
}
