using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiahaVoyages.Migrations
{
    public partial class add_Message_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "App_Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageSubject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    OriginMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SenderMarked = table.Column<bool>(type: "bit", nullable: false),
                    SenderStared = table.Column<bool>(type: "bit", nullable: false),
                    SenderArchived = table.Column<bool>(type: "bit", nullable: false),
                    SenderDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecipientMarked = table.Column<bool>(type: "bit", nullable: false),
                    RecipientStared = table.Column<bool>(type: "bit", nullable: false),
                    RecipientArchived = table.Column<bool>(type: "bit", nullable: false),
                    RecipientDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_App_Message_AbpUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_App_Message_AbpUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_App_Message_App_Message_OriginMessageId",
                        column: x => x.OriginMessageId,
                        principalTable: "App_Message",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_App_Message_OriginMessageId",
                table: "App_Message",
                column: "OriginMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_App_Message_RecipientId",
                table: "App_Message",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_App_Message_SenderId",
                table: "App_Message",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App_Message");
        }
    }
}
