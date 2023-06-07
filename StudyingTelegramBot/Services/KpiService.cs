using Newtonsoft.Json;
using StudyingTelegramApi;
using StudyingTelegramApi.Models;
using System.Text;

namespace StudyingTelegramApi.Services {
    public class KpiService {
        private readonly string _baseURL;
        private readonly HttpClient _httpClient;

        public KpiService() {
            _baseURL = Config.KpiApiAdress;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseURL);
        }

        public async Task<List<Group>?> GetGroupsAsync() {
            try {
                var response = await _httpClient.GetAsync($"api/schedule/groups");
                response.EnsureSuccessStatusCode();

                var groupsJson = await response.Content.ReadAsStringAsync();
                var groupResponse = JsonConvert.DeserializeObject<GroupResponse>(groupsJson);
                var groupsList = groupResponse?.Data;

                return groupsList;
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP Request exception: {ex.Message}");
                return null;
            } catch (JsonException ex) {
                Console.WriteLine($"Json Deserialize exception: {ex.Message}");
                return null;
            } catch (Exception ex) {
                Console.WriteLine($"Unknown exception: {ex.Message} {ex.InnerException}");
                return null;
            }
        }
    }
}
