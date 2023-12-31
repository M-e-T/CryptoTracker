﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using Newtonsoft.Json.Linq;

using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;

using CryptoTracker.Model;
using CryptoTracker.Command;
using CryptoTracker.Api;
using CryptoTracker.Model.Charts;
using CryptoTracker.Model.Data;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;
using System.Collections.Generic;

namespace CryptoTracker.ViewModel
{
    public enum Interval
    {
        m1,
        m5, 
        m15,
        m30,
        h1, 
        h2, 
        h6,
        h12,
        d1
    }
    public enum Chart 
    {
        Line,
        Candlestick
    }
    public class ChartViewModel : BaseViewModel
    {
        private Brush _primaryForegroundBrush;
        public Brush PrimaryForegroundBrush
        {
            get { return _primaryForegroundBrush; }
            set
            {
                _primaryForegroundBrush = value;
                OnPropertyChanged(nameof(PrimaryForegroundBrush));
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
                UpdateChart();
            }
        }

        private ObservableCollection<History> _history;
        public ObservableCollection<History> History
        {
            get => _history;
            set
            {
                _history = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Candl> _candles;
        public ObservableCollection<Candl> Candles
        {
            get => _candles;
            set
            {
                _candles = value;
                OnPropertyChanged();
            }
        }
        private Interval _chartInterval;
        public Interval ChartInterval
        {
            get { return _chartInterval; }
            set
            {
                if (_chartInterval != value)
                {
                    _chartInterval = value;
                    UpdateChart();
                    OnPropertyChanged();
                }
            }
        }
        private Scale _chartScale;
        public Scale ChartScale
        {
            get { return _chartScale; }
            set
            {
                if (_chartScale != value)
                {
                    _chartScale = value;
                    UpdateChart();
                    OnPropertyChanged();
                }
            }
        }
        private Chart _chart = Chart.Candlestick;
        public Chart Chart
        {
            get => _chart;
            set
            {
                _chart = value;
                OnPropertyChanged();
            }
        }
        private Axis[] _sharedXAxis;
        public Axis[] SharedXAxis
        {
            get => _sharedXAxis;
            set
            {
                _sharedXAxis = value;
                OnPropertyChanged();
            }
        }
        private ISeries[] _series;
        public ISeries[] Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        public ChartViewModel()
        {

        }
        public void UpdateChart()
        {
            string jsonString;
            if (SelectedCoin is null)
                return;
            jsonString = new DataDownloader(new CoincapRequest().HistoryRequest(SelectedCoin.Name.ToLower(), ChartInterval)).Download();
            History = new DataParser().PrseJson<ObservableCollection<History>>(jsonString);        
           
            if (ChartScale != Scale.Max)
                History = new ObservableCollection<History>(History.Where(x => x.Date > TimeScale.GetPeriod(ChartScale)));;

            List<string> labels = new List<string>();
            long unitWidth = 0;
            if (ChartInterval == Interval.m1 || ChartInterval == Interval.m5 ||
                ChartInterval == Interval.m15 || ChartInterval == Interval.m30)
            {
                unitWidth = TimeSpan.FromMinutes(1).Ticks;
            }
            else if (ChartInterval == Interval.h1)
            {
                unitWidth = TimeSpan.FromHours(1).Ticks;
            }
            else if (ChartInterval == Interval.d1)
            {
                unitWidth = TimeSpan.FromDays(1).Ticks;
            }
            Series = new ISeries[] { new LineSeries<double> {
                Values = History.Select(x => x.PriceUsd),
                Stroke = new SolidColorPaint(SKColors.DarkBlue) { StrokeThickness = 0 },
                LineSmoothness = 0,
                GeometrySize = 1,
            } };

            
            /*if(ChartInterval == Interval.m5 || ChartInterval == Interval.m15 || ChartInterval == Interval.m30)
            {

            }
            else if(ChartInterval == Interval.h1 || ChartInterval == Interval.h2 || ChartInterval == Interval.)
            {

            }
            else*/
            if (ChartScale ==  Scale.H1)
                labels = History.Select(x => x.Date.ToString("HH:mm:ss")).ToList();   
            else if (ChartScale ==  Scale.D1)
                labels = History.Select(x => x.Date.ToString("dd/MM")).ToList();  
            else
                labels = History.Select(x => x.Date.ToString("MM/yyyy")).ToList();
            SharedXAxis = new Axis[] { new Axis()
            {
                Labels = labels,
            }};
            
          

            /*
            long unitWidth = 0;
            if (ChartInterval == Interval.m1 || ChartInterval == Interval.m5 ||
                ChartInterval == Interval.m15 || ChartInterval == Interval.m30)
            {
                unitWidth = TimeSpan.FromMinutes(1).Ticks;
            }
            else if (ChartInterval == Interval.h1)
            {
                unitWidth = TimeSpan.FromHours(1).Ticks;
            } 
            else if (ChartInterval == Interval.d1)
            {
                unitWidth = TimeSpan.FromDays(1).Ticks;
            }*/


        }
        private ICommand _selectChart;
        public ICommand SelectChart
        {
            get
            {
                return _selectChart ?? (_selectChart = new RelayCommand(param =>
                {
                    
                }, command => true));
            }
        }
        private ICommand _selectChartInterval;
        public ICommand SelectChartInterval
        {
            get
            {
                return _selectChartInterval ?? (_selectChartInterval = new RelayCommand(param =>
                {
                    if(Enum.TryParse<Interval>(param.ToString(), out Interval selectedInterval))
                    {
                        ChartInterval = selectedInterval;
                    }
                }, command => true));
            }
        } 
        private ICommand _selectChartScale;
        public ICommand SelectChartScale
        {
            get
            {
                return _selectChartScale ?? (_selectChartScale = new RelayCommand(param =>
                {
                    if(Enum.TryParse<Scale>(param.ToString(), out Scale selectedScale))
                    {
                        ChartScale = selectedScale;
                    }
                }, command => true));
            }
        }
        private ICommand _selectTimePint;
        public ICommand UpdateMarket
        {
            get
            {
                return _selectTimePint ?? (_selectTimePint = new RelayCommand(command =>
                {

                }, command => true));
            }
        }
    }
}
