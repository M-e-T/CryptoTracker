using CryptoTracker.Interface;
using CryptoTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Api
{
    public class CoincapRequest //: IApiRequest
    {
        public string RatesRequest { get => "https://api.coincap.io/v2/rates"; }
        public string AssetsRequest { get => "https://api.coincap.io/v2/assets"; }
        public string HistoryRequest(Interval interval)
        {
            return $"https://api.coincap.io/v2/assets/bitcoin/history?interval={interval}";
        }
    }
}
