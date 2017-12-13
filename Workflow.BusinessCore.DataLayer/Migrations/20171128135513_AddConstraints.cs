using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Workflow.BusinessCore.DataLayer.Migrations
{
    public partial class AddConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Constraint",
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
                    table.PrimaryKey("PK_Constraint", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ConstraintParameter",
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
                    table.PrimaryKey("PK_ConstraintParameter", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ConstraintSequence",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ConstraintId = table.Column<long>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Reference = table.Column<long>(nullable: false),
                    SequenceName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstraintSequence", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConstraintSequence_Constraint_ConstraintId",
                        column: x => x.ConstraintId,
                        principalTable: "Constraint",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConstraintSequence_ConstraintId",
                table: "ConstraintSequence",
                column: "ConstraintId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConstraintParameter");

            migrationBuilder.DropTable(
                name: "ConstraintSequence");

            migrationBuilder.DropTable(
                name: "Constraint");
        }
    }
}
