using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;

using CryptoTracker.Model;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using LiveChartsCore.Defaults;
using System.Windows.Input;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Diagnostics;
using static CryptoTracker.ViewModel.TimeScale;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

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
    public enum Scale
    {
        H1,
        H6,
        D1,
        D7,
        M1,
        M6,
        M12,
        Max
    }
    public static class TimeScale
    {
        private static readonly Dictionary<Scale, DateTime> TimeByScale = new Dictionary<Scale, DateTime>() 
        {
            { Scale.H1, DateTime.UtcNow.AddHours(-1) },
            { Scale.H6 , DateTime.UtcNow.AddHours(-6) },
          
            { Scale.D1 , DateTime.UtcNow.AddDays(-1) },
            { Scale.D7 , DateTime.UtcNow.AddDays(-7) },

            { Scale.M1 , DateTime.UtcNow.AddMonths(-1) },
            { Scale.M6 , DateTime.UtcNow.AddMonths(-6) },
            { Scale.M12 , DateTime.UtcNow.AddMonths(-12) },

            { Scale.Max , DateTime.UtcNow }
        };
        public static DateTime GetPeriod(Scale scale)
        {
            if (TimeByScale.TryGetValue(scale, out DateTime value))
            {
                return value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(scale), "Key not found");
            }
        }
    }
    public class History
    {
        public double PriceUsd { get; set; }
        public DateTime Date { get; set; }
    }
    public class ChartViewModel : BaseViewModel
    {
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

        public ChartViewModel()
        {
            UpdateChart();
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
        private ISeries[] _seriesCollection1;
        public ISeries[] SeriesCollection1
        {
            get => _seriesCollection1;
            set
            {
                _seriesCollection1 = value;
                OnPropertyChanged();
            }
        }

        public Axis[] SharedXAxis1 { get; set;}
        public Axis[] SharedXAxis2 { get; set;}
        public Axis[] SharedXAxis3 { get; set;}
        public Axis[] SharedXAxis4 { get; set;}
        public void UpdateChart()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.coincap.io/v2/assets/bitcoin/history?interval={ChartInterval}");
            var response = client.Send(request);
            var jsonString = response.Content.ReadAsStringAsync().Result.ToString();

            JObject jsonObject = JObject.Parse(jsonString);
            JArray dataArray = (JArray)jsonObject["data"];
            History = dataArray.ToObject<ObservableCollection<History>>();
            var time = History[^1].Date.ToLongDateString(); 
            var time2 = History[^1].Date.ToLongTimeString(); 

            History = new ObservableCollection<History>(History.Where(x => x.Date > TimeScale.GetPeriod(ChartScale)));

            Debug.WriteLine("Точок " + History.Count);
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
            //SharedXAxis = new Axis[] { new Axis() { Labels = History.Select(x => x.Time.ToString()).ToArray()}};


            SeriesCollection1 = new ISeries[] { new LineSeries<double> { 
                Values = History.Select(x => x.PriceUsd),
                Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = 2 },
                LineSmoothness = 0,
                GeometrySize = 1,
            } };

            SharedXAxis = new Axis[] { new Axis() 
            { 
                Labels = History.Select(x => x.Date.ToLongTimeString()).ToArray(),
                UnitWidth = unitWidth,
                MinStep = unitWidth
            }};

            SharedXAxis1 = new Axis[] { new Axis() 
            { 
                Labels = History.Select(x => x.Date.ToShortTimeString()).ToArray(),
            }};
            SharedXAxis2 = new Axis[] { new Axis() 
            { 
                Labels = History.Select(x => x.Date.ToShortTimeString()).ToArray(),
                UnitWidth = long.MaxValue,
                MinStep = long.MaxValue
            }};                
            SharedXAxis3 = new Axis[] { new Axis() 
            { 
                Labels = History.Select(x => x.Date.ToShortTimeString()).ToArray(),
                UnitWidth = TimeSpan.FromSeconds(1).Ticks,
                MinStep  = -5d
            }};

            OnPropertyChanged("SharedXAxis1");
            OnPropertyChanged("SharedXAxis2");
            OnPropertyChanged("SharedXAxis3");

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
        /*public ISeries[] SeriesCollection2
        {
            get => new ISeries[] { new ColumnSeries<int> { Values = Enumerable.Range(0, 50).Select(x=> x = new Random().Next(-900,10000)) } } ;
        }*/
        // public Axis[] SharedXAxis { get; set; }
        //public Margin DrawMargin { get; set; }
    }
}
