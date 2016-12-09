using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTTestServer.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using EasyRESTFramework.Client.Abstractions;
using EasyRESTFramework.Client;
namespace EasyRESTTestServer.ModelViews
{
    public class MainWindowModelView
    {
        public MainWindowModelView()
        {
            Students = new ObservableCollection<Student>();
            AddSampleStudentCommand = new RelayCommand(AddSampleStudent);
        }

        public ObservableCollection<Student> Students
        {
            get;
            set;
        }

        void AddSampleStudent()
        {
            Students.Add(new Student() { FirstName = "Test", LastName = "Test Last Name", BirthDate = new DateTime()});
        }

        public ICommand AddSampleStudentCommand
        {
            get;
            set;
        }

       
    }
}
