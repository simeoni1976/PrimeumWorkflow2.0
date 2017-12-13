using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Workflow.BusinessCore.DataLayer.Migrations
{
    public partial class AddActAndOthers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ActionSequenceRef",
                table: "WorkflowConfig",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<double>(
                name: "InitialValue",
                table: "ValueObject",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "FutureValue",
                table: "ValueObject",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "CurrentValue",
                table: "ValueObject",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<long>(
                name: "ActionSequenceRef",
                table: "SelectorConfig",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "NumericalFormat",
                table: "DistinctValue",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Action",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ActionParameter",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    OrderSequence = table.Column<int>(nullable: false),
                    ParameterName = table.Column<string>(nullable: true),
                    ReferenceSequence = table.Column<long>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionParameter", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ConfigVariable",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigVariable", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ActionSequence",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionId = table.Column<long>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Reference = table.Column<long>(nullable: false),
                    SequenceName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionSequence", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ActionSequence_Action_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Action",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionSequence_ActionId",
                table: "ActionSequence",
                column: "ActionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionParameter");

            migrationBuilder.DropTable(
                name: "ActionSequence");

            migrationBuilder.DropTable(
                name: "ConfigVariable");

            migrationBuilder.DropTable(
                name: "Action");

            migrationBuilder.DropColumn(
                name: "ActionSequenceRef",
                table: "WorkflowConfig");

            migrationBuilder.DropColumn(
                name: "ActionSequenceRef",
                table: "SelectorConfig");

            migrationBuilder.DropColumn(
                name: "NumericalFormat",
                table: "DistinctValue");

            migrationBuilder.AlterColumn<double>(
                name: "InitialValue",
                table: "ValueObject",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FutureValue",
                table: "ValueObject",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CurrentValue",
                table: "ValueObject",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
