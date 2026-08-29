using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NailDesignerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDurationAndDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "ServiceTypes",
                newName: "IsActive");

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "ServiceAddOns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Appointments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "ServiceAddOns");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ServiceTypes",
                newName: "isActive");
        }
    }
}
