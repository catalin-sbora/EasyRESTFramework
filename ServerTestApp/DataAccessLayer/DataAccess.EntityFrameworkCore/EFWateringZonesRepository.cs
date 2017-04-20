using DataAccess.DataModels;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.EntityFrameworkCore
{
    public class EFWateringZonesRepository:EFGenericRepository<WateringZone>, IWateringZonesRepository
    {
        public EFWateringZonesRepository(StudentsDbContext context) : base(context)
        {
        }
    }
}
