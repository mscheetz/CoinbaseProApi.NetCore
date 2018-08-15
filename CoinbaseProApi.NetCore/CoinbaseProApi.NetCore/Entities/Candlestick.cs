using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class Candlestick
    {
        public long openTime { get; set; }
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public decimal volume { get; set; }
        public long closeTime { get; set; }
        public decimal volumeChange { get; set; }
        public decimal volumePercentChange { get; set; }
    }
}
