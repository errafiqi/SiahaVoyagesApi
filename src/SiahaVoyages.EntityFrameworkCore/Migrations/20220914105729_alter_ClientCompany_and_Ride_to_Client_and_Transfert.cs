using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiahaVoyages.Migrations
{
    public partial class alter_ClientCompany_and_Ride_to_Client_and_Transfert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App_Ride");

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
                name: "App_Transfert",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassengerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FMNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PickupTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAirportTransfert = table.Column<bool>(type: "bit", nullable: false),
                    FlightDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<byte>(type: "tinyint", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Transfert", x => x.Id);
                    table.ForeignKey(
                        name: "FK_App_Transfert_App_ClientCompany_ClientCompanyId",
                        column: x => x.ClientCompanyId,
                        principalTable: "App_ClientCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_App_Transfert_App_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "App_Driver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_App_Transfert_ClientCompanyId",
                table: "App_Transfert",
                column: "ClientCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_App_Transfert_DriverId",
                table: "App_Transfert",
                column: "DriverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App_Transfert");

            migrationBuilder.DropColumn(
                name: "Contact1",
                table: "App_Driver");

            migrationBuilder.DropColumn(
                name: "Contact2",
                table: "App_Driver");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "App_Driver");

            migrationBuilder.CreateTable(
                name: "App_Ride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    State = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Ride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_App_Ride_App_ClientCompany_ClientCompanyId",
                        column: x => x.ClientCompanyId,
                        principalTable: "App_ClientCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_App_Ride_App_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "App_Driver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_App_Ride_ClientCompanyId",
                table: "App_Ride",
                column: "ClientCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_App_Ride_DriverId",
                table: "App_Ride",
                column: "DriverId");
        }
    }
}
