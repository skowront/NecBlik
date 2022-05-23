using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
