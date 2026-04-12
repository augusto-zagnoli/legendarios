using legendarios_API.Interfaces;
using legendarios_API.Middleware;
using legendarios_API.Repository;
using legendarios_API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace legendarios_API
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
            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            // Authentication (JWT)
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            // Repositories (DI)
            services.AddScoped<ILegendariosRepository, LegendariosRepositoryV2>();
            services.AddScoped<ILoginRepository, LoginRepositoryV2>();
            services.AddScoped<IAnunciosRepository, AnunciosRepository>();
            services.AddScoped<IEventosRepository, EventosRepository>();
            services.AddScoped<IInscricoesRepository, InscricoesRepository>();
            services.AddScoped<ICheckinRepository, CheckinRepository>();
            services.AddScoped<IVoluntariosRepository, VoluntariosRepository>();
            services.AddScoped<IAuditRepository, AuditRepository>();

            // Services (DI)
            services.AddScoped<ILegendariosService, LegendariosServiceV2>();
            services.AddScoped<ILoginService, LoginServiceV2>();
            services.AddScoped<IAnunciosService, AnunciosServiceV2>();
            services.AddScoped<IEventosService, EventosServiceV2>();
            services.AddScoped<IInscricoesService, InscricoesServiceV2>();
            services.AddScoped<ICheckinService, CheckinServiceV2>();
            services.AddScoped<IVoluntariosService, VoluntariosServiceV2>();
            services.AddScoped<IAuditService, AuditServiceV2>();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Legendários API", Version = "v2" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        System.Array.Empty<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Global error handling
            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Legendários API v2"));

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
