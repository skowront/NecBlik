using NecBlik.Core.Interfaces;
using NecBlik.PyDigi.Models;
using NecBlik.Digi.Models;
using NecBlik.Performance.CLI;
using System.Linq;
using Python.Runtime;
using CsvHelper;
using System.Globalization;

public class Program
{
    public static string Port = "COM4" ;
    public static int iterations = 500;

    /// <summary>
    /// Mostly for testing purposes
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var digiPerformance = Digi();
        var pyDigiPerformance = PyDigi();
#if RELEASE
        using (var writer = new StreamWriter("release-digi.csv"))
#else
        using (var writer = new StreamWriter("debug-digi.csv"))
#endif
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteField<string>(nameof(PerformanceMeasurement.EnvironmentInitializationTime));
            csv.NextRecord();
            csv.WriteField<string>(digiPerformance.EnvironmentInitializationTime.Seconds.ToString()+":"+digiPerformance.EnvironmentInitializationTime.Milliseconds.ToString());
            csv.NextRecord();
            csv.WriteField<string>(nameof(PerformanceMeasurement.CoordinatorCreationTime));
            csv.NextRecord();
            csv.WriteField<string>(digiPerformance.CoordinatorCreationTime.Seconds.ToString() + ":" + digiPerformance.CoordinatorCreationTime.Milliseconds.ToString());
            csv.NextRecord();
            csv.WriteField<string>(nameof(PerformanceMeasurement.GetCoordinatorAddressTime));
            csv.NextRecord();
            csv.WriteRecord<int>(digiPerformance.GetCoordinatorAddressTime.Milliseconds);
            csv.NextRecord();
            csv.WriteField<string>(nameof(PerformanceMeasurement.DiscoveryTime));
            csv.NextRecord();
            csv.WriteField<string>(digiPerformance.DiscoveryTime.Seconds.ToString() + ":" + digiPerformance.DiscoveryTime.Milliseconds.ToString());
            csv.NextRecord();
            csv.WriteField<string>(nameof(PerformanceMeasurement.RoundTripTimes));
            csv.NextRecord();
            foreach (var item in digiPerformance.RoundTripTimes)
            {
                csv.WriteRecord<int>(item.Milliseconds);
                csv.NextRecord();
            }
        }
#if RELEASE
        using (var writer = new StreamWriter("release-pydigi.csv"))
#else
        using (var writer = new StreamWriter("debug-pydigi.csv"))
