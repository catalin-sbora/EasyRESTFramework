using DataAccess.Models;
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
            modelBuilder.Entity<Program>().HasKey(p => p.Id);
            modelBuilder.Entity<WateringDay>().HasKey(wd => wd.Id);
            modelBuilder.Entity<WateringDay>()
                .HasOne<Program>(p => p.Program)
                .WithMany(p => p.WateringDays).IsRequired()                                
                .HasForeignKey(p => p.ProgramId);
            

            modelBuilder.Entity<Student>().ToTable("Student");
           /* modelBuilder.Entity<Program>()
                .HasMany<WateringDay>(s => s.WateringDays)
                .WithOne(s => s.Program)
                .HasForeignKey(s => s.ProgramId);

            modelBuilder.Entity<Program>()
                .HasMany<WateringZone>(s => s.WateringZones)
                .WithOne(s => s.Program)
                .HasForeignKey(s => s.ProgramId);
            */
        }

    }
}
