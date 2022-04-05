using NecBlik.Core.Interfaces;
using NecBlik.PyDigi.Models;

public class Program
{
    /// <summary>
    /// Mostly for testing purposes
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
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