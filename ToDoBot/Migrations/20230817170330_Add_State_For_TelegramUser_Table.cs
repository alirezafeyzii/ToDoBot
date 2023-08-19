using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoBot.Migrations
{
    /// <inheritdoc />
    public partial class Add_State_For_TelegramUser_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "TelegramUsers",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "TelegramUsers");
        }
    }
}
