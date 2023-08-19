using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ToDoBot.Services.WebHooks
{
	public class ConfigureWebhook : IHostedService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly BotConfiguration _botConfiguration;

		public ConfigureWebhook(
			IServiceProvider serviceProvider,
			IOptions<BotConfiguration> botOptions)
		{
			_serviceProvider = serviceProvider;
			_botConfiguration = botOptions.Value;

		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			using var scope = _serviceProvider.CreateScope();
			var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

			var webHookAddress = $"{_botConfiguration.HostAddress}{_botConfiguration.Route}";

			await botClient.SetWebhookAsync(
				url: webHookAddress,
				allowedUpdates: Array.Empty<UpdateType>(),
				secretToken: _botConfiguration.SecretToken,
				cancellationToken: cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			using var scope = _serviceProvider.CreateScope();
			var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

			await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
		}
	}
}