#endif
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteField<string>(nameof(PerformanceMeasurement.EnvironmentInitializationTime));
            csv.NextRecord();
            csv.WriteField<string>(pyDigiPerformance.EnvironmentInitializationTime.Seconds.ToString() + ":" + pyDigiPerformance.EnvironmentInitializationTime.Milliseconds.ToString());
            csv.NextRecord();
            csv.WriteField<string>(nameof(PerformanceMeasurement.CoordinatorCreationTime));
            csv.NextRecord();
            csv.WriteField<string>(pyDigiPerformance.CoordinatorCreationTime.Seconds.ToString() + ":" + pyDigiPerformance.CoordinatorCreationTime.Milliseconds.ToString());
            csv.NextRecord();
            csv.WriteField<string>(nameof(PerformanceMeasurement.GetCoordinatorAddressTime));
            csv.NextRecord();
            csv.WriteRecord<int>(pyDigiPerformance.GetCoordinatorAddressTime.Milliseconds);
            csv.NextRecord();
            csv.WriteField<string>(nameof(PerformanceMeasurement.DiscoveryTime));
            csv.NextRecord();
            csv.WriteField<string>(pyDigiPerformance.DiscoveryTime.Seconds.ToString() + ":" + pyDigiPerformance.DiscoveryTime.Milliseconds.ToString());
            csv.NextRecord();
            csv.WriteField<string>(nameof(PerformanceMeasurement.RoundTripTimes));
            csv.NextRecord();
            foreach (var item in pyDigiPerformance.RoundTripTimes)
            {
                csv.WriteRecord<int>(item.Milliseconds);
                csv.NextRecord();
            }
        }
    }

    public static PerformanceMeasurement Digi()
    {
        Console.WriteLine($"{nameof(Digi)} performance test.");

        var pm = new PerformanceMeasurement();
        var dt = DateTime.Now;
        pm.EnvironmentInitializationTime = TimeSpan.Zero;
        Console.WriteLine($"Environment initialization time:{pm.EnvironmentInitializationTime}");

        var coordinator = new DigiZigBeeUSBCoordinator(new NecBlik.Digi.Factories.DigiZigBeeFactory(),
            new DigiZigBeeUSBCoordinator.DigiUSBConnectionData() { baud = 9600, port = Port });
        pm.CoordinatorCreationTime = DateTime.Now - dt;
        Console.WriteLine($"Coordinator initialization time:{pm.CoordinatorCreationTime}");

        dt = DateTime.Now;
        coordinator.GetAddress();
        pm.GetCoordinatorAddressTime = DateTime.Now - dt;
        Console.WriteLine($"Coordinator address getter time:{pm.GetCoordinatorAddressTime}");

        dt = DateTime.Now;
        coordinator.Discover().Wait();
        pm.DiscoveryTime = DateTime.Now - dt;
        Console.WriteLine($"Discovery time:{pm.DiscoveryTime}");

        bool gotMessage = false;
        coordinator.SubscribeToDataRecieved(new DataRecievedListener(
            (s) => { gotMessage = true; })
            );
        for (int i = 0; i < iterations; i++)
        {
            dt = DateTime.Now;
            coordinator.Send("GetValue", "0013A20040A739ED");
#if RELEASE
            Task.Delay(200).Wait();
#endif
            while (!gotMessage) { }
            pm.RoundTripTimes.Add(DateTime.Now - dt);
            gotMessage = false;
            Console.WriteLine($"Round trip time:{pm.RoundTripTimes.Last()} Test number: {i}");
        }
        Console.WriteLine($"Mean round trip time:{pm.RoundTripTimes.Average((x) => { return x.Milliseconds; })}");

        coordinator.Dispose();
        return pm;
    }

    public static PerformanceMeasurement PyDigi()
    {
        Console.WriteLine($"{nameof(PyDigi)} performance test.");
        var pm = new PerformanceMeasurement();
        var dt = DateTime.Now;
        ZigBeePyEnv.Initialize();
        pm.EnvironmentInitializationTime = DateTime.Now - dt;
        Console.WriteLine($"Environment initialization time:{pm.EnvironmentInitializationTime}");

        var coordinator = new PyDigiZigBeeUSBCoordinator(new NecBlik.PyDigi.Factories.PyDigiZigBeeFactory(),
            new PyDigiZigBeeUSBCoordinator.PyDigiUSBConnectionData() { baud = 9600, port = Port });
        pm.CoordinatorCreationTime = DateTime.Now - dt;
        Console.WriteLine($"Coordinator initialization time:{pm.CoordinatorCreationTime}");

        dt = DateTime.Now;
        coordinator.GetAddress();
        pm.GetCoordinatorAddressTime = DateTime.Now - dt;
        Console.WriteLine($"Coordinator address getter time:{pm.GetCoordinatorAddressTime}");

        dt = DateTime.Now;
        Task.Run(async () =>
        {
            await coordinator.Discover();
        }).Wait();
        pm.DiscoveryTime = DateTime.Now - dt;
        Console.WriteLine($"Discovery time:{pm.DiscoveryTime}");

        bool gotMessage = false;
        coordinator.SubscribeToDataRecieved(new DataRecievedListener((s) => { gotMessage = true; }));
        for (int i = 0; i < iterations; i++)
        {
            dt = DateTime.Now;
            coordinator.Send("GetValue", "0013A20040A739ED");
#if RELEASE
            Task.Delay(200).Wait();
#endif
            while (!gotMessage) { }
            pm.RoundTripTimes.Add(DateTime.Now - dt);
            gotMessage = false;
            Console.WriteLine($"Round trip time:{pm.RoundTripTimes.Last()} Test number: {i}");
        }
        Console.WriteLine($"Mean round trip time:{pm.RoundTripTimes.Average((x) => { return x.Milliseconds; })}");

        coordinator.Dispose();
        return pm;
    }
}