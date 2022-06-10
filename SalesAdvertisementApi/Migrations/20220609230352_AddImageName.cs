using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesAdvertisementApi.Migrations
{
    public partial class AddImageName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Advertisements",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Advertisements",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Advertisements");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Advertisements",
                newName: "Image");
        }
    }
}
