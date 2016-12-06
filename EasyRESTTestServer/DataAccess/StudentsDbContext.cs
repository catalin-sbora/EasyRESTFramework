using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EasyRESTTestServer.Models;

namespace EasyRESTTestServer.DataAccess
{
    public class StudentsDbContext:DbContext
    {
        public DbSet<Student> Students;
    }
}
