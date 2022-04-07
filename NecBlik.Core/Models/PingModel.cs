using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Core.Enums
{
    public class PingModel
    {
        public double ResponseTime { get; set; }

        public PingResult Result { get; set; }

        public string Payload { get; set; }

        public string Message { get; set; }

        public PingModel(double responseTime, PingResult result, string payload = "")
        {
            this.ResponseTime = responseTime;
            this.Result = result;
            this.Payload = payload; 
        }

        public PingModel(double responseTime)
        {
            this.ResponseTime = responseTime;
        }

        public PingModel(PingResult result)
        {
            this.Result = result; 
        }

        public PingModel()
        { 
        }

        public enum PingResult
        {
            //When the packet returned without harm.
            Ok,
            //When the packet did not return at all.
            NotOk
        }
    }
}
