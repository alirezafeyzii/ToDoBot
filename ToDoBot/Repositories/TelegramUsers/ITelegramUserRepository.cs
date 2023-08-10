using ToDoBot.DataLayer.Entities;
using ToDoBot.Models.TelegramUsers.Outputs;

namespace ToDoBot.Repositories.TelegramUsers
{
	public interface ITelegramUserRepository
	{
		public Task<TelegramUser> CreateUserAsync(TelegramUser telegramUser);
	}
}
