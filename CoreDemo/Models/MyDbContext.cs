using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CoreDemo.Models
{
    public class MyDbContext : IdentityDbContext<User>//DbContext
    {
        public MyDbContext() { }

        //public MyDbContext(DbContextOptions<MyDbContext> options)
        //    : base(options)
        //{ }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=mydemo.db");
        }
    }

    //public class MySqlServerDbContext : DbContext
    //{
    //    public MySqlServerDbContext() { }

    //    //public MySqlServerDbContext(DbContextOptions<MyDbContext> options)
    //    //    : base(options)
    //    //{ }

    //    public DbSet<Employee> Employees { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseSqlServer("");
    //    }
    //}
}
