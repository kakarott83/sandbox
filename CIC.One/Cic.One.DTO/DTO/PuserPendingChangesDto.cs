using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
	public class PuserPendingChangesDto
    {
        /*	Id	*/
		////////public long syspuser { get; set; }
		////////public long syswfuser { get; set; }
		////////public long sysperson { get; set; }
		/////////// <summary>
		/////////// VK Perole SYSRGR???
		/////////// </summary>
		////////public long sysperole { get; set; }

		//override public long getEntityId()
		//{
		//	return 0;
		//}
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
		public String role { get; set; }
		/*	Geburtsdatum DateTime?	*/
		public String gebdatum { get; set; }
        /*	Nationalität	*/
		public String syslandnat { get; set; }
		///*	Korrespondenzsprache	*/ // rh 20170117: ALT
		//public String sysctlangkorr { get; set; }
		/*	Sprache  (rh 20170117: neu lt. Stas)	*/
		public String sysctlang { get; set; }
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
		public String sysland { get; set; }
        /*	Kanton	*/
		public String sysstaat { get; set; }
        /*	Bank	*/
        public String bank { get; set; }
		public String sysblz { get; set; }
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
		public String steuerflag { get; set; }
        /*	Adminrecht	*/
		public String adminflag { get; set; }
        /*	Vtabschlussrecht	*/
		public String vtflag { get; set; }
		/*	Eintritt DateTime?	*/
		public String validfrom { get; set; }
		/*	Austritt DateTime?	*/
		public String validuntil { get; set; }
        /*	Austrittsgrund	*/
        public String inaktivgrund { get; set; }
        /*	Provisionsanteil	*/
		public String provision { get; set; }
        /*	Status	*/
        public String status { get; set; }
        /* External Id */
        public String externeid { get; set; }
        /* Zustand perole */
        public String zustand { get; set; }
        /* attribut perole */
        public String attribut { get; set; }

    }
}
