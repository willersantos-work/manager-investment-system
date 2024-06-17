using InvestManagerSystem.Global.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using InvestManagerSystem.Global.Injection;
using static InvestManagerSystem.Global.Injection.ConfigInjection;
using InvestManagerSystem.Global.Helpers.CustomException;
using InvestManagerSystem.Auth.Middlewares;
using Coravel;
using InvestManagerSystem.TaskScheduler.Tasks;
using InvestManagerSystem.TaskScheduler;

namespace InvestManagerSystem.Global
{
    public class Startup
    {
        private readonly string _version = "v1";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add controllers to the container.
            services.AddControllers();

            // Database connection
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Generate swagger doc
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(_version, new OpenApiInfo { Title = Configuration["ProjectName"], Version = _version });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    // Type = SecuritySchemeType.ApiKey,
                    // BearerFormat = "JWT",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            services.AddEndpointsApiExplorer();

            // Adding automaper
            services.AddAutoMapper(typeof(Startup));
            
            // Use ServiceInjection to inject services
            services.ServiceLayerInjection();

            // Use RepositoryInjection to inject repositories
            services.RepositoryLayerInjection();

            // Use ConfigLayerInjection to inject environment variables
            services.ConfigLayerInjection(Configuration);

            // Add task scheduler and inject schedulers
            services.TaskLayerInjection();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // TODO: Remover ao final as linhas de swagger
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint($"/swagger/{_version}/swagger.json", "Teste");
                options.RoutePrefix = string.Empty;
            });

            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                // Create Swagger
                app.UseSwagger();
                app.UseSwaggerUI(options => {
                    options.SwaggerEndpoint($"/swagger/{_version}/swagger.json", "Teste");
                    options.RoutePrefix = string.Empty;
                });
            }

            // Exception handler middleware
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // Authentication middleware
            app.UseMiddleware<AuthMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Exposing endpoints in app
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.ApplicationServices.UseScheduler(SchedulerConfig.ConfigureScheduler);
        }
    }
}
