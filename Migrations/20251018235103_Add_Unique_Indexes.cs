using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.Backend.Migrations
{
    /// <inheritdoc />
    public partial class Add_Unique_Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Deliverers_DelivererId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Motos_MotoId",
                table: "Rentals");

            migrationBuilder.CreateIndex(
                name: "IX_Motos_Placa",
                table: "Motos",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliverers_CNHNumber",
                table: "Deliverers",
                column: "CNHNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliverers_CNPJ",
                table: "Deliverers",
                column: "CNPJ",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Deliverers_DelivererId",
                table: "Rentals",
                column: "DelivererId",
                principalTable: "Deliverers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Motos_MotoId",
                table: "Rentals",
                column: "MotoId",
                principalTable: "Motos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Deliverers_DelivererId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Motos_MotoId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Motos_Placa",
                table: "Motos");

            migrationBuilder.DropIndex(
                name: "IX_Deliverers_CNHNumber",
                table: "Deliverers");

            migrationBuilder.DropIndex(
                name: "IX_Deliverers_CNPJ",
                table: "Deliverers");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Deliverers_DelivererId",
                table: "Rentals",
                column: "DelivererId",
                principalTable: "Deliverers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Motos_MotoId",
                table: "Rentals",
                column: "MotoId",
                principalTable: "Motos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
