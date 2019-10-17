using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;

namespace Cic.OpenOne.GateBANKNOW.TestUI
{
    /// <summary>
    /// Listen Data Provider
    /// </summary>
    public static class ListenDataProvider
    {
        /// <summary>
        /// Get Wohnverhältnis
        /// </summary>
        /// <returns>Wohnverhältnis</returns>
        public static Dictionary<double, string> GetWohnverhaeltnis()
        {
            Dictionary<Double, String> Wohnverhaeltnis = new Dictionary<double, String>();
            //0= alleinstehend; 1= in Wohn-/Lebensgemeinschaft lebend ; 2= Ehepaar bzw. eingetragene Partnerschaft; 3= alleinerziehend ohne Haushaltgemeinschaft; 4= alleinerziehend mit Haushaltgemeinschaft

            Wohnverhaeltnis.Add(0, "alleinstehend");
            Wohnverhaeltnis.Add(1, "in Wohn-/Lebensgemeinschaft lebend");
            Wohnverhaeltnis.Add(2, "Ehepaar bzw. eingetragene Partnerschaft");
            Wohnverhaeltnis.Add(3, "alleinerziehend ohne Haushaltgemeinschaf");
            Wohnverhaeltnis.Add(4, "alleinerziehend mit Haushaltgemeinschaft");

            return Wohnverhaeltnis;
        }

        /// <summary>
        /// Get Kanton ID
        /// </summary>
        /// <returns>Canton ID</returns>
        public static Dictionary<double, string> GetKantonen()
        {
            Dictionary<double, string> Kantonen = new Dictionary<double, string>();

            Kantonen.Add(0, "AG"); 
            Kantonen.Add(1 ,"AR"); 
            Kantonen.Add(2 ,"AI"); 
            Kantonen.Add(3 ,"BS"); 
            Kantonen.Add(4 ,"BL"); 
            Kantonen.Add(5 ,"BE"); 
            Kantonen.Add(6 ,"FR"); 
            Kantonen.Add(7 ,"GE"); 
            Kantonen.Add(8 ,"GL"); 
            Kantonen.Add(9 ,"GR"); 
            Kantonen.Add(10 ,"JU"); 
            Kantonen.Add(11 ,"LU"); 
            Kantonen.Add(12 ,"NE"); 
            Kantonen.Add(13 ,"NW"); 
            Kantonen.Add(14 ,"OW"); 
            Kantonen.Add(15 ,"SH"); 
            Kantonen.Add(16 ,"SZ"); 
            Kantonen.Add(17 ,"SO"); 
            Kantonen.Add(18 ,"SG"); 
            Kantonen.Add(19 ,"TG"); 
            Kantonen.Add(20 ,"TI");
            Kantonen.Add(21, "UR");
            Kantonen.Add(22, "VS");
            Kantonen.Add(23, "VD");
            Kantonen.Add(24, "ZG");
            Kantonen.Add(25, "ZH");
            Kantonen.Add(26, "FL");

            return Kantonen;
      
        }

        /// <summary>
        /// Get Zivilstand
        /// </summary>
        /// <returns>Data</returns>
        public static Dictionary<double, String> GetZivilstand()
        {
            Dictionary<double, String> Zivilstand = new Dictionary<double, String>();

            Zivilstand.Add(0, "ledig");
            Zivilstand.Add(1, "verheiratet");
            Zivilstand.Add(2, "geschieden");
            Zivilstand.Add(3, "gerichtlich getrennt");
            Zivilstand.Add(4, "verwitwet");
            Zivilstand.Add(5, "eingetragene Partnerschaft");
            Zivilstand.Add(6, "gerichtlich aufgelöste Partnerschaft");

            return Zivilstand;
        }

  
        /// <summary>
        /// Get Anrede
        /// </summary>
        /// <returns>Data</returns>
        public static Dictionary<double, String> GetAnrede()
        {
            Dictionary<double, String> Anrede= new Dictionary<double, String>();

            Anrede.Add(0, "Herr");
            Anrede.Add(1, "Frau");
     
            return Anrede;
        }

        /// <summary>
        /// Get Kalkcode
        /// </summary>
        /// <returns>Data</returns>
        public static Dictionary<double, String> GetKalkcode()
        {
            //0=Kredit (fix), 1=Leasing, 2=Teilzahler
            Dictionary<double, String> Kalkcode = new Dictionary<double, String>();

            Kalkcode.Add(0, "Kredit (fix)");
            Kalkcode.Add(1, "Leasing");
            Kalkcode.Add(2, "Teilzahler");

            return Kalkcode;
        }

        /// <summary>
        /// Get Zinsart
        /// </summary>
        /// <returns>Data</returns>
        public static Dictionary<double, String> GetZinsart()
        {
            Dictionary<double, String> Zinsart = new Dictionary<double, String>();

            Zinsart.Add(0, "nominal(fix)");
            Zinsart.Add(1, "effectiv");

            return Zinsart;
        }

        /// <summary>
        /// Get Quellensteuer
        /// </summary>
        /// <returns></returns>
        public static Dictionary<double, String> GetQuellensteuer()
        {
            Dictionary<double, String> Quellensteuer = new Dictionary<double, String>();

            Quellensteuer.Add(0, "nein");
            Quellensteuer.Add(1, "ja");

            return Quellensteuer;
        }

        /// <summary>
        /// Get Prüfungstyp Decision Engine
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, String> GetPruefungenTypeDecisionEngine()
        {

            Dictionary<int, String> PruefungenType = new Dictionary<int, String>();

            PruefungenType.Add(0, "Vorpruefung");
            PruefungenType.Add(1, "Bonitaetspruefung");
            PruefungenType.Add(1, "Risikopruefung");

            return PruefungenType;

        }
    }
}
  