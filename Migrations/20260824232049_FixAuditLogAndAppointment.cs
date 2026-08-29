using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NailDesignerAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixAuditLogAndAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangeBy",
                table: "AuditLogs",
                newName: "ChangedBy");

            migrationBuilder.RenameColumn(
                name: "ChangeAt",
                table: "AuditLogs",
                newName: "ChangedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Appointments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangedBy",
                table: "AuditLogs",
                newName: "ChangeBy");

            migrationBuilder.RenameColumn(
                name: "ChangedAt",
                table: "AuditLogs",
                newName: "ChangeAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
