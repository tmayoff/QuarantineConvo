using Microsoft.EntityFrameworkCore.Migrations;

namespace QuarantineConvo.Migrations
{
    public partial class Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Username = table.Column<string>(maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    isAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Connection",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectionID = table.Column<string>(nullable: true),
                    user1Username = table.Column<string>(nullable: true),
                    user2Username = table.Column<string>(nullable: true),
                    active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connection", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Connection_User_user1Username",
                        column: x => x.user1Username,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Connection_User_user2Username",
                        column: x => x.user2Username,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connection_user1Username",
                table: "Connection",
                column: "user1Username");

            migrationBuilder.CreateIndex(
                name: "IX_Connection_user2Username",
                table: "Connection",
                column: "user2Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connection");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
