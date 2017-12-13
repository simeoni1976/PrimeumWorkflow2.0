using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Workflow.BusinessCore.DataLayer.Migrations
{
    public partial class ChangeModifyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEditable",
                table: "SelectorInstanceValueObject",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "SelectorConfigModifyDataId",
                table: "Criteria",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_SelectorConfigModifyDataId",
                table: "Criteria",
                column: "SelectorConfigModifyDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Criteria_SelectorConfig_SelectorConfigModifyDataId",
                table: "Criteria",
                column: "SelectorConfigModifyDataId",
                principalTable: "SelectorConfig",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criteria_SelectorConfig_SelectorConfigModifyDataId",
                table: "Criteria");

            migrationBuilder.DropIndex(
                name: "IX_Criteria_SelectorConfigModifyDataId",
                table: "Criteria");

            migrationBuilder.DropColumn(
                name: "IsEditable",
                table: "SelectorInstanceValueObject");

            migrationBuilder.DropColumn(
                name: "SelectorConfigModifyDataId",
                table: "Criteria");
        }
    }
}
