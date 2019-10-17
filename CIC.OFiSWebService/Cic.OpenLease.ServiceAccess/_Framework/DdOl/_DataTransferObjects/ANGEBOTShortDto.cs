// OWNER JJ, 08-02-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class ANGEBOTShortDto
    {
        #region Ids

        [System.Runtime.Serialization.DataMember]
        public long SYSID
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSPRPRODUCT
        {
            get;
            set;
        }
        #endregion

        #region Properties

        // TODO JJ 0 JJ, Dokumentart
        /// <summary>
        /// Eingangsrechnung inkl. MWST
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int? ANGOBERINKLMWST
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? ERFASSUNG
        {
            get;
            set;
        }



        [System.Runtime.Serialization.DataMember]
        public string ZUSTAND
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string ANTRAGSSTATUS
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ANGEBOT1
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string AUFLAGEN
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSIT
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSVK
        {
            get;
            set;
        }



        [System.Runtime.Serialization.DataMember]
        public System.DateTime? DATANGEBOT
        {
            get;
            set;
        }




        [System.Runtime.Serialization.DataMember]
        public string ITVORNAME
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ITNAME
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ITPLZ
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ITORT
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string OBJEKTVT
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string PRPRODUCTNAME
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKLZ
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? ANGOBJAHRESKM
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRATE
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ANGKALKBGEXTERNNACHLBRUTTO
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKBGEXTERNBRUTTO
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKSZ
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKSZBRUTTO
        {
            get;
            set;
        }



        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKDEPOT
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ANGKALKRWKALK
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string VERKAUFERNAME
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string VERKAUFERVORNAME
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public decimal? AHKEXTERNBRUTTO
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public decimal? RWKALKBRUTTO
        {
            get;
            set;
        }





        [System.Runtime.Serialization.DataMember]
        public ANGKALKShortDto[] ANGKALKShortDtos
        {
            get;
            set;
        }



        [System.Runtime.Serialization.DataMember]
        public ANGOBShortDto[] ANGOBShortDtos
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string PRODUCTNAME
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string SCALCUSERNAME
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string SCALCUSERVORNAME
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSBERATADDB
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSBRAND
        {
            get;
            set;
        }



        [System.Runtime.Serialization.DataMember]
        public System.DateTime GUELTIGBIS
        {
            get;
            set;
        }



        [System.Runtime.Serialization.DataMember]
        public bool Gueltig
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ABTRETUNGVON
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SPECIALCALCCOUNT
        {
            get;
            set;
        }

        /// <summary>
        /// Sonderkalkulationsstatus
        /// 1 = Angefordert
        /// 2 = inBearbeitung
        /// 3 = Durchgeführt
        /// </summary>

        [System.Runtime.Serialization.DataMember]
        public int? SPECIALCALCSTATUS
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string SPECIALCALCSTATUSTEXT
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public int? CONTRACTEXT
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public long? SYSVORVT
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string VORVERTRAGSNUMMER
        {
            get;
            set;
        }
        #endregion
    }
}
