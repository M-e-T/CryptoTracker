using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoTracker.Model
{
    public class DataDownloader
    {
        public readonly string Url;
        public DataDownloader(string url)
        {
            Url = url;
        }
        public async Task<string> DownloadAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Url);
                if (response.IsSuccessStatusCode)
                {
                    return  await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"The request failed with status code: {response.StatusCode}");
                }
                //throw new Exception($"An error occurred: {ex.Message}");
            }
        }
    }
}
