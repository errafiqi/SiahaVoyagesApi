using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiahaVoyages.Migrations
{
    public partial class alter_Transfer_replace_pickupTime_with_Hour_and_Minute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickupTime",
                table: "App_Transfert");

            migrationBuilder.AddColumn<int>(
                name: "Hour",
                table: "App_Transfert",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Minute",
                table: "App_Transfert",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hour",
                table: "App_Transfert");

            migrationBuilder.DropColumn(
                name: "Minute",
                table: "App_Transfert");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PickupTime",
                table: "App_Transfert",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
