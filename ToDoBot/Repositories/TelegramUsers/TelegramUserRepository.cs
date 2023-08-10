using Microsoft.EntityFrameworkCore;
using ToDoBot.DataLayer;
using ToDoBot.DataLayer.Entities;

namespace ToDoBot.Repositories.TelegramUsers
{
	public class TelegramUserRepository : ITelegramUserRepository
	{
		private readonly ToDoDbContext _dbContext;

		public TelegramUserRepository(ToDoDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<TelegramUser> CreateUserAsync(TelegramUser telegramUser)
		{
			bool telegramUserExist = telegramUser.Email == null
				? await _dbContext.TelegramUsers
				.AnyAsync(tUser => tUser.Id == telegramUser.Id
				|| tUser.ChatId == telegramUser.ChatId
				|| tUser.UserTelegramId == telegramUser.UserTelegramId
				|| tUser.PhoneNumber == telegramUser.PhoneNumber)
				: await _dbContext.TelegramUsers
				.AnyAsync(tUser => tUser.Id == telegramUser.Id
				|| tUser.ChatId == telegramUser.ChatId
				|| tUser.UserTelegramId == telegramUser.UserTelegramId
				|| tUser.PhoneNumber == telegramUser.PhoneNumber
				|| tUser.Email == telegramUser.Email);

			if (telegramUserExist)
				return null!;

			var telegramUserAfterCreate = await _dbContext.TelegramUsers.AddAsync(telegramUser);

			await _dbContext.SaveChangesAsync();

			return telegramUserAfterCreate.Entity;
		}
	}
}
