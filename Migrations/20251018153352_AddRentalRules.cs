using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlanDays",
                table: "Rentals",
                newName: "Plan");

            migrationBuilder.RenameColumn(
                name: "ExpectedEndDate",
                table: "Rentals",
                newName: "EndDate");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Rentals",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Plan",
                table: "Rentals",
                newName: "PlanDays");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Rentals",
                newName: "ExpectedEndDate");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Rentals",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
