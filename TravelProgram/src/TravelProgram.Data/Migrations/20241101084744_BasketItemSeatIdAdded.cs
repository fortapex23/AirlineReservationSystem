using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelProgram.Data.Migrations
{
    /// <inheritdoc />
    public partial class BasketItemSeatIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Flights_FlightId",
                table: "BasketItems");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "BasketItems",
                newName: "SeatId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItems_FlightId",
                table: "BasketItems",
                newName: "IX_BasketItems_SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Seats_SeatId",
                table: "BasketItems",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Seats_SeatId",
                table: "BasketItems");

            migrationBuilder.RenameColumn(
                name: "SeatId",
                table: "BasketItems",
                newName: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItems_SeatId",
                table: "BasketItems",
                newName: "IX_BasketItems_FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Flights_FlightId",
                table: "BasketItems",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id");
        }
    }
}
