using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBeeNet;

namespace NecBlik.ZigBeeNet.USB
{
    public class ConsoleCommandListener : IZigBeeCommandListener
    {
        public void CommandReceived(ZigBeeCommand command)
        {
            Console.WriteLine(command);
        }
    }

}
