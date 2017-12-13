using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Workflow.BusinessCore.DataLayer.Repositories;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.DataLayer.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20171023151619_ModifySelectorInstance")]
    partial class ModifySelectorInstance
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<long>("Author");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsRead");

                    b.Property<string>("Message");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long>("Next");

                    b.Property<long>("Previous");

                    b.Property<long>("Receiver");

                    b.Property<string>("Username");

                    b.Property<long?>("ValueObjectId");

                    b.HasKey("Id");

                    b.HasIndex("ValueObjectId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.ConditionnedCriteria", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<long?>("DimensionId");

                    b.Property<string>("Formula");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("Order");

                    b.Property<long?>("SelectorConfigId");

                    b.Property<string>("Username");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("DimensionId");

                    b.HasIndex("SelectorConfigId");

                    b.ToTable("ConditionnedCriteria");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.ConditionnedCriteriaValues", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<long?>("ConditionnedCriteriaId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long?>("SelectorInstanceId");

                    b.Property<string>("Username");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ConditionnedCriteriaId");

                    b.HasIndex("SelectorInstanceId");

                    b.ToTable("ConditionnedCriteriaValues");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.Criteria", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<long?>("DimensionId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("Order");

                    b.Property<long?>("SelectorConfigId");

                    b.Property<long?>("SelectorConfigModifiersId");

                    b.Property<long?>("SelectorConfigValidatorsId");

                    b.Property<string>("Username");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("DimensionId");

                    b.HasIndex("SelectorConfigId");

                    b.HasIndex("SelectorConfigModifiersId");

                    b.HasIndex("SelectorConfigValidatorsId");

                    b.ToTable("Criteria");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.CriteriaValues", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<long?>("CriteriaId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long?>("SelectorInstanceId");

                    b.Property<long?>("SelectorInstanceModifierId");

                    b.Property<long?>("SelectorInstanceValidatorId");

                    b.Property<string>("Username");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("CriteriaId");

                    b.HasIndex("SelectorInstanceId");

                    b.HasIndex("SelectorInstanceModifierId");

                    b.HasIndex("SelectorInstanceValidatorId");

                    b.ToTable("CriteriaValues");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.DataSet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<string>("Username");

                    b.Property<long?>("WorkflowInstanceId");

                    b.HasKey("Id");

                    b.ToTable("DataSet");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.DataSetDimension", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("ColumnName");

                    b.Property<long?>("DataSetId");

                    b.Property<long?>("DimensionId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("DataSetId");

                    b.HasIndex("DimensionId");

                    b.ToTable("DataSetDimension");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.Dimension", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<long?>("DimensionGroupDataId");

                    b.Property<long?>("DimensionPeriodDataId");

                    b.Property<long?>("DimensionScalarDataId");

                    b.Property<long?>("DimensionTreeDataId");

                    b.Property<long?>("DimensionValuesId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<int>("TypeDimension");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("DimensionGroupDataId");

                    b.HasIndex("DimensionPeriodDataId");

                    b.HasIndex("DimensionScalarDataId");

                    b.HasIndex("DimensionTreeDataId");

                    b.HasIndex("DimensionValuesId");

                    b.ToTable("Dimension");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.DimensionGroupData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Username");

                    b.Property<string>("ValueKey");

                    b.HasKey("Id");

                    b.ToTable("DimensionGroupData");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.DimensionPeriodData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Username");

                    b.Property<string>("ValueKey");

                    b.HasKey("Id");

                    b.ToTable("DimensionPeriodData");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.DimensionScalarData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Username");

                    b.Property<string>("ValueKey");

                    b.HasKey("Id");

                    b.ToTable("DimensionScalarData");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.DimensionTreeData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("DisplayName");

                    b.Property<string>("LevelName");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("TreeName");

                    b.Property<string>("Username");

                    b.Property<string>("ValueKey");

                    b.Property<double>("Weight");

                    b.HasKey("Id");

                    b.ToTable("DimensionTreeData");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.DimensionValues", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Username");

                    b.Property<string>("ValueKey");

                    b.HasKey("Id");

                    b.ToTable("DimensionValues");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.DistinctValue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<long>("DataSetId");

                    b.Property<long>("DimensionId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Username");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("DataSetId", "DimensionId");

                    b.ToTable("DistinctValue");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.GridConfig", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<string>("Username");

                    b.Property<long?>("WorkflowConfigId");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowConfigId");

                    b.ToTable("GridConfig");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.GridDimensionConfig", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("DisplayName");

                    b.Property<long?>("GridColumnId");

                    b.Property<long?>("GridFixedId");

                    b.Property<long?>("GridRowId");

                    b.Property<int>("InternalName");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("Order");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("GridColumnId");

                    b.HasIndex("GridFixedId");

                    b.HasIndex("GridRowId");

                    b.ToTable("GridDimensionConfig");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.GridValueConfig", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<long?>("GridDimensionConfigId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("Order");

                    b.Property<string>("Username");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("GridDimensionConfigId");

                    b.ToTable("GridValueConfig");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.SelectorConfig", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<long>("FailPropagateId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<long>("PrevPropagateId");

                    b.Property<long>("PropagateId");

                    b.Property<string>("Username");

                    b.Property<long?>("WorkflowConfigId");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowConfigId");

                    b.ToTable("SelectorConfig");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.SelectorInstance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long>("ParentSelectorInstanceId");

                    b.Property<int?>("PreviousStatus");

                    b.Property<int?>("RunStatus");

                    b.Property<long?>("SelectorConfigId");

                    b.Property<int?>("Status");

                    b.Property<string>("Username");

                    b.Property<long?>("WorkflowInstanceId");

                    b.HasKey("Id");

                    b.HasIndex("SelectorConfigId");

                    b.HasIndex("WorkflowInstanceId");

                    b.ToTable("SelectorInstance");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.SelectorInstanceValueObject", b =>
                {
                    b.Property<long>("SelectorInstanceId");

                    b.Property<long>("ValueObjectId");

                    b.Property<DateTime>("AddedDate");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Username");

                    b.HasKey("SelectorInstanceId", "ValueObjectId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("ValueObjectId");

                    b.ToTable("SelectorInstanceValueObject");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Email");

                    b.Property<string>("EmployeeID");

                    b.Property<string>("Firstname");

                    b.Property<string>("Lastname");

                    b.Property<string>("Login");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.UserSet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("DimensionPosition");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("UserSet");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.UserSetUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("Role");

                    b.Property<long?>("UserId");

                    b.Property<long?>("UserSetId");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserSetId");

                    b.ToTable("UserSetUser");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.ValueObject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<double>("CurrentValue");

                    b.Property<long?>("DataSetId");

                    b.Property<string>("Dimension1");

                    b.Property<string>("Dimension10");

                    b.Property<string>("Dimension2");

                    b.Property<string>("Dimension3");

                    b.Property<string>("Dimension4");

                    b.Property<string>("Dimension5");

                    b.Property<string>("Dimension6");

                    b.Property<string>("Dimension7");

                    b.Property<string>("Dimension8");

                    b.Property<string>("Dimension9");

                    b.Property<double>("FutureValue");

                    b.Property<double>("InitialValue");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long?>("SelectionInstanceId");

                    b.Property<int?>("Status");

                    b.Property<string>("TypeValue");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("DataSetId");

                    b.HasIndex("SelectionInstanceId");

                    b.ToTable("ValueObject");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.WorkflowConfig", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("WorkflowConfig");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.WorkflowDimension", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("ColumnName");

                    b.Property<long?>("DimensionId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Username");

                    b.Property<long?>("WorkflowConfigId");

                    b.HasKey("Id");

                    b.HasIndex("DimensionId");

                    b.HasIndex("WorkflowConfigId");

                    b.ToTable("WorkflowDimension");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.WorkflowInstance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasMaxLength(20);

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Constraints");

                    b.Property<long>("DataSetId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long?>("ModifyUserId");

                    b.Property<string>("Name");

                    b.Property<int?>("Status");

                    b.Property<long>("UserSetId");

                    b.Property<string>("Username");

                    b.Property<long?>("ValidateUserId");

                    b.Property<long?>("WorkflowConfigId");

                    b.HasKey("Id");

                    b.HasIndex("ModifyUserId");

                    b.HasIndex("ValidateUserId");

                    b.HasIndex("WorkflowConfigId");

                    b.ToTable("WorkflowInstance");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.Comment", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.ValueObject", "ValueObject")
                        .WithMany("Comment")
                        .HasForeignKey("ValueObjectId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.ConditionnedCriteria", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.Dimension", "Dimension")
                        .WithMany()
                        .HasForeignKey("DimensionId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.SelectorConfig", "SelectorConfig")
                        .WithMany("ModifyCriterias")
                        .HasForeignKey("SelectorConfigId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.ConditionnedCriteriaValues", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.ConditionnedCriteria", "ConditionnedCriteria")
                        .WithMany()
                        .HasForeignKey("ConditionnedCriteriaId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.SelectorInstance", "SelectorInstance")
                        .WithMany()
                        .HasForeignKey("SelectorInstanceId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.Criteria", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.Dimension", "Dimension")
                        .WithMany("Criteria")
                        .HasForeignKey("DimensionId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.SelectorConfig", "SelectorConfig")
                        .WithMany("Criterias")
                        .HasForeignKey("SelectorConfigId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.SelectorConfig", "SelectorConfigModifiers")
                        .WithMany("Modifiers")
                        .HasForeignKey("SelectorConfigModifiersId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.SelectorConfig", "SelectorConfigValidators")
                        .WithMany("Validators")
                        .HasForeignKey("SelectorConfigValidatorsId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.CriteriaValues", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.Criteria", "Criteria")
                        .WithMany("CriteriaValues")
                        .HasForeignKey("CriteriaId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.SelectorInstance", "SelectorInstance")
                        .WithMany("CriteriaValues")
                        .HasForeignKey("SelectorInstanceId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.SelectorInstance", "SelectorInstanceModifier")
                        .WithMany("ModifyCriteriasValues")
                        .HasForeignKey("SelectorInstanceModifierId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.SelectorInstance", "SelectorInstanceValidator")
                        .WithMany("ValidatorCriteriasValues")
                        .HasForeignKey("SelectorInstanceValidatorId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.DataSetDimension", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.DataSet", "DataSet")
                        .WithMany("DataSetDimensions")
                        .HasForeignKey("DataSetId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.Dimension", "Dimension")
                        .WithMany("DataSetDimension")
                        .HasForeignKey("DimensionId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.Dimension", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.DimensionGroupData")
                        .WithMany("Dimensions")
                        .HasForeignKey("DimensionGroupDataId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.DimensionPeriodData")
                        .WithMany("Dimensions")
                        .HasForeignKey("DimensionPeriodDataId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.DimensionScalarData")
                        .WithMany("Dimensions")
                        .HasForeignKey("DimensionScalarDataId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.DimensionTreeData")
                        .WithMany("Dimensions")
                        .HasForeignKey("DimensionTreeDataId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.DimensionValues")
                        .WithMany("Dimensions")
                        .HasForeignKey("DimensionValuesId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.GridConfig", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.WorkflowConfig", "WorkflowConfig")
                        .WithMany()
                        .HasForeignKey("WorkflowConfigId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.GridDimensionConfig", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.GridConfig", "GridColumn")
                        .WithMany("ColumnDimensions")
                        .HasForeignKey("GridColumnId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.GridConfig", "GridFixed")
                        .WithMany("FixedDimensions")
                        .HasForeignKey("GridFixedId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.GridConfig", "GridRow")
                        .WithMany("RowDimensions")
                        .HasForeignKey("GridRowId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.GridValueConfig", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.GridDimensionConfig", "GridDimensionConfig")
                        .WithMany("Values")
                        .HasForeignKey("GridDimensionConfigId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.SelectorConfig", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.WorkflowConfig", "WorkflowConfig")
                        .WithMany("SelectorConfig")
                        .HasForeignKey("WorkflowConfigId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.SelectorInstance", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.SelectorConfig", "SelectorConfig")
                        .WithMany("SelectorInstance")
                        .HasForeignKey("SelectorConfigId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.WorkflowInstance", "WorkflowInstance")
                        .WithMany("SelectorInstance")
                        .HasForeignKey("WorkflowInstanceId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.SelectorInstanceValueObject", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.SelectorInstance", "SelectorInstance")
                        .WithMany("SelectorInstanceValueObject")
                        .HasForeignKey("SelectorInstanceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.ValueObject", "ValueObject")
                        .WithMany("SelectorInstanceValueObject")
                        .HasForeignKey("ValueObjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.UserSetUser", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.User", "User")
                        .WithMany("UserSetUser")
                        .HasForeignKey("UserId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.UserSet", "UserSet")
                        .WithMany("UserSetUser")
                        .HasForeignKey("UserSetId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.ValueObject", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.DataSet", "DataSet")
                        .WithMany("ValueObjects")
                        .HasForeignKey("DataSetId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.SelectorInstance", "SelectionInstance")
                        .WithMany()
                        .HasForeignKey("SelectionInstanceId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.WorkflowDimension", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.Dimension", "Dimension")
                        .WithMany("WorkflowDimension")
                        .HasForeignKey("DimensionId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.WorkflowConfig", "WorkflowConfig")
                        .WithMany("WorkflowDimension")
                        .HasForeignKey("WorkflowConfigId");
                });

            modelBuilder.Entity("Workflow.BusinessCore.DataLayer.Entities.WorkflowInstance", b =>
                {
                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.User", "ModifyUser")
                        .WithMany()
                        .HasForeignKey("ModifyUserId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.User", "ValidateUser")
                        .WithMany()
                        .HasForeignKey("ValidateUserId");

                    b.HasOne("Workflow.BusinessCore.DataLayer.Entities.WorkflowConfig", "WorkflowConfig")
                        .WithMany("WorkflowInstance")
                        .HasForeignKey("WorkflowConfigId");
                });
        }
    }
}
