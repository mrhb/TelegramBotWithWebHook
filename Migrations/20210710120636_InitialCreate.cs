using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KosarRB_Bot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblBotMessage",
                columns: table => new
                {
                    fldid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    fldMobileNumberOrId = table.Column<string>(type: "varchar(50)", nullable: false),
                    fldMes = table.Column<string>(type: "varchar(1000)", nullable: false),
                    ImageData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    fldOK = table.Column<int>(type: "INTEGER", nullable: true),
                    fldTime = table.Column<string>(type: "varchar(20)", nullable: false),
                    flddate = table.Column<string>(type: "varchar(50)", nullable: false),
                    fldSendTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    fldResponse = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBotMessage", x => x.fldid);
                });

            migrationBuilder.CreateTable(
                name: "tblContactInfo",
                columns: table => new
                {
                    fldChatId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    fldChatType = table.Column<int>(type: "INTEGER", nullable: false),
                    fldChatState = table.Column<int>(type: "INTEGER", nullable: false),
                    fldMobileNumberOrId = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblContactInfo", x => x.fldChatId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblBotMessage");

            migrationBuilder.DropTable(
                name: "tblContactInfo");
        }
    }
}
