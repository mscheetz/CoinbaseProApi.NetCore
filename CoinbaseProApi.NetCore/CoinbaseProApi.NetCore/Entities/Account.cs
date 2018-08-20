using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public class Account
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "currency")]
        public string currency { get; set; }
        [JsonProperty(PropertyName = "balance")]
        public decimal balance { get; set; }
        [JsonProperty(PropertyName = "available")]
        public decimal available { get; set; }
        [JsonProperty(PropertyName = "hold")]
        public decimal hold { get; set; }
        [JsonProperty(PropertyName = "profile_id")]
        public string profileId { get; set; }
    }
}
