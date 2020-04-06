using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuarantineConvo.Migrations
{
    public partial class Messages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    ConnectionID = table.Column<int>(nullable: true),
                    SentByUsername = table.Column<string>(nullable: true),
                    Msg = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Message_Connection_ConnectionID",
                        column: x => x.ConnectionID,
                        principalTable: "Connection",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_User_SentByUsername",
                        column: x => x.SentByUsername,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_ConnectionID",
                table: "Message",
                column: "ConnectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SentByUsername",
                table: "Message",
                column: "SentByUsername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");
        }
    }
}
