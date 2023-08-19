using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ToDoBot.DataLayer;
using ToDoBot.Repositories.TelegramUsers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDoBot.Services.TelegramServices
{
	public static class TelegramService
	{

		//public TelegramService()
		//{
		//	_telegramBotClient = new TelegramBotClient("6088170455:AAFSrZ2fgZn9ukI2HBqeA52Vfy9wFHTCo18");
		//}

		public static async Task StartReceiver(ITelegramUserRepository telegramUserRepositorY)
		{
			var _telegramBotClient = new TelegramBotClient("6088170455:AAFSrZ2fgZn9ukI2HBqeA52Vfy9wFHTCo18");
			var canToken = new CancellationTokenSource().Token;
			var reOpt = new ReceiverOptions { AllowedUpdates = { } };
			await _telegramBotClient.ReceiveAsync(OnMessage, ErrorMessage, reOpt, canToken);
		}

		public static async Task OnMessage(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToke)
		{
			var message = update.Message;
			if (message.Text is "/Test")
			{
				var key = new KeyboardButton("Confirm PhoneNumber");
				key.RequestLocation = true;
				var replyKey = new ReplyKeyboardMarkup(new[] { new[] { key } });
				//var key = new KeyboardButton("Last name");
				//var repk = new ReplyKeyboardMarkup();

				//await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Hi {message.Chat.FirstName} \n Please enter your last name");
				await botClient.SendTextMessageAsync(message.Chat.Id, $"Hi{message.Chat.FirstName} \n Please confirm your phonenumber", replyMarkup: replyKey);
				//using var db = new ToDoDbContext();
				//await db.TelegramUsers.AddAsync(new DataLayer.Entities.TelegramUser
				//{
				//	Id = message.From.Id,
				//	ChatId = message.Chat.Id,
				//	UserTelegramId = message.From.Username,
				//	FirstName = message.From.FirstName,

				//});
				return;
			}

			if (update.Message.Type is Telegram.Bot.Types.Enums.MessageType.Text)
			{
				var lastName = message.Text;
				await botClient.SendTextMessageAsync(update.Message.Chat.Id, "hhii", replyMarkup: new ReplyKeyboardRemove());
			}

			if (update.Message.Type is Telegram.Bot.Types.Enums.MessageType.Contact)
			{
				await botClient.SendTextMessageAsync(update.Message.Chat.Id, "hhii", replyMarkup: new ReplyKeyboardRemove());
			}
		}

		public static async Task ErrorMessage(ITelegramBotClient botClient, Exception e, CancellationToken cancellationToken)
		{
			if (e is ApiRequestException exp)
			{
				await botClient.SendTextMessageAsync("", e.Message.ToString());
			}
		}


	}
}
