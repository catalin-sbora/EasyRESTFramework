using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Program : UniqueObject
    {
        public string Name
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public virtual ICollection<WateringZone> WateringZones
        {
            get; set;
        }
        public virtual ICollection<WateringDay> WateringDays
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }
        public DateTime EndDate
        {
            get; set;
        }
        public bool IsActive
        {
            get; set;
        }
    }
}
