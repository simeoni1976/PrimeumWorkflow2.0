using Microsoft.EntityFrameworkCore;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.DataLayer.Repositories
{
    /// <summary>
    /// ApplicationContext class.
    /// </summary>
    /// <remarks>
    /// This class permits to define the database context.
    /// </remarks>
    public class ApplicationContext : AbstractContext
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public ApplicationContext()
        {
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="options">Context options</param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        /// <summary>
        /// This method permits to configure the context database when is null.
        /// </summary>
        /// <param name="optionsBuilder">Options</param>
        /// <remarks>
        /// Initializes the parameters for the migration.
        /// </remarks>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                ConfigureDatabase(optionsBuilder);
            }

            base.OnConfiguring(optionsBuilder);
        }

        // Database table list definition.
        public DbSet<Criteria> Criteria { get; set; }
        public DbSet<CriteriaValues> CriteriaValues { get; set; }
        public DbSet<DataSet> DataSet { get; set; }
        public DbSet<DataSetDimension> DataSetDimension { get; set; }
        public DbSet<Dimension> Dimension { get; set; }
        public DbSet<SelectorConfig> SelectorConfig { get; set; }
        public DbSet<SelectorInstance> SelectorInstance { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<ValueObject> ValueObject { get; set; }
        public DbSet<WorkflowConfig> WorkflowConfig { get; set; }
        public DbSet<WorkflowDimension> WorkflowDimension { get; set; }
        public DbSet<WorkflowInstance> WorkflowInstance { get; set; }
        public DbSet<DimensionGroupData> DimensionGroupData { get; set; }
        public DbSet<DimensionPeriodData> DimensionPeriodData { get; set; }
        public DbSet<DimensionScalarData> DimensionScalarData { get; set; }
        public DbSet<DimensionTreeData> DimensionTreeData { get; set; }
        public DbSet<DimensionValues> DimensionValues { get; set; }
        public DbSet<UserSet> UserSet { get; set; }
        public DbSet<UserSetUser> UserSetUser { get; set; }
        public DbSet<ConditionnedCriteria> ConditionnedCriteria { get; set; }
        public DbSet<ConditionnedCriteriaValues> ConditionnedCriteriaValues { get; set; }
        public DbSet<DistinctValue> DistinctValue { get; set; }
        public DbSet<SelectorInstanceValueObject> SelectorInstanceValueObject { get; set; }
        public DbSet<GridConfig> GridConfig { get; set; }
        public DbSet<GridDimensionConfig> GridDimensionConfig { get; set; }
        public DbSet<GridValueConfig> GridValueConfig { get; set; }
        public DbSet<ConfigVariable> ConfigVariable { get; set; }
        public DbSet<Action> Action { get; set; }
        public DbSet<ActionSequence> ActionSequence { get; set; }
        public DbSet<ActionParameter> ActionParameter { get; set; }
        public DbSet<Constraint> Constraint { get; set; }
        public DbSet<ConstraintSequence> ConstraintSequence { get; set; }
        public DbSet<ConstraintParameter> ConstraintParameter { get; set;  }
        public DbSet<SelectorInstanceUser> SelectorInstanceUser { get; set; }


        /// <summary>
        /// This method permits to create a database model.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder object</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Criteria>().ToTable("Criteria");
            modelBuilder.Entity<CriteriaValues>().ToTable("CriteriaValues");
            modelBuilder.Entity<DataSet>().ToTable("DataSet");
            modelBuilder.Entity<DataSetDimension>().ToTable("DataSetDimension");
            modelBuilder.Entity<Dimension>().ToTable("Dimension");
            modelBuilder.Entity<SelectorConfig>().ToTable("SelectorConfig");
            modelBuilder.Entity<SelectorInstance>().ToTable("SelectorInstance");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<ValueObject>().ToTable("ValueObject");
            modelBuilder.Entity<WorkflowConfig>().ToTable("WorkflowConfig");
            modelBuilder.Entity<WorkflowDimension>().ToTable("WorkflowDimension");
            modelBuilder.Entity<WorkflowInstance>().ToTable("WorkflowInstance");
            modelBuilder.Entity<DimensionGroupData>().ToTable("DimensionGroupData");
            modelBuilder.Entity<DimensionPeriodData>().ToTable("DimensionPeriodData");
            modelBuilder.Entity<DimensionScalarData>().ToTable("DimensionScalarData");
            modelBuilder.Entity<DimensionTreeData>().ToTable("DimensionTreeData");
            modelBuilder.Entity<DimensionValues>().ToTable("DimensionValues");
            modelBuilder.Entity<UserSet>().ToTable("UserSet");
            modelBuilder.Entity<UserSetUser>().ToTable("UserSetUser");
            modelBuilder.Entity<ConditionnedCriteria>().ToTable("ConditionnedCriteria");
            modelBuilder.Entity<ConditionnedCriteriaValues>().ToTable("ConditionnedCriteriaValues");
            modelBuilder.Entity<DistinctValue>().ToTable("DistinctValue");
            modelBuilder.Entity<SelectorInstanceValueObject>().ToTable("SelectorInstanceValueObject");
            modelBuilder.Entity<GridConfig>().ToTable("GridConfig");
            modelBuilder.Entity<GridDimensionConfig>().ToTable("GridDimensionConfig");
            modelBuilder.Entity<GridValueConfig>().ToTable("GridValueConfig");
            modelBuilder.Entity<Action>().ToTable("Action");
            modelBuilder.Entity<ActionSequence>().ToTable("ActionSequence");
            modelBuilder.Entity<ActionParameter>().ToTable("ActionParameter");
            modelBuilder.Entity<Constraint>().ToTable("Constraint");
            modelBuilder.Entity<ConstraintSequence>().ToTable("ConstraintSequence");
            modelBuilder.Entity<ConstraintParameter>().ToTable("ConstraintParameter");
            modelBuilder.Entity<SelectorInstanceUser>().ToTable("SelectorInstanceUser");


            // RelationShips
            //modelBuilder.Entity<UserSetUser>()
            //    .HasOne(s => s.)
            //    .WithMany(c => c.ValueObjects);

            //modelBuilder.Entity<Criteria>().HasIndex(c => c.Id).IsUnique(true);

            modelBuilder.Entity<Criteria>()
                .HasOne(c => c.Dimension)
                .WithMany(d => d.Criteria);

            modelBuilder.Entity<DistinctValue>()
                .HasIndex(k => new { k.DataSetId, k.DimensionId });


            modelBuilder.Entity<SelectorInstanceValueObject>()
                .HasKey(k => new { k.SelectorInstanceId, k.ValueObjectId });

            modelBuilder.Entity<SelectorInstanceValueObject>()
                .HasOne(sivo => sivo.SelectorInstance)
                .WithMany(si => si.SelectorInstanceValueObject)
                .HasForeignKey(sivo => sivo.SelectorInstanceId);

            modelBuilder.Entity<SelectorInstanceValueObject>()
                .HasOne(sivo => sivo.ValueObject)
                .WithMany(vo => vo.SelectorInstanceValueObject)
                .HasForeignKey(sivo => sivo.ValueObjectId);

            modelBuilder.Entity<SelectorInstanceUser>()
                .HasKey(k => new { k.SelectorInstanceId, k.UserId });
            modelBuilder.Entity<SelectorInstanceUser>()
                .HasOne(siu => siu.SelectorInstance)
                .WithMany(si => si.SelectorInstanceUser)
                .HasForeignKey(siu => siu.SelectorInstanceId);
            modelBuilder.Entity<SelectorInstanceUser>()
                .HasOne(siu => siu.User)
                .WithMany(u => u.SelectorInstanceUser)
                .HasForeignKey(siu => siu.UserId);

            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
