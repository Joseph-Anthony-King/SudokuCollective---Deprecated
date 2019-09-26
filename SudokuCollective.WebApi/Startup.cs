using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SudokuCollective.WebApi.Models;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Services.Interfaces;
using SudokuCollective.WebApi.Services;

namespace SudokuCollective.WebApi {

    public class Startup {

        public Startup(IConfiguration configuration) {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseNpgsql(Configuration.GetConnectionString("DatabaseConnection")));

            services
                .AddMvc()
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.Configure<TokenManagement>(Configuration.GetSection("TokenManagement"));
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            var secret = Encoding.ASCII.GetBytes(token.Secret);

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IAppsService, AppsService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IGamesService, GamesService>();
            services.AddScoped<IDifficultiesService, DifficultiesService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<ISolutionsService, SolutionsService>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sudoku Collective API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

            if (env.IsDevelopment()) {

                app.UseDeveloperExceptionPage();

            }

            app.UseCors(x => x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseSwagger();

            app.UseSwaggerUI(c => {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sudoku Collective API V1");
            });

            // Initialize and set the path for the welcome page saved in wwwroot
            DefaultFilesOptions defaultFile = new DefaultFilesOptions();
            defaultFile.DefaultFileNames.Clear();
            defaultFile.DefaultFileNames.Add("index.html");

            app.UseDefaultFiles(defaultFile);

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SeedData.EnsurePopulated(app, Configuration);
        }
    }
}
