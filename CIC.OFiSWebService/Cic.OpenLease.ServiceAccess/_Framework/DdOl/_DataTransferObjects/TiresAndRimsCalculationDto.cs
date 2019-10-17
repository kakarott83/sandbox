using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.ServiceAccess
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class TiresAndRimsCalculationDto
    {
        
        [System.Runtime.Serialization.DataMember]
        public decimal Nachlass
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal SRPriceTotal
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal SRPriceTotalNetto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal SRPriceTotalUSt
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal WFPriceTotal
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal WFPriceTotalNetto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal WFPriceTotalUst
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal WRPriceTotal
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal WRPriceTotalNetto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal WRPriceTotalUst
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal SFPriceTotal
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal SFPriceTotalNetto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal SFPriceTotalUst
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public decimal RRmisPrice
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public decimal NebenKostenPrice
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public decimal ReifenGesamt
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal ReifenRateBrutto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal ReifenRate_Default
        {
            get;
            set;
        }

        public decimal ReifenRate_Subvention
        {
            get;
            set;
        }

        public decimal ReifenRate_SubventionUst
        {
            get;
            set;
        }

        public decimal ReifenRate_SubventionNetto
        {
            get;
            set;
        }
 

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal ReifenRateNetto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal ReifenRateUst
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public string SRCode
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public string WFCode
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string WRCode
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public string SFCode
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal FRimsPriceTotal
        {
            get;
            set;
        }


        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal RRimsPriceTotal
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal RRimsPriceTotalNetto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal RRimsPriceTotalUst
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal FRimsPriceTotalNetto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal FRimsPriceTotalUst
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public decimal SFPrice
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public int SFCount
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public decimal SRPrice
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public int SRCount
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public decimal WFPrice
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public int WFCount
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public decimal WRPrice
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public int WRCount
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public decimal FRimsPrice
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public decimal RRimsPrice
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public int FRimsCount
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public int RRimsCount
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public TiresAndRimsCalcModes CalcMode
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public int Reifensaetze
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal NebenKostenTotal
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal NebenKostenTotalNetto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal NebenKostenTotalUst
        {
            get;
            set;
        }

        //Input
        
        [System.Runtime.Serialization.DataMember]
        public int Laufzeit
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long Laufleistung
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long Leistung
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int ReifensaetzeChanged
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal Aufschlag
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal AufschlagP
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public int Gesamtreifenanzahl
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal FRimsPriceNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal FRimsPriceUst
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal RRimsPriceNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RRimsPriceUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal SFPriceNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal SFPriceUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal SRPriceNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal SRPriceUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal WFPriceNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal WFPriceUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal WRPriceNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal WRPriceUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal NebenKostenPriceNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal NebenKostenPriceUst
        {
            get;
            set;
        }

    }



}
