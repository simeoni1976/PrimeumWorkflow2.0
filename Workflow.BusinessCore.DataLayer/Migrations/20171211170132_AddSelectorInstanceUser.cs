using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Workflow.BusinessCore.DataLayer.Migrations
{
    public partial class AddSelectorInstanceUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SelectorInstanceUser",
                columns: table => new
                {
                    SelectorInstanceId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Right = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectorInstanceUser", x => new { x.SelectorInstanceId, x.UserId });
                    table.UniqueConstraint("AK_SelectorInstanceUser_ID", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SelectorInstanceUser_SelectorInstance_SelectorInstanceId",
                        column: x => x.SelectorInstanceId,
                        principalTable: "SelectorInstance",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectorInstanceUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SelectorInstanceUser_UserId",
                table: "SelectorInstanceUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectorInstanceUser");
        }
    }
}
