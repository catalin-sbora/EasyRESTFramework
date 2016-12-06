using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.DataModels
{
    public class Student : UniqueObject
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
