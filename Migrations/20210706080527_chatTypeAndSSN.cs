using Microsoft.EntityFrameworkCore.Migrations;

namespace KosarRB_TelegramBot.Migrations
{
    public partial class chatTypeAndSSN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Chat",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "ChatType",
                table: "Chat",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "fldSSN",
                table: "Chat",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatType",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "fldSSN",
                table: "Chat");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Chat",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
