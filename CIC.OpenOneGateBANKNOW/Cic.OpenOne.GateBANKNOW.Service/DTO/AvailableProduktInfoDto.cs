using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Contains a set of produkt and its parameters
    /// </summary>
    public class AvailableProduktInfoDto
    {
        public AvailableProduktDto produkt { get;set;}
        public ParamDto[] parameters { get;set;}
    }
}