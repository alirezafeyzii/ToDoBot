using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using ToDoBot.Services.TelegramServices;

namespace ToDoBot.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		[HttpPost]
		public async void Test()
		{

		}
	}
}
