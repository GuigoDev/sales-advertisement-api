using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesAdvertisement.Migrations
{
    public partial class DeployApi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Images",
                table: "Advertisements",
                newName: "Image");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Advertisements",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Advertisements",
                newName: "Images");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Advertisements",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
