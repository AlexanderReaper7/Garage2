using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage2.Migrations
{
    /// <inheritdoc />
    public partial class addParkingLots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParkingslotNr",
                table: "ParkedVehicle",
                newName: "ParkingSubSpace");

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpace",
                table: "ParkedVehicle",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingSpace",
                table: "ParkedVehicle");

            migrationBuilder.RenameColumn(
                name: "ParkingSubSpace",
                table: "ParkedVehicle",
                newName: "ParkingslotNr");
        }
    }
}
