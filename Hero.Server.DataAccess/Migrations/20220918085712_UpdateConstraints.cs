﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hero.Server.DataAccess.Migrations
{
    public partial class UpdateConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "Hero",
                table: "Skills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "Hero",
                table: "Abilities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Skills_UserId",
                schema: "Hero",
                table: "Skills",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_UserId",
                schema: "Hero",
                table: "Abilities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abilities_Users_UserId",
                schema: "Hero",
                table: "Abilities",
                column: "UserId",
                principalSchema: "Hero",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Users_UserId",
                schema: "Hero",
                table: "Skills",
                column: "UserId",
                principalSchema: "Hero",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abilities_Users_UserId",
                schema: "Hero",
                table: "Abilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Users_UserId",
                schema: "Hero",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_UserId",
                schema: "Hero",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Abilities_UserId",
                schema: "Hero",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Hero",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Hero",
                table: "Abilities");
        }
    }
}
