using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class Account
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }
        [JsonProperty(PropertyName = "balance")]
        public decimal Balance { get; set; }
        [JsonProperty(PropertyName = "available")]
        public decimal Available { get; set; }
        [JsonProperty(PropertyName = "hold")]
        public decimal Hold { get; set; }
        [JsonProperty(PropertyName = "profile_id")]
        public string ProfileId { get; set; }
    }
}
