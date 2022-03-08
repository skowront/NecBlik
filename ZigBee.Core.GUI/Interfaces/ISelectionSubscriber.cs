using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Core.GUI.Interfaces
{
    public interface ISelectionSubscriber<T>
    {
        public void NotifySelected(T obj);
    }
}
