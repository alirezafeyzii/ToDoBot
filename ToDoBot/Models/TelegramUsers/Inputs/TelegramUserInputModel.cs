using FluentValidation;

namespace ToDoBot.Models.TelegramUsers.Inputs
{
	public class TelegramUserInputModel
	{
		public long Id { get; set; }
		public long ChatId { get; set; }
		public string? UserTelegramId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string? Email { get; set; }
	}

	public class TelegramUserValidator : AbstractValidator<TelegramUserInputModel>
	{
        public TelegramUserValidator()
        {
			RuleFor(tUser => tUser.Id)
				.NotNull()
				.NotEmpty()
				.WithMessage("Required");

			RuleFor(tUser => tUser.ChatId)
				.NotNull()
				.NotEmpty()
				.WithMessage("Required"); ;

			RuleFor(tUser => tUser.PhoneNumber)
				.NotNull()
				.NotEmpty()
				.WithMessage("Required")
				.MinimumLength(11)
				.MaximumLength(11);

			RuleFor(tUser => tUser.FirstName)
				.NotNull()
				.NotEmpty()
				.WithMessage("Required");

			RuleFor(tUser => tUser.LastName)
				.NotNull()
				.NotEmpty()
				.WithMessage("Required");

			RuleFor(tUser => tUser.Email)
				.EmailAddress();
		}
    }
}
