using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiahaVoyages.Migrations
{
    public partial class alter_Transfer_set_DriverId_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_App_Transfert_App_Driver_DriverId",
                table: "App_Transfert");

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "App_Transfert",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_App_Transfert_App_Driver_DriverId",
                table: "App_Transfert",
                column: "DriverId",
                principalTable: "App_Driver",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_App_Transfert_App_Driver_DriverId",
                table: "App_Transfert");

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "App_Transfert",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_App_Transfert_App_Driver_DriverId",
                table: "App_Transfert",
                column: "DriverId",
                principalTable: "App_Driver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
