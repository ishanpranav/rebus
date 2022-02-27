using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rebus.Server.Migrations
{
    internal sealed partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArgumentSignature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Argument = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArgumentSignature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommandPrototype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Guid = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandPrototype", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Concept",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Discriminator = table.Column<byte>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concept", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<int>(type: "INTEGER", nullable: false),
                    Arguments = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    ConceptId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Concept_ConceptId",
                        column: x => x.ConceptId,
                        principalTable: "Concept",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandSignature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommandId = table.Column<int>(type: "INTEGER", nullable: false),
                    VerbId = table.Column<int>(type: "INTEGER", nullable: false),
                    AdverbId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandSignature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandSignature_CommandPrototype_CommandId",
                        column: x => x.CommandId,
                        principalTable: "CommandPrototype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommandSignature_Token_AdverbId",
                        column: x => x.AdverbId,
                        principalTable: "Token",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommandSignature_Token_VerbId",
                        column: x => x.VerbId,
                        principalTable: "Token",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConceptSignature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArticleId = table.Column<int>(type: "INTEGER", nullable: true),
                    SubstantiveId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptSignature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConceptSignature_Token_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Token",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConceptSignature_Token_SubstantiveId",
                        column: x => x.SubstantiveId,
                        principalTable: "Token",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArgumentSignatureCommandSignature",
                columns: table => new
                {
                    ArgumentsId = table.Column<int>(type: "INTEGER", nullable: false),
                    CommandsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArgumentSignatureCommandSignature", x => new { x.ArgumentsId, x.CommandsId });
                    table.ForeignKey(
                        name: "FK_ArgumentSignatureCommandSignature_ArgumentSignature_ArgumentsId",
                        column: x => x.ArgumentsId,
                        principalTable: "ArgumentSignature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArgumentSignatureCommandSignature_CommandSignature_CommandsId",
                        column: x => x.CommandsId,
                        principalTable: "CommandSignature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConceptConceptSignature",
                columns: table => new
                {
                    ConceptsId = table.Column<int>(type: "INTEGER", nullable: false),
                    SignaturesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptConceptSignature", x => new { x.ConceptsId, x.SignaturesId });
                    table.ForeignKey(
                        name: "FK_ConceptConceptSignature_Concept_ConceptsId",
                        column: x => x.ConceptsId,
                        principalTable: "Concept",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConceptConceptSignature_ConceptSignature_SignaturesId",
                        column: x => x.SignaturesId,
                        principalTable: "ConceptSignature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConceptSignatureToken",
                columns: table => new
                {
                    AdjectivesId = table.Column<int>(type: "INTEGER", nullable: false),
                    SignaturesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptSignatureToken", x => new { x.AdjectivesId, x.SignaturesId });
                    table.ForeignKey(
                        name: "FK_ConceptSignatureToken_ConceptSignature_SignaturesId",
                        column: x => x.SignaturesId,
                        principalTable: "ConceptSignature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConceptSignatureToken_Token_AdjectivesId",
                        column: x => x.AdjectivesId,
                        principalTable: "Token",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArgumentSignature_Argument_Type",
                table: "ArgumentSignature",
                columns: new[] { "Argument", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArgumentSignatureCommandSignature_CommandsId",
                table: "ArgumentSignatureCommandSignature",
                column: "CommandsId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandPrototype_Guid",
                table: "CommandPrototype",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommandSignature_AdverbId",
                table: "CommandSignature",
                column: "AdverbId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandSignature_CommandId",
                table: "CommandSignature",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandSignature_VerbId",
                table: "CommandSignature",
                column: "VerbId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptConceptSignature_SignaturesId",
                table: "ConceptConceptSignature",
                column: "SignaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptSignature_ArticleId",
                table: "ConceptSignature",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptSignature_SubstantiveId",
                table: "ConceptSignature",
                column: "SubstantiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptSignatureToken_SignaturesId",
                table: "ConceptSignatureToken",
                column: "SignaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_ConceptId",
                table: "Player",
                column: "ConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_UserId",
                table: "Player",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Key_Arguments",
                table: "Resource",
                columns: new[] { "Key", "Arguments" });

            migrationBuilder.CreateIndex(
                name: "IX_Token_Value",
                table: "Token",
                column: "Value",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArgumentSignatureCommandSignature");

            migrationBuilder.DropTable(
                name: "ConceptConceptSignature");

            migrationBuilder.DropTable(
                name: "ConceptSignatureToken");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "ArgumentSignature");

            migrationBuilder.DropTable(
                name: "CommandSignature");

            migrationBuilder.DropTable(
                name: "ConceptSignature");

            migrationBuilder.DropTable(
                name: "Concept");

            migrationBuilder.DropTable(
                name: "CommandPrototype");

            migrationBuilder.DropTable(
                name: "Token");
        }
    }
}
