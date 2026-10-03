using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NailDesignerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddServicePriceToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ServicePrice",
                table: "Appointments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServicePrice",
                table: "Appointments");
        }
    }
}
