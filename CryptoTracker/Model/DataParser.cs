using CryptoTracker.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CryptoTracker.Model
{
    public class DataParser
    {
        public T PrseJson<T>(string jsonString)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<T>(jsonString);
                return result;
            }
            catch
            {
                JObject jsonObject = JObject.Parse(jsonString);
                JArray dataArray = (JArray)jsonObject["data"];
                var result = dataArray.ToObject<T>();
                return result;
            }
        }
    }
}
