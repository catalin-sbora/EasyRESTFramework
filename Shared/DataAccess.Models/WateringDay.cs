using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DataAccess.Models
{
    public class WateringDay : UniqueObject
    {

        public Int32 ProgramId { get; set; }
        public virtual Program Program
        {
            get;
            set;
        }

        public int DayIndex
        {
            get; set;
        }

        public DateTime WateringStart
        {
            get; set;
        }

        public int WateringRepeatCount
        {
            get;set;
        }

        public bool IsActive
        {
            get; set;
        }
    }
}
