using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiahaVoyages.Migrations
{
    public partial class Voucher_and_Invoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "App_Invoice",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "App_Invoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "App_Bon",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "App_Bon",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "App_Invoice");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "App_Invoice");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "App_Bon");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "App_Bon");
        }
    }
}
