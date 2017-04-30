using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Interfaces;

namespace DataAccess.EntityFrameworkCore
{
    public class EFZonesRepository:EFGenericRepository<Zone>, IZonesRepository
    {
        public EFZonesRepository(StudentsDbContext dbContext):base(dbContext)
        {
        }
    }
}
