using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage2.Migrations
{
	/// <inheritdoc />
	public partial class CHangedForeignKeyInParkedVehicle : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ParkedVehicle_VehicleType_VehicleTypeName",
				table: "ParkedVehicle");

			migrationBuilder.DropColumn(
				name: "VehicleTypeId",
				table: "ParkedVehicle");

			migrationBuilder.AlterColumn<string>(
				name: "VehicleTypeName",
				table: "ParkedVehicle",
				type: "nvarchar(450)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(450)");

			migrationBuilder.AddForeignKey(
				name: "FK_ParkedVehicle_VehicleType_VehicleTypeName",
				table: "ParkedVehicle",
				column: "VehicleTypeName",
				principalTable: "VehicleType",
				principalColumn: "Name");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ParkedVehicle_VehicleType_VehicleTypeName",
				table: "ParkedVehicle");

			migrationBuilder.AlterColumn<string>(
				name: "VehicleTypeName",
				table: "ParkedVehicle",
				type: "nvarchar(450)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(450)",
				oldNullable: true);

			migrationBuilder.AddColumn<int>(
				name: "VehicleTypeId",
				table: "ParkedVehicle",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddForeignKey(
				name: "FK_ParkedVehicle_VehicleType_VehicleTypeName",
				table: "ParkedVehicle",
				column: "VehicleTypeName",
				principalTable: "VehicleType",
				principalColumn: "Name",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
