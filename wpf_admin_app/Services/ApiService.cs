using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace chamcong.WpfAdmin.Services
{
    public class ApiService
    {
        private static readonly HttpClient _client = new HttpClient();
        public static string BaseUrl { get; set; } = "http://localhost:5052";
        public static string? Token { get; private set; }

        public ApiService()
        {
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { username, password }), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{BaseUrl}/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(responseBody);
                Token = data?.token;
                return true;
            }
            return false;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}{endpoint}");
            if (!string.IsNullOrEmpty(Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return default;
        }

        public async Task<bool> PostAsync(string endpoint, object data)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}{endpoint}");
            if (!string.IsNullOrEmpty(Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync(string endpoint, object data)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}{endpoint}");
            if (!string.IsNullOrEmpty(Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}{endpoint}");
            if (!string.IsNullOrEmpty(Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            var response = await _client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
