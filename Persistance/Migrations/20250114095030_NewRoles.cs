using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class NewRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissionEntity",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "PermissionEntity",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "PermissionEntity",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Write");

            migrationBuilder.UpdateData(
                table: "PermissionEntity",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "GetUsers");

            migrationBuilder.InsertData(
                table: "RolePermissionEntity",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { 2, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissionEntity",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "PermissionEntity",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Create");

            migrationBuilder.UpdateData(
                table: "PermissionEntity",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Update");

            migrationBuilder.InsertData(
                table: "PermissionEntity",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Delete" });

            migrationBuilder.InsertData(
                table: "RolePermissionEntity",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { 4, 1 });
        }
    }
}
