using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Common.WpfExtensions.Interfaces
{
    public interface IUpdatableResponseProvider<U,R,Q>:IResponseProvider<R,Q> where Q:class
    {
        public void Update(U newValue);

        public void SealUpdates();

        public void SetLimit(U limitValue);
    }
}
