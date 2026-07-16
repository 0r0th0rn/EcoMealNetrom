using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoMeal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPackageDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableQty",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "OldPrice",
                table: "Packages",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "weightInKg",
                table: "Packages",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableQty",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "OldPrice",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "weightInKg",
                table: "Packages");
        }
    }
}
