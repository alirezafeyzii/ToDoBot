using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoBot.Migrations
{
    /// <inheritdoc />
    public partial class Add_TelegramUsers_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TelegramUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChatId = table.Column<long>(type: "INTEGER", nullable: false),
                    UserTelegramId = table.Column<string>(type: "TEXT", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelegramUsers_ChatId",
                table: "TelegramUsers",
                column: "ChatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelegramUsers_Email",
                table: "TelegramUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelegramUsers_Id",
                table: "TelegramUsers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelegramUsers_PhoneNumber",
                table: "TelegramUsers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelegramUsers_UserTelegramId",
                table: "TelegramUsers",
                column: "UserTelegramId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramUsers");
        }
    }
}
