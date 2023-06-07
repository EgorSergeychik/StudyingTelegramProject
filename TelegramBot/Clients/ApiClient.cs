using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Newtonsoft.Json;
using StudyingTelegramApi.Models;

namespace TelegramBot.Clients
{
    internal class ApiClient
    {
        private string _baseUrl;
        private HttpClient _httpClient;

        public ApiClient()
        {
            _baseUrl = Config.ApiAdress;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);

        }

        public async Task<User?> GetUserAsync(Guid userId) {
            try {
                var response = await _httpClient.GetAsync($"api/User/{userId}");
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

        public async Task<User?> GetUserByTelegramIdAsync(long telegramId) {
            try {
                var response = await _httpClient.GetAsync($"api/User?telegramId={telegramId}");
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

        public async Task<Lesson?> GetLessonAsync(Guid lessonId) {
            try {
                var response = await _httpClient.GetAsync($"api/Lesson/{lessonId}");
                response.EnsureSuccessStatusCode();

                var lessonJson = await response.Content.ReadAsStringAsync();
                var lesson = JsonConvert.DeserializeObject<Lesson>(lessonJson);

                return lesson;
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

        public async Task<List<Lesson>?> GetLessonsAsync(Guid userId) {
            try {
                var response = await _httpClient.GetAsync($"api/Lesson?userId={userId}");
                response.EnsureSuccessStatusCode();

                var lessonsJson = await response.Content.ReadAsStringAsync();
                var lessons = JsonConvert.DeserializeObject<List<Lesson>>(lessonsJson);

                return lessons;
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP Request exception: {ex.Message} + {ex.InnerException}");
                return null;
            } catch (JsonException ex) {
                Console.WriteLine($"Json Deserialize exception: {ex.Message} + {ex.InnerException}");
                return null;
            } catch (Exception ex) {
                Console.WriteLine($"Unknown exception: {ex.Message} + {ex.InnerException}");
                return null;
            }
        }

        public async Task<Guid?> CreateLessonAsync(Lesson lesson) {
            try {
                var lessonJson = JsonConvert.SerializeObject(lesson);
                var content = new StringContent(lessonJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"/api/Lesson", content);
                response.EnsureSuccessStatusCode();

                var createdLessonJson = await response.Content.ReadAsStringAsync();
                var createdLesson = JsonConvert.DeserializeObject<Lesson>(createdLessonJson);

                return createdLesson.Id;
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

        public async Task<bool> DeleteLessonAsync(Guid lessonId) {
            try {
                var response = await _httpClient.DeleteAsync($"api/Lesson/{lessonId}");

                if (response.StatusCode == HttpStatusCode.NoContent) {
                    return true; 
                } else if (response.StatusCode == HttpStatusCode.NotFound) {
                    return false; 
                }

                 Console.WriteLine($"Unexpected status code: {response.StatusCode}");
                 return false;
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP Request exception: {ex.Message}");
                return false;
            } catch (JsonException ex) {
                Console.WriteLine($"Json Serialize exception: {ex.Message}");
                return false;
            } catch (Exception ex) {
                Console.WriteLine($"Unknown exception: {ex.Message}");
                return false;
            }
        }

        public async Task<Homework?> GetHomeworkAsync(Guid homeworkId) {
            try {
                var response = await _httpClient.GetAsync($"api/Homework/{homeworkId}");
                response.EnsureSuccessStatusCode();

                var homeworkJson = await response.Content.ReadAsStringAsync();
                var homework = JsonConvert.DeserializeObject<Homework>(homeworkJson);

                return homework;
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

        public async Task<List<Homework>?> GetHomeworksAsync(Guid userId) {
            try {
                var response = await _httpClient.GetAsync($"api/Homework?userId={userId}");
                response.EnsureSuccessStatusCode();

                var homeworkJson = await response.Content.ReadAsStringAsync();
                var homework = JsonConvert.DeserializeObject<List<Homework>>(homeworkJson);

                return homework;
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP Request exception: {ex.Message} + {ex.InnerException}");
                return null;
            } catch (JsonException ex) {
                Console.WriteLine($"Json Deserialize exception: {ex.Message} + {ex.InnerException}");
                return null;
            } catch (Exception ex) {
                Console.WriteLine($"Unknown exception: {ex.Message} + {ex.InnerException}");
                return null;
            }
        }

        public async Task<Guid?> CreateHomeworkAsync(Homework homework) {
            try {
                var homeworkJson = JsonConvert.SerializeObject(homework);
                var content = new StringContent(homeworkJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"/api/Homework", content);
                response.EnsureSuccessStatusCode();

                var createdHomeworkJson = await response.Content.ReadAsStringAsync();
                var createdHomework = JsonConvert.DeserializeObject<Homework>(createdHomeworkJson);

                return createdHomework.Id;
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

        public async Task<Guid?> UpdateHomeworkAsync(Guid homeworkId, Homework homework) {
            try {
                var homeworkJson = JsonConvert.SerializeObject(homework);
                var content = new StringContent(homeworkJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Homework/{homeworkId}", content);
                response.EnsureSuccessStatusCode();

                var updatedHomeworkJson = await response.Content.ReadAsStringAsync();
                var updatedHomework = JsonConvert.DeserializeObject<Homework>(updatedHomeworkJson);

                return updatedHomework.Id;
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

        public async Task<bool> DeleteHomeworkAsync(Guid homweorkId) {
            try {
                var response = await _httpClient.DeleteAsync($"api/Homework/{homweorkId}");

                if (response.StatusCode == HttpStatusCode.NoContent) {
                    return true;
                } else if (response.StatusCode == HttpStatusCode.NotFound) {
                    return false;
                }

                Console.WriteLine($"Unexpected status code: {response.StatusCode}");
                return false;
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP Request exception: {ex.Message}");
                return false;
            } catch (JsonException ex) {
                Console.WriteLine($"Json Serialize exception: {ex.Message}");
                return false;
            } catch (Exception ex) {
                Console.WriteLine($"Unknown exception: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Group>?> GetKpiGroupsAsync() {
            try {
                var response = await _httpClient.GetAsync($"api/Kpi/groups");
                response.EnsureSuccessStatusCode();

                var groupsJson = await response.Content.ReadAsStringAsync();
                var groups = JsonConvert.DeserializeObject<List<Group>>(groupsJson);

                return groups;
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP Request exception: {ex.Message} + {ex.InnerException}");
                return null;
            } catch (JsonException ex) {
                Console.WriteLine($"Json Deserialize exception: {ex.Message} + {ex.InnerException}");
                return null;
            } catch (Exception ex) {
                Console.WriteLine($"Unknown exception: {ex.Message} + {ex.InnerException}");
                return null;
            }
        }

        public async Task<List<WeekData>?> GetKpiLessonsAsync(Guid groupId) {
            try {
                var response = await _httpClient.GetAsync($"api/Kpi/lessons?groupId={groupId}");
                response.EnsureSuccessStatusCode();

                var pairsJson = await response.Content.ReadAsStringAsync();
                var pairs = JsonConvert.DeserializeObject<List<WeekData>>(pairsJson);

                return pairs;
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP Request exception: {ex.Message} + {ex.InnerException}");
                return null;
            } catch (JsonException ex) {
                Console.WriteLine($"Json Deserialize exception: {ex.Message} + {ex.InnerException}");
                return null;
            } catch (Exception ex) {
                Console.WriteLine($"Unknown exception: {ex.Message} + {ex.InnerException}");
                return null;
            }
        }
    }
}
