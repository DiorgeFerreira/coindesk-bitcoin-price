using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CoinDeskBitcoinPrice
{
    public class CurrencyRate
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Rate { get; set; }
        public string Description { get; set; }
        public decimal Rate_Float { get; set; }
    }

    public class CurrentPrice
    {
        public Dictionary<string, string> Time { get; set; }
        public string Disclaimer { get; set; }
        public string ChartName { get; set; }
        public Dictionary<string, CurrencyRate> Bpi { get; set; }
    }

    public class HistoricalPrice
    {
        public Dictionary<string, decimal> Bpi { get; set; }
        public string Disclaimer { get; set; }
        public Dictionary<string, string> Time { get; set; }
    }

    public class CoinDeskBitcoinPriceClient
    {
        private static readonly HttpClient httpClient;

        static CoinDeskBitcoinPriceClient()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.coindesk.com");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<CurrentPrice> GetCurrentPrice()
        {
            var path = $"/v1/bpi/currentprice.json";
            HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CurrentPrice>(responseBody);
            }
            return null;
        }

        public async Task<CurrentPrice> GetCurrentPrice(string code)
        {
            var path = $"/v1/bpi/currentprice/{code}.json";            
            HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CurrentPrice>(responseBody);
            }
            return null;
        }

        public async Task<HistoricalPrice> GetHistoricalPrice(string index = null, string currency = null, string start = null, string end = null, string forYesterday = null)
        {
            var path = $"/v1/bpi/historical/close.json?";
            path = (index != null) ? path += $"index={index}" : path += "index=USD";
            if (currency != null) path += $"&currency={currency}";
            if (start != null) path += $"&start={start}";
            if (end != null) path += $"&end={end}";
            if (forYesterday != null) path += $"&for=yesterday";
            HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<HistoricalPrice>(responseBody);                
            }
            return null;
        }
    }
}
