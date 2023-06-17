using CryptoTracker.Model.Data;

namespace CryptoTracker.ViewModel
{
    public class ConverterViewModel : MainViewModel
    {           
        private CryptoCurrency _convertFrom;
        public CryptoCurrency ConvertFrom 
        {
            get => _convertFrom;
            set
            {
                _convertFrom = value;
                Converter();
                OnPropertyChanged();
            }
        }
        private CryptoCurrency _convertTo;
        public CryptoCurrency ConvertTo 
        {
            get => _convertTo;
            set
            {
                _convertTo = value;
                Converter();
                OnPropertyChanged();
            }
        }
        private decimal _leftNumber = 1;
        public decimal LeftNumber
        {
            get => _leftNumber;
            set
            {
                _leftNumber = value;
                Converter();
                OnPropertyChanged();
            }
        } 
        private decimal _rightNumber;
        public decimal RightNumber
        {
            get => _rightNumber;
            set
            {
                _rightNumber = value;
                OnPropertyChanged();
            }
        }
        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }
        
        private void Converter()
        {
            //if (LeftNumber <= 0 || RightNumber <= 0)
            //    return;
            if (ConvertFrom is null || ConvertTo is null)
                return;
            Amount = (decimal)ConvertFrom.PriceUsd / (decimal)ConvertTo.PriceUsd;
            RightNumber = (LeftNumber * (decimal)ConvertFrom.PriceUsd) / (decimal)ConvertTo.PriceUsd;
            //return Amount;
        }
        public ConverterViewModel()
        {
          
        }
    }
}
