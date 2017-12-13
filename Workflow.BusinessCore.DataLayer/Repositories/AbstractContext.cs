using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Workflow.BusinessCore.DataLayer.Repositories
{
    /// <summary>
    /// AbstractContext abstract class.
    /// </summary>
    public abstract class AbstractContext : DbContext
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        public AbstractContext()
        { }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="options">Options</param>
        public AbstractContext(DbContextOptions options) : base(options)
        { }

        /// <summary>
        /// This method permits to re-configure the database connection for the EF migrations.
        /// </summary>
        /// <param name="optionsBuilder">DbContextOptionsBuilder</param>
        public void ConfigureDatabase(DbContextOptionsBuilder optionsBuilder)
        {
            string applicationRoot = AppContext.BaseDirectory
                .Substring(0, AppContext.BaseDirectory.IndexOf("bin")) 
                + "../Workflow.BusinessCore.ServiceLayer/";

            IConfigurationRoot Configuration = new ConfigurationBuilder()
            .SetBasePath(applicationRoot)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            optionsBuilder.UseSqlServer(Configuration["AppSettings:ConnectionStrings:ServerConnection"],
                b => b.MigrationsAssembly("Workflow.BusinessCore.DataLayer"));
        }
    }
}