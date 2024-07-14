using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Models;

namespace Project.DAL.Contexts
{
    public class MVCDbContext :IdentityDbContext<ApplicationUser>
    {
        public MVCDbContext(DbContextOptions<MVCDbContext> options):base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.UseSqlServer("Server=. ; Database = MVCProject; Trusted_Connection = true;");


        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
