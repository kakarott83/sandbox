using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    public class FoodasBeauftragungInDto
    {
        public String KundenRef {get;set;}
        public String FahrzeugIdent {get;set;}

        public String Anschreiben_typ {get;set;}
        public String Anschreiben_Anrede {get;set;}
        public String Versandart {get;set;}
        public String Empfaenger_Ansprechpartner {get;set;}
        public String Empfaenger_Name {get;set;}
        public String Empfaenger_Name2 {get;set;}
        public String Empfaenger_Strasse {get;set;}
        public String Empfaenger_Plz {get;set;}
        public String Empfaenger_Ort {get;set;}

        public String Empfaenger_Land {get;set;}
        public String Bemerkung_1 {get;set;}
        public String Bemerkung_2 {get;set;}
        public String Bemerkung_3 {get;set;}
        public String Bemerkung_4 {get;set;}
        public String Bemerkung_5 {get;set;}
        public String Bemerkung_6 {get;set;}
        public String Anschreiben_Bemerkung { get; set; }
       
    }
}
