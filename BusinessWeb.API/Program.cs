using System.Text;
using BusinessWeb.API.Middleware;
using BusinessWeb.Application.Validators.Sales;
using BusinessWeb.Infrastructure;
using BusinessWeb.Infrastructure.Data;
using BusinessWeb.Infrastructure.Seed;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// ✅ Controllers
builder.Services.AddControllers()
    .AddApplicationPart(typeof(BusinessWeb.API.Controllers.SalesController).Assembly);

// ✅ Controllers + JSON default (Swagger'da text/plain emas, application/json chiqishi uchun)
builder.Services.AddControllers(options =>
{
    // Agar client Accept: text/plain yuborsa 406 qaytaradi (professional behavior)
    options.ReturnHttpNotAcceptable = true;
})
.AddApplicationPart(typeof(BusinessWeb.API.Controllers.SalesController).Assembly)
.ConfigureApiBehaviorOptions(options =>
{
    // Validation errors JSON bo‘lsin (422)
    options.InvalidModelStateResponseFactory = context =>
    {
        var problem = new ValidationProblemDetails(context.ModelState)
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Validation failed"
        };
        return new UnprocessableEntityObjectResult(problem);
    };
});

 main
builder.Services.AddAutoMapper(typeof(BusinessWeb.Application.Mapping.ProductProfile).Assembly);

// ✅ Swagger + JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BusinessWeb API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header: Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// ✅ FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateSaleValidator>();

// ✅ Infrastructure DI
builder.Services.AddInfrastructure(builder.Configuration);

// ✅ JWT Auth
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new Exception("Jwt:Secret missing");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

// ✅ CORS (Development only)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ✅ Global Exception Middleware
builder.Services.AddTransient<ExceptionMiddleware>();

var app = builder.Build();

// ✅ Auto migrate + seed admin (Development only)
if (app.Environment.IsDevelopment())
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await DatabaseSeeder.SeedAsync(db);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Database migration/seed failed. App will continue without crashing.");
    }
}
else
{
    app.Logger.LogWarning("Auto-migrate is disabled in Production. Run migrations manually before starting the app.");
}

// ✅ Swagger
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
}

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
