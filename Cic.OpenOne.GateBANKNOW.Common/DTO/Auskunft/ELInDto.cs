using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    public class ELInDto
    {
        /// <summary>
        /// Antrag sysid
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// Barkaufpreis / ANTKALK.BGINTERNBRUTTO
        /// </summary>
        public double barkaufpreis { get; set; }

        /// <summary>
        /// Anzahlung / antkalk.szbrutto
        /// </summary>
        public double anzahlung { get; set; }

        /// <summary>
        /// anz_bkp / round((antkalk.szbrutto/ANTKALK.BGINTERNBRUTTO),4)
        /// </summary>
        public double anz_bkp { get; set; }

        /// <summary>
        /// Finanzierungsbetrag / (ANTKALK.BGINTERNBRUTTO-antkalk.szbrutto)
        /// </summary>
        public double finanzierungsbetrag { get; set; }

        /// <summary>
        /// Laufzeit / antkalk.LZ 
        /// </summary>
        public int laufzeit { get; set; }

        /// <summary>
        /// Beginn
        /// </summary>
        public DateTime beginn { get; set; }

        /// <summary>
        /// Verweis zum Kalkulation
        /// </summary>
        public long syskalk { get; set; }

        /// <summary>
        /// sysAntrag
        /// </summary>
        public long sysantrag { get; set; }

        /// <summary>
        /// sysAntrag
        /// </summary>
        public string antrag { get; set; }

        /// <summary>
        /// Restwert
        /// </summary>
        public double restwert { get; set; }

        /// <summary>
        /// Zins
        /// </summary>
        public double zins { get; set; }

        /// <summary>
        /// Scorebezeichnung
        /// </summary>
        public String scorebezeichnung { get; set; }

        /// <summary>
        /// Verweis zum Obtyp
        /// </summary>
        public long sysobtyp { get; set; }

        /// <summary>
        /// kmStand
        /// </summary>
        public long kmStand  { get; set; }

        /// <summary>
        /// JahresKM / antkalk.ll
        /// </summary>
        public long jahresKm { get; set; }

        /// <summary>
        /// Erstzulassung
        /// </summary>
        public DateTime erstzulassung { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double zubehoer { get; set; }

        /// <summary>
        /// Laufzeit min
        /// </summary>
        public int minLz { get; set; }

        /// <summary>
        /// Schwacke
        /// </summary>
        public String schwacke { get; set; }

        /// <summary>
        /// Fahrzeugsalter in Monate
        /// </summary>
        public int alter_Fhz_Mt { get; set; }

        /// <summary>
        /// mwst
        /// </summary>
        public double? mwst { get; set; }

       

        /// <summary>
        /// Verweis zum Vertragsart
        /// </summary>
        public long sysvart { get; set; }

        /// <summary>
        /// Rate / antkalk.RATEBRUTTO 
        /// </summary>
        public double? rate { get; set; }

        /// <summary>
        /// Verweis zum Wertetabelle
        /// </summary>
        public long sysvg { get; set; }

        /// <summary>
        /// Name von vg
        /// </summary>
        public string vgName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VgDto vg { get; set; }

        /// <summary>
        /// syskdtyp
        /// </summary>
        public long syskdtyp { get; set; }

        /// <summary>
        /// sysbrand 
        /// </summary>
        public long sysbrand { get; set; }

        /// <summary>
        /// scorewert
        /// </summary>
        public double scorewert { get; set; }

        /// <summary>
        /// Ausfallwahrscheinlichkeit Wertetabelle Verweis
        /// </summary>
        public double ausfallwvg { get; set; }

        /// <summary>
        /// zinsertrag  
        /// </summary>
        public double zinsertrag { get; set; }

        /// <summary>
        /// sysob
        /// </summary>
        public long sysob { get; set; }

        /// <summary>
        /// Vermittler = Händler / antrag.sysVM
        /// </summary>
        public long sysVM { get; set; }

        /// <summary>
        /// Marktwerte in Db speichern
        /// </summary>
        public bool saveMarktwerteInDb { get; set; }

        /// <summary>
        /// MAX_RW_FAKTOR
        /// </summary>
        public double rwFaktor { get; set; }

        /// <summary>
        /// bgexternbrutto
        /// </summary>
        public double zinskosten{ get; set; }

        /// <summary>
        /// sysobart
        /// </summary>
        public long sysobart{ get; set; }


        /// <summary>
        /// ahkBrutto
        /// </summary>
        public double ahkBrutto { get; set; }


        /// <summary>
        /// neupreis4DoRemo {
        /// </summary>
        public double neupreis4DoRemo { get; set; }

        /// <summary>
        /// neupreis4DoRemo Interne Werte{
        /// </summary>
        public double neupreis4DoRemoIW { get; set; }

        /// <summary>
        /// neupreis4DoRemoDefault
        /// </summary>
        public double neupreis4DoRemoDefault { get; set; }

        /// <summary>
        /// listenpreis4DoRemo {
        /// </summary>
        public double listenpreis4DoRemo { get; set; }

        /// <summary>
        /// listenpreis4DoRemoDefault {
        /// </summary>
        public double listenpreis4DoRemoIW { get; set; }

        /// <summary>
        /// listenpreis4DoRemoDefault {
        /// </summary>
        public double listenpreis4DoRemoDefault { get; set; }

        /// <summary>
        /// LP lp_prozent default {
        /// </summary>
        public decimal lp_prozent { get; set; }

        /// <summary>
        /// LP lp_prozent Interne Werte {
        /// </summary>
        public decimal lp_prozentIW { get; set; }


        /// <summary>
        /// neupreis {
        /// </summary>
        public double neupreis { get; set; }

        /// <summary>
        /// sysWfuser
        /// </summary>
        public long sysWfuser { get; set; }

        /// <summary>
        /// sysprproduct.
        /// </summary>
        public long sysprproduct { get; set; }

        /// <summary>
        /// sysCreate
        /// </summary>
        public DateTime? sysCreate { get; set; }


        /// <summary>
        /// Zinssatz Kundenzins bei Differenzleasing
        /// </summary>
        public double zinscust { get; set; }

        /// <summary>
        /// neupreis4DoRemo intern VGREF
        /// </summary>
        public double neupreis4DoRemoVGREF { get; set; }

        /// <summary>
        /// LP lp_prozent Interne VGREF
        /// </summary>
        public decimal lp_prozentVGREF { get; set; }


        /// <summary>
        /// listenpreis4DoRemoVGREF
        /// </summary>
        public double listenpreis4DoRemoVGREF { get; set; }

        /// <summary>
        /// MW_FAKTOR
        /// </summary>
        public double mwFaktor { get; set; }
    }
}
