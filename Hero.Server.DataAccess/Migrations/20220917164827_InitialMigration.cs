using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hero.Server.DataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Hero");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Hero",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                schema: "Hero",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    HealthPoints = table.Column<int>(type: "integer", nullable: false),
                    LightPoints = table.Column<int>(type: "integer", nullable: false),
                    MovementSpeed = table.Column<double>(type: "double precision", nullable: false),
                    Resistance = table.Column<double>(type: "double precision", nullable: false),
                    OpticalRange = table.Column<double>(type: "double precision", nullable: false),
                    Parry = table.Column<double>(type: "double precision", nullable: false),
                    Dodge = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Hero",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Abilities",
                schema: "Hero",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsPassive = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Abilities_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Hero",
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "NodeTrees",
                schema: "Hero",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: false),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsActiveTree = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeTrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeTrees_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Hero",
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                schema: "Hero",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AbilityName = table.Column<string>(type: "character varying(100)", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    HealthPointsBoost = table.Column<int>(type: "integer", nullable: false),
                    LightPointsBoost = table.Column<int>(type: "integer", nullable: false),
                    MovementSpeedBoost = table.Column<double>(type: "double precision", nullable: false),
                    ResistanceBoost = table.Column<double>(type: "double precision", nullable: false),
                    OpticalRangeBoost = table.Column<double>(type: "double precision", nullable: false),
                    MeleeDamageBoost = table.Column<double>(type: "double precision", nullable: false),
                    RangeDamageBoost = table.Column<double>(type: "double precision", nullable: false),
                    LightDamageBoost = table.Column<double>(type: "double precision", nullable: false),
                    DamageBoost = table.Column<double>(type: "double precision", nullable: false),
                    ParryBoost = table.Column<double>(type: "double precision", nullable: false),
                    DodgeBoost = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Abilities_AbilityName",
                        column: x => x.AbilityName,
                        principalSchema: "Hero",
                        principalTable: "Abilities",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                schema: "Hero",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    NodeTreeId = table.Column<Guid>(type: "uuid", nullable: true),
                    Importance = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    XPos = table.Column<double>(type: "double precision", nullable: false),
                    YPos = table.Column<double>(type: "double precision", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    IsUnlocked = table.Column<bool>(type: "boolean", nullable: false),
                    IsEasyReachable = table.Column<bool>(type: "boolean", nullable: false),
                    Precessors = table.Column<List<Guid>>(type: "uuid[]", nullable: false),
                    Successors = table.Column<List<Guid>>(type: "uuid[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nodes_NodeTrees_NodeTreeId",
                        column: x => x.NodeTreeId,
                        principalSchema: "Hero",
                        principalTable: "NodeTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nodes_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "Hero",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_CharacterId",
                schema: "Hero",
                table: "Abilities",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserId",
                schema: "Hero",
                table: "Characters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_NodeTreeId",
                schema: "Hero",
                table: "Nodes",
                column: "NodeTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_SkillId",
                schema: "Hero",
                table: "Nodes",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeTrees_CharacterId",
                schema: "Hero",
                table: "NodeTrees",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_AbilityName",
                schema: "Hero",
                table: "Skills",
                column: "AbilityName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nodes",
                schema: "Hero");

            migrationBuilder.DropTable(
                name: "NodeTrees",
                schema: "Hero");

            migrationBuilder.DropTable(
                name: "Skills",
                schema: "Hero");

            migrationBuilder.DropTable(
                name: "Abilities",
                schema: "Hero");

            migrationBuilder.DropTable(
                name: "Characters",
                schema: "Hero");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Hero");
        }
    }
}
