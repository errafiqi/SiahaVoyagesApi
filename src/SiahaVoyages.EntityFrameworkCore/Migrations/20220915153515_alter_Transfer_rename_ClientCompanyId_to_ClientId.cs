using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiahaVoyages.Migrations
{
    public partial class alter_Transfer_rename_ClientCompanyId_to_ClientId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_App_Transfert_App_ClientCompany_ClientCompanyId",
                table: "App_Transfert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_App_ClientCompany",
                table: "App_ClientCompany");

            migrationBuilder.RenameTable(
                name: "App_ClientCompany",
                newName: "App_Client");

            migrationBuilder.RenameColumn(
                name: "ClientCompanyId",
                table: "App_Transfert",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_App_Transfert_ClientCompanyId",
                table: "App_Transfert",
                newName: "IX_App_Transfert_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_App_Client",
                table: "App_Client",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_App_Transfert_App_Client_ClientId",
                table: "App_Transfert",
                column: "ClientId",
                principalTable: "App_Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_App_Transfert_App_Client_ClientId",
                table: "App_Transfert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_App_Client",
                table: "App_Client");

            migrationBuilder.RenameTable(
                name: "App_Client",
                newName: "App_ClientCompany");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "App_Transfert",
                newName: "ClientCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_App_Transfert_ClientId",
                table: "App_Transfert",
                newName: "IX_App_Transfert_ClientCompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_App_ClientCompany",
                table: "App_ClientCompany",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_App_Transfert_App_ClientCompany_ClientCompanyId",
                table: "App_Transfert",
                column: "ClientCompanyId",
                principalTable: "App_ClientCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
