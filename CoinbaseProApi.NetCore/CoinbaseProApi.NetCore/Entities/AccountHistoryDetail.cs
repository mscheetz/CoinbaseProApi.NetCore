using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class AccountHistoryDetail
    {
        public string order_id { get; set; }
        public long trade_id { get; set; }
        public string product_id { get; set; }
    }
}
