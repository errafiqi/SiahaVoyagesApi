using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiahaVoyages.Migrations
{
    public partial class alter_Client_add_Logo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "App_Client",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "App_Client");
        }
    }
}
