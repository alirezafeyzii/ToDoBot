namespace ToDoBot.DataLayer.Entities
{
	public class TelegramUser
	{
		public long Id { get; set; }
		public long ChatId { get; set; }
		public string? UserTelegramId { get; set; }
		public string FirstName { get; set; }
		public string? LastName { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Email { get; set; }
		public DateTimeOffset CreatedDate { get; set; }
		public DateTimeOffset UpdateDate { get; set; }
		public string? LastMessage { get; set; }
		public int? State { get; set; }

		public enum TelegramUserState
		{
			NotExist = 0,
			CreateWithioutPhoneNumber = 1,
			CreatedWithoutLastName = 2,
		}
	}
}
