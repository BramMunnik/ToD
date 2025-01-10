using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ToD.Services
{
    public class TruthOrDareService
    {
        private readonly HttpClient _httpClient;

        public TruthOrDareService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetRandomQuestionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.truthordarebot.xyz/v1/truth");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Response: {json}");  // Dit logt de API-respons

                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(json);
                    return apiResponse?.Question ?? "Geen vraag beschikbaar.";
                }
                else
                {
                    Console.WriteLine($"API fout: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception opgetreden: {ex.Message}");
            }

            return "Fout bij het ophalen van de vraag";
        }

        private class ApiResponse
        {
            [JsonProperty("question")]
            public string Question { get; set; }
        }
    }
}
