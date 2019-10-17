using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{

   
    public  class VarianteDto
    {

        public double Anzahlung
        {
            get;
            set;
        }

        public double Laufzeit
        {
            get;
            set;
        }

        public double Restwert
        {
            get;
            set;

        }

        public double Buchwert
        {
            get;
            set;
        }


        public double TR_Risk
        {
            get;
            set;
        }

        public double Rate
        {
            get;
            set;
        }
        

        public ELOutDto eLOutDto
        {
            get;
            set;
        }


    }


    public class Ursprungskalkulation : VarianteDto
    {
    }

    public class Variante1 : VarianteDto
    {
    }

    public class Variante2 : VarianteDto
    {
    }

    public class Freivariante : VarianteDto
    {
    }


    public class KOMBI0ANZ : VarianteDto
    {
    }

    public class KOMBI0 : VarianteDto
    {
    }

    public class KOMBI1 : VarianteDto
    {
    }


        /// <summary>
        /// Parammeter für Varianten Transaction Risiko Prüfung
        /// </summary>
        public class VariantenParam
        {
            //Höhe der minimalen Anzahlung 
            public double ANZ { get; set; }

            // isdefaultanz
            public bool ISDEFAULTANZ { get; set; }

            //Laufzeitgrenzen für die Verwendung von 3 (ALZ1) oder 6 (ALZ2) Monaten
            public int LZ { get; set; }

            // isdefaultlz
            public bool ISDEFAULTLZ { get; set; }

            //Reduktion der Laufzeit bei kleiner LZ 
            public int ALZ1 { get; set; }

            // isdefaultALZ1
            public bool ISDEFAULTALZ1 { get; set; }

            //Reduktion der Laufzeit bei größer/gleich LZ
            public int ALZ2 { get; set; }

            // isdefaultALZ2
            public bool ISDEFAULTALZ2 { get; set; }

            // 1 = es müssen beiden Prüfungen OK sein /
            // 0 = es muss nur eine Prüfung OK sein
            // 1 = die Kalkulation wird mit reduzierter Laufzeit und Anzahlung von 2000 (wann < 2000) durchgeführt (Tabelle 13 Nummer 1 und 2).
            // 0 = es wird zuerst mit Anzahlung 2000 (wann < 2000) gerechnet. Anschließend mit Laufzeitreduktion kalkuliert (Tabelle 13 Nummer 3 und 4)
            public int KOMBI { get; set; }

            // isdefaultKOMBI
            public bool ISDEFAULTKOMBI{ get; set; }

        

        }


    }

