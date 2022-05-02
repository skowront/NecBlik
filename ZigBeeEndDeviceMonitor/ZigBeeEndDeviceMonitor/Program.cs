using SPMonitor;
using System.IO.Ports;

public static class Program
{
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

        SerialPortMonitor monitor = new SerialPortMonitor(port, baud);
        monitor.OnDataRecieved = new Action<string>((s) =>
        {
            Console.Write(s);
        });
        monitor.Open();

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}