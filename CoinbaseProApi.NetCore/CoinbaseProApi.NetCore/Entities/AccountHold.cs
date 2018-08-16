using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class AccountHold
    {
        public string id { get; set; }
        public string account_id { get; set; }
        public DateTimeOffset created_at { get; set; }
        public DateTimeOffset updated_at { get; set; }
        public decimal amount { get; set; }
        public string type { get; set; }
        [JsonProperty(PropertyName = "ref")]
        public string reference { get; set; }
    }
}
