using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    // DbContext is a class that represents a session with the database and is responsible for interacting with the underlying database.
    public class DataContext : DbContext
    {
        /**
        * DbContextOptions class is used to configure the behavior of the DbContext class. 
            It allow to specify various options such as the database provider, connection string, and other settings related to how the database should be accessed and queried.

        * This constructor allows to configure the DataContext class with the desired options for connecting to and interacting with the underlying database.
        **/
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        /**
        * Property named Users, representing the "Users" table in the underlying database. 
        **/
        public DbSet<AppUser> Users { get; set; }
    }
}