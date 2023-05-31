namespace StudyingTelegramBot {
    public static class Config {
        public static string? DefaultConnection {
            get {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                return configuration.GetConnectionString("DefaultConnection");
            }
        }
    }
    // TODO: rewrite config system
}
