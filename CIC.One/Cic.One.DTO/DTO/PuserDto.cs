using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class PuserDto : EntityDto
    {
        /*	Id	*/
        public long syspuser { get; set; }
        public long syswfuser { get; set; }
        public long sysperson { get; set; }
        /// <summary>
        /// VK Perole SYSRGR???
        /// </summary>
        public long sysperole { get; set; }

        override public long getEntityId()
        {
            return syswfuser;
        }
        public override string getEntityBezeichnung()
        {
            return (vorname!=null?vorname:"") + " " + (name!=null?name:"");
        }

        /*	Anrede	*/
        public String anrede { get; set; }
		/*	AnredeCode	*/
		public String anredecode { get; set; }
		/*	Name	*/
        public String name { get; set; }
        /*	Vorname	*/
        public String vorname { get; set; }
        /*	Rolle	*/
        public String rolle { get; set; }
        /*	Rolle	*/
        public int role { get; set; }
        /*	Geburtsdatum	*/
        public DateTime? gebdatum { get; set; }
        /*	Nationalität	*/
        public long syslandnat { get; set; }
		/*	Korrespondenzsprache	*/
		public long sysctlangkorr { get; set; }
		/*	Sprache (rh 20170117: lt.Stas)	*/
		public long sysctlang { get; set; }
		/*	Telefon	*/
        public String telefon { get; set; }
        /*	Mobilnr	*/
        public String mobile { get; set; }
        /*	Email	*/
        public String email { get; set; }
        /*	Fax	*/
        public String fax { get; set; }
        /*	Strasse	*/
        public String strasse { get; set; }
        /* Hausnummer */
        public String hsnr { get; set; }
        /*	PLZ	*/
        public String plz { get; set; }
        /*	Ort	*/
        public String ort { get; set; }
        /*	Land	*/
        public long sysland { get; set; }
        /*	Kanton	*/
        public long sysstaat { get; set; }
        /*	Bank	*/
        public String bank { get; set; }
        public long sysblz { get; set; }
        public String kontonummer { get; set; }
        public String blz { get; set; } // Clearingnummer
        public String bic { get; set; }
        /*	IBAN	*/
        public String iban { get; set; }
        /*	AHVNummer	*/
        public String ahv { get; set; }
        /*	Kontoinhaber	*/
        public String kontoinhaber { get; set; }
        /*	Quellensteuer	*/
        public int steuerflag { get; set; }
        /*	Adminrecht	*/
        public int adminflag { get; set; }
        /*	Vtabschlussrecht	*/
        public int vtflag { get; set; }
        /*	Eintritt	*/
        public DateTime? validfrom { get; set; }
        /*	Austritt	*/
        public DateTime? validuntil { get; set; }
        /*	Austrittsgrund	*/
        public String inaktivgrund { get; set; }
        /*	Provisionsanteil	*/
        public double? provision { get; set; }
        /*	Status	*/
        public String status { get; set; }
        /* External Id */
        public String externeid { get; set; }
        /* Zustand perole */
        public String zustand { get; set; }
        /* attribut perole */
        public String attribut { get; set; }

		/* pendingChanges */
		public PuserPendingChangesDto pendingChanges { get; set; }
        /* Anzahl anderer Admins */
        public long othersAdminCount { get; set; }

        /// <summary>
        /// Verkäufer ID Partnersystem
        /// </summary>
        public String extuserid { get; set; }

    }
}
