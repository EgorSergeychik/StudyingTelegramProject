using TelegramBot;
using TelegramBot.Clients;

ApiClient apiClient = new ApiClient();
Bot bot = new Bot(apiClient);
bot.Run();
Console.ReadKey();