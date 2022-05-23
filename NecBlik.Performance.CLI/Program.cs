using NecBlik.Core.Interfaces;
using NecBlik.PyDigi.Models;
using NecBlik.Digi.Models;
using NecBlik.Performance.CLI;
using Python.Runtime;

public class Program
{
    /// <summary>
    /// Mostly for testing purposes
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var digiPerformance = Digi();
        var pyDigiPerformance = PyDigi();

        var x = 10;
    }

    public static PerformanceMeasurement Digi()
    {
        Console.WriteLine($"{nameof(Digi)} performance test.");

        var pm = new PerformanceMeasurement();
        var dt = DateTime.Now;
        pm.EnvironmentInitializationTime = TimeSpan.Zero;
        Console.WriteLine($"Environment initialization time:{pm.EnvironmentInitializationTime}");

        var coordinator = new DigiZigBeeUSBCoordinator(new NecBlik.Digi.Factories.DigiZigBeeFactory(),
            new DigiZigBeeUSBCoordinator.DigiUSBConnectionData() { baud = 9600, port = "COM4" });
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
        coordinator.SubscribeToDataRecieved(new DataRecievedListener((s) => { gotMessage = true; }));
        for (int i = 0; i < 50; i++)
        {
            dt = DateTime.Now;
            coordinator.Send("GetValue", "0013A20040A739ED");
            while (!gotMessage) { }
            gotMessage = false;
            pm.RoundTripTimes.Add(DateTime.Now - dt);
            Console.WriteLine($"Round trip time:{pm.RoundTripTimes.Last()} Test number: {i}");
        }

        coordinator.Dispose();
        return pm;
    }

    public static PerformanceMeasurement PyDigi()
    {
        Console.WriteLine($"{nameof(PyDigi)} performance test.");
        var pm = new PerformanceMeasurement();
        var dt = DateTime.Now;
        ZigBeePyEnv.Initialize();
        Console.WriteLine($"Environment initialization time:{pm.EnvironmentInitializationTime}");

        var coordinator = new PyDigiZigBeeUSBCoordinator(new NecBlik.PyDigi.Factories.PyDigiZigBeeFactory(),
            new PyDigiZigBeeUSBCoordinator.PyDigiUSBConnectionData() { baud = 9600, port = "COM4" });
        pm.CoordinatorCreationTime = DateTime.Now - dt;
        Console.WriteLine($"Coordinator initialization time:{pm.CoordinatorCreationTime}");

        dt = DateTime.Now;
        coordinator.GetAddress();
        pm.GetCoordinatorAddressTime = DateTime.Now - dt;
        Console.WriteLine($"Coordinator address getter time:{pm.GetCoordinatorAddressTime}");

        //dt = DateTime.Now;
        //Thread thread = new Thread(() => {
        //    Task.Run(async () =>
        //    {
        //            await coordinator.Discover();
        //    }).Wait();
        //});
        //thread.SetApartmentState(ApartmentState.MTA);
        //thread.Start();
        //thread.Join();

        //pm.DiscoveryTime = DateTime.Now - dt;
        //Console.WriteLine($"Discovery time:{pm.DiscoveryTime}");

        bool gotMessage = false;
        coordinator.SubscribeToDataRecieved(new DataRecievedListener((s) => { gotMessage = true; }));
        for (int i = 0; i < 50; i++)
        {
            dt = DateTime.Now;
            coordinator.Send("GetValue", "0013A20040A739ED");
            while (!gotMessage) { }
            gotMessage = false;
            pm.RoundTripTimes.Add(DateTime.Now - dt);
            Console.WriteLine($"Round trip time:{pm.RoundTripTimes.Last()} Test number: {i}");
        }

        coordinator.Dispose();
        return pm;
    }
}