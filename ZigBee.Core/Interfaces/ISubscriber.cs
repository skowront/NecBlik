﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Core.Interfaces
{
    public interface ISubscriber<T>
    {
        public void NotifySubscriber(T updateInformation);
    }
}