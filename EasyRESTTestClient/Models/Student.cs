using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTFramework.Client;
namespace EasyRESTTestClient.Models
{
    public class Student : ObservableObject
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }
    }
}
