using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDoBot.Services.TelegramServices
{
	public static class TelegramService 
	{
		
		//public TelegramService()
		//{
		//	_telegramBotClient = new TelegramBotClient("6088170455:AAFSrZ2fgZn9ukI2HBqeA52Vfy9wFHTCo18");
		//}

		public static async Task StartReceiver(this IServiceCollection ser)
		{
			var _telegramBotClient = new TelegramBotClient("6088170455:AAFSrZ2fgZn9ukI2HBqeA52Vfy9wFHTCo18");
			var canToken = new CancellationTokenSource().Token;
			var reOpt = new ReceiverOptions { AllowedUpdates = { } };
			await _telegramBotClient.ReceiveAsync(OnMessage, ErrorMessage, reOpt, canToken);
		}

		public static async Task OnMessage(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
		{
			if(update.Message is Message message)
			{
				await botClient.SendTextMessageAsync(message.Chat.Id, );
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
