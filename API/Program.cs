using System.Text;
using API.Data;
using API.Interface;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

/**
1. AddDbContext method is called on the builder.Services object, which represents the collection of services
     that will be available for dependency injection within the application.
2. UseSqlite method is called on the opt object, which represents the DbContextOptionsBuilder for the DataContext.
2.1. UseSqlite method is used to configure the SQLite database provider for the DataContext. 
     It takes the connection string retrieved from the configuration using builder.Configuration.GetConnectionString("DefaultConnection").

This code sets up the DataContext to use SQLite as the database provider and configures the connection string for the DefaultConnection.
**/
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//CORS is a security mechanism that allows web browsers to make requests to a different domain than the one from which the resource originated.
builder.Services.AddCors();

// AddScoped is a method used to register a service with a scoped lifetime.
// The scoped lifetime means that a new instance of the service will be created for each HTTP request, 
// and that instance will be reused throughout the lifetime of that request.
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
    options.TokenValidationParameters = new TokenValidationParameters{
     ValidateIssuerSigningKey = true,
     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
     ValidateIssuer = false, 
     ValidateAudience = false
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try 
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
