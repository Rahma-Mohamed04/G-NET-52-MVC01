using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVC1.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "Description", "DurationDays", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Access to gym equipment during working hours.", 30, true, "Basic Plan", 500m },
                    { 2, "Full gym access with basic fitness guidance.", 90, true, "Standard Plan", 1200m },
                    { 3, "Full gym access with personal training sessions.", 180, false, "Premium Plan", 2200m },
                    { 4, "Unlimited gym access for one year with premium benefits.", 365, true, "Annual Plan", 4000m },
                    { 5, "Affordable gym membership for students.", 30, false, "Student Plan", 350m },
                    { 6, "Gym access on weekends only.", 30, true, "Weekend Plan", 300m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
