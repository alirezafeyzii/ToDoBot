using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using ToDoBot.Controllers;
using ToDoBot.DataLayer;
using ToDoBot.Infrastructure.Extensions;
using ToDoBot.Repositories.TelegramUsers;
using ToDoBot.Services.Handlers;
using ToDoBot.Services.TelegramServices;
using ToDoBot.Services.WebHooks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(options =>
{
	options.UseSqlite(builder.Configuration.GetConnectionString("ToDoDbConnection"));
});

var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>();

builder.Services.AddHttpClient("telegram_bot_client")
				.AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
				{
					BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
					TelegramBotClientOptions options = new(botConfig.BotToken);
					return new TelegramBotClient(options, httpClient);
				});

builder.Services.AddScoped<ITelegramUserRepository, TelegramUserRepository>();

builder.Services.AddScoped<UpdateHandlers>();


//builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddControllers()
	            .AddNewtonsoftJson();

//builder.Services.StartReceiver();

var app = builder.Build();



//app.UseHttpsRedirection();

//app.UseAuthorization();
app.MapWebhookRoute<BotController>(route: botConfiguration.Route);

app.MapControllers();

app.Run();


#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable RCS1110 // Declare type inside namespace.
public class BotConfiguration
#pragma warning restore RCS1110 // Declare type inside namespace.
#pragma warning restore CA1050 // Declare types in namespaces
{
	public static readonly string Configuration = "BotConfiguration";

	public string BotToken { get; init; } = default!;
	public string HostAddress { get; init; } = default!;
	public string Route { get; init; } = default!;
	public string SecretToken { get; init; } = default!;
}