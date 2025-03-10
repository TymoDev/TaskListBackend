using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "UsersProfiles",
                newName: "ProfileImageId");

            migrationBuilder.CreateTable(
                name: "ProfileImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePublicId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileImages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersProfiles_ProfileImageId",
                table: "UsersProfiles",
                column: "ProfileImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersProfiles_ProfileImages_ProfileImageId",
                table: "UsersProfiles",
                column: "ProfileImageId",
                principalTable: "ProfileImages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersProfiles_ProfileImages_ProfileImageId",
                table: "UsersProfiles");

            migrationBuilder.DropTable(
                name: "ProfileImages");

            migrationBuilder.DropIndex(
                name: "IX_UsersProfiles_ProfileImageId",
                table: "UsersProfiles");

            migrationBuilder.RenameColumn(
                name: "ProfileImageId",
                table: "UsersProfiles",
                newName: "ProfileImageUrl");
        }
    }
}
