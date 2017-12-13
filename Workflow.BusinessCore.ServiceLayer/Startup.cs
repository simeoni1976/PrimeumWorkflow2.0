using AutoMapper;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Selector.BusinessCore.ServiceLayer.Adapters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Workflow.BusinessCore.BusinessLayer.Domains;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Logic;
using Workflow.BusinessCore.BusinessLayer.Process;
using Workflow.BusinessCore.BusinessLayer.Process.Interfaces;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.BusinessCore.DataLayer.Repositories;
using Workflow.BusinessCore.ServiceLayer.Adapters;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Helpers;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer
{
    public class Startup
    {
        private readonly log4net.ILog _logger = LogManager.GetLogger(typeof(Startup));
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            // Initializes the log repository
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.Info("Start...");
            _logger.Info("Configure the database services...");

            // TDU - 16/08/2017 - Culture et globalisation
            services.AddLocalization(options => options.ResourcesPath = "Languages");

            // Adds services required for using options.
            services.AddOptions();

            // Register the IConfiguration instance which AppSettings binds against.
	        services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Add db context.
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration["AppSettings:ConnectionStrings:ServerConnection"],
            b => b.MigrationsAssembly("Workflow.BusinessCore.ServiceLayer")), ServiceLifetime.Scoped);

            services.AddScoped(p => new ApplicationContext(p.GetService<DbContextOptions<ApplicationContext>>()));

            _logger.Info("Configure the services...");

            // Register application services.

            // Domains
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IValueObjectDomain, ValueObjectDomain>();
            services.AddSingleton<ICriteriaDomain, CriteriaDomain>();
            services.AddSingleton<ICriteriaValuesDomain, CriteriaValuesDomain>();
            services.AddSingleton<IDimensionDomain, DimensionDomain>();
            services.AddSingleton<IDataSetDomain, DataSetDomain>();
            services.AddSingleton<IDataSetDimensionDomain, DataSetDimensionDomain>();
            services.AddSingleton<ISelectorConfigDomain, SelectorConfigDomain>();
            services.AddSingleton<ISelectorInstanceDomain, SelectorInstanceDomain>();
            services.AddSingleton<IWorkflowConfigDomain, WorkflowConfigDomain>();
            services.AddSingleton<IWorkflowInstanceDomain, WorkflowInstanceDomain>();
            services.AddSingleton<IWorkflowDimensionDomain, WorkflowDimensionDomain>();
            services.AddSingleton<ICommentDomain, CommentDomain>();
            services.AddSingleton<IUserDomain, UserDomain>();
            services.AddSingleton<IUserSetDomain, UserSetDomain>();
            services.AddSingleton<IGridConfigurationDomain, GridConfigurationDomain>();
            services.AddSingleton<IActionSequenceDomain, ActionSequenceDomain>();
            services.AddSingleton<IConfigVariableDomain, ConfigVariableDomain>();
            services.AddSingleton<IConstraintSequenceDomain, ConstraintSequenceDomain>();

            // Adapters
            services.AddSingleton<ICommentAdapter, CommentAdapter>();
            services.AddSingleton<IValueObjectAdapter, ValueObjectAdapter>();
            services.AddSingleton<ICriteriaAdapter, CriteriaAdapter>();
            services.AddSingleton<IDimensionAdapter, DimensionAdapter>();
            services.AddSingleton<IDataSetAdapter, DataSetAdapter>();
            services.AddSingleton<IProcessAdapter, ProcessAdapter>();
            services.AddSingleton<IWorkflowInstanceAdapter, WorkflowInstanceAdapter>();
            services.AddSingleton<ISelectorInstanceAdapter, SelectorInstanceAdapter>();
            services.AddSingleton<IUserAdapter, UserAdapter>();
            services.AddSingleton<IUserSetAdapter, UserSetAdapter>();
            services.AddSingleton<ISelectorConfigAdapter, SelectorConfigAdapter>();
            services.AddSingleton<IGridConfigurationAdapter, GridConfigurationAdapter>();
            services.AddSingleton<IWorkflowConfigAdapter, WorkflowConfigAdapter>();
            services.AddSingleton<IActionAdapter, ActionAdapter>();
            services.AddSingleton<IActionSequenceAdapter, ActionSequenceAdapter>();
            services.AddSingleton<IActionParameterAdapter, ActionParameterAdapter>();
            services.AddSingleton<IConstraintAdapter, ConstraintAdapter>();
            services.AddSingleton<IConstraintSequenceAdapter, ConstraintSequenceAdapter>();
            services.AddSingleton<IConstraintParameterAdapter, ConstraintParameterAdapter>();


            // Register process proc
            services.AddSingleton<IWorkflowEngine, WorkflowEngine>();
            services.AddSingleton<ISelectorEngine, SelectorEngine>();

            // TDU - 20/06/2017 - Ajout du singleton des réponses synchronisées.
            services.AddSingleton<ISynchronizeDictionary, SynchronizeDictionary>();

            // TDU - Logs
            // services.AddTransient<TRS.ILogger, LoggerLog4Net>();
            // services.AddTransient<ILogger, LoggerLog4Net>();

            services.AddApplicationInsightsTelemetry(Configuration);

            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });

            // Add AutoMapper after AddMvc().
            services.AddAutoMapper(typeof(Startup));

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(5);
                options.CookieHttpOnly = true;
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Business Core WebAPI", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApplicationContext context)
        {
            _logger.Info("Configure the HTTP request pipeline...");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddLog4Net();

            CultureInfo[] supportedCultures = new CultureInfo[]
            {
                    new CultureInfo("en-US"),
                    new CultureInfo("fr-FR")
            };
            RequestLocalizationOptions optionsLocalization = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            optionsLocalization.RequestCultureProviders.Insert(0, new CustomChainedCultureProvider());
            app.UseRequestLocalization(optionsLocalization);

            app.UseMvc();

            if (env.IsDevelopment())
            {
                // Create data tests by default
                ApplicationInitializer.Initialize(context);
                _logger.Info("[Configure] Create data test by default");

                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Business Core WebAPI");
            });
        }
    }
}
