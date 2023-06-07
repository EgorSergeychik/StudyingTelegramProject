using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot {
    public static class Config {
        private static IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

        public static string TelegramBotToken {
            get => configuration["TelegramBotToken"];
        }

        public static string ApiAdress {
            get => configuration["ApiAdress"];
        }

        public static string KpiApiAdress {
            get => configuration["KpiApiAdress"];
        }
    }
}
