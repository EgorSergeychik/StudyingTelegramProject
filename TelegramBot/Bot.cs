using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Clients;
using StudyingTelegramBot.Models;
using System.Reflection;

namespace TelegramBot {
    internal class Bot {
        TelegramBotClient botClient = new TelegramBotClient(Config.TelegramBotToken);
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

        private ApiClient _apiClient;

        public Bot(ApiClient apiClient) {
            _apiClient = apiClient;
        }
        
        public async Task Run() {
            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);
            
            var self = await botClient.GetMeAsync();
            Console.WriteLine($"{self.Username} started.");
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
            var errorMessage = exception switch {
                ApiRequestException apiRequestException => $"Telegram API error:\n {apiRequestException.ErrorCode}" +
                $"\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
            switch (update?.Type) {
                case UpdateType.Message:
                    await HandleMessageAsync(botClient, update?.Message);
                    break;
            }
        }

        private async Task HandleMessageAsync(ITelegramBotClient botClient, Message message) {
            switch (message.Text) {
                case "/start":
                    await HandleStartCommandAsync(botClient, message);
                    break;
                case "/help":
                    await HandleHelpCommandAsync(botClient, message);
                    break;
            }
        }

        private async Task HandleStartCommandAsync(ITelegramBotClient botClient, Message message) {
            var user = new StudyingTelegramBot.Models.User {
                Id = Guid.NewGuid(),
                TelegramId = message.From.Id,
                RemindStartLesson = false,
                RemindWriteHomework = false,
                RemindCompleteHomework = false
            };
            var createdUserId = await _apiClient.CreateUserAsync(user);

            await botClient.SendTextMessageAsync(message.Chat.Id, $"Привіт! Я можу стати твоїм помічником у навчальному процесі.\n" +
                                                                  $"Я можу пам'ятати твій розклад, домашні завдання та нагадувати" +
                                                                  $" про початок уроків/пар і необхідність записати/виконати ДЗ.\n" +
                                                                  $"/help — список наявних команд.");
        }

        private async Task HandleHelpCommandAsync(ITelegramBotClient botClient, Message message) {
            var commandMethods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(method => method.Name.StartsWith("Handle") && method.Name.EndsWith("CommandAsync"))
                .ToList();

            var helpMessage = new StringBuilder();
            helpMessage.AppendLine("Список команд:");

            foreach (var method in commandMethods) {
                var commandName = method.Name.Replace("Handle", "").Replace("CommandAsync", "").ToLower();
                helpMessage.AppendLine($"/{commandName}");
            }

            await botClient.SendTextMessageAsync(message.Chat.Id, helpMessage.ToString());
        }
    }
}
