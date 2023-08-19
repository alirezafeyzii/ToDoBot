using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using ToDoBot.Repositories.TelegramUsers;
using static ToDoBot.DataLayer.Entities.TelegramUser;

namespace ToDoBot.Services.Handlers
{
	public class UpdateHandlers
	{
		private readonly ITelegramBotClient _botClient;
		private readonly ITelegramUserRepository _telegramUserRepository;

		public UpdateHandlers(
			ITelegramBotClient botClient,
			ITelegramUserRepository telegramUserRepository)
		{
			_botClient = botClient;
			_telegramUserRepository = telegramUserRepository;
		}

		public Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
		{
			var errorMessage = exception switch
			{
				ApiRequestException apiRequestException => $"Telegram API Error: \n [{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
				_ => exception.ToString()
			};

			return Task.CompletedTask;
		}

		public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
		{
			var handler = update switch
			{
				{ Message: { } message } => BotOnMessageReceived(message, cancellationToken),
				{ EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
				{ CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
				{ InlineQuery: { } inlineQuery } => BotOnInlineQueryReceived(inlineQuery, cancellationToken),
				{ ChosenInlineResult: { } chosenInlineResult } => BotOnChosenInlineResultReceived(chosenInlineResult, cancellationToken),
				_ => UnknownUpdateHandlerAsync(update, cancellationToken)
			};

			await handler;
		}

		private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
		{
			//if (message.Text is not { } messageText)
			//	return;

			if (message.Type == MessageType.Contact && message.Text is null)
			{
				await ConfirmPhoneNumber(_botClient, _telegramUserRepository, message, cancellationToken);
				return;
			}

			var messageText = message.Text;
			var action = messageText.Split(' ')[0] switch
			{
				"/start" => SendStart(_botClient, _telegramUserRepository, message, cancellationToken),
				"/inline_keyboard" => SendInlineKeyboard(_botClient, message, cancellationToken),
				"/keyboard" => SendReplyKeyboard(_botClient, message, cancellationToken),
				"/remove" => RemoveKeyboard(_botClient, message, cancellationToken),
				"/photo" => SendFile(_botClient, message, cancellationToken),
				"/request" => RequestContactAndLocation(_botClient, message, cancellationToken),
				"/inline_mode" => StartInlineQuery(_botClient, message, cancellationToken),
				_ => Usage(_botClient, message, cancellationToken)
			};
			Message sentMessage = await action;

			static async Task<Message> SendStart(
				ITelegramBotClient telegramBotClient,
				ITelegramUserRepository telegramUserRepository,
				Message message,
				CancellationToken cancellationToken)
			{
				await telegramBotClient.SendChatActionAsync(
					chatId: message.Chat.Id,
					chatAction: ChatAction.Typing,
					cancellationToken: cancellationToken);

				await Task.Delay(500, cancellationToken);

				ReplyKeyboardMarkup RequestReplyKeyboard = new(
					new[]
					{
					KeyboardButton.WithRequestContact("Contact"),
					});

				await telegramUserRepository.CreateUserAsync(new DataLayer.Entities.TelegramUser()
				{
					ChatId = message.Chat.Id,
					//bayad be user name taghir peyda kone khat paeen => usertelegram Id beshe usertelegram name masalan alireza_fzx
					UserTelegramId = message.Chat.Username,
					FirstName = message.From!.FirstName,
					State = (int)TelegramUserState.CreateWithioutPhoneNumber,
					CreatedDate = DateTimeOffset.Now,
					UpdateDate = DateTimeOffset.Now,
				});

				return await telegramBotClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: "Please confirm your phone number",
					replyMarkup: RequestReplyKeyboard,
					cancellationToken: cancellationToken);

			}

			static async Task<Message> ConfirmPhoneNumber(
				ITelegramBotClient telegramBotClient,
				ITelegramUserRepository telegramUserRepository,
				Message message,
				CancellationToken cancellationToken)
			{
				//await telegramUserRepository. state avaz she va user update beshe va telephone ham sabt beshe 
				return await telegramBotClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: "Successfully",
					replyMarkup: new ReplyKeyboardRemove(),
					cancellationToken: cancellationToken);
			}

			static async Task<Message> SendInlineKeyboard(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
			{
				message.Chat.Id = 96330247;
				await telegramBotClient.SendChatActionAsync(
					chatId: message.Chat.Id,
					chatAction: ChatAction.Typing,
					cancellationToken: cancellationToken);

				await Task.Delay(500, cancellationToken);

				InlineKeyboardMarkup inlineKeyboardMarkup = new(
					new[]
					{
						new[]
						{
							InlineKeyboardButton.WithCallbackData("1.1", "11"),
							InlineKeyboardButton.WithCallbackData("1.2", "12"),
						},
						new[]
						{
							InlineKeyboardButton.WithCallbackData("2.1", "21"),
							InlineKeyboardButton.WithCallbackData("2.2", "22")
						}
					});

				return await telegramBotClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: "Choose",
					replyMarkup: inlineKeyboardMarkup,
					cancellationToken: cancellationToken);
			}

			static async Task<Message> SendReplyKeyboard(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
			{
				ReplyKeyboardMarkup replyKeyboardMarkup = new(
				new[]
				{
						new KeyboardButton[] { "1.1", "1.2" },
						new KeyboardButton[] { "2.1", "2.2" },
				})
				{
					ResizeKeyboard = true,
				};

				return await telegramBotClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: "Choose",
					replyMarkup: replyKeyboardMarkup,
					cancellationToken: cancellationToken);
			}

			static async Task<Message> RemoveKeyboard(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
			{
				return await telegramBotClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: "Remove keyboard",
					replyMarkup: new ReplyKeyboardRemove(),
					cancellationToken: cancellationToken);
			}

			static async Task<Message> SendFile(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
			{
				await telegramBotClient.SendChatActionAsync(
					message.Chat.Id,
					ChatAction.UploadPhoto,
					cancellationToken: cancellationToken);

				const string filePath = "Files/biil.png";

				await using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

				var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

				return await telegramBotClient.SendPhotoAsync(
					chatId: message.Chat.Id,
					photo: new InputFileStream(fileStream, fileName),
					caption: "Ddasham BillGates",
					cancellationToken: cancellationToken);
			}

			static async Task<Message> RequestContactAndLocation(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
			{
				ReplyKeyboardMarkup RequestReplyKeyboard = new(
					new[]
					{
					KeyboardButton.WithRequestLocation("Location"),
					KeyboardButton.WithRequestContact("Contact"),
					});

				return await telegramBotClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: "Who or Where are you?",
					replyMarkup: RequestReplyKeyboard,
					cancellationToken: cancellationToken);
			}

			static async Task<Message> Usage(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
			{
				const string usage = "Usage:\n" +
									 "/start - start Bot \n" +
									 "/inline_keyboard - send inline keyboard\n" +
									 "/keyboard    - send custom keyboard\n" +
									 "/remove      - remove custom keyboard\n" +
									 "/photo       - send a photo\n" +
									 "/request     - request location or contact\n" +
									 "/inline_mode - send keyboard with Inline Query";

				return await telegramBotClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: usage,
					replyMarkup: new ReplyKeyboardRemove(),
					cancellationToken: cancellationToken);
			}

			static async Task<Message> StartInlineQuery(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
			{
				InlineKeyboardMarkup inlineKeyboardMarkup = new(
					InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Inline Mode"));

				return await telegramBotClient.SendTextMessageAsync(
					chatId: message.Chat.Id,
					text: "Press the button to start query",
					replyMarkup: inlineKeyboardMarkup,
					cancellationToken: cancellationToken);
			}
		}

		private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
		{
			await _botClient.AnswerCallbackQueryAsync(
				callbackQueryId: callbackQuery.Id,
				text: $"Received {callbackQuery.Data}",
				cancellationToken: cancellationToken);

			await _botClient.SendTextMessageAsync(
				chatId: callbackQuery.Message.Chat.Id,
				text: $"Received {callbackQuery.Data}",
				cancellationToken: cancellationToken);
		}

		private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery, CancellationToken cancellationToken)
		{
			InlineQueryResult[] results =
			{
				new InlineQueryResultArticle(
					id: "1",
					title: "TgBots",
					inputMessageContent: new InputTextMessageContent("hello"))
			};

			await _botClient.AnswerInlineQueryAsync(
				inlineQueryId: inlineQuery.Id,
				results: results,
				cacheTime: 0,
				isPersonal: true,
				cancellationToken: cancellationToken);
		}

		private async Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
		{
			await _botClient.SendTextMessageAsync(
				chatId: chosenInlineResult.From.Id,
				text: $"You chose result with Id: {chosenInlineResult.ResultId}",
				cancellationToken: cancellationToken);
		}

		private async Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
		{
			await Task.CompletedTask;
		}
	}
}
