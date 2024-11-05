using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelProgram.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderItemsToSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItems_SeatId",
                table: "OrderItems");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_SeatId",
                table: "OrderItems",
                column: "SeatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItems_SeatId",
                table: "OrderItems");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_SeatId",
                table: "OrderItems",
                column: "SeatId",
                unique: true);
        }
    }
}
