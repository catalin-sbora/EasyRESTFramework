using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DataAccess.DataModels;
namespace DataAccess.EntityFrameworkCore
{
    public class EFPersistenceContext : IPersistenceContext
    {
        private StudentsDbContext _dbContext;

        
        public IStudentsRepository GetStudentsRepository()
        {
            return new EFStudentsRepository(_dbContext);
        }

        public IZonesRepository GetZonesRepository()
        {
            return new EFZonesRepository(_dbContext);
        }

        public IWateringZonesRepository GetWateringZonesRepository()
        {
            return new EFWateringZonesRepository(_dbContext);
        }


        public IWateringDaysRepository GetWateringDaysRepository()
        {
            return new EFWateringDaysRepository(_dbContext);
        }
        public IProgramsRepository GetProgramsRepository()
        {
            return new EFProgramsRepository(_dbContext) ;
        }

        public EFPersistenceContext(IConfigurationRoot config)
        {
            InitializeContext(config);
        }
        private void InitTablesWithData()
        {
           var dbZones = _dbContext.Set<Zone>();
            List<Zone> zonesList = new List<Zone> {
                new Zone { Name = "Zone 1", Description = "This is zone 1", Enabled = true, AssignedImage = "https://ecommons.cornell.edu/bitstream/handle/1813/3574/Lawn%20Care%20without%20Pesticides.pdf.jpg?sequence=4&isAllowed=y" },
                new Zone { Name = "Zone 2", Description = "This is zone 2", Enabled = true, AssignedImage = "https://s-media-cache-ak0.pinimg.com/564x/5a/31/09/5a3109c0e229680b1d60473777738e43.jpg" },
                new Zone { Name = "Zone 3", Description = "This is zone 3", Enabled = true, AssignedImage = "https://s-media-cache-ak0.pinimg.com/236x/44/88/45/448845c3874ca215c8ee98e6482ff46f.jpg" },
                new Zone { Name = "Zone 4", Description = "This is zone 4", Enabled = true, AssignedImage = "https://s-media-cache-ak0.pinimg.com/564x/5a/31/09/5a3109c0e229680b1d60473777738e43.jpg" },
                new Zone {  Name = "Zone 5", Description = "This is zone 5", Enabled = true, AssignedImage = "https://s-media-cache-ak0.pinimg.com/236x/44/88/45/448845c3874ca215c8ee98e6482ff46f.jpg" },
                new Zone {  Name = "Zone 6", Description = "This is zone 6", Enabled = true, AssignedImage = "https://s-media-cache-ak0.pinimg.com/236x/44/88/45/448845c3874ca215c8ee98e6482ff46f.jpg" },
                new Zone {  Name = "Zone 7", Description = "This is zone 7", Enabled = true, AssignedImage = "https://s-media-cache-ak0.pinimg.com/564x/5a/31/09/5a3109c0e229680b1d60473777738e43.jpg" },
                new Zone {  Name = "Zone 8", Description = "This is zone 8", Enabled = true, AssignedImage = "https://s-media-cache-ak0.pinimg.com/236x/44/88/45/448845c3874ca215c8ee98e6482ff46f.jpg" }
            };
            dbZones.AddRange(zonesList);

            var dbPrograms = _dbContext.Set<Program>();
            List<Program> programList = new List<Program>
            {
                new Program {  Name = "Program Demo", Description = "This is the default program",StartDate = DateTime.Now, EndDate = new DateTime(2020, 10, 10),IsActive = true, StartTime = DateTime.Now }
            };
            dbPrograms.AddRange(programList);

            List<WateringDay> wateringDaysList = new List<WateringDay>
            {
                new WateringDay {  Program = programList.ElementAt(0), DayIndex = 0, IsActive = true, WateringRepeatCount = 1, WateringStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 10, 0, 0) },
                new WateringDay {  Program = programList.ElementAt(0), DayIndex = 1, IsActive = true, WateringRepeatCount = 1, WateringStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 10, 0, 0) },
                new WateringDay {  Program = programList.ElementAt(0), DayIndex = 2, IsActive = true, WateringRepeatCount = 1, WateringStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 10, 0, 0) },
                new WateringDay {  Program = programList.ElementAt(0), DayIndex = 3, IsActive = true, WateringRepeatCount = 2, WateringStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 10, 0, 0) },
                new WateringDay {  Program = programList.ElementAt(0), DayIndex = 4, IsActive = true, WateringRepeatCount = 1, WateringStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 10, 0, 0) },
                new WateringDay {  Program = programList.ElementAt(0), DayIndex = 5, IsActive = true, WateringRepeatCount = 3, WateringStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 10, 0, 0) },
                new WateringDay {  Program = programList.ElementAt(0), DayIndex = 6, IsActive = true, WateringRepeatCount = 2, WateringStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 10, 0, 0) }

            };

            var dbWateringDays = _dbContext.Set<WateringDay>();
            dbWateringDays.AddRange(wateringDaysList);

            var dbWateringZones = _dbContext.Set<WateringZone>();
            List<WateringZone> wateringZonesList = new List<WateringZone>
            {
                 new WateringZone {  Program=programList.ElementAt(0), Zone = zonesList.ElementAt(0), IsActive=true, Priority = 1, WateringDuration = new TimeSpan(0, 0, 200), WaitAfterWatering = new TimeSpan(0, 0, 200) },
                 new WateringZone {  Program=programList.ElementAt(0), Zone = zonesList.ElementAt(1), IsActive=true, Priority = 1, WateringDuration = new TimeSpan(0, 0, 200), WaitAfterWatering = new TimeSpan(0, 0, 10) },
                 new WateringZone {  Program=programList.ElementAt(0), Zone = zonesList.ElementAt(2), IsActive=true, Priority = 1, WateringDuration = new TimeSpan(0, 0, 100), WaitAfterWatering = new TimeSpan(0, 0, 10) },
                 new WateringZone { Program=programList.ElementAt(0), Zone = zonesList.ElementAt(4), IsActive=true, Priority = 1, WateringDuration = new TimeSpan(0, 0, 300), WaitAfterWatering = new TimeSpan(0, 0, 10) }
            };
            dbWateringZones.AddRange(wateringZonesList);

        }
        public bool InitializeContext(IConfigurationRoot configuration)
        {
            string connectionString = configuration.GetConnectionString("StudentsDbContext");
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(connectionString);
            _dbContext = new StudentsDbContext(optionsBuilder.Options);

            if (_dbContext.Database.EnsureCreated())
            {
                InitTablesWithData();
                _dbContext.SaveChanges();
            }
                return true;
        }

        public void ReleaseContext()
        {
            //throw new NotImplementedException();
        }

        public bool SaveAll()
        {
            if (_dbContext != null)
            {
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }

       
    }
}
