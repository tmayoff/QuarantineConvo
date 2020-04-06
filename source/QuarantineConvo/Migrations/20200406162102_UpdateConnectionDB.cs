using Microsoft.EntityFrameworkCore.Migrations;

namespace QuarantineConvo.Migrations
{
    public partial class UpdateConnectionDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionID",
                table: "Connection",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "Connection",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "user1ID",
                table: "Connection",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "user2ID",
                table: "Connection",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connection_user1ID",
                table: "Connection",
                column: "user1ID");

            migrationBuilder.CreateIndex(
                name: "IX_Connection_user2ID",
                table: "Connection",
                column: "user2ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Connection_User_user1ID",
                table: "Connection",
                column: "user1ID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Connection_User_user2ID",
                table: "Connection",
                column: "user2ID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connection_User_user1ID",
                table: "Connection");

            migrationBuilder.DropForeignKey(
                name: "FK_Connection_User_user2ID",
                table: "Connection");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Connection_user1ID",
                table: "Connection");

            migrationBuilder.DropIndex(
                name: "IX_Connection_user2ID",
                table: "Connection");

            migrationBuilder.DropColumn(
                name: "ConnectionID",
                table: "Connection");

            migrationBuilder.DropColumn(
                name: "active",
                table: "Connection");

            migrationBuilder.DropColumn(
                name: "user1ID",
                table: "Connection");

            migrationBuilder.DropColumn(
                name: "user2ID",
                table: "Connection");
        }
    }
}
