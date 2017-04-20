using DataAccess.DataModels;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.EntityFrameworkCore
{
    public class EFProgramsRepository:EFGenericRepository<Program>, IProgramsRepository
    {
        public EFProgramsRepository(StudentsDbContext context) : base(context)
        {
        }
    }
}
