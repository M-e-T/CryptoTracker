using System.Collections.Generic;
using Newtonsoft.Json;

namespace CryptoTracker.Model.Data
{
    public class Market
    {
        [JsonProperty("Data")]
        public List<CryptoCurrency> CryptoCrrencies { get; set; }
    }
}
