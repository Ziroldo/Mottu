using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.Backend.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverLicenseImagePath",
                table: "Deliverers");

            migrationBuilder.DropColumn(
                name: "DriverLicenseNumber",
                table: "Deliverers");

            migrationBuilder.RenameColumn(
                name: "Cnpj",
                table: "Deliverers",
                newName: "CNPJ");

            migrationBuilder.RenameColumn(
                name: "DriverLicenseType",
                table: "Deliverers",
                newName: "CNHType");

            migrationBuilder.AlterColumn<string>(
                name: "CNPJ",
                table: "Deliverers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14);

            migrationBuilder.AddColumn<string>(
                name: "CNHImagePath",
                table: "Deliverers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CNHNumber",
                table: "Deliverers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CNHImagePath",
                table: "Deliverers");

            migrationBuilder.DropColumn(
                name: "CNHNumber",
                table: "Deliverers");

            migrationBuilder.RenameColumn(
                name: "CNPJ",
                table: "Deliverers",
                newName: "Cnpj");

            migrationBuilder.RenameColumn(
                name: "CNHType",
                table: "Deliverers",
                newName: "DriverLicenseType");

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Deliverers",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "DriverLicenseImagePath",
                table: "Deliverers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverLicenseNumber",
                table: "Deliverers",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");
        }
    }
}
