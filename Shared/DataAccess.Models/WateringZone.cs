using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class WateringZone : UniqueObject
    {
        public Int32 ProgramId { get; set; }
        public Program Program
        {
            get;
            set;
        }
        public Int32 ZoneId { get; set; }
        public Zone Zone
        {
            get; set;
        }

        public TimeSpan WateringDuration
        {
            get; set;
        }

        public TimeSpan WaitAfterWatering
        {
            get; set;
        }

        public int Priority
        {
            get; set;
        }

        public bool IsActive
        {
            get; set;
        }
    }
}
