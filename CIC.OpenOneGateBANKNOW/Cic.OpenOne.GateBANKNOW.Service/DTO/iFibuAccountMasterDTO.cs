using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Transferobjekt UploadAccountMasterdata Methode
    /// </summary>
    public class iFibuAccountMasterDTO
    {
        public string externalID { get; set; } //externe Referenzid (Belegnummer)
        public string cicrefID { get; set; }   //EAIJOB:SYSEAIJOB 
        public string vkz { get; set; } // Verarbeitungskennzeichen == Status
        public string SumBetrag { get; set; }
        public string SumKonto { get; set; }

        public string Typ { get; set; } // Belegtyp (Rechnung, Buchung, Auftrag)
        public string ZinsRelevanzFlag { get; set; }

        //public string NebenkontenFlag { get; set; } // wenn 1 dann sind des Nebenkonten wenn 0 dann sind es Sachkonten
        //public string Waehrung { get; set; } // CHF, EUR etc. 
        //public string Kurs { get; set; } // z.B: 3.33456
        //public string SysIFBookTyp { get; set; } // Referenziert auf die Tabelle IFBOOKTYP und definiert den Typen des IFBOOK Satzes

        public List<iFibuAccountDTO> fibuAccountList { get; set; }
    }
}