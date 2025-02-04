using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class TasksKanbanModel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskPositions_Users_UserId",
                table: "TaskPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskPositions",
                table: "TaskPositions");

            migrationBuilder.RenameTable(
                name: "TaskPositions",
                newName: "KanbanTasks");

            migrationBuilder.RenameIndex(
                name: "IX_TaskPositions_UserId",
                table: "KanbanTasks",
                newName: "IX_KanbanTasks_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KanbanTasks",
                table: "KanbanTasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanTasks_Users_UserId",
                table: "KanbanTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanTasks_Users_UserId",
                table: "KanbanTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KanbanTasks",
                table: "KanbanTasks");

            migrationBuilder.RenameTable(
                name: "KanbanTasks",
                newName: "TaskPositions");

            migrationBuilder.RenameIndex(
                name: "IX_KanbanTasks_UserId",
                table: "TaskPositions",
                newName: "IX_TaskPositions_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskPositions",
                table: "TaskPositions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskPositions_Users_UserId",
                table: "TaskPositions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
