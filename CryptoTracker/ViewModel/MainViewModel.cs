using CryptoTracker.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CryptoTracker.ViewModel
{
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
