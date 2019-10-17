using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    
    public class VKInfo
    {
        public String NAME { get; set; }
        public String VORNAME { get; set; }
        public String ABTEILUNG { get; set; }
        public String TELEFON { get; set; }
        public String EMAIL { get; set; }
    }

    [System.Runtime.Serialization.DataContract]
    public class PrintDto
    {
        [System.Runtime.Serialization.DataMember]
        public ANGEBOTDto angebot { get; set; }
        [System.Runtime.Serialization.DataMember]
        public ITDto itDto { get; set; }
        [System.Runtime.Serialization.DataMember]
        public VTDto vtDto { get; set; }
       
        public decimal? RSDVPRAEMIE
        {
            get;
            set;
        }
        public decimal UST
        {
            get;
            set;
        }
        public String FZART
        {
            get;
            set;
        }
        public decimal ZUSATZGESAMT
        {
            get;
            set;
        }
        public decimal SAPAKETEBRUTTO
        {
            get;
            set;
        }
        public decimal UEBZUL
        {
            get;
            set;
        }
        public double ANGOBKMGESAMT
        {
            get;
            set;
        }
        
        /// <summary>
        /// ANGKALKGESAMTBRUTTO+ANGKALKSZBRUTTO
        /// </summary>
        public decimal GESAMTBETRAG
        {
            get;
            set;
        }
        public String VERZINSUNGSART
        {
            get;
            set;
        }
        public String GUELTIGBIS
        {
            get;
            set;
        }
        public String ERSTZULASSUNG
        {
            get;
            set;
        }
        public String HEUTE
        {
            get;
            set;
        }
        public String BRAND
        {
            get;
            set;
        }
        public String BRANDINFO
        {
            get;
            set;
        }
        /// <summary>
        /// Momentan wird in /images/logo_IMGSUFFIX_print.png gesucht
        /// </summary>
        public String IMGSUFFIX
        {
            get;
            set;
        }
        public VKInfo VKINFO
        {
            get;
            set;
        }
        public String KASKONAME
        { get; set; }
        public String HPNAME
        { get; set; }
        public String IUVNAME
        { get; set; }
        public String RSVNAME
        { get; set; }
        public String RSDVNAME
        { get; set; }
        public String BANKNAME
        {
            get;
            set;
        }
        public String BANKSTRASSE
        {
            get;
            set;
        }
        public String BANKPLZ
        {
            get;
            set;
        }
        public String BANKORT
        {
            get;
            set;
        }
    }
}
