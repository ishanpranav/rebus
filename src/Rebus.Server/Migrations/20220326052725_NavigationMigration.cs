using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rebus.Server.Migrations
{
    internal partial class NavigationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Navigation_PlayerId_Q_R",
                table: "Navigation");

            migrationBuilder.CreateIndex(
                name: "IX_Navigation_PlayerId",
                table: "Navigation",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Navigation_Q_R_PlayerId",
                table: "Navigation",
                columns: new[] { "Q", "R", "PlayerId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Navigation_PlayerId",
                table: "Navigation");

            migrationBuilder.DropIndex(
                name: "IX_Navigation_Q_R_PlayerId",
                table: "Navigation");

            migrationBuilder.CreateIndex(
                name: "IX_Navigation_PlayerId_Q_R",
                table: "Navigation",
                columns: new[] { "PlayerId", "Q", "R" },
                unique: true);
        }
    }
}
