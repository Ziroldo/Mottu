using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.Backend.Migrations
{
    /// <inheritdoc />
    public partial class Update_Rental_FullModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Rentals",
                newName: "TotalCost");

            migrationBuilder.RenameColumn(
                name: "DailyRate",
                table: "Rentals",
                newName: "DailyPrice");

            migrationBuilder.RenameColumn(
                name: "ActualEndDate",
                table: "Rentals",
                newName: "EndDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalCost",
                table: "Rentals",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Rentals",
                newName: "ActualEndDate");

            migrationBuilder.RenameColumn(
                name: "DailyPrice",
                table: "Rentals",
                newName: "DailyRate");
        }
    }
}
