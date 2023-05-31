if (message.Text == "/start") {
    await botClient.SendTextMessageAsync(message.Chat.Id, "/keyboard");
    return;
} else if (message.Text == "/keyboard") {
    ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
        new[] {
                        new KeyboardButton[] { "your inline", "my inline" }
        }) { ResizeKeyboard = true };
    await botClient.SendTextMessageAsync(message.Chat.Id, "menu", replyMarkup: replyKeyboardMarkup);
    return;
} else if (message.Text == "my inline") {
    InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
        new[] {
                        new InlineKeyboardButton[] { "inline 1", "inline 2" }
        });
    await botClient.SendTextMessageAsync(message.Chat.Id, "inline", replyMarkup: inlineKeyboard);
    return;
} else if (message.Text == "your inline") {
    InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
        new[] {
                        InlineKeyboardButton.WithCallbackData("inline 1"),
                        InlineKeyboardButton.WithCallbackData("inline 2", $"INLINE2")
        });
    await botClient.SendTextMessageAsync(message.Chat.Id, "inline", replyMarkup: inlineKeyboard);
    return;
}