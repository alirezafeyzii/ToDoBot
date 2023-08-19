using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Telegram.Bot.Types;
using ToDoBot.Infrastructure.Filters;
using ToDoBot.Services.Handlers;

namespace ToDoBot.Controllers
{
	public class BotController : Controller
	{
		[HttpPost]
		public async Task<IActionResult> Post(
			[FromBody] Update update,
			[FromServices] UpdateHandlers handleUpdateService,
			CancellationToken cancellationToken)
		{
			await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
			return Ok();
		}
	}
}
