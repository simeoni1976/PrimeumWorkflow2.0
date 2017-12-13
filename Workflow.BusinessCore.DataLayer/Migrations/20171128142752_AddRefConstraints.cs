using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Workflow.BusinessCore.DataLayer.Migrations
{
    public partial class AddRefConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ConstraintSequenceRef",
                table: "WorkflowConfig",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ConstraintSequenceRef",
                table: "SelectorConfig",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConstraintSequenceRef",
                table: "WorkflowConfig");

            migrationBuilder.DropColumn(
                name: "ConstraintSequenceRef",
                table: "SelectorConfig");
        }
    }
}
