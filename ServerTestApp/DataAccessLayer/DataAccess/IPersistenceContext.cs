using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Interfaces
{
    public interface IPersistenceContext
    {
        bool InitializeContext(IConfigurationRoot configuration);

        IStudentsRepository GetStudentsRepository();
        IZonesRepository GetZonesRepository();
        IWateringZonesRepository GetWateringZonesRepository();
        IWateringDaysRepository GetWateringDaysRepository();
        IProgramsRepository GetProgramsRepository();

        bool SaveAll();

        void ReleaseContext();
    }
}
