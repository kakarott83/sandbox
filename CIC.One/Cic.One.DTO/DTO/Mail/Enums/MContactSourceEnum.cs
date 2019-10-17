using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    // Summary:
    //     Defines the source of a contact or group.
    public enum MContactSourceEnum
    {
        // Summary:
        //     The contact or group is stored in the Global Address List
        ActiveDirectory = 0,
        //
        // Summary:
        //     The contact or group is stored in Exchange.
        Store = 1,
    }
}
