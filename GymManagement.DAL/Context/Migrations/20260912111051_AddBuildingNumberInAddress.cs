using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.DAL.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildingNumberInAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Address_BuildingNumber",
                table: "Trainers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Address_BuildingNumber",
                table: "Members",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_BuildingNumber",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Address_BuildingNumber",
                table: "Members");
        }
    }
}
