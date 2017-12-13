using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Workflow.BusinessCore.DataLayer.Migrations
{
    public partial class AddChangesForModifyer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "RunStatus",
                table: "SelectorInstance");

            migrationBuilder.AddColumn<string>(
                name: "Position1",
                table: "UserSetUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position10",
                table: "UserSetUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position2",
                table: "UserSetUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position3",
                table: "UserSetUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position4",
                table: "UserSetUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position5",
                table: "UserSetUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position6",
                table: "UserSetUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position7",
                table: "UserSetUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position8",
                table: "UserSetUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position9",
                table: "UserSetUser",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Right",
                table: "UserSetUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChainNumberModifyer",
                table: "SelectorInstance",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ModifyerId",
                table: "SelectorInstance",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "ChainNumber",
                table: "Criteria",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position1",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "Position10",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "Position2",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "Position3",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "Position4",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "Position5",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "Position6",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "Position7",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "Position8",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "Position9",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "Right",
                table: "UserSetUser");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ChainNumberModifyer",
                table: "SelectorInstance");

            migrationBuilder.DropColumn(
                name: "ModifyerId",
                table: "SelectorInstance");

            migrationBuilder.DropColumn(
                name: "ChainNumber",
                table: "Criteria");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "UserSetUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RunStatus",
                table: "SelectorInstance",
                nullable: true);
        }
    }
}
