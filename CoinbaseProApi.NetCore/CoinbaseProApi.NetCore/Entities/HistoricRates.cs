using CoinbaseProApi.NetCore.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    [JsonConverter(typeof(Converter.ObjectToArrayConverter<HistoricRates>))]
    public class HistoricRates
    {
        [JsonProperty(Order = 1)]
        public long time { get; set; }
        [JsonProperty(Order = 2)]
        public decimal low { get; set; }
        [JsonProperty(Order = 3)]
        public decimal high { get; set; }
        [JsonProperty(Order = 4)]
        public decimal open { get; set; }
        [JsonProperty(Order = 5)]
        public decimal close { get; set; }
        [JsonProperty(Order = 6)]
        public decimal volume { get; set; }
    }
}
