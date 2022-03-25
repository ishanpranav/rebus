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
                    UserId = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "", collation: "NOCASE"),
                    Credential = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "", collation: "NOCASE"),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    Wealth = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "", collation: "NOCASE"),
                    Arguments = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
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
                name: "Navigation",
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
                    table.PrimaryKey("PK_Navigation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Navigation_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Command",
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
                    table.PrimaryKey("PK_Command", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Command_Token_AdverbValue",
                        column: x => x.AdverbValue,
                        principalTable: "Token",
                        principalColumn: "Value");
                    table.ForeignKey(
                        name: "FK_Command_Token_VerbValue",
                        column: x => x.VerbValue,
                        principalTable: "Token",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Concept",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArticleValue = table.Column<string>(type: "TEXT", nullable: true),
                    SubstantiveValue = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concept", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Concept_Token_ArticleValue",
                        column: x => x.ArticleValue,
                        principalTable: "Token",
                        principalColumn: "Value");
                    table.ForeignKey(
                        name: "FK_Concept_Token_SubstantiveValue",
                        column: x => x.SubstantiveValue,
                        principalTable: "Token",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Adjective",
                columns: table => new
                {
                    ConceptId = table.Column<int>(type: "INTEGER", nullable: false),
                    TokenValue = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adjective", x => new { x.TokenValue, x.ConceptId });
                    table.ForeignKey(
                        name: "FK_Adjective_Concept_ConceptId",
                        column: x => x.ConceptId,
                        principalTable: "Concept",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Adjective_Token_TokenValue",
                        column: x => x.TokenValue,
                        principalTable: "Token",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Q = table.Column<int>(type: "INTEGER", nullable: false),
                    R = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planet_Concept_Id",
                        column: x => x.Id,
                        principalTable: "Concept",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Spacecraft",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Q = table.Column<int>(type: "INTEGER", nullable: false),
                    R = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spacecraft", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spacecraft_Concept_Id",
                        column: x => x.Id,
                        principalTable: "Concept",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Spacecraft_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Star",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Q = table.Column<int>(type: "INTEGER", nullable: false),
                    R = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Star", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Star_Concept_Id",
                        column: x => x.Id,
                        principalTable: "Concept",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adjective_ConceptId",
                table: "Adjective",
                column: "ConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_Command_AdverbValue",
                table: "Command",
                column: "AdverbValue");

            migrationBuilder.CreateIndex(
                name: "IX_Command_VerbValue",
                table: "Command",
                column: "VerbValue");

            migrationBuilder.CreateIndex(
                name: "IX_Concept_ArticleValue",
                table: "Concept",
                column: "ArticleValue");

            migrationBuilder.CreateIndex(
                name: "IX_Concept_SubstantiveValue",
                table: "Concept",
                column: "SubstantiveValue");

            migrationBuilder.CreateIndex(
                name: "IX_Navigation_PlayerId_Q_R",
                table: "Navigation",
                columns: new[] { "PlayerId", "Q", "R" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Planet_Q_R",
                table: "Planet",
                columns: new[] { "Q", "R" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_UserId",
                table: "Player",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Key_Arguments_Value",
                table: "Resource",
                columns: new[] { "Key", "Arguments", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spacecraft_PlayerId",
                table: "Spacecraft",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Spacecraft_Q_R",
                table: "Spacecraft",
                columns: new[] { "Q", "R" });

            migrationBuilder.CreateIndex(
                name: "IX_Star_Q_R",
                table: "Star",
                columns: new[] { "Q", "R" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adjective");

            migrationBuilder.DropTable(
                name: "Command");

            migrationBuilder.DropTable(
                name: "Navigation");

            migrationBuilder.DropTable(
                name: "Planet");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "Spacecraft");

            migrationBuilder.DropTable(
                name: "Star");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Concept");

            migrationBuilder.DropTable(
                name: "Token");
        }
    }
}
