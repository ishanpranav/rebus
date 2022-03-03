// Ishan Pranav's REBUS: InitialMigration.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rebus.Server.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Key = table.Column<int>(type: "INTEGER", nullable: false),
                    Arguments = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => new { x.Key, x.Arguments, x.Value });
                });

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Value = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "Spacecraft",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Q = table.Column<int>(type: "INTEGER", nullable: false),
                    R = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spacecraft", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spacecraft_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandSignature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Guid = table.Column<Guid>(type: "TEXT", nullable: false),
                    VerbValue = table.Column<string>(type: "TEXT", nullable: false),
                    AdverbValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandSignature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandSignature_Token_AdverbValue",
                        column: x => x.AdverbValue,
                        principalTable: "Token",
                        principalColumn: "Value");
                    table.ForeignKey(
                        name: "FK_CommandSignature_Token_VerbValue",
                        column: x => x.VerbValue,
                        principalTable: "Token",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConceptSignature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    ArticleValue = table.Column<string>(type: "TEXT", nullable: true),
                    SubstantiveValue = table.Column<string>(type: "TEXT", nullable: false),
                    SpacecraftId = table.Column<int>(type: "INTEGER", nullable: true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptSignature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConceptSignature_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConceptSignature_Spacecraft_SpacecraftId",
                        column: x => x.SpacecraftId,
                        principalTable: "Spacecraft",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConceptSignature_Token_ArticleValue",
                        column: x => x.ArticleValue,
                        principalTable: "Token",
                        principalColumn: "Value");
                    table.ForeignKey(
                        name: "FK_ConceptSignature_Token_SubstantiveValue",
                        column: x => x.SubstantiveValue,
                        principalTable: "Token",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConceptSignatureToken",
                columns: table => new
                {
                    AdjectivesValue = table.Column<string>(type: "TEXT", nullable: false),
                    SignaturesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptSignatureToken", x => new { x.AdjectivesValue, x.SignaturesId });
                    table.ForeignKey(
                        name: "FK_ConceptSignatureToken_ConceptSignature_SignaturesId",
                        column: x => x.SignaturesId,
                        principalTable: "ConceptSignature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConceptSignatureToken_Token_AdjectivesValue",
                        column: x => x.AdjectivesValue,
                        principalTable: "Token",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommandSignature_AdverbValue",
                table: "CommandSignature",
                column: "AdverbValue");

            migrationBuilder.CreateIndex(
                name: "IX_CommandSignature_VerbValue",
                table: "CommandSignature",
                column: "VerbValue");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptSignature_ArticleValue",
                table: "ConceptSignature",
                column: "ArticleValue");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptSignature_PlayerId",
                table: "ConceptSignature",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptSignature_Priority",
                table: "ConceptSignature",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptSignature_SpacecraftId",
                table: "ConceptSignature",
                column: "SpacecraftId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptSignature_SubstantiveValue",
                table: "ConceptSignature",
                column: "SubstantiveValue");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptSignatureToken_SignaturesId",
                table: "ConceptSignatureToken",
                column: "SignaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_UserId",
                table: "Player",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spacecraft_PlayerId",
                table: "Spacecraft",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandSignature");

            migrationBuilder.DropTable(
                name: "ConceptSignatureToken");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "ConceptSignature");

            migrationBuilder.DropTable(
                name: "Spacecraft");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "Player");
        }
    }
}
