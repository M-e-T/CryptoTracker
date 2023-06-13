using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptoTracker.Model
{
    public class Market
    {
        [JsonProperty("Data")]
        public List<Cryptocurrency> Cryptocurrencies { get; set; }
    }
}
