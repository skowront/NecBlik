using NecBlik.Digi.Models;
using SPMonitor;
using System.IO.Ports;

public static class Program
{
    public static bool HoldConsoleLogger = false;

    [STAThread]
    public static void Main(string[] args)
    {
        string port = string.Empty;
        int baud = 0;
        if (args.Length < 2)
            return;
        else
        {
            port = args[0];
            int.TryParse(args[1], out baud);
        }
        PacketLogger packetLogger = new PacketLogger(string.Empty,DateTime.Now,string.Empty);

        SerialPortMonitor monitor = new SerialPortMonitor(port, baud);
        monitor.OnDataRecieved = new Action<string>((s) =>
        {
            LogTestData(s, DateTime.Now, packetLogger);
        });
        monitor.Open();

        var logsLocation = Directory.GetCurrentDirectory();
        if (!Directory.Exists(logsLocation + "/tests"))
            Directory.CreateDirectory(logsLocation + "/tests");

        Console.WriteLine("Press 'h' for help.");
        while (true)
        {
            HoldConsoleLogger = false;
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                if(key.Key == ConsoleKey.H)
                {
                    HoldConsoleLogger = true;
                    bool endConfiguration = false;
                    while(!endConfiguration)
                    {
                        Console.Clear();
                        Console.WriteLine(packetLogger.Info());
                        Console.WriteLine(
                            "For packet logger name change press 'n'\n" +
                            "For packet logger timestamp now press 't'\n" +
                            "For packet logger comment change press 'c'\n" +
                            "To create new logger press 'r'\n" +
                            "To save logger entries press 's'\n" +
                            "For logs location change press 'l'\n" +
                            "To return to packet reading press 'x'");
                        var cKey = Console.ReadKey();
                        switch (cKey.KeyChar)
                        {
                            case 'n':
                                Console.WriteLine("Enter new logger name:");
                                packetLogger.Name = Console.ReadLine()??string.Empty;
                                break;
                            case 't':
                                packetLogger.DateTime = DateTime.Now;
                                break;
                            case 'c':
                                Console.WriteLine("Enter new logger comment:");
                                packetLogger.Comment = Console.ReadLine() ?? string.Empty;
                                break;
                            case 'r':
                                packetLogger = new PacketLogger(string.Empty,DateTime.Now,string.Empty);
                                break;
                            case 's':
                                var savedLocation = packetLogger.Save(logsLocation + "/tests");
                                Console.WriteLine($"Logs saved to:\n {logsLocation + "/tests"}");
                                break;
                            case 'l':
                                Console.WriteLine($"Current logs are located in:{logsLocation + "/tests"}.\nLeave empty to cancel change.");
                                var loc = Console.ReadLine()??string.Empty;
                                if (loc == string.Empty)
                                    break;
                                if (!Directory.Exists(loc))
                                {
                                    Console.WriteLine("Location does not exist.");
                                    break;
                                }
                                else
                                {
                                    logsLocation = loc;
                                }
                                break;
                            case 'x':
                                Console.Clear();
                                endConfiguration = true;
                                break;
                            default:
                                endConfiguration = true;
                                break;
                        }
                    }
                }
            }
        }
    }

    public static void LogTestData(string data, DateTime timestamp, PacketLogger packetLogger)
    {
        if(!HoldConsoleLogger)
        {
            Console.Write(data);
        }
        var p = CSharpDigiTestPayload.FromString(data.Split('\n').Where((x) => { return x.Contains("C#TD:"); }).FirstOrDefault()?.Replace("\r","")??string.Empty);
        if(p!=null)
        {
            packetLogger.AddEntry(DateTime.Now,data,p.Id.ToString(),"IN");
        }
    }
}