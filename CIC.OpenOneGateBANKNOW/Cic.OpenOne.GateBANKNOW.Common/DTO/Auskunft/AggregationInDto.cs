using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Aggregation Input Data Transfer Object
    /// </summary>
    public class AggregationInDto
    {
        /// <summary>
        /// SYS Auskunft
        /// </summary>
        public long? SysAuskunft { get; set; }

        // public long SysAggInp { get; set; }               //     NUMBER(12,0), 

        /// <summary>
        /// AntragsID
        /// </summary>
        public long AntragID { get; set; }                //    NUMBER(12,0) 

        /// <summary>
        /// Finanzierungsbetrag aktueller Antrag
        /// </summary>
        public decimal? ADFinBetrag { get; set; }             //       NUMBER(15,2) 

        /// <summary>
        /// Anzahlung/1.Leasingrate aktueller Antrag
        /// </summary>
        public decimal? ADAnzahlung { get; set; }             //       NUMBER(15,2) 

        /// <summary>
        /// Restwert aktueller Antrag
        /// </summary>
        public decimal? ADRestwert { get; set; }              //      NUMBER(15,2) 

        /// <summary>
        /// VertragsID
        /// </summary>
        public long ADVertragID { get; set; }             //       NUMBER(12,0) 

        /// <summary>
        /// Summe Eigenablöse
        /// </summary>
        public decimal? ADAbBetrag { get; set; }              //      NUMBER(15,2) 

        /// <summary>
        /// Deltavista ID
        /// </summary>
        public long DeltaVistaID { get; set; }            //        NUMBER(12,0) 

        /// <summary>
        /// Forderungsstatus
        /// </summary>
        public string DVForderungsStatus { get; set; }      //      VARCHAR2(80 BYTE)

        /// <summary>
        /// Datum Forderung
        /// Format: YYYYMMDD
        /// </summary>
        public DateTime? DVEroeffnet { get; set; }             //       DATE, 

        /// <summary>
        /// Zahlungsstatus
        /// </summary>
        public string DVZahlungsStatus { get; set; }        //      VARCHAR2(80 BYTE), 

        /// <summary>
        /// Offener Betrag
        /// </summary>
        public decimal? DVOffen { get; set; }                 //   NUMBER(15,2) 

        /// <summary>
        /// ZEK Vertragsart
        /// </summary>
        public string ZKVertragsart { get; set; }           //         VARCHAR2(20 BYTE), 

        /// <summary>
        /// Herkunft (Eigen/Fremd)
        /// </summary>
        public string ZKHerkunft { get; set; }              //      VARCHAR2(10 BYTE), 

        /// <summary>
        /// Status (laufend/geschlossen)
        /// </summary>
        public string ZKStatus { get; set; }                //    VARCHAR2(20 BYTE), 

        /// <summary>
        /// Bonitätscode
        /// </summary>
        public decimal? ZKBoniCode { get; set; }              //      NUMBER(15,2) 

        /// <summary>
        /// Datum Bonitätscode
        /// Format: YYYYMMDD
        /// </summary>
        public DateTime? ZKBoniCodeDat { get; set; }           //         DATE, 

        /// <summary>
        /// Engagement
        /// </summary>
        public decimal? ZKEngagement { get; set; }            //        NUMBER(15,2) 

        /// <summary>
        /// Status
        /// </summary>
        public string ZKKGstatus { get; set; }              //      VARCHAR2(25 BYTE), 

        /// <summary>
        /// Herkunft
        /// </summary>
        public string ZKKGHerkunft { get; set; }            //        VARCHAR2(25 BYTE), 

        /// <summary>
        /// Ablehnungscode
        /// </summary>
        public decimal? ZKKGAblehnCode { get; set; }          //          NUMBER(15,2) 

        /// <summary>
        /// Datum Ablehnungscode
        /// Format: YYYYMMDD
        /// </summary>
        public DateTime? ZKKGAblehnCodeDat { get; set; }       //             DATE, 

        /// <summary>
        /// Herkunft
        /// </summary>
        public string ZKKMHerkunft { get; set; }            //        VARCHAR2(25 BYTE), 

        /// <summary>
        /// Datum
        /// Format: YYYYMMDD
        /// </summary>
        public DateTime? ZKKMDatNeg { get; set; }              //      DATE, 

        /// <summary>
        /// Ereigniscode
        /// </summary>
        public decimal? ZKKMEreignisCode { get; set; }        //            NUMBER(15,2) 

        /// <summary>
        /// Datum Amtscode
        /// Format: YYYYMMDD
        /// </summary>
        public DateTime? ZKAMMeldungDat { get; set; }          //          DATE, 

        /// <summary>
        /// Amtscode
        /// </summary>
        public decimal? ZKAMCode { get; set; }                //    NUMBER(15,2) 

        /// <summary>
        /// Vertragsnummer
        /// </summary>
        public long? SysVT { get; set; }                   //    NUMBER(12,0) 

        /// <summary>
        /// Vertriebspartnernummer
        /// </summary>
        public long? SysVPVT { get; set; }                 //   NUMBER(12,0) 

        /// <summary>
        /// Finanzierungsbetrag
        /// </summary>
        public decimal? OLVTFinBetrag { get; set; }           //         NUMBER(15,2) 

        /// <summary>
        /// Restwert
        /// </summary>
        public decimal? OLVTRestwert { get; set; }            //        NUMBER(15,2) 

        /// <summary>
        /// Vertrag Beginn
        /// Format: YYYYMMDD
        /// </summary>
        public DateTime? OLVTBeginn { get; set; }              //      DATE, 
        
        /// <summary>
        /// Vertrag Ende
        /// Format: YYYYMMDD
        /// </summary>
        public DateTime? OLVTEnde { get; set; }                //    DATE, 

        /// <summary>
        /// Vertrag Saldierung
        /// </summary>
        public decimal? OLVTSaldierung { get; set; }          //          NUMBER(15,2) 

        /// <summary>
        /// Zustand
        /// </summary>
        public string OLVTZustand { get; set; }             //       VARCHAR2(25 BYTE), 

        /// <summary>
        /// Anzahl 1.Mahnungen
        /// </summary>
        public long OLVTAnzmahn1 { get; set; }            //        NUMBER(12,0) 

        /// <summary>
        /// Anzahl 2.Mahnungen
        /// </summary>
        public long OLVTanzmahn2 { get; set; }            //        NUMBER(12,0) 

        /// <summary>
        /// Anzahl 3.Mahnungen
        /// </summary>
        public long OLVTanzmahn3 { get; set; }            //        NUMBER(12,0) 

        /// <summary>
        /// Anzahl Zahlplanänderungen
        /// </summary>
        public long OLVTZVB { get; set; }                 //   NUMBER(12,0) 

        /// <summary>
        /// Anzahl Stundungen
        /// </summary>
        public long OLVTStundungen { get; set; }          //          NUMBER(12,0) 

        /// <summary>
        /// Maximale Risikoklasse
        /// </summary>
        public string OLVTMaxRisikoKl { get; set; }         //           NUMBER(12,0) 

        /// <summary>
        /// Buchsaldo
        /// </summary>
        public decimal? OLVTEngagement { get; set; }          //          NUMBER(15,2) 

        /// <summary>
        /// Anzahl offener Posten
        /// </summary>
        public long OLVTAnzahlOp { get; set; }            //        NUMBER(12,0) 

        /// <summary>
        /// Summe offener Posten
        /// </summary>
        public decimal? OLVTSumOp { get; set; }               //     NUMBER(15,2) 

        /// <summary>
        /// Antragsnummer
        /// </summary>
        public long? SysAntrag { get; set; }                 //     NUMBER(12,0) 

        /// <summary>
        /// Vertriebspartnernummer
        /// </summary>
        public long? SysVPAT { get; set; }                 //   NUMBER(12,0) 

        /// <summary>
        /// Finanzierungsbetrag
        /// </summary>
        public decimal? OLATFinBetrag { get; set; }           //         NUMBER(15,2) 

        /// <summary>
        /// Anzahlung/1.Leasingrate
        /// </summary>
        public decimal? OLATAnzahlung { get; set; }           //         NUMBER(15,2) 

        /// <summary>
        /// Restwert
        /// </summary>
        public decimal? OLATRestwert { get; set; }            //        NUMBER(15,2) 

        /// <summary>
        /// Entscheidungsstatus
        /// </summary>
        public string OLATDeStatus { get; set; }            //        VARCHAR2(25 BYTE), 

        /// <summary>
        /// Entscheidungsdatum
        /// Format: YYYYMMDD
        /// </summary>
        public DateTime? OLATDeDat { get; set; }               //     DATE, 

        /// <summary>
        /// Bearbeitungsstatus
        /// </summary>
        public string OLATBeStatus { get; set; }            //        VARCHAR2(25 BYTE), 

        /// <summary>
        /// Bearbeitungsdatum
        /// Format: YYYYMMDD
        /// </summary>
        public DateTime? OLATBeDat { get; set; }               //     DATE
        
    }
}
