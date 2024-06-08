using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPlacement.Backend.Migrations
{
    /// <inheritdoc />
    public partial class FixedDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllocationRequests_Departments_DepartmentId",
                table: "AllocationRequests");

            migrationBuilder.DropIndex(
                name: "IX_AllocationRequests_DepartmentId",
                table: "AllocationRequests");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AllocationRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "AllocationRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AllocationRequests_DepartmentId",
                table: "AllocationRequests",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllocationRequests_Departments_DepartmentId",
                table: "AllocationRequests",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
