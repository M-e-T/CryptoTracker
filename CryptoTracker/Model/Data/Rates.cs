namespace CryptoTracker.Model.Data
{
    public class Rates
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string CurrencySymbol { get; set; }
        public string Type { get; set; }
        public double RateUsd { get; set; }
    }
}
