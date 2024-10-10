using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelProgram.Data.Migrations
{
    /// <inheritdoc />
    public partial class PriceToSeatClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Seats",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SeatPrice",
                table: "Flights",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatPrice",
                table: "Flights");
        }
    }
}
