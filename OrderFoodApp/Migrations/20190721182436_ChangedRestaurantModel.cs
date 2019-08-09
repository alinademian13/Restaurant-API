using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderFoodApp.Migrations
{
    public partial class ChangedRestaurantModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Altitude",
                table: "Restaurants",
                newName: "Latitude");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Restaurants",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Restaurants",
                newName: "Altitude");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Restaurants",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
