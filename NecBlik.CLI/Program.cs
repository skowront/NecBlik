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
            var devices = new List<IZigBeeSource>(await coordinator.GetDevices());
        }).Wait();

    }
}