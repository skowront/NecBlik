using System.IO.Ports;
using System.Text;

namespace SPMonitor
{
    public class SerialPortMonitor:IDisposable
    {
        private SerialPort serialPort;

        public Action<string> OnDataRecieved = null;

        public SerialPortMonitor(string port, int baud)
        {
            this.serialPort = new SerialPort(port, baud);
            this.serialPort.DtrEnable = true;
            this.serialPort.DataReceived += SerialPort_DataReceived;
            this.serialPort.Open();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[this.serialPort.BytesToRead];
            this.serialPort.Read(data, 0, this.serialPort.BytesToRead);
            string asciiString = Encoding.ASCII.GetString(data, 0, data.Length);
            this.OnDataRecieved?.Invoke(asciiString);
        }

        public bool Open()
        {
            if (this.serialPort.IsOpen)
            {
                return true;
            }
            else
            {
                this.serialPort.Open();
                if (this.serialPort.IsOpen)
                    return true;
                else
                    return false;
            }
        }

        public void Close()
        {
            if(this.serialPort.IsOpen)
                this.serialPort.Close();
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}