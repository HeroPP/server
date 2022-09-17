using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hero.Server.DataAccess.Migrations
{
    public partial class RemoveTheAbiliesFromTheCharacter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abilities_Characters_CharacterId",
                schema: "Hero",
                table: "Abilities");

            migrationBuilder.DropIndex(
                name: "IX_Abilities_CharacterId",
                schema: "Hero",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                schema: "Hero",
                table: "Abilities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CharacterId",
                schema: "Hero",
                table: "Abilities",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_CharacterId",
                schema: "Hero",
                table: "Abilities",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abilities_Characters_CharacterId",
                schema: "Hero",
                table: "Abilities",
                column: "CharacterId",
                principalSchema: "Hero",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
