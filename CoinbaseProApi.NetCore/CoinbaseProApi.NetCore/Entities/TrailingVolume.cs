using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class TrailingVolume
    {
        public string product_id { get; set; }
        public decimal exchange_volume { get; set; }
        public decimal volume { get; set; }
        public DateTimeOffset recorded_at { get; set; }
    }
}
