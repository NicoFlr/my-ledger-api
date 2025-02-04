
using Data.Models;
using Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Presentation.Middlewares;
using Serilog;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Services.Managers.User;
using Services.Managers.Category;
using Services.Managers.Role;
using Services.Managers.Transaction;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            services.AddTransient<ICategoryManager, CategoryManager>();
            services.AddTransient<IRoleManager, RoleManager>();
            services.AddTransient<ITransactionManager, TransactionManager>();
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

            /*services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<MyLedgerDbContext>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = false;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["AuthSettings:Audience"],
                    ValidIssuer = Configuration["AuthSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"]))
                };
            });*/

            services.AddControllers();
            string? environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environmentName != null && (environmentName.Equals("Development")))
            {
                services.AddSwaggerGen(p =>
                {
                    p.SwaggerDoc("v1", new OpenApiInfo { Title = "Ledger API", Version = "v1" });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    p.IncludeXmlComments(xmlPath);

                    p.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Please insert your JWT Token into field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Scheme = "Bearer",
                        BearerFormat = "JWT"
                    });
                    p.AddSecurityRequirement(new OpenApiSecurityRequirement{
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference{
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                });
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Ledger Settings file loaded: " + "appsettings." + env.EnvironmentName + ".json");

            var builder = new SqlConnectionStringBuilder(Configuration["ConnectionStrings:LedgerSQLDatabase"]);

            Log.Information("Ledger connection string to DB: " + builder.DataSource + " Database: " + builder.InitialCatalog);

            app.UseSerilogRequestLogging();

            app.UseMiddleware(typeof(ExceptionsMiddleware));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAll");

            //app.UseAuthentication();

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
