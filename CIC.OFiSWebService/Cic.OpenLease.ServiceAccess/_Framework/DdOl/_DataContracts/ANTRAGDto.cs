// OWNER MK, 05-03-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class ANTRAGDto
    {
        #region Enums
        public enum AntragState
        {
            New,                        //neu,
            BeingChecked,               //inPrüfung,
            Rejected,                   //abgelehnt,
            Approved,                   //genehmigt,
            ApprovedWithRequirement,    //genehmigt mit Auflagen,
            FallenThrough,              //nicht zu Stande gekommen,
            LedOver,                    //übergeleitet
            Resubmitted,                //erneut eingereicht
        }
        #endregion

        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long? SysId
        {
            get;
            set;
        }
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string PersonAnrede
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PersonTitelVornameName
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PersonStrasse
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PersonPlzOrt
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? PersonSysLand
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PersonLand
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PersonTelefon
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PersonPTelefon
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PersonHandy
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PersonEmail
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? PersonSysLandNat
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PersonLandNat
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public DateTime? PersonGebDatum
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string AntObFzArt
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string AntObHersteller
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string AntObFabrikat
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? AntObAbNahmeKm
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public DateTime? AntObLieferung
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntragGrund
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntKalkSonder
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntKalkSPaket
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntKalkZubehoer
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntKalkZnovaf
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntKalkRabattO
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntObNoVAzuab
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? AntKalkBGExtern
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? AntKalkSZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? AntKalkDepot
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? AntKalkRW
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? AntKalkLZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? AntObJahresKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? AntObSatzMehrKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? AntObSatzMinderKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? AntObKmToleranz
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? AntKalkRgGebuehr
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? AntKalkGebuehr
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public AntObSlDto[] AntObSl
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntObSlBetragSumme
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public AntObSichDto[] AntObSich
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public String ZUSTAND
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public String ANTRAG
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public String VART
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? RATE
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public String KONSTELLATION
        {
            get;
            set;
        }
        #endregion
    }
}
