using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelProgram.Data.Migrations
{
    /// <inheritdoc />
    public partial class CountRemovedFromBasketItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "BasketItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "BasketItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
