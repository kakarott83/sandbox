using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.Common.DTO
{
    public class icreateOrUpdateAppSettingsItemsDto
    {
        public RegVarDto[] regVars { set; get; }
        public long sysWfuser { set; get; }

    }
}