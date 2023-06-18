using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using CryptoTracker.Model;
using CryptoTracker.Model.Data;
using CryptoTracker.Command;
using CryptoTracker.Api;

namespace CryptoTracker.ViewModel
{
    public class ViewModelLocator
    {
        private static MainViewModel _mainViewModel;
        private static ChartViewModel _chartViewModel;

        public static MainViewModel MainViewModel
        {
            get
            {
                if (_mainViewModel == null)
                    _mainViewModel = new MainViewModel();

                return _mainViewModel;
            }
        }

        public static ChartViewModel ChartViewModel
        {
            get
            {
                if (_chartViewModel == null)
                    _chartViewModel = new ChartViewModel();

                return _chartViewModel;
            }
        }
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
        private CryptoCurrency _selectedCoin;
        public CryptoCurrency SelectedCoin
        {
            get => _selectedCoin;
            set
            {
                _selectedCoin = value;
                OnPropertyChanged();

                var chartViewModel = ViewModelLocator.ChartViewModel;
                chartViewModel.SelectedCoin = value;
                // Step 3: Assign the value to ChartViewModel's property
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
                        while (true) 
                        {
                            var jsonString = new DataDownloader(new CoincapRequest().AssetsRequest).Download();
                            MarketCap = new DataParser().PrseJson<Market>(jsonString);
                            await Task.Delay(500000000);
                        }
                    });
                }, command => true));
            }
        }
    }
}
