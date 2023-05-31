using StudyingTelegramBot.Models;
using Npgsql;

namespace StudyingTelegramBot.Services {
    public class UserService {
        private readonly NpgsqlConnection _connection;

        public UserService() {
            string connectionString = Config.DefaultConnection;
            _connection = new NpgsqlConnection(connectionString);
        }

        public UserService(string connectionString) {
            _connection = new NpgsqlConnection(connectionString);
        }

        public async Task<User?> GetUserAsync(Guid id) {
            var query = "SELECT * FROM \"Users\" WHERE \"Id\" = @Id";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", id);

            await _connection.OpenAsync();

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            User? user = null;
            if (await reader.ReadAsync()) {
                user = new User {
                    Id = reader.GetGuid(0),
                    TelegramId = reader.GetInt32(1),
                    RemindStartLesson = reader.GetBoolean(2),
                    RemindWriteHomework = reader.GetBoolean(3),
                    RemindCompleteHomework = reader.GetBoolean(4)
                };
            }

            await reader.CloseAsync();
            await _connection.CloseAsync();
            return user;
        }

        public async Task<User?> GetUserByFieldAsync(string fieldName, object value) {
            var query = $"SELECT * FROM \"Users\" WHERE \"{fieldName}\" = @Value";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Value", value);

            await _connection.OpenAsync();

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            User? user = null;
            if (await reader.ReadAsync()) {
                user = new User {
                    Id = reader.GetGuid(0),
                    TelegramId = reader.GetInt32(1),
                    RemindStartLesson = reader.GetBoolean(2),
                    RemindWriteHomework = reader.GetBoolean(3),
                    RemindCompleteHomework = reader.GetBoolean(4)
                };
            }

            await reader.CloseAsync();
            await _connection.CloseAsync();
            return user;
        }

        public async Task CreateUserAsync(User user) {
            var query = "INSERT INTO \"Users\" (\"Id\", \"TelegramId\", \"RemindStartLesson\", \"RemindWriteHomework\", \"RemindCompleteHomework\")"
                        + $"VALUES (@Id, @TelegramId, @RemindStartLesson, @RemindWriteHomework, @RemindCompleteHomework)";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", user.Id);
            command.Parameters.AddWithValue("TelegramId", user.TelegramId);
            command.Parameters.AddWithValue("RemindStartLesson", user.RemindStartLesson);
            command.Parameters.AddWithValue("RemindWriteHomework", user.RemindWriteHomework);
            command.Parameters.AddWithValue("RemindCompleteHomework", user.RemindCompleteHomework);

            await _connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public async Task UpdateUserAsync(User user) {
            var query = "UPDATE \"Users\" SET \"TelegramId\" = @TelegramId, \"RemindStartLesson\" = @RemindStartLesson, \"RemindWriteHomework\" = @RemindWriteHomework, \"RemindCompleteHomework\" = @RemindCompleteHomework WHERE \"Id\" = @Id";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", user.Id);
            command.Parameters.AddWithValue("TelegramId", user.TelegramId);
            command.Parameters.AddWithValue("RemindStartLesson", user.RemindStartLesson);
            command.Parameters.AddWithValue("RemindWriteHomework", user.RemindWriteHomework);
            command.Parameters.AddWithValue("RemindCompleteHomework", user.RemindCompleteHomework);

            await _connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public async Task<bool> DeleteUserAsync(Guid id) {
            var query = "DELETE FROM \"Users\" WHERE \"Id\" = @Id";

            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("Id", id);

            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();

            return rowsAffected > 0;
        }
    }
}
