using DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.EntityFrameworkCore
{
    public class StudentsDbContext : DbContext
    {
        private readonly DbSet<Student> Students;

        public StudentsDbContext(DbContextOptions options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Student>().ToTable("Student");

            
        }

    }
}
