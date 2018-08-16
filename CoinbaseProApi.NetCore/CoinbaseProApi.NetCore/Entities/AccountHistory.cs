using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class AccountHistory
    {
        public long id { get; set; }
        public DateTimeOffset created_at { get; set; }
        public decimal amount { get; set; }
        public decimal balance { get; set; }
        public string type { get; set; }
        public AccountHistoryDetail details { get; set; }
    }
}
