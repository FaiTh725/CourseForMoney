using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPlacement.Backend.Migrations
{
    /// <inheritdoc />
    public partial class fixedRelationOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllocationRequests_Users_UserId",
                table: "AllocationRequests");

            migrationBuilder.DropIndex(
                name: "IX_AllocationRequests_UserId",
                table: "AllocationRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AllocationRequests");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_UserId",
                table: "Organizations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Users_UserId",
                table: "Organizations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Users_UserId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_UserId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Organizations");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AllocationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AllocationRequests_UserId",
                table: "AllocationRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllocationRequests_Users_UserId",
                table: "AllocationRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
