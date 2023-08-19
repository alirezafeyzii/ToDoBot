using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoBot.Migrations
{
    /// <inheritdoc />
    public partial class Add_LastMessage_To_TelegramUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastMessage",
                table: "TelegramUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessage",
                table: "TelegramUsers");
        }
    }
}
