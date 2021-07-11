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
                    fldid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fldMobileNumberOrId = table.Column<string>(type: "varchar(50)", nullable: false),
                    fldMes = table.Column<string>(type: "varchar(1000)", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    fldOK = table.Column<int>(type: "int", nullable: true),
                    fldTime = table.Column<string>(type: "varchar(20)", nullable: false),
                    flddate = table.Column<string>(type: "varchar(50)", nullable: false),
                    fldSendTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fldResponse = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBotMessage", x => x.fldid);
                });

            migrationBuilder.CreateTable(
                name: "tblContactInfo",
                columns: table => new
                {
                    fldChatId = table.Column<long>(type: "bigint", nullable: false),
                    fldChatType = table.Column<int>(type: "int", nullable: false),
                    fldChatState = table.Column<int>(type: "int", nullable: false),
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
