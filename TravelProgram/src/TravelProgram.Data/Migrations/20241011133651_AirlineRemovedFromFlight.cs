using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelProgram.Data.Migrations
{
    /// <inheritdoc />
    public partial class AirlineRemovedFromFlight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airlines_AirlineId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Planes_PlaneId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "AirlineId",
                table: "Flights");

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Planes_PlaneId",
                table: "Seats",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Planes_PlaneId",
                table: "Seats");

            migrationBuilder.AddColumn<int>(
                name: "AirlineId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights",
                column: "AirlineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airlines_AirlineId",
                table: "Flights",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Airlines_AirlineId",
                table: "Planes",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Planes_PlaneId",
                table: "Seats",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
