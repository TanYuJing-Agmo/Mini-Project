using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mini_Project.Data;
using Mini_Project.Models;
using Microsoft.Extensions.DependencyInjection;
using Mini_Project;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Identity Setup
builder.Services.AddIdentity<AppUser,  IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Connect Database
var connectionString = builder.Configuration.GetConnectionString("SQLServerIdentityConnection") ?? throw new InvalidOperationException("Connection string 'SQLServerIdentityConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();
 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await AddRole.SeedRolesAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
