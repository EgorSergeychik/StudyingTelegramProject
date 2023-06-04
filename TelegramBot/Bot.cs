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
using MyUser = StudyingTelegramBot.Models.User;
using System.Globalization;

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
                ApiRequestException apiRequestException => $"[{apiRequestException.ErrorCode}] Telegram API error:\n" +
                $"{apiRequestException.Message}",
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
            string[] commandParts = message.Text.Split();
            string command = commandParts[0].ToLower();

            switch (command) {
                case "/start":
                    await HandleStartCommandAsync(botClient, message);
                    break;
                case "/help":
                    await HandleHelpCommandAsync(botClient, message);
                    break;
                case "/addlesson":
                    await HandleAddLessonCommandAsync(botClient, message);
                    break;
                case "/mylessons":
                    await HandleMyLessonsCommandAsync(botClient, message);
                    break;
            }
        }

        private async Task HandleStartCommandAsync(ITelegramBotClient botClient, Message message) {
            var user = new MyUser {
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

        private async Task HandleAddLessonCommandAsync(ITelegramBotClient botClient, Message message) {
            string[] parameters = message.Text.Split();

            if (parameters.Length != 5) {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Неправильний формат.\n" +
                    "/addlesson Назва_уроку Початок Кінець ДеньТижня");
                return;
            }

            var lessonName = parameters[1].Replace("_", " ").Replace("*", "");
            var startTimeStr = parameters[2];
            var endTimeStr = parameters[3];
            var dayOfWeekStr = parameters[4];

            DateTime startTime, endTime;
            string[] timeFormats = new string[] { "HH:mm", "H:m", "H:mm", "HH:m" };
            bool tryParseStartTime = DateTime.TryParseExact(startTimeStr, timeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime);
            bool tryParseEndTime = DateTime.TryParseExact(endTimeStr, timeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime);
            if (!tryParseStartTime || !tryParseEndTime) {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Неправильний формат.\n" +
                    "Час потрібно вказувати у форматі Г:Х (наприклад, 16:40).");
                return;
            }

            DayOfWeek dayOfWeek;
            if (!Enum.TryParse(dayOfWeekStr, true, out dayOfWeek)) {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Неправильний формат.\n" +
                    "День тижня потрібно вказувати числом від 0 (неділя) до 6 (субота) або назву на англійській мові.");
                return;
            }

            MyUser user = await _apiClient.GetUserByTelegramIdAsync(message.From.Id);

            var lesson = new Lesson {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Title = lessonName,
                StartTime = startTime,
                EndTime = endTime,
                DayOfWeek = dayOfWeek
            };

            await _apiClient.CreateLessonAsync(lesson);

            await botClient.SendTextMessageAsync(message.Chat.Id, $"Успішно додано урок *{lessonName}*\\!", parseMode: ParseMode.MarkdownV2);
        }

        private async Task HandleMyLessonsCommandAsync(ITelegramBotClient botClient, Message message) {
            var user = await _apiClient.GetUserByTelegramIdAsync(message.From.Id);
            List<Lesson>? lessons = await _apiClient.GetLessonsAsync(user.Id);

            if (lessons == null) {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Ваш розклад __пустий__.", parseMode: ParseMode.MarkdownV2);
                return;
            }

            var lessonsMessage = new StringBuilder();
            lessonsMessage.AppendLine("*__Ваш розклад:__*\n");
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek))) {
                var lessonsOfTheDay = lessons.FindAll(lesson => lesson.DayOfWeek == day);
                lessonsMessage.AppendLine($"*\\- {day.ToString()}*");
                byte lessonNumber = 1;
                foreach (var lesson in lessonsOfTheDay) {
                    var startTime = lesson.StartTime.ToLocalTime().ToString("HH:mm");
                    var endTime = lesson.EndTime.ToLocalTime().ToString("HH:mm");
                    lessonsMessage.AppendLine($"*\\[{lessonNumber}\\]* {lesson.Title} \\| _{startTime}_\\-_{endTime}_");

                    lessonNumber++;
                }
                lessonsMessage.AppendLine();
            }

            await botClient.SendTextMessageAsync(message.Chat.Id, lessonsMessage.ToString(), parseMode: ParseMode.MarkdownV2);
        }
    }
}
