using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UserProfileFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersProfiles",
                table: "UsersProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UsersProfiles_UserId",
                table: "UsersProfiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsersProfiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersProfiles",
                table: "UsersProfiles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersProfiles",
                table: "UsersProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UsersProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersProfiles",
                table: "UsersProfiles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersProfiles_UserId",
                table: "UsersProfiles",
                column: "UserId",
                unique: true);
        }
    }
}
