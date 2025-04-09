using Ambev.Domain.Interfaces;
using Ambev.Infrastructure.Data;
using Ambev.Infrastructure.Repositories;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Versioning;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Ambev.Application.Interfaces;
using Ambev.Application.Services;
using FluentValidation;
using Ambev.Application.DTOs;
using Ambev.Api.Validators;
using Ambev.API.Validators;
using Ambev.Infrastructure.Data.Context;
using Ambev.Infrastructure.HealthChecks;
using Ambev.API.Middleware;
using Ambev.Infrastructure;
using Ambev.Infrastructure.Data.Seeds;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add AutoMapper - Scan both Api and Application assemblies
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(Ambev.Application.Interfaces.IAuthService).Assembly);

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

// Add Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();

// Registra os validadores
builder.Services.AddScoped<IValidator<ProductDTO>, ProductDTOValidator>();
builder.Services.AddScoped<IValidator<CartDTO>, CartDTOValidator>();
builder.Services.AddScoped<IValidator<UserDTO>, UserDTOValidator>();

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key não configurada")))
        };
    });

// Adiciona o serviço de cache
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, CacheService>();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddCheck<MemoryHealthCheck>("memory")
    .AddCheck<ExternalServicesHealthCheck>("external");

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .WithOrigins("http://localhost:4200") // Angular default port
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});


// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add Application Layer
//builder.Services.AddApplication();

// Add Infrastructure Layer
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Executar migrations e seeds
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    await AdminUserSeed.SeedAdminUser(context);
}

// Adicionar o middleware no pipeline
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<ValidationMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use Serilog
app.UseSerilogRequestLogging();

// Use CORS
app.UseCors("CorsPolicy");

// Use Health Checks
app.UseHealthChecks("/health");

// Use Rate Limiting
app.UseIpRateLimiting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
