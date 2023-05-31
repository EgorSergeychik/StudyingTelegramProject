using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Newtonsoft.Json;
using StudyingTelegramBot.Models;

namespace TelegramBot.Clients
{
    internal class ApiClient
    {
        private string _baseURl;
        private HttpClient _httpClient;

        public ApiClient()
        {
            _baseURl = Config.ApiAdress;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseURl);

        }

        public async Task<User?> GetUserAsync(Guid id) {
            try {
                var response = await _httpClient.GetAsync($"api/Users/{id}");
                response.EnsureSuccessStatusCode();

                var userJson = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(userJson);

                return user;
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP Request exception: {ex.Message}");
                return null;
            } catch (JsonException ex) { 
                Console.WriteLine($"Json Deserialize exception: {ex.Message}");
                return null;
            } catch (Exception ex) {
                Console.WriteLine($"Unknown exception: {ex.Message}");
                return null;
            }
        }

        public async Task<Guid?> CreateUserAsync(User user) {
            try {
                var userJson = JsonConvert.SerializeObject(user);
                var content = new StringContent(userJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"/api/User", content);
                response.EnsureSuccessStatusCode();

                var createdUserJson = await response.Content.ReadAsStringAsync();
                var createdUser = JsonConvert.DeserializeObject<User>(createdUserJson);

                return createdUser.Id;
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP Request exception: {ex.Message}");
                return null;
            } catch (JsonException ex) {
                Console.WriteLine($"Json Serialize exception: {ex.Message}");
                return null;
            } catch (Exception ex) {
                Console.WriteLine($"Unknown exception: {ex.Message}");
                return null;
            }
        }
    }
}
