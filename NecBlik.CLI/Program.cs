using NecBlik.Core.Interfaces;
using NecBlik.PyDigi.Models;
using NecBlik.Digi.Models;

public class Program
{
    /// <summary>
    /// Mostly for testing purposes
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        Digi();

    }

    public static void Digi()
    {
        var conData = new DigiZigBeeUSBCoordinator.DigiUSBConnectionData() { baud = 9600, port = "COM4" };
        var coordinator = new DigiZigBeeUSBCoordinator(new NecBlik.Digi.Factories.DigiZigBeeFactory(), conData);
        coordinator.Open();
        coordinator.Dispose();

        coordinator.Open();

        conData = new DigiZigBeeUSBCoordinator.DigiUSBConnectionData() { baud = 9600, port = "COM4" };
        coordinator = new DigiZigBeeUSBCoordinator(new NecBlik.Digi.Factories.DigiZigBeeFactory(), conData);

        coordinator.Open();
    }

    public static void PyDigi()
    {
        Task.Run(async () =>
        {
            ZigBeePyEnv.Initialize();

            var conData = new PyDigiZigBeeUSBCoordinator.PyDigiUSBConnectionData() { baud = 9600, port = "COM4" };
            var coordinator = new PyDigiZigBeeUSBCoordinator(new NecBlik.PyDigi.Factories.PyDigiZigBeeFactory(), conData);
            coordinator.Discover().Wait();
            coordinator.Send("GetValue", "0013A20040A739ED");
            //coordinator.Discover().Wait();


        });
        while (true)
        {
            //Console.WriteLine("--");
        };
    }
}