using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class TasksKanbanModel4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Column",
                table: "KanbanTasks",
                newName: "ColumnId");

            migrationBuilder.CreateTable(
                name: "KanbanColumns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanbanColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KanbanColumns_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KanbanTasks_ColumnId",
                table: "KanbanTasks",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanColumns_UserId",
                table: "KanbanColumns",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanTasks_KanbanColumns_ColumnId",
                table: "KanbanTasks",
                column: "ColumnId",
                principalTable: "KanbanColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanTasks_KanbanColumns_ColumnId",
                table: "KanbanTasks");

            migrationBuilder.DropTable(
                name: "KanbanColumns");

            migrationBuilder.DropIndex(
                name: "IX_KanbanTasks_ColumnId",
                table: "KanbanTasks");

            migrationBuilder.RenameColumn(
                name: "ColumnId",
                table: "KanbanTasks",
                newName: "Column");
        }
    }
}
