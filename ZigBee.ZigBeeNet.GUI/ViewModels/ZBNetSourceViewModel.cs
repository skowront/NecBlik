﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.ZigBeeNet.GUI.ViewModels
{
    public class ZBNetSourceViewModel: VirtualZigBeeViewModel
    {
        public ZBNetSourceViewModel(ZigBeeModel model = null) : base(model)
        {
            this.Model = model;
        }

        public override string GetCacheId()
        {
            return this.Model.CacheId;
        }
    }
}