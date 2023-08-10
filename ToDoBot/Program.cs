using Microsoft.EntityFrameworkCore;
using ToDoBot.DataLayer;
using ToDoBot.Services.TelegramServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(options =>
{
	options.UseSqlite(builder.Configuration.GetConnectionString("ToDoDbConnection"));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.StartReceiver();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
