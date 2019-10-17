using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public enum MSendCancellationModeEnum
    {
        SendToNone = 0,
        SendOnlyToAll = 1,
        SendToAllAndSaveCopy = 2,
    }
}