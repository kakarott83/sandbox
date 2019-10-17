using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Input for fetching a list by code
    /// </summary>
    public class igetListItems
    {
        public String code { get; set; }
        public String domainid { get; set; }
    }
}