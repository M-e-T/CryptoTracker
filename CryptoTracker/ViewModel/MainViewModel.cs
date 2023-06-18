using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using CryptoTracker.Model;
using CryptoTracker.Model.Data;
using CryptoTracker.Command;
using CryptoTracker.Api;
using CryptoTracker.Model.Locator;

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
        private CryptoCurrency _selectedCoin;
        public CryptoCurrency SelectedCoin
        {
            get => _selectedCoin;
            set
            {
                if(_selectedCoin == null)
                {
                    _selectedCoin = value;
                    OnPropertyChanged();
                    return;
                }
                _selectedCoin = value;
                var chartViewModel = ViewModelLocator.ChartViewModel;
                chartViewModel.SelectedCoin = value;
                TabIndex = 2;
            }
        }
        private int _tabIndex;
        public int TabIndex
        {
            get => _tabIndex;
            set
            {
                _tabIndex = value;
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
                return _updateMarket ?? (_updateMarket = new RelayCommand(command =>
                {
                    var jsonString = new DataDownloader(new CoincapRequest().RatesRequest).Download();
                    Rates = new DataParser().PrseJson<ObservableCollection<Rates>>(jsonString);

                    Task.Run(async() =>
                    {
                        var jsonString = new DataDownloader(new CoincapRequest().AssetsRequest).Download();
                        MarketCap = new DataParser().PrseJson<Market>(jsonString);
                        SelectedCoin = MarketCap.CryptoCrrencies[0];
                        
                    });
                }, command => true));
            }
        }
    }
}
