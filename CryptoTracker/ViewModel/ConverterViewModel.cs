using CryptoTracker.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.ViewModel
{
    public class ConverterViewModel : MainViewModel
    {           
        private Rates _convertFrom;
        public Rates ConvertFrom 
        {
            get => _convertFrom;
            set
            {
                _convertFrom = value;
                OnPropertyChanged();
            }
        }
        private Rates _convertTo;
        public Rates ConvertTo 
        {
            get => _convertTo;
            set
            {
                _convertTo = value;
                OnPropertyChanged();
            }
        }
        public ConverterViewModel()
        {
          
        }
    }
}
