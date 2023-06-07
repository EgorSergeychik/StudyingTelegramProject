using Npgsql;
using StudyingTelegramApi.Models;

namespace StudyingTelegramApi.Services {
    public class HomeworkService {
        private readonly NpgsqlConnection _connection;

        public HomeworkService() {
            string connectionString = Config.DefaultConnection;
            _connection = new NpgsqlConnection(connectionString);
        }

        public HomeworkService(string connectionString) {
            _connection = new NpgsqlConnection(connectionString);
        }

        public async Task<Homework?> GetHomeworkAsync(Guid id) {
            var query = "SELECT * FROM \"Homework\" WHERE \"Id\" = @Id";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", id);

            await _connection.OpenAsync();

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            Homework? homework = null;
            if (await reader.ReadAsync()) {
                homework = new Homework {
                    Id = reader.GetGuid(0),
                    UserId = reader.GetGuid(1),
                    Title = reader.GetString(2),
                    Description = reader.GetString(3),
                    DueDate = reader.GetDateTime(4),
                    IsCompleted = reader.GetBoolean(5)
                };
            }

            await reader.CloseAsync();
            await _connection.CloseAsync();
            return homework;
        }

        public async Task<Homework?> GetHomeworkByFieldAsync(string fieldName, object value) {
            var query = $"SELECT * FROM \"Users\" WHERE \"{fieldName}\" = @Value";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Value", value);

            await _connection.OpenAsync();

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            Homework? homework = null;
            if (await reader.ReadAsync()) {
                homework = new Homework {
                    Id = reader.GetGuid(0),
                    UserId = reader.GetGuid(1),
                    Title = reader.GetString(2),
                    Description = reader.GetString(3),
                    DueDate = reader.GetDateTime(4),
                    IsCompleted = reader.GetBoolean(5)
                };
            }

            await reader.CloseAsync();
            await _connection.CloseAsync();
            return homework;
        }

        public async Task<List<Homework>?> GetHomeworkByUserIdAsync(Guid userId) {
            var query = $"SELECT * FROM \"Homework\" WHERE \"UserId\" = @UserId";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("UserId", userId);

            await _connection.OpenAsync();

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            List<Homework> homeworkList = new List<Homework>();
            while (await reader.ReadAsync()) {
                Homework homework = new Homework {
                    Id = reader.GetGuid(0),
                    UserId = reader.GetGuid(1),
                    Title = reader.GetString(2),
                    Description = reader.GetString(3),
                    DueDate = reader.GetDateTime(4),
                    IsCompleted = reader.GetBoolean(5)
                };
                homeworkList.Add(homework);
            }

            await reader.CloseAsync();
            await _connection.CloseAsync();

            if (homeworkList.Count < 1)
                return null;

            return homeworkList;

        }

        public async Task CreateHomeworkAsync(Homework homework) {
            var query = "INSERT INTO \"Homework\" (\"Id\", \"UserId\", \"Title\", \"Description\", \"DueDate\", \"IsCompleted\")"
                        + $"VALUES (@Id, @UserId, @Title, @Description, @DueDate, @IsCompleted)";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", homework.Id);
            command.Parameters.AddWithValue("UserId", homework.UserId);
            command.Parameters.AddWithValue("Title", homework.Title);
            command.Parameters.AddWithValue("Description", homework.Description);
            command.Parameters.AddWithValue("DueDate", homework.DueDate);
            command.Parameters.AddWithValue("IsCompleted", homework.IsCompleted);

            await _connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public async Task UpdateHomeworkAsync(Homework homework) {
            var query = "UPDATE \"Homework\" SET \"UserId\" = @UserId, \"Title\" = @Title, \"Description\" = @Description, \"DueDate\" = @DueDate, \"IsCompleted\" = @IsCompleted WHERE \"Id\" = @Id";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", homework.Id);
            command.Parameters.AddWithValue("UserId", homework.UserId);
            command.Parameters.AddWithValue("Title", homework.Title);
            command.Parameters.AddWithValue("Description", homework.Description);
            command.Parameters.AddWithValue("DueDate", homework.DueDate);
            command.Parameters.AddWithValue("IsCompleted", homework.IsCompleted);

            await _connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public async Task<bool> DeleteHomeworkAsync(Guid id) {
            var query = "DELETE FROM \"Homework\" WHERE \"Id\" = @Id";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", id);

            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();

            return rowsAffected > 0;
        }
    }
}
