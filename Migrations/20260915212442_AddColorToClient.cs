using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NailDesignerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Clients");
        }
    }
}
