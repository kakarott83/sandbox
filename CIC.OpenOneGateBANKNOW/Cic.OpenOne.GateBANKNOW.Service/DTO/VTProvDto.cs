using Cic.OpenOne.Common.DTO;
using System;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    public class VTProvDto
    {
        //Kanal
        /// <summary>
        /// Vertragsart
        /// </summary>
        public long sysvart { get; set; }
        //Brand
        ObjektDto Objektart { get; set; }
        ObTypDto Objekttyp { get; set; }
        //Nutzungsart
        AvailableProduktDto Produkt { get; set; }
        DateTime PerDatum { get; set; }
        VertragDto Vertrag { get; set; }
        //Verkäufer
    }
}