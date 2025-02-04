using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class TasksKanbanModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskPositions_Tasks_TaskId",
                table: "TaskPositions");

            migrationBuilder.DropIndex(
                name: "IX_TaskPositions_TaskId",
                table: "TaskPositions");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "TaskPositions",
                newName: "TaskName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskName",
                table: "TaskPositions",
                newName: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskPositions_TaskId",
                table: "TaskPositions",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskPositions_Tasks_TaskId",
                table: "TaskPositions",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
