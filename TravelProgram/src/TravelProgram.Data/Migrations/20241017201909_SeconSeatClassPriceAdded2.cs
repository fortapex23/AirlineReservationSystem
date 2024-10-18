using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelProgram.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeconSeatClassPriceAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SeatPrice",
                table: "Flights",
                newName: "EconomySeatPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "BusinessSeatPrice",
                table: "Flights",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessSeatPrice",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "EconomySeatPrice",
                table: "Flights",
                newName: "SeatPrice");
        }
    }
}
