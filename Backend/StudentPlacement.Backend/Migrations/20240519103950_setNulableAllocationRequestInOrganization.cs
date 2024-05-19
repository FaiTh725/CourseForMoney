using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPlacement.Backend.Migrations
{
    /// <inheritdoc />
    public partial class setNulableAllocationRequestInOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_AllocationRequests_AllocationRequestId",
                table: "Organizations");

            migrationBuilder.AlterColumn<int>(
                name: "AllocationRequestId",
                table: "Organizations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_AllocationRequests_AllocationRequestId",
                table: "Organizations",
                column: "AllocationRequestId",
                principalTable: "AllocationRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_AllocationRequests_AllocationRequestId",
                table: "Organizations");

            migrationBuilder.AlterColumn<int>(
                name: "AllocationRequestId",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_AllocationRequests_AllocationRequestId",
                table: "Organizations",
                column: "AllocationRequestId",
                principalTable: "AllocationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
