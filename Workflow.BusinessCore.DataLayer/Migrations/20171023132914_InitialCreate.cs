using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Workflow.BusinessCore.DataLayer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataSet",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    WorkflowInstanceId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DimensionGroupData",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    ValueKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionGroupData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DimensionPeriodData",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    ValueKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionPeriodData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DimensionScalarData",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    ValueKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionScalarData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DimensionTreeData",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    LevelName = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    TreeName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    ValueKey = table.Column<string>(nullable: true),
                    Weight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionTreeData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DimensionValues",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    ValueKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionValues", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DistinctValue",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    DataSetId = table.Column<long>(nullable: false),
                    DimensionId = table.Column<long>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistinctValue", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EmployeeID = table.Column<string>(nullable: true),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserSet",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    DimensionPosition = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowConfig",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowConfig", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Dimension",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    DimensionGroupDataId = table.Column<long>(nullable: true),
                    DimensionPeriodDataId = table.Column<long>(nullable: true),
                    DimensionScalarDataId = table.Column<long>(nullable: true),
                    DimensionTreeDataId = table.Column<long>(nullable: true),
                    DimensionValuesId = table.Column<long>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TypeDimension = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dimension", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dimension_DimensionGroupData_DimensionGroupDataId",
                        column: x => x.DimensionGroupDataId,
                        principalTable: "DimensionGroupData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dimension_DimensionPeriodData_DimensionPeriodDataId",
                        column: x => x.DimensionPeriodDataId,
                        principalTable: "DimensionPeriodData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dimension_DimensionScalarData_DimensionScalarDataId",
                        column: x => x.DimensionScalarDataId,
                        principalTable: "DimensionScalarData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dimension_DimensionTreeData_DimensionTreeDataId",
                        column: x => x.DimensionTreeDataId,
                        principalTable: "DimensionTreeData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dimension_DimensionValues_DimensionValuesId",
                        column: x => x.DimensionValuesId,
                        principalTable: "DimensionValues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSetUser",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    UserSetId = table.Column<long>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSetUser", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserSetUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSetUser_UserSet_UserSetId",
                        column: x => x.UserSetId,
                        principalTable: "UserSet",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GridConfig",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    WorkflowConfigId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridConfig", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GridConfig_WorkflowConfig_WorkflowConfigId",
                        column: x => x.WorkflowConfigId,
                        principalTable: "WorkflowConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SelectorConfig",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    FailPropagateId = table.Column<long>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PrevPropagateId = table.Column<long>(nullable: false),
                    PropagateId = table.Column<long>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    WorkflowConfigId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectorConfig", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SelectorConfig_WorkflowConfig_WorkflowConfigId",
                        column: x => x.WorkflowConfigId,
                        principalTable: "WorkflowConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowInstance",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Constraints = table.Column<string>(nullable: true),
                    DataSetId = table.Column<long>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ModifyUserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    UserSetId = table.Column<long>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    ValidateUserId = table.Column<long>(nullable: true),
                    WorkflowConfigId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowInstance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkflowInstance_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowInstance_User_ValidateUserId",
                        column: x => x.ValidateUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowInstance_WorkflowConfig_WorkflowConfigId",
                        column: x => x.WorkflowConfigId,
                        principalTable: "WorkflowConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DataSetDimension",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    DataSetId = table.Column<long>(nullable: true),
                    DimensionId = table.Column<long>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSetDimension", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DataSetDimension_DataSet_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSet",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataSetDimension_Dimension_DimensionId",
                        column: x => x.DimensionId,
                        principalTable: "Dimension",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowDimension",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    DimensionId = table.Column<long>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    WorkflowConfigId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowDimension", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkflowDimension_Dimension_DimensionId",
                        column: x => x.DimensionId,
                        principalTable: "Dimension",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowDimension_WorkflowConfig_WorkflowConfigId",
                        column: x => x.WorkflowConfigId,
                        principalTable: "WorkflowConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GridDimensionConfig",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    GridColumnId = table.Column<long>(nullable: true),
                    GridFixedId = table.Column<long>(nullable: true),
                    GridRowId = table.Column<long>(nullable: true),
                    InternalName = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridDimensionConfig", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GridDimensionConfig_GridConfig_GridColumnId",
                        column: x => x.GridColumnId,
                        principalTable: "GridConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridDimensionConfig_GridConfig_GridFixedId",
                        column: x => x.GridFixedId,
                        principalTable: "GridConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridDimensionConfig_GridConfig_GridRowId",
                        column: x => x.GridRowId,
                        principalTable: "GridConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConditionnedCriteria",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    DimensionId = table.Column<long>(nullable: true),
                    Formula = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    SelectorConfigId = table.Column<long>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionnedCriteria", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConditionnedCriteria_Dimension_DimensionId",
                        column: x => x.DimensionId,
                        principalTable: "Dimension",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConditionnedCriteria_SelectorConfig_SelectorConfigId",
                        column: x => x.SelectorConfigId,
                        principalTable: "SelectorConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Criteria",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    DimensionId = table.Column<long>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    SelectorConfigId = table.Column<long>(nullable: true),
                    SelectorConfigModifiersId = table.Column<long>(nullable: true),
                    SelectorConfigValidatorsId = table.Column<long>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criteria", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Criteria_Dimension_DimensionId",
                        column: x => x.DimensionId,
                        principalTable: "Dimension",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Criteria_SelectorConfig_SelectorConfigId",
                        column: x => x.SelectorConfigId,
                        principalTable: "SelectorConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Criteria_SelectorConfig_SelectorConfigModifiersId",
                        column: x => x.SelectorConfigModifiersId,
                        principalTable: "SelectorConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Criteria_SelectorConfig_SelectorConfigValidatorsId",
                        column: x => x.SelectorConfigValidatorsId,
                        principalTable: "SelectorConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SelectorInstance",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ParentSelectorInstanceId = table.Column<long>(nullable: false),
                    PreviousStatus = table.Column<int>(nullable: true),
                    SelectorConfigId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    WorkflowInstanceId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectorInstance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SelectorInstance_SelectorConfig_SelectorConfigId",
                        column: x => x.SelectorConfigId,
                        principalTable: "SelectorConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SelectorInstance_WorkflowInstance_WorkflowInstanceId",
                        column: x => x.WorkflowInstanceId,
                        principalTable: "WorkflowInstance",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GridValueConfig",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    GridDimensionConfigId = table.Column<long>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridValueConfig", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GridValueConfig_GridDimensionConfig_GridDimensionConfigId",
                        column: x => x.GridDimensionConfigId,
                        principalTable: "GridDimensionConfig",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConditionnedCriteriaValues",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ConditionnedCriteriaId = table.Column<long>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    SelectorInstanceId = table.Column<long>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionnedCriteriaValues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConditionnedCriteriaValues_ConditionnedCriteria_ConditionnedCriteriaId",
                        column: x => x.ConditionnedCriteriaId,
                        principalTable: "ConditionnedCriteria",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConditionnedCriteriaValues_SelectorInstance_SelectorInstanceId",
                        column: x => x.SelectorInstanceId,
                        principalTable: "SelectorInstance",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CriteriaValues",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    CriteriaId = table.Column<long>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    SelectorInstanceId = table.Column<long>(nullable: true),
                    SelectorInstanceModifierId = table.Column<long>(nullable: true),
                    SelectorInstanceValidatorId = table.Column<long>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriteriaValues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CriteriaValues_Criteria_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "Criteria",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CriteriaValues_SelectorInstance_SelectorInstanceId",
                        column: x => x.SelectorInstanceId,
                        principalTable: "SelectorInstance",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CriteriaValues_SelectorInstance_SelectorInstanceModifierId",
                        column: x => x.SelectorInstanceModifierId,
                        principalTable: "SelectorInstance",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CriteriaValues_SelectorInstance_SelectorInstanceValidatorId",
                        column: x => x.SelectorInstanceValidatorId,
                        principalTable: "SelectorInstance",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ValueObject",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    CurrentValue = table.Column<double>(nullable: false),
                    DataSetId = table.Column<long>(nullable: true),
                    Dimension1 = table.Column<string>(nullable: true),
                    Dimension10 = table.Column<string>(nullable: true),
                    Dimension2 = table.Column<string>(nullable: true),
                    Dimension3 = table.Column<string>(nullable: true),
                    Dimension4 = table.Column<string>(nullable: true),
                    Dimension5 = table.Column<string>(nullable: true),
                    Dimension6 = table.Column<string>(nullable: true),
                    Dimension7 = table.Column<string>(nullable: true),
                    Dimension8 = table.Column<string>(nullable: true),
                    Dimension9 = table.Column<string>(nullable: true),
                    FutureValue = table.Column<double>(nullable: false),
                    InitialValue = table.Column<double>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    SelectionInstanceId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    TypeValue = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueObject", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ValueObject_DataSet_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSet",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValueObject_SelectorInstance_SelectionInstanceId",
                        column: x => x.SelectionInstanceId,
                        principalTable: "SelectorInstance",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Author = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Next = table.Column<long>(nullable: false),
                    Previous = table.Column<long>(nullable: false),
                    Receiver = table.Column<long>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    ValueObjectId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comment_ValueObject_ValueObjectId",
                        column: x => x.ValueObjectId,
                        principalTable: "ValueObject",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SelectorInstanceValueObject",
                columns: table => new
                {
                    SelectorInstanceId = table.Column<long>(nullable: false),
                    ValueObjectId = table.Column<long>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ID = table.Column<long>(maxLength: 20, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectorInstanceValueObject", x => new { x.SelectorInstanceId, x.ValueObjectId });
                    table.UniqueConstraint("AK_SelectorInstanceValueObject_ID", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SelectorInstanceValueObject_SelectorInstance_SelectorInstanceId",
                        column: x => x.SelectorInstanceId,
                        principalTable: "SelectorInstance",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectorInstanceValueObject_ValueObject_ValueObjectId",
                        column: x => x.ValueObjectId,
                        principalTable: "ValueObject",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ValueObjectId",
                table: "Comment",
                column: "ValueObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionnedCriteria_DimensionId",
                table: "ConditionnedCriteria",
                column: "DimensionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionnedCriteria_SelectorConfigId",
                table: "ConditionnedCriteria",
                column: "SelectorConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionnedCriteriaValues_ConditionnedCriteriaId",
                table: "ConditionnedCriteriaValues",
                column: "ConditionnedCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionnedCriteriaValues_SelectorInstanceId",
                table: "ConditionnedCriteriaValues",
                column: "SelectorInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_DimensionId",
                table: "Criteria",
                column: "DimensionId");

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_SelectorConfigId",
                table: "Criteria",
                column: "SelectorConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_SelectorConfigModifiersId",
                table: "Criteria",
                column: "SelectorConfigModifiersId");

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_SelectorConfigValidatorsId",
                table: "Criteria",
                column: "SelectorConfigValidatorsId");

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaValues_CriteriaId",
                table: "CriteriaValues",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaValues_SelectorInstanceId",
                table: "CriteriaValues",
                column: "SelectorInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaValues_SelectorInstanceModifierId",
                table: "CriteriaValues",
                column: "SelectorInstanceModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaValues_SelectorInstanceValidatorId",
                table: "CriteriaValues",
                column: "SelectorInstanceValidatorId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetDimension_DataSetId",
                table: "DataSetDimension",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetDimension_DimensionId",
                table: "DataSetDimension",
                column: "DimensionId");

            migrationBuilder.CreateIndex(
                name: "IX_Dimension_DimensionGroupDataId",
                table: "Dimension",
                column: "DimensionGroupDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Dimension_DimensionPeriodDataId",
                table: "Dimension",
                column: "DimensionPeriodDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Dimension_DimensionScalarDataId",
                table: "Dimension",
                column: "DimensionScalarDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Dimension_DimensionTreeDataId",
                table: "Dimension",
                column: "DimensionTreeDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Dimension_DimensionValuesId",
                table: "Dimension",
                column: "DimensionValuesId");

            migrationBuilder.CreateIndex(
                name: "IX_DistinctValue_DataSetId_DimensionId",
                table: "DistinctValue",
                columns: new[] { "DataSetId", "DimensionId" });

            migrationBuilder.CreateIndex(
                name: "IX_GridConfig_WorkflowConfigId",
                table: "GridConfig",
                column: "WorkflowConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_GridDimensionConfig_GridColumnId",
                table: "GridDimensionConfig",
                column: "GridColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_GridDimensionConfig_GridFixedId",
                table: "GridDimensionConfig",
                column: "GridFixedId");

            migrationBuilder.CreateIndex(
                name: "IX_GridDimensionConfig_GridRowId",
                table: "GridDimensionConfig",
                column: "GridRowId");

            migrationBuilder.CreateIndex(
                name: "IX_GridValueConfig_GridDimensionConfigId",
                table: "GridValueConfig",
                column: "GridDimensionConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectorConfig_WorkflowConfigId",
                table: "SelectorConfig",
                column: "WorkflowConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectorInstance_SelectorConfigId",
                table: "SelectorInstance",
                column: "SelectorConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectorInstance_WorkflowInstanceId",
                table: "SelectorInstance",
                column: "WorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectorInstanceValueObject_ValueObjectId",
                table: "SelectorInstanceValueObject",
                column: "ValueObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSetUser_UserId",
                table: "UserSetUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSetUser_UserSetId",
                table: "UserSetUser",
                column: "UserSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueObject_DataSetId",
                table: "ValueObject",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueObject_SelectionInstanceId",
                table: "ValueObject",
                column: "SelectionInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDimension_DimensionId",
                table: "WorkflowDimension",
                column: "DimensionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDimension_WorkflowConfigId",
                table: "WorkflowDimension",
                column: "WorkflowConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_ModifyUserId",
                table: "WorkflowInstance",
                column: "ModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_ValidateUserId",
                table: "WorkflowInstance",
                column: "ValidateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_WorkflowConfigId",
                table: "WorkflowInstance",
                column: "WorkflowConfigId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "ConditionnedCriteriaValues");

            migrationBuilder.DropTable(
                name: "CriteriaValues");

            migrationBuilder.DropTable(
                name: "DataSetDimension");

            migrationBuilder.DropTable(
                name: "DistinctValue");

            migrationBuilder.DropTable(
                name: "GridValueConfig");

            migrationBuilder.DropTable(
                name: "SelectorInstanceValueObject");

            migrationBuilder.DropTable(
                name: "UserSetUser");

            migrationBuilder.DropTable(
                name: "WorkflowDimension");

            migrationBuilder.DropTable(
                name: "ConditionnedCriteria");

            migrationBuilder.DropTable(
                name: "Criteria");

            migrationBuilder.DropTable(
                name: "GridDimensionConfig");

            migrationBuilder.DropTable(
                name: "ValueObject");

            migrationBuilder.DropTable(
                name: "UserSet");

            migrationBuilder.DropTable(
                name: "Dimension");

            migrationBuilder.DropTable(
                name: "GridConfig");

            migrationBuilder.DropTable(
                name: "DataSet");

            migrationBuilder.DropTable(
                name: "SelectorInstance");

            migrationBuilder.DropTable(
                name: "DimensionGroupData");

            migrationBuilder.DropTable(
                name: "DimensionPeriodData");

            migrationBuilder.DropTable(
                name: "DimensionScalarData");

            migrationBuilder.DropTable(
                name: "DimensionTreeData");

            migrationBuilder.DropTable(
                name: "DimensionValues");

            migrationBuilder.DropTable(
                name: "SelectorConfig");

            migrationBuilder.DropTable(
                name: "WorkflowInstance");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "WorkflowConfig");
        }
    }
}
