using Npgsql;
using StudyingTelegramBot.Models;

namespace StudyingTelegramBot.Services {
    public class LessonService {
        private readonly NpgsqlConnection _connection;

        public LessonService() {
            string connectionString = Config.DefaultConnection;
            _connection = new NpgsqlConnection(connectionString);
        }

        public LessonService(string connectionString) {
            _connection = new NpgsqlConnection(connectionString);
        }

        public async Task<Lesson?> GetLesson(Guid id) {
            var query = "SELECT * FROM \"Lessons\" WHERE \"Id\" = @Id";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", id);

            await _connection.OpenAsync();

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            Lesson? lesson = null;
            if (await reader.ReadAsync()) {
                lesson = new Lesson {
                    Id = reader.GetGuid(0),
                    UserId = reader.GetGuid(1),
                    Title = reader.GetString(2),
                    StartTime = reader.GetDateTime(3),
                    EndTime = reader.GetDateTime(4),
                };
            }

            await reader.CloseAsync();
            await _connection.CloseAsync();
            return lesson;
        }

        public async Task<Lesson?> GetLessonByField(string fieldName, object value) {
            var query = $"SELECT * FROM \"Lessons\" WHERE \"{fieldName}\" = @Value";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Value", value);

            await _connection.OpenAsync();

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            Lesson? lesson = null;
            if (await reader.ReadAsync()) {
                  lesson = new Lesson {
                    Id = reader.GetGuid(0),
                    UserId = reader.GetGuid(1),
                    Title = reader.GetString(2),
                    StartTime = reader.GetDateTime(3),
                    EndTime = reader.GetDateTime(4),
                };
            }

            await reader.CloseAsync();
            await _connection.CloseAsync();
            return lesson;
        }

        public async Task<List<Lesson>?> GetLessonsByUserId(Guid userId) {
            var query = $"SELECT * FROM \"Lessons\" WHERE \"UserId\" = @UserId";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("UserId", userId);

            await _connection.OpenAsync();

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            List<Lesson> lessons = new List<Lesson>();
            while (await reader.ReadAsync()) {
                Lesson lesson = new Lesson {
                    Id = reader.GetGuid(0),
                    UserId = reader.GetGuid(1),
                    Title = reader.GetString(2),
                    StartTime = reader.GetDateTime(3),
                    EndTime = reader.GetDateTime(4),
                };
                lessons.Add(lesson);
            }

            await reader.CloseAsync();
            await _connection.CloseAsync();

            if (lessons.Count < 1)
                return null;

            return lessons;

        }

        public async Task CreateLesson(Lesson lesson) {
            var query = "INSERT INTO \"Lessons\" (\"Id\", \"UserId\", \"Title\", \"StartTime\", \"EndTime\")" +
                "VALUES (@Id, @UserId, @Title, @StartTime, @EndTime)";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", lesson.Id);
            command.Parameters.AddWithValue("UserId", lesson.UserId);
            command.Parameters.AddWithValue("Title", lesson.Title);
            command.Parameters.AddWithValue("StartTime", lesson.StartTime);
            command.Parameters.AddWithValue("EndTime", lesson.EndTime);

            await _connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public async Task UpdateLesson(Lesson lesson) {
            var query = "UPDATE \"Lessons\" SET \"UserId\" = @UserId, \"Title\" = @Title, \"StartTime\" = @StartTime, \"EndTime\" = @EndTime " +
                "WHERE \"Id\" = @Id";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", lesson.Id);
            command.Parameters.AddWithValue("UserId", lesson.UserId);
            command.Parameters.AddWithValue("Title", lesson.Title);
            command.Parameters.AddWithValue("StartTime", lesson.StartTime);
            command.Parameters.AddWithValue("EndTime", lesson.EndTime);

            await _connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public async Task<bool> DeleteLesson(Guid id) {
            var query = "DELETE FROM \"Lessons\" WHERE \"Id\" = @Id";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", id);

            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();

            return rowsAffected > 0;
        }
    }
}
