using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Performance.CLI
{
    public class DataRecievedListener:ISubscriber<RecievedData>
    {
        public Guid CacheId = Guid.NewGuid();

        public Action<string> OnDataRecieved = new Action<string>((s) => { });
        
        public DataRecievedListener(Action<string> onDataRecieved)
        {
            this.OnDataRecieved = onDataRecieved;
        }

        public string GetCacheId()
        {
            return CacheId.ToString();
        }

        public void NotifySubscriber(RecievedData updateInformation)
        {
            OnDataRecieved?.Invoke(updateInformation.Data);
        }
    }
}
