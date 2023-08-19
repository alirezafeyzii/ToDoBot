using Microsoft.EntityFrameworkCore;
using ToDoBot.DataLayer.Entities;

namespace ToDoBot.DataLayer
{
	public class ToDoDbContext : DbContext
	{
		public ToDoDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<TelegramUser> TelegramUsers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TelegramUser>(telegramUser =>
			{
				telegramUser
				.HasIndex(tUser => tUser.Id)
				.IsUnique();
				telegramUser
				.Property(tUser => tUser.Id)
				.IsRequired();

				telegramUser
				.HasIndex(tUser => tUser.ChatId)
				.IsUnique();
				telegramUser
				.Property(tUser => tUser.ChatId)
				.IsRequired();

				telegramUser
				.HasIndex(tUser => tUser.UserTelegramId)
				.IsUnique();

				telegramUser
				.HasIndex(tUser => tUser.PhoneNumber)
				.IsUnique();
				telegramUser
				.Property(tUser => tUser.PhoneNumber)
				.HasDefaultValue("1")
				.IsRequired();

				telegramUser
				.HasIndex(tUser => tUser.Email)
				.IsUnique();
				telegramUser
				.Property(tUser => tUser.Email)
				.HasDefaultValue("null");
			});
		}
	}
}
