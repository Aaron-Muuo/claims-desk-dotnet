using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClaimsDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAiCreditsTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TopUpAiCreditsBalance",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FreeAiCreditsBalance",
                table: "LicensePools",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TopUpAiCreditsBalance",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "FreeAiCreditsBalance",
                table: "LicensePools");
        }
    }
}
