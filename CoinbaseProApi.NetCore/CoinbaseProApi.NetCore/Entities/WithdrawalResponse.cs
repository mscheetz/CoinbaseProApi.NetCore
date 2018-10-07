using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class WithdrawalResponse
    {
        public string id { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
    }
}
