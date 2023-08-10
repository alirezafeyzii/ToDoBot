namespace ToDoBot.Models.TelegramUsers.Outputs
{
	public class TelegramUserOutputModel
	{
		public long Id { get; set; }
		public long ChatId { get; set; }
		public string UserTelegramId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
	}
}
	