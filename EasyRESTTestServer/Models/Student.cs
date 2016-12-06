using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTTestServer.Models
{
    public class Student 
    {
        private String _firstName = "";
        private String _lastName = "";
        private DateTime _birthDate = new DateTime();

        public String FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;                
            }
        }
        public String LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;                
            }
        }
        public DateTime BirthDate
        {
            get
            {
                return _birthDate;
            }
            set
            {
                _birthDate = value;                
            }
        }
    }
}
