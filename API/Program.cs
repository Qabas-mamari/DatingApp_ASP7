using API.Data;
using Microsoft.EntityFrameworkCore;

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
var app = builder.Build();

//CORS is a security mechanism that allows web browsers to make requests to a different domain than the one from which the resource originated.
builder.Services.AddCors();

// Configure the HTTP request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.MapControllers();

app.Run();
