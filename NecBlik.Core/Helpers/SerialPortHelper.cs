using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NecBlik.Core.Helpers
{
    //Author: http://blog.gorski.pm/serial-port-details-in-c-sharp
    public class SerialPortHelper
    {
        public struct ComPort // custom struct with our desired values
        {
            public string name;
            public string vid;
            public string pid;
            public string description;
        }

        private const string vidPattern = @"VID_([0-9A-F]{4})";
        private const string pidPattern = @"PID_([0-9A-F]{4})";
        private const string namePattern = @"\bCOM\d+\b";
        public static List<ComPort> GetSerialPorts()
        {
            using (var searcher = new ManagementObjectSearcher
                ("SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%(COM%'"))
            {
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                return ports.Select(p =>
                {
                    ComPort c = new ComPort();
                    c.name = p.GetPropertyValue("DeviceID").ToString();
                    c.vid = p.GetPropertyValue("PNPDeviceID").ToString();
                    c.description = p.GetPropertyValue("Caption").ToString();

                    Match mVID = Regex.Match(c.vid, vidPattern, RegexOptions.IgnoreCase);
                    Match mPID = Regex.Match(c.vid, pidPattern, RegexOptions.IgnoreCase);
                    Match mDesc = Regex.Match(c.description, namePattern, RegexOptions.IgnoreCase);

                    if (mVID.Success)
                        c.vid = mVID.Groups[1].Value;
                    if (mPID.Success)
                        c.pid = mPID.Groups[1].Value;
                    if (mDesc.Success)
                        c.name = mDesc.Groups[0].Value;

                    return c;

                }).ToList();
            }
        }
    }
}
