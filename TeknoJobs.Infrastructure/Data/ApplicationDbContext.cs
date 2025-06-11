using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeknoJobs.Domain.Entities;

namespace TeknoJobs.Infrastructure.Data
{
    //setting up th applicationDbContext class that inherits from DbContext to utilize Entity Framework Core for database operations      
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Departments> Departments { get; set; }
    }
}
