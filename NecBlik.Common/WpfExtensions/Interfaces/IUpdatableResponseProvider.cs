﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Common.WpfExtensions.Interfaces
{
    public interface IUpdatableResponseProvider<U,R,Q>:IResponseProvider<R,Q> where Q:class
    {
        public void Init(U min, U max, U startValue);
        public void Update(U newValue);

        public void SealUpdates();

        public void SetLimit(U limitValue);
    }
}
