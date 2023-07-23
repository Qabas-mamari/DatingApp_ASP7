
using API.Data;
using API.Interface;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            /**
            1. AddDbContext method is called on the builder.Services object, which represents the collection of services
                that will be available for dependency injection within the application.
            2. UseSqlite method is called on the opt object, which represents the DbContextOptionsBuilder for the DataContext.
            2.1. UseSqlite method is used to configure the SQLite database provider for the DataContext. 
                It takes the connection string retrieved from the configuration using builder.Configuration.GetConnectionString("DefaultConnection").

            This code sets up the DataContext to use SQLite as the database provider and configures the connection string for the DefaultConnection.
            **/
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            //CORS is a security mechanism that allows web browsers to make requests to a different domain than the one from which the resource originated.
            services.AddCors();
            
            // AddScoped is a method used to register a service with a scoped lifetime.
            // The scoped lifetime means that a new instance of the service will be created for each HTTP request, 
            // and that instance will be reused throughout the lifetime of that request
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}