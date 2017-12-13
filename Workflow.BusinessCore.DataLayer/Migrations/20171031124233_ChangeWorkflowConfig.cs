using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Workflow.BusinessCore.DataLayer.Migrations
{
    public partial class ChangeWorkflowConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IdWorkflowConfigOriginal",
                table: "WorkflowConfig",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdWorkflowConfigOriginal",
                table: "WorkflowConfig");
        }
    }
}
