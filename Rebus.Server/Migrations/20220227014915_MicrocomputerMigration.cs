using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rebus.Server.Migrations
{
    public partial class MicrocomputerMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Concept_ConceptId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_ConceptId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Player");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Concept",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Concept",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Concept_PlayerId",
                table: "Concept",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Concept_RegionId",
                table: "Concept",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Concept_Concept_RegionId",
                table: "Concept",
                column: "RegionId",
                principalTable: "Concept",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Concept_Player_PlayerId",
                table: "Concept",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concept_Concept_RegionId",
                table: "Concept");

            migrationBuilder.DropForeignKey(
                name: "FK_Concept_Player_PlayerId",
                table: "Concept");

            migrationBuilder.DropIndex(
                name: "IX_Concept_PlayerId",
                table: "Concept");

            migrationBuilder.DropIndex(
                name: "IX_Concept_RegionId",
                table: "Concept");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Concept");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Concept");

            migrationBuilder.AddColumn<int>(
                name: "ConceptId",
                table: "Player",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Player_ConceptId",
                table: "Player",
                column: "ConceptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Concept_ConceptId",
                table: "Player",
                column: "ConceptId",
                principalTable: "Concept",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
