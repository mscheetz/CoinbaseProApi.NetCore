using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class ExchangeProduct
    {
        public string id { get; set; }
        public string base_currency { get; set; }
        public string quote_currency { get; set; }
        public decimal base_min_size { get; set; }
        public decimal base_max_size { get; set; }
        public decimal quote_increment { get; set; }
    }
}
