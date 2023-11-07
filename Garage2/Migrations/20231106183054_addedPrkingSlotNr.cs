using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage2.Migrations
{
    /// <inheritdoc />
    public partial class addedPrkingSlotNr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParkingslotNr",
                table: "ParkedVehicle",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingslotNr",
                table: "ParkedVehicle");
        }
    }
}
