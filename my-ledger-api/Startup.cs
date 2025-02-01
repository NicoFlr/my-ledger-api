
using Data.Models;
using Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Presentation.Middlewares;
using Serilog;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Services.Managers.User;

namespace Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyLedgerDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("LedgerSQLDatabase")));
            services.AddScoped<UnitOfWork, UnitOfWork>(p => new UnitOfWork(new MyLedgerDbContext()));
            services.AddTransient<IUserManager, UserManager>();


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.WithOrigins("*")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .WithExposedHeaders("Authorization")
                                      );
            });

            services.AddControllers();
            string? environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new OpenApiInfo { Title = "Ledger API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                p.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Ledger Settings file loaded: " + "appsettings." + env.EnvironmentName + ".json");

            var builder = new SqlConnectionStringBuilder(Configuration["ConnectionStrings:ClientDatabase"]);

            Log.Information("Ledger connection string to DB: " + builder.DataSource + " Database: " + builder.InitialCatalog);

            app.UseSerilogRequestLogging();

            app.UseMiddleware(typeof(ExceptionsMiddleware));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            string? environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environmentName != null &&  (environmentName.Equals("Development")))
            {
                app.UseSwagger();

                app.UseSwaggerUI(p =>
                {
                    p.SwaggerEndpoint("/swagger/v1/swagger.json", "Ledger API V1");
                });
            }
            else
            {
                app.UseHsts();
            }
        }
    }
}
