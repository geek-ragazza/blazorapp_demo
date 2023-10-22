namespace BlazorApp.Data
{
    public class BigMacData
    {
        public Country Country { get; set; } 
        public string CurrencyCode { get; set; }
        public decimal LocalPrice { get; set; }
        public decimal DollarExchangeRate { get; set; }
        public decimal GdpDollar { get; set; }
        public decimal GdpLocal  { get; set; }
        public DateTime Date { get; set; }
    }
}
