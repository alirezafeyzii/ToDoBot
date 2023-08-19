using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using ToDoBot.Repositories.TelegramUsers;
using ToDoBot.Services.Handlers;
using ToDoBot.Services.TelegramServices;

namespace ToDoBot.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		private readonly ITelegramBotClient _botClient;
		private readonly ITelegramUserRepository _telegramUserRepository;

		public TestController(
			ITelegramBotClient botClient,
			ITelegramUserRepository telegramUserRepository)
		{
			_botClient = botClient;
			_telegramUserRepository = telegramUserRepository;
		}

		[HttpPost]
		public async void Test()
		{
			var updateHandlers = new UpdateHandlers(_botClient, _telegramUserRepository);
			
			var a = new CancellationToken();
			await updateHandlers.HandleUpdateAsync(update: new Update()
			{
				Message = new Message()
				{
					Text = "/inline_keyboard",
					Chat = new Chat()
					{
						Id = 96330247,
						FirstName = "Alireza",
						Username = "Alireza_fzx"
					}
				}
			}, a);
		}
	}
}
