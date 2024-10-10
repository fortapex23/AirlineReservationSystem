using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelProgram.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigNullableFlightIdAndManyThings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Airports");

            migrationBuilder.RenameColumn(
                name: "SeatNumber",
                table: "Bookings",
                newName: "SeatId");

            migrationBuilder.AddColumn<int>(
                name: "City",
                table: "Airports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Country",
                table: "Airlines",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SeatId",
                table: "Bookings",
                column: "SeatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Seats_SeatId",
                table: "Bookings",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Seats_SeatId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_SeatId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Airports");

            migrationBuilder.RenameColumn(
                name: "SeatId",
                table: "Bookings",
                newName: "SeatNumber");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Airports",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Airlines",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
