using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoMeal.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangePackageDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NoPackage",
                table: "Packages",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Packages",
                newName: "NoPackage");
        }
    }
}
