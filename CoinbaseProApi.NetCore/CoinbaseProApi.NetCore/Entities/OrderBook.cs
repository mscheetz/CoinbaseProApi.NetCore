using CoinbaseProApi.NetCore.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    [JsonConverter(typeof(Converter.ObjectToArrayConverter<OrderBook>))]
    public class OrderBook
    {
        [JsonProperty(Order = 1)]
        public decimal price { get; set; }
        [JsonProperty(Order = 2)]
        public decimal size { get; set; }
        [JsonProperty(Order = 3)]
        public string orders { get; set; }
    }
}
