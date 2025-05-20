using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mini_Project.Controllers;
using Mini_Project.Data;
using Mini_Project.Models;
using Mini_Project.Services;
using Mini_Project.Validators;
using System.Security.Claims;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<AdminValidator>();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Your API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer [space] your JWT token.\n\nExample: Bearer abc123...",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // This should match the name in AddSecurityDefinition
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        //ValidateIssuer = true,
        //ValidateAudience = true,
        ValidateLifetime = true,
        //ValidateIssuerSigningKey = true,

        //ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //ValidAudience = builder.Configuration["Jwt:Audience"],
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated successfully!");
            var claims = context.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");
            if (claims != null)
            {
                Console.WriteLine("Claims in token:");
                foreach (var claim in claims)
                {
                    Console.WriteLine($"  {claim}");
                }
            }
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            Console.WriteLine("Token received for validation");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Identity Setup
builder.Services.AddIdentity<AppUser,  IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Enrollment Service
builder.Services.AddScoped<IEnrollmentServices, EnrollmentServices>();

// Course Service
builder.Services.AddScoped<ICourseServices, CourseServices>();

// Connect Database
var connectionString = builder.Configuration.GetConnectionString("SQLServerIdentityConnection") ?? throw new InvalidOperationException("Connection string 'SQLServerIdentityConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Add Roles to the table 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbSeed.SeedRolesAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStatusCodePages();
app.UseRouting();

app.Use(async (context, next) =>
{
    Console.WriteLine("Authorization: " + context.Request.Headers["Authorization"]);
    await next();
});

app.Use(async (context, next) =>
{
    var originalPath = context.Request.Path;
    var headers = context.Request.Headers;

    Console.WriteLine($"Request path: {originalPath}");
    Console.WriteLine($"Authorization header present: {headers.ContainsKey("Authorization")}");

    await next();

    Console.WriteLine($"Response status code: {context.Response.StatusCode}");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
