using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoMeal.API.Migrations
{
    /// <inheritdoc />
    public partial class CorrectingTypos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bussiness_BusinessType_BusinessTypeId",
                table: "Bussiness");

            migrationBuilder.DropForeignKey(
                name: "FK_Package_Bussiness_BusinessId",
                table: "Package");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bussiness",
                table: "Bussiness");

            migrationBuilder.RenameTable(
                name: "Bussiness",
                newName: "Business");

            migrationBuilder.RenameColumn(
                name: "No_Package",
                table: "Package",
                newName: "NOPackage");

            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "Business",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_Bussiness_BusinessTypeId",
                table: "Business",
                newName: "IX_Business_BusinessTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Business",
                table: "Business",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Business_BusinessType_BusinessTypeId",
                table: "Business",
                column: "BusinessTypeId",
                principalTable: "BusinessType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Package_Business_BusinessId",
                table: "Package",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Business_BusinessType_BusinessTypeId",
                table: "Business");

            migrationBuilder.DropForeignKey(
                name: "FK_Package_Business_BusinessId",
                table: "Package");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Business",
                table: "Business");

            migrationBuilder.RenameTable(
                name: "Business",
                newName: "Bussiness");

            migrationBuilder.RenameColumn(
                name: "NOPackage",
                table: "Package",
                newName: "No_Package");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Bussiness",
                newName: "Adress");

            migrationBuilder.RenameIndex(
                name: "IX_Business_BusinessTypeId",
                table: "Bussiness",
                newName: "IX_Bussiness_BusinessTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bussiness",
                table: "Bussiness",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bussiness_BusinessType_BusinessTypeId",
                table: "Bussiness",
                column: "BusinessTypeId",
                principalTable: "BusinessType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Package_Bussiness_BusinessId",
                table: "Package",
                column: "BusinessId",
                principalTable: "Bussiness",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
