using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPlacement.Backend.Migrations
{
    /// <inheritdoc />
    public partial class Organizationcanaddmorethanonerequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_AllocationRequests_AllocationRequestId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_AllocationRequestId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "AllocationRequestId",
                table: "Organizations");

            migrationBuilder.AddColumn<int>(
                name: "IdOrganization",
                table: "AllocationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "AllocationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AllocationRequests_OrganizationId",
                table: "AllocationRequests",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllocationRequests_Organizations_OrganizationId",
                table: "AllocationRequests",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllocationRequests_Organizations_OrganizationId",
                table: "AllocationRequests");

            migrationBuilder.DropIndex(
                name: "IX_AllocationRequests_OrganizationId",
                table: "AllocationRequests");

            migrationBuilder.DropColumn(
                name: "IdOrganization",
                table: "AllocationRequests");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "AllocationRequests");

            migrationBuilder.AddColumn<int>(
                name: "AllocationRequestId",
                table: "Organizations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_AllocationRequestId",
                table: "Organizations",
                column: "AllocationRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_AllocationRequests_AllocationRequestId",
                table: "Organizations",
                column: "AllocationRequestId",
                principalTable: "AllocationRequests",
                principalColumn: "Id");
        }
    }
}
