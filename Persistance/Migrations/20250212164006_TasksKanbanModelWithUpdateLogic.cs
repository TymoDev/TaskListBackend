using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class TasksKanbanModelWithUpdateLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KanbanTasks_ColumnId",
                table: "KanbanTasks");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanTasks_ColumnId_Order",
                table: "KanbanTasks",
                columns: new[] { "ColumnId", "Order" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KanbanTasks_ColumnId_Order",
                table: "KanbanTasks");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanTasks_ColumnId",
                table: "KanbanTasks",
                column: "ColumnId");
        }
    }
}
