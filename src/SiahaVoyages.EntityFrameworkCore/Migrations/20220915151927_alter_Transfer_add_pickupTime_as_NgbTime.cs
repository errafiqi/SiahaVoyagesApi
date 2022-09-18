using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiahaVoyages.Migrations
{
    public partial class alter_Transfer_add_pickupTime_as_NgbTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PickupTimeId",
                table: "App_Transfert",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NgbTime",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Hour = table.Column<int>(type: "int", nullable: false),
                    Minute = table.Column<int>(type: "int", nullable: false),
                    Seconde = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NgbTime", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_App_Transfert_PickupTimeId",
                table: "App_Transfert",
                column: "PickupTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_App_Transfert_NgbTime_PickupTimeId",
                table: "App_Transfert",
                column: "PickupTimeId",
                principalTable: "NgbTime",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_App_Transfert_NgbTime_PickupTimeId",
                table: "App_Transfert");

            migrationBuilder.DropTable(
                name: "NgbTime");

            migrationBuilder.DropIndex(
                name: "IX_App_Transfert_PickupTimeId",
                table: "App_Transfert");

            migrationBuilder.DropColumn(
                name: "PickupTimeId",
                table: "App_Transfert");
        }
    }
}
