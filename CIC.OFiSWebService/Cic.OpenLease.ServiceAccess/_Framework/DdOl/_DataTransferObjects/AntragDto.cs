namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    [System.Runtime.Serialization.DataContract]
    public class AntragDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string ITAnrede
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ITTitelVornameName
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ITStrasse
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ITPlzOrt
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long ITSysLand
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ITTelefon
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ITPTelefon
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ITHandy
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ITEmail
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long ITSysLandNat
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public DateTime ITGebDatum
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        MitantragstellerDto MitantranSteller
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
        public long AntObUbNahmeKm
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public DateTime AntObLieferung
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long ITSysLandNat
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
        public decimal AntKalkBGExtern
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntKalkSZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntKalkDepot
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntKalkRW
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int AntKalkLZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long AntObJahresKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntObSatzMehrKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntObSatzMinderKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long AntObKmToleranz
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntKalkRgGebuehr
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AntKalkGebuehr
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public AntObSlDto AntObSl
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
        #endregion
    }
}
