using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImageTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersProfiles_ProfileImages_ProfileImageId",
                table: "UsersProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UsersProfiles_ProfileImageId",
                table: "UsersProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_UsersProfiles_ProfileImageId",
                table: "UsersProfiles",
                column: "ProfileImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersProfiles_ProfileImages_ProfileImageId",
                table: "UsersProfiles",
                column: "ProfileImageId",
                principalTable: "ProfileImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersProfiles_ProfileImages_ProfileImageId",
                table: "UsersProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UsersProfiles_ProfileImageId",
                table: "UsersProfiles");

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
    }
}
