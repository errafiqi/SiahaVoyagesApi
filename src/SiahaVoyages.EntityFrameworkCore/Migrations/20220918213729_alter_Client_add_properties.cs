using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiahaVoyages.Migrations
{
    public partial class alter_Client_add_properties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PickupPoint",
                table: "App_Transfert",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Rate",
                table: "App_Transfert",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Adresse",
                table: "App_Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "App_Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "App_Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ICE",
                table: "App_Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IF",
                table: "App_Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "App_Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RC",
                table: "App_Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RIB",
                table: "App_Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TP",
                table: "App_Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "App_Client",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_App_Client_UserId",
                table: "App_Client",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_App_Client_AbpUsers_UserId",
                table: "App_Client",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_App_Client_AbpUsers_UserId",
                table: "App_Client");

            migrationBuilder.DropIndex(
                name: "IX_App_Client_UserId",
                table: "App_Client");

            migrationBuilder.DropColumn(
                name: "PickupPoint",
                table: "App_Transfert");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "App_Transfert");

            migrationBuilder.DropColumn(
                name: "Adresse",
                table: "App_Client");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "App_Client");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "App_Client");

            migrationBuilder.DropColumn(
                name: "ICE",
                table: "App_Client");

            migrationBuilder.DropColumn(
                name: "IF",
                table: "App_Client");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "App_Client");

            migrationBuilder.DropColumn(
                name: "RC",
                table: "App_Client");

            migrationBuilder.DropColumn(
                name: "RIB",
                table: "App_Client");

            migrationBuilder.DropColumn(
                name: "TP",
                table: "App_Client");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "App_Client");
        }
    }
}
