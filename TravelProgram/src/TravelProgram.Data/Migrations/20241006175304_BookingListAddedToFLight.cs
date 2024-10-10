using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelProgram.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookingListAddedToFLight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlightId1",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FlightId1",
                table: "Bookings",
                column: "FlightId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Flights_FlightId1",
                table: "Bookings",
                column: "FlightId1",
                principalTable: "Flights",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Flights_FlightId1",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FlightId1",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FlightId1",
                table: "Bookings");
        }
    }
}
