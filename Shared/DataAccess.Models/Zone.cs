using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Zone: UniqueObject 
    {
        public string Name
        {
            get;set;
        }

        public string Description
        {
            get;set;
        }

        public int State
        {
            get;set;
        }

        public bool IsStarted
        {
            get;set;
        }

        public bool Enabled
        {
            get;set;
        }

        public String AssignedImage
        {
            get; set;
        }

    }
}
