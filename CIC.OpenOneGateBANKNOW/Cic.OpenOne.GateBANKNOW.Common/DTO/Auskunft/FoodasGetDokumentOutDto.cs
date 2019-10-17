using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Foodas get Dokument return structure
    /// </summary>
    public class FoodasGetDokumentOutDto:FoodasOutDto
    {
        public String Id {get;set;}
        public String Fahrzeug_id {get;set;}
        public String AKt_Briefnummer {get;set;}
        public String AKt_Kennzeichen {get;set;}
        public String FahrzeugIdent {get;set;}
        public String KundenRef {get;set;}
        public String Vertragsnummer {get;set;}
        public String Haendler_id {get;set;}
        public String Dokument_typ {get;set;}
        public String Akt_Bewegung_typ_id {get;set;}
        public String Psbarcode {get;set;}
        public byte[] data {get;set;}
    }
}
