using BelajarAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BelajarAPI.MyContext
{
    public class myContext : DbContext
    {
        public myContext() : base("belajar_api") { }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Division> Divisions { get; set; }
    }

}