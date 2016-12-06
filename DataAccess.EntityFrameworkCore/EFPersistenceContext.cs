using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DataAccess.DataModels;
namespace DataAccess.EntityFrameworkCore
{
    public class EFPersistenceContext : IPersistenceContext
    {
        private StudentsDbContext _dbContext;

        
        public IStudentsRepository GetStudentsRepository()
        {
            return new EFStudentsRepository(_dbContext);
        }
        
        public EFPersistenceContext(IConfigurationRoot config)
        {
            InitializeContext(config);
        }
        public bool InitializeContext(IConfigurationRoot configuration)
        {
            string connectionString = configuration.GetConnectionString("StudentsDbContext");
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(connectionString);
            _dbContext = new StudentsDbContext(optionsBuilder.Options);
            return true;
        }

        public void ReleaseContext()
        {
            //throw new NotImplementedException();
        }

        public bool SaveAll()
        {
            if (_dbContext != null)
            {
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
