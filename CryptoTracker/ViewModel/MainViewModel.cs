using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Newtonsoft.Json;
using CryptoTracker.Model;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;

namespace CryptoTracker.ViewModel
{
    public class Rates
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string CurrencySymbol { get; set; }
        public string Type { get; set; }
        public double RateUsd { get; set; }
    }
    public class MainViewModel : BaseViewModel
    {  
        private Market _marketCap;
        public Market MarketCap
        {
            get => _marketCap;
            set
            {
                _marketCap = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Rates> _rates;
        public ObservableCollection<Rates> Rates
        {
            get => _rates;
            set
            {
                _rates = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            UpdateMarket.Execute(true);
        }
        private ICommand _updateMarket;
        public ICommand UpdateMarket
        {
            get
            {
                return _updateMarket ?? (_updateMarket = new RelayCommand(async command =>
                {
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Get, "https://api.coincap.io/v2/rates");
                    var response = client.Send(request);
                    response.EnsureSuccessStatusCode();
                    var jsonString = response.Content.ReadAsStringAsync().Result.ToString();
                    
                    JObject jsonObject = JObject.Parse(jsonString);
                    JArray dataArray = (JArray)jsonObject["data"];
                    Rates = dataArray.ToObject<ObservableCollection<Rates>>();

                    Console.WriteLine();
                    //List<Rates> currencyItems = JsonConvert.DeserializeObject<Rates>(data);
                    //Rates = new ObservableCollection<Rates>(JsonConvert.DeserializeObject<List<Rates>>(data));

                    await Task.Run(async() =>
                    {
                        while (true) 
                        { 
                            var data = await new DataDownloader("https://api.coincap.io/v2/assets").DownloadAsync();
                            MarketCap = JsonConvert.DeserializeObject<Market>(data);
                            await Task.Delay(500000000);
                        }
                    });
                }, command => true));
            }
        }
    }
}
