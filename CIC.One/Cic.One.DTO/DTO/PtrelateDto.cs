using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class PtrelateDto:EntityDto
    {
        /*Primärschlüssel */
        public long sysPtrelate { get; set; }
        /*Verweis zur Person */
        public long sysPerson1 { get; set; }
        /*Verweis zur Partner */
        public long sysPerson2 { get; set; }
        /*Sortierung in Anzeige */
        public int rank { get; set; }
        /*Code, verweis auf ddlkppos (Geschäftsführer, ... ) */
        public String funcCode { get; set; }
        /*Code, verweis auf ddlkppos (Ansprechpartner, Unterstützer, Verbündeter …) */
        public String typCode { get; set; }
        /*Beziehungsrichtung (aktuell nicht verwendet, ergibt sich aus person und partner) */
        public int relDirection { get; set; }
        /*Beziehungsbeginn */
        public DateTime? relBeginnDate { get; set; }
        /*Beziehungsende */
        public DateTime? relEndDate { get; set; }
        /*Zusatzinformation */
        public String additionalInfo { get; set; }
        /*Active Flag*/
        public int activeFlag { get; set; }


        override public long getEntityId()
        {
            return sysPtrelate;
        }


        //flag if relation to add or remove
        public int addToPerson { get; set; }
    }
}