using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiahaVoyages.Migrations
{
    public partial class alter_Driver_Transfer_and_drop_Passenger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App_Passenger");

            migrationBuilder.DropColumn(
                name: "Contact1",
                table: "App_Driver");

            migrationBuilder.DropColumn(
                name: "Contact2",
                table: "App_Driver");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "App_Driver");

            migrationBuilder.AddColumn<string>(
                name: "Passengers",
                table: "App_Transfert",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassengersPhone",
                table: "App_Transfert",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "App_Driver",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_App_Driver_UserId",
                table: "App_Driver",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_App_Driver_AbpUsers_UserId",
                table: "App_Driver",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_App_Driver_AbpUsers_UserId",
                table: "App_Driver");

            migrationBuilder.DropIndex(
                name: "IX_App_Driver_UserId",
                table: "App_Driver");

            migrationBuilder.DropColumn(
                name: "Passengers",
                table: "App_Transfert");

            migrationBuilder.DropColumn(
                name: "PassengersPhone",
                table: "App_Transfert");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "App_Driver");

            migrationBuilder.AddColumn<string>(
                name: "Contact1",
                table: "App_Driver",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact2",
                table: "App_Driver",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "App_Driver",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "App_Passenger",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Passenger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_App_Passenger_App_Transfert_TransferId",
                        column: x => x.TransferId,
                        principalTable: "App_Transfert",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_App_Passenger_TransferId",
                table: "App_Passenger",
                column: "TransferId");
        }
    }
}
