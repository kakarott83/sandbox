using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Bezeichnungen für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchAntragService.getAntragDetail"/> Methode
    /// </summary>
    public class AntragDescriptionDto
    {
        /// <summary>
        /// PrProduct
        /// </summary>
        public String PrProduct { get; set; }

        /// <summary>
        /// Bankkunde
        /// </summary>
        public String BankKunde { get; set; }

        /// <summary>
        /// KundeTyp (syskdtyp)
        /// </summary>
        public String KundeTyp { get; set; }

        /// <summary>
        /// Kanal (FF, KF)
        /// </summary>
        public String PrChannel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String PrhGroup { get; set; }

        /// <summary>
        /// Sprache (sysctlang)
        /// </summary>
        public String Lang { get; set; }

        /// <summary>
        /// Korrespondenz-Sprache (sysctlangkorr)
        /// </summary>
        public String CtLangKorr { get; set; }

        /// <summary>
        /// Land Nationalität (syslandnat)
        /// </summary>
        public String LandNat { get; set; }

        /// <summary>
        /// Land (sysland)
        /// </summary>
        public String Land { get; set; }

        /// <summary>
        /// Land 2
        /// </summary>
        public String Land2 { get; set; }

        /// <summary>
        /// Staat
        /// </summary>
        public String Staat { get; set; }

        /// <summary>
        /// Staat 2
        /// </summary>
        public String Staat2 { get; set; }

        /// <summary>
        /// Branche
        /// </summary>
        public String Branche { get; set; }

        /// <summary>
        /// Kontoführende Bank 
        /// </summary>
        public String[] BLZ { get; set; }

        /// <summary>
        /// Objekt
        /// </summary>
        public String Objekt { get; set; }

        /// <summary>
        /// Objekt-Art
        /// </summary>
        public String ObjektArt { get; set; }

        /// <summary>
        /// Objekt-Typ
        /// </summary>
        public String ObjektTyp { get; set; }

        /// <summary>
        /// Nutzungsart (privat, geschäftlich, demo)
        /// </summary>
        public String ObUseType { get; set; }

        /// <summary>
        /// Währung 
        /// </summary>
        public String Waehrung { get; set; }

        /// <summary>
        /// Versicherungstyp 
        /// </summary>
        public String[] VersicherungsTyp { get; set; }

        /// <summary>
        /// Versicherer 
        /// </summary>
        public String Versicherer { get; set; }

        /// <summary>
        /// Provisionstyp (Umsatz, Zins …)
        /// </summary>
        public String PrProvType { get; set; }

        /// <summary>
        /// Provisionsempfänger (Händler, nicht Verkäufer)
        /// </summary>
        public String[] Provisionsempfaenger { get; set; }

        /// <summary>
        /// Ablösetyp (Eigen, Fremd) 
        /// </summary>
        public String[] AblTyp { get; set; }

        /// <summary>
        /// Abzulösender Vertrag im Eigenbestand 
        /// </summary>
        public String[] VorVT { get; set; }

        /// <summary>
        /// Subventionstyp 
        /// </summary>
        public String[] SubvTyp { get; set; }

        /// <summary>
        /// Subventionsgeber (Händler bei Differenzleasing) 
        /// </summary>
        public String[] SubvGeber { get; set; }

        /// <summary>
        /// Brand
        /// </summary>
        public String Brand { get; set; }

        /// <summary>
        /// Marketingaktion (Kampagnencode)
        /// </summary>
        public String MarktAb { get; set; }

        /// <summary>
        /// Erfasser
        /// </summary>
        public String WfUser { get; set; }

        /// <summary>
        /// Änderer 
        /// </summary>
        public String WfUserChange { get; set; }

        /// <summary>
        /// Betreuer (Antragsowner) 
        /// </summary>
        public String Berater { get; set; }

        /// <summary>
        /// Korrespondenzadresse 
        /// </summary>
        public String KorrAdresse { get; set; }

        /// <summary>
        /// It Konto 
        /// </summary>
        public String ItKonto { get; set; }

        /// <summary>
        /// Kunde.Adressen.Land 
        /// </summary>
        public String[] AdressenLand { get; set; }

        /// <summary>
        /// Kunde.Adressen.Staat 
        /// </summary>
        public String[] AdressenStaat { get; set; }

        /// <summary>
        /// Kunde.Adressen.CtLang 
        /// </summary>
        public String[] AdressenCtLang { get; set; }
    }
}
