using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Core.Interfaces
{
    public interface IVendable
    {
        /// <summary>
        /// Global identifier of internal type of the object.
        /// </summary>
        /// <returns>Returns an identifier of a class that instantiated this object.</returns>
        /// 
        string GetVendorID();

        /// <summary>
        /// Indicates if any producer may try to build this object.
        /// </summary>
        /// <returns>Returns if object may be instantiated only by particular licensees. </returns>
        bool IsLicensed();

        /// <summary>
        /// Licensees are a list of VendorIDs allowed to instantiate this class.
        /// </summary>
        /// <returns>Whitelist licensees.</returns>
        IEnumerable<string> GetLicensees();
    }
}
