using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Digi.Models
{
    public class PacketLogger
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTime DateTime { get; set; }

        public List<PacketLoggerEntry> records = new List<PacketLoggerEntry>();

        public PacketLogger(string Name, DateTime dateTime, string Comment)
        {
            this.Name = Name;
            this.DateTime = dateTime;
            this.Comment = Comment;
        }

        public virtual string Save(string location)
        {
            var loc = location + "\\" + this.Name + "_" +
                $"{this.DateTime.Day}_{this.DateTime.Month}_{this.DateTime.Year}-{this.DateTime.Hour}_{this.DateTime.Minute}_{this.DateTime.Second}.csv";
            using (var writer = new StreamWriter(loc))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
            return loc;
        }
        public virtual void AddEntry(DateTime timestamp, string payload, string attribute, string kind)
        {
            var p = new PacketLoggerEntry() { Timestamp = timestamp, Payload = payload, Attribute = attribute, Kind = kind, Comment = this.Comment };
            this.records.Add(p);
        }

        public string Info()
        {
            return $"Name: {this.Name}\nTimeStamp: {this.DateTime}\nComment: {this.Comment}";
        }


        public class PacketLoggerEntry
        {
            public DateTime Timestamp { get; set; }
            public string Attribute { get; set; } = string.Empty;
            public string Payload { get; set; } = string.Empty;
            public string Kind { get; set; } = string.Empty;
            public string Comment { get; set; } = string.Empty;
        }

    }
}
