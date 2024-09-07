using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Telegram.Bot.Types;
using ToDoBot.Infrastructure.Filters;
using ToDoBot.Services.Handlers;

namespace ToDoBot.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BotController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> Post(
			[FromBody] Update update,
			[FromServices] UpdateHandlers handleUpdateService,
			CancellationToken cancellationToken)
		{
			await handleUpdateService.HandleUpdateAsync(update, cancellationToken);

            //BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));

            //using (var server = new BackgroundJobServer())
            //{
            //    Console.ReadLine();
            //}

            return Ok();
		}
	}
}
