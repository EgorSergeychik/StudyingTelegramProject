namespace StudyingTelegramApi {
    public static class Config {
        private static IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

        public static string? DefaultConnection {
            get => configuration.GetConnectionString("DefaultConnection");
        }

        public static string? KpiApiAdress {
            get => configuration["KpiApiAdress"];
        }
    }
}
