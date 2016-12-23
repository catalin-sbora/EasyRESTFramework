using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTTestClient.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

using EasyRESTTestClient.DataAccess;

namespace EasyRESTTestClient.ModelViews
{
    
    public class MainWindowModelView
    {

        private TestClientDataContext _dataContext = null;
        private ObservableCollection<Student> _students = new ObservableCollection<Student>();
        private IStudentRepository _studentsRepository = null;

        public MainWindowModelView(TestClientDataContext dataContext)
        {            
            AddSampleStudentCommand = new RelayCommand(AddSampleStudent);
            LoadDataCommand = new RelayCommand(LoadData);
            SaveDataCommand = new RelayCommand(SaveData);
            _dataContext = dataContext;
            if (_dataContext != null)
            {
                _studentsRepository = _dataContext.Students;
            }
        }

        public ObservableCollection<Student> Students
        {
            get
            {
                return _students;
            }
            private set
            {
                _students = value;
            }
        }

        void AddSampleStudent()
        {

            var studentToAdd = new Student() { FirstName = "Test", LastName = "Test Last Name", BirthDate = new DateTime() };
            _studentsRepository.AddStudent(studentToAdd);
            Students.Add(studentToAdd);            
        }

        async void LoadData()
        {
            //consider loading it as Task async
            if (_studentsRepository != null)
            {
                IEnumerable<Student> foundStudents = await _studentsRepository.GetAll();
                foreach (Student crtStudent in foundStudents)
                {
                    if (!Students.Contains(crtStudent))
                        Students.Add(crtStudent);
                }
            }
        }

        async void SaveData()
        {
            try
            {

                if (_dataContext != null)
                {
                    await _dataContext.SaveAll();
                }
            }
            catch (Exception e)
            {
                //failed to save data.
            }
        }

        public ICommand AddSampleStudentCommand
        {
            get;
            set;
        }

        public ICommand LoadDataCommand
        {
            get;
            set;
        }
        public ICommand SaveDataCommand
        {
            get;
            set;
        }

       
    }
}
