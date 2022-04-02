using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Core.Interfaces
{
    public interface ISubscriber<T>: ICachable
    {
        public void NotifySubscriber(T updateInformation);
    }
}
