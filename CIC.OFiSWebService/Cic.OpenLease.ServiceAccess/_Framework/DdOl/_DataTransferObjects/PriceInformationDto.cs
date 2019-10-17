using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class PriceInformationDto
    {
        //Brutto unrabattiert inkl Nova inkl. Zuschlag
        public decimal GRUNDBRUTTO { get; set; }
        public decimal SONZUBBRUTTO { get; set; }
        public decimal PAKETEBRUTTO { get; set; }
        public decimal HERZUBBRUTTO { get; set; }
        public decimal ZUBEHOERBRUTTO { get; set; }      
        public decimal AHKBRUTTO { get; set; }

        //Brutto exkl. Nova
        public decimal GRUNDBRUTTOEXKLN { get; set; }
        public decimal SONZUBBRUTTOEXKLN { get; set; }
        public decimal PAKETEBRUTTOEXKLN { get; set; }

        //Netto inkl. Nova inkl. Zuschlag
        public decimal GRUND { get; set; }
        public decimal SONZUB { get; set; }
        public decimal PAKETE { get; set; }
        

        //Novazuschlag
        public decimal GRUNDNOVAZU { get; set; }
        public decimal SONZUBNOVAZU { get; set; }
        public decimal PAKETENOVAZU { get; set; }
        public decimal AHKNOVAZU { get; set; }

        //Nova
        public decimal GRUNDNOVA { get; set; }
        public decimal SONZUBNOVA { get; set; }
        public decimal PAKETENOVA { get; set; }
        public decimal AHKNOVA { get; set; }

        //Netto exkl. Nova exkl. Zuschlag
        public decimal GRUNDEXKLN { get; set; }
        public decimal SONZUBEXKLN { get; set; }
        public decimal PAKETEEXKLN { get; set; }
        public decimal HERZUB { get; set; }
        public decimal ZUBEHOER { get; set; }
        public decimal AHKEXKLN { get; set; }

        //Nachlass Prozent
        public decimal GRUNDRABATTOP { get; set; }
        public decimal SONZUBRABATTOP { get; set; }
        public decimal PAKETERABATTOP { get; set; }
        public decimal HERZUBRABATTOP { get; set; }
        public decimal ZUBEHOERRABATTOP { get; set; }
        public decimal AHKRABATTOP { get; set; }

        //Nachlass Betrag Brutto
        public decimal GRUNDRABATTO { get; set; }
        public decimal SONZUBRABATTO { get; set; }
        public decimal PAKETERABATTO { get; set; }
        public decimal HERZUBRABATTO { get; set; }
        public decimal ZUBEHOERRABATTO { get; set; }
        public decimal AHKRABATTO { get; set; }


        //Brutto inkl Nova inkl. Zuschlag nach rabatt
        public decimal GRUNDEXTERNBRUTTO { get; set; }
        public decimal SONZUBEXTERNBRUTTO { get; set; }
        public decimal PAKETEEXTERNBRUTTO { get; set; }
        public decimal HERZUBEXTERNBRUTTO { get; set; }
        public decimal ZUBEHOEREXTERNBRUTTO { get; set; }
        public decimal NOVAZUABBRUTTO { get; set; }
        public decimal AHKEXTERNBRUTTO { get; set; }

        /// <summary>
        /// UST nach nachlass
        /// </summary>
        public decimal GRUNDEXTERNUST { get; set; }
        public decimal SONZUBEXTERNUST { get; set; }
        public decimal PAKETEEXTERNUST { get; set; }
        public decimal HERZUBEXTERNUST { get; set; }
        public decimal ZUBEHOEREXERNUST { get; set; }
        public decimal AHKEXTERNUST { get; set; }


        //Novazuschlag nach Rabatt
        public decimal GRUNDEXTERNNOVAZU { get; set; }
        public decimal SONZUBEXTERNNOVAZU { get; set; }
        public decimal PAKETEEXTERNNOVAZU { get; set; }
        public decimal NOVAZUABNOVAZU { get; set; }
        public decimal AHKEXTERNNOVAZU { get; set; }


        //Nova nach Rabatt
        public decimal GRUNDEXTERNNOVA { get; set; }
        public decimal SONZUBEXTERNNOVA { get; set; }
        public decimal PAKETEEXTERNNOVA { get; set; }
        public decimal AHKEXTERNNOVA { get; set; }

        //Netto inkl. Nova inkl. Zuschlag
        public decimal GRUNDEXTERN { get; set; }
        public decimal SONZUBEXTERN { get; set; }
        public decimal PAKETEEXTERN { get; set; }
        public decimal AHKEXTERN { get; set; }


        //Netto ohne Nova ohne Zuschlag nach Rabatt
        public decimal GRUNDEXTERNEXKLN { get; set; }
        public decimal SONZUBEXTERNEXKLN { get; set; }
        public decimal PAKETEEXTERNEXKLN { get; set; }
        public decimal HERZUBEXTERN { get; set; }
        public decimal ZUBEHOEREXTERN { get; set; }
        public decimal NOVAZUAB { get; set; }
        public decimal AHKEXTERNEXKLN { get; set; }
    }
}
