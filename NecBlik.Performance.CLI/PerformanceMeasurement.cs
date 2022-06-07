using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace NecBlik.Performance.CLI
{
    public class PerformanceMeasurement
    {
        public TimeSpan EnvironmentInitializationTime = TimeSpan.Zero;
        public TimeSpan CoordinatorCreationTime = TimeSpan.Zero;
        public TimeSpan GetCoordinatorAddressTime = TimeSpan.Zero;
        public TimeSpan DiscoveryTime = TimeSpan.Zero;
        public List<TimeSpan> RoundTripTimes = new List<TimeSpan>();

        
    }

    public class PerformanceMeasurementMap:ClassMap<PerformanceMeasurement>
    {
        public PerformanceMeasurementMap()
        {
            Map(m => m.EnvironmentInitializationTime).Index(0).Name(nameof(PerformanceMeasurement.EnvironmentInitializationTime));
            Map(m => m.CoordinatorCreationTime).Index(1).Name(nameof(PerformanceMeasurement.CoordinatorCreationTime));
            Map(m => m.GetCoordinatorAddressTime).Index(2).Name(nameof(PerformanceMeasurement.GetCoordinatorAddressTime));
            Map(m => m.DiscoveryTime).Index(3).Name(nameof(PerformanceMeasurement.DiscoveryTime));
        }
    }
}
