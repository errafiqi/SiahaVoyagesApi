using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiahaVoyages.Migrations
{
    public partial class alter_invoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_App_Invoice_App_Transfert_TransferId",
                table: "App_Invoice");

            migrationBuilder.RenameColumn(
                name: "TransferId",
                table: "App_Invoice",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_App_Invoice_TransferId",
                table: "App_Invoice",
                newName: "IX_App_Invoice_ClientId");

            migrationBuilder.AddColumn<float>(
                name: "Prix",
                table: "App_Invoice",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddForeignKey(
                name: "FK_App_Invoice_App_Client_ClientId",
                table: "App_Invoice",
                column: "ClientId",
                principalTable: "App_Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_App_Invoice_App_Client_ClientId",
                table: "App_Invoice");

            migrationBuilder.DropColumn(
                name: "Prix",
                table: "App_Invoice");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "App_Invoice",
                newName: "TransferId");

            migrationBuilder.RenameIndex(
                name: "IX_App_Invoice_ClientId",
                table: "App_Invoice",
                newName: "IX_App_Invoice_TransferId");

            migrationBuilder.AddForeignKey(
                name: "FK_App_Invoice_App_Transfert_TransferId",
                table: "App_Invoice",
                column: "TransferId",
                principalTable: "App_Transfert",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
