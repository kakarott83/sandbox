// OWNER MK, 05-03-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{

    public class VTObligoDto
    {
        public decimal RISIKOGR1 { get; set; }
        public decimal? RISIKOGR2 { get; set; }
        public decimal? RISIKOGR3 { get; set; }
        public decimal RISIKOGR4 { get; set; }
        public decimal RISIKOGR5 { get; set; }
        public decimal RISIKOGR6 { get; set; }
        public decimal RISIKOGR7 { get; set; }
        public decimal OP { get; set; }
        public DateTime STAND { get; set; }
    }

    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class VTDto
    {
        
        
        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long? SysId
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SysIt
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
        public string PersonTitelNameVorname
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
        public string ObFzArt
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ObHersteller
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ObFabrikat
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? ObAbNahmeKm
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public DateTime? ObLieferung
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal VTGrund
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KalkSonder
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KalkSPaket
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KalkZubehoer
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KalkZnovaf
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KalkRabattO
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal ObNoVAzuab
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KalkBGExtern
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KalkSZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KalkSZBRUTTO
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KalkDepot
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KalkRW
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? KalkLZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? ObJahresKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ObSatzMehrKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ObSatzMinderKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? ObKmToleranz
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KalkRgGebuehr
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KalkGebuehr
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public ObSlDto[] ObSl
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal ObSlBetragSumme
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
        public decimal? KalkAnzahlung
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public String VERTRAG
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
        public int KDTYP
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

        //Aufloesewerte
        [System.Runtime.Serialization.DataMember]
        public VTRueckDto[] AUFLOESEWERTE {get;set;}
        //offene Posten
        [System.Runtime.Serialization.DataMember]
        public OposDto OPOS {get;set;}
        //Bonus Folgevertrag
        [System.Runtime.Serialization.DataMember]
        public BonusDto BONUSFF {get;set;}
        [System.Runtime.Serialization.DataMember]
        public decimal EUROTAXBLAU { get; set; }
        [System.Runtime.Serialization.DataMember]
        public decimal EUROTAXGELB { get; set; }
        [System.Runtime.Serialization.DataMember]
        public decimal EUROTAXMITTE { get; set; }
        [System.Runtime.Serialization.DataMember]
        public DateTime? ERSTZULASSUNG { get; set; }
        [System.Runtime.Serialization.DataMember]
        public String SCHWACKE { get; set; }

        [System.Runtime.Serialization.DataMember]
        public long SYSBRAND { get; set; }

        [System.Runtime.Serialization.DataMember]
        public bool EUROTAXVALID { get; set; }

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? VTENDE
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? VTBEGINN
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public String BAUREIHE { get; set; }

        [System.Runtime.Serialization.DataMember]
        public String SERIE { get; set; }

        [System.Runtime.Serialization.DataMember]
        public String KENNZEICHEN { get; set; }

      

        [System.Runtime.Serialization.DataMember]
        public String ZINSTYP { get; set; }
    }

   [System.Runtime.Serialization.DataContract]
    public class OposDto{
        [System.Runtime.Serialization.DataMember]
        public decimal betrag { get; set; }
        /// <summary>
        /// Jobausführungsdatum
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public DateTime datum { get; set; }
    }

    [System.Runtime.Serialization.DataContract]
    public class VTRueckDto{
        [System.Runtime.Serialization.DataMember]
        public decimal betrag { get; set; }
        /// <summary>
        /// Ultimo Jobausführungdatum + N Monate
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public DateTime datum {get;set;}
    }
    [System.Runtime.Serialization.DataContract]
    public class BonusDto{
        [System.Runtime.Serialization.DataMember]
        public decimal betrag { get; set; }
        /// <summary>
        /// Ultimo Jobausführungsdatum
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public DateTime datum { get; set; }
    }
}

