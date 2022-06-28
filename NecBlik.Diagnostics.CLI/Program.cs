using NecBlik.Digi.Factories;
using NecBlik.Digi.Models;

public class Program
{
    /// <summary>
    /// Mostly for testing purposes
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        string port = string.Empty;
        int baud = 115200;
        string input = string.Empty;
        while(input!="exit")
        {
            Console.Clear();
            Console.WriteLine(
                $"Port: {port}\n" +
                $"Baud: {baud}");
            Console.WriteLine(
                "Write port to change port.\n" +
                "Write baud to change baud.\n" +
                "Write exit to quit.\n" +
                "Write run to run test.\n");
            input = Console.ReadLine()??string.Empty;
            if(input == "exit")
            {
                break;
            }
            else if(input == "run")
            {
                Task.Run(async () =>
                {
                    await TestThroughput(port, baud, "0013A20040A739ED", 10, 200);
                }).Wait();
                Console.WriteLine("Press enter to continue.");
                Console.ReadLine();
            }
            else if (input == "port")
            {
                Console.WriteLine("Enter new port name:");
                var s = Console.ReadLine() ?? string.Empty;
                if (s == string.Empty)
                    continue;
                else
                    port = s;
            }
            else if (input == "baud")
            {
                Console.WriteLine("Enter new baud rate:");
                var s = Console.ReadLine() ?? string.Empty;
                if (s == string.Empty)
                    continue;
                else
                    if (!int.TryParse(s, out baud))
                        Console.WriteLine("Bad value.");
            }
        }
    }

    public static async Task TestThroughput(string port, int baud, string destinationAddress, int testingTime = 1, int packetLength = 15, string outputDirectpry = "")
    {
        DateTime TestTime = DateTime.Now;

        const int packetCount = 1000000;
        DigiZigBeeUSBCoordinator coordinator = new(new DigiZigBeeFactory(), new DigiZigBeeUSBCoordinator.DigiUSBConnectionData() { baud = baud, port = port});
        coordinator.PacketLogger = new PacketLogger("TestNoAck", TestTime, "NO_ACK");
        coordinator.Open();
        Console.WriteLine($"Device discovering.");
        var devices = await coordinator.GetDevices();
        Console.WriteLine($"Discovered {devices.Count()} devices.");

        //non-static
        Console.WriteLine($"Preparing test.");
        List<string> toSend = new List<string>();
        for(int i = 0;i<packetCount;i++)
        {
            toSend.Add(new CSharpDigiTestPayload(i,packetLength).GetOutput());
        }

        var endTime = DateTime.Now.AddSeconds(testingTime);
        int iterator = 0;
        while(endTime> DateTime.Now)
        {
            await coordinator.PingPacket(0, toSend[iterator], destinationAddress, false);
            iterator++;
            if (iterator >= toSend.Count)
                break;
        }

        var dir = outputDirectpry == string.Empty? Directory.GetCurrentDirectory():outputDirectpry;
        if (!Directory.Exists(dir + "/tests"))
            Directory.CreateDirectory(dir + "/tests");
        if (Directory.Exists(dir + "/tests"))
            coordinator.PacketLogger.Save(dir + "/tests");

        
        Console.WriteLine($"Sent {iterator} packets of size {toSend[0].Length} without waiting for confirmation in {testingTime} seconds.");
        
        Console.WriteLine("Sleeping for 5 seconds.");
        Thread.Sleep(1000);
        Console.WriteLine("Sleeping for 4 seconds.");
        Thread.Sleep(1000);
        Console.WriteLine("Sleeping for 3 seconds.");
        Thread.Sleep(1000);
        Console.WriteLine("Sleeping for 2 seconds.");
        Thread.Sleep(1000);
        Console.WriteLine("Sleeping for 1 second.");
        Thread.Sleep(1000);

        coordinator.PacketLogger = new PacketLogger("TestAck", TestTime, "ACK");

        endTime = DateTime.Now.AddSeconds(testingTime);
        iterator = 0;
        while (endTime > DateTime.Now)
        {
            await coordinator.PingPacket(0, toSend[iterator], destinationAddress, true);
            iterator++;
            if (iterator >= toSend.Count)
                break;
        }

        dir = Directory.GetCurrentDirectory();
        if (!Directory.Exists(dir + "/tests"))
            Directory.CreateDirectory(dir + "/tests");
        if (Directory.Exists(dir + "/tests"))
            coordinator.PacketLogger.Save(dir + "/tests");

        Console.WriteLine($"Sent {iterator} packets of size {toSend[0].Length} waiting for confirmation in {testingTime} seconds.");

        coordinator.Dispose();
    }
}
