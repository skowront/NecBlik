using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Common.WpfExtensions.Interfaces
{
    public interface IDuplicable<T>
    {
        T Duplicate();
    }
}
