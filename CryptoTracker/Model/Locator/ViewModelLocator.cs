using CryptoTracker.ViewModel;

namespace CryptoTracker.Model.Locator
{
    public class ViewModelLocator
    {
        private static MainViewModel _mainViewModel = new MainViewModel();
        private static ChartViewModel _chartViewModel = new ChartViewModel();

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
}
