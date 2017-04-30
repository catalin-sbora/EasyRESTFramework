using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Interfaces;

namespace DataAccess.EntityFrameworkCore
{
    public class EFWateringDaysRepository: EFGenericRepository<WateringDay>, IWateringDaysRepository
    {
        public EFWateringDaysRepository(StudentsDbContext context) : base(context)
        {

        }
    }
}
