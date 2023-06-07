using Npgsql;
using StudyingTelegramApi;
using StudyingTelegramApi.Models;

namespace StudyingTelegramAPI.Services {
    public class KpiService {
        private readonly string _baseURL;
        private readonly HttpClient _httpClient;

        public KpiService() {
            _baseURL = Config.KpiApiAdress;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseURL);
        }

        //public async Task<List<Group>>
    }
}
