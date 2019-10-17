using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Abstract Class: ZEK Business Object
    /// </summary>
    public abstract class AbstractZekBo : IZekBo
    {
        /// <summary>
        /// ZEK Web Service Data Access Object
        /// </summary>
        protected IZekWSDao zekWSDao;

        /// <summary>
        /// ZEK DB Data Access Object
        /// </summary>
        protected IZekDBDao zekDBDao;

        /// <summary>
        /// Information Data Access Object
        /// </summary>
        protected IAuskunftDao auskunftDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="zekWSDao">ZEK Web Service Data Access Object</param>
        /// <param name="zekDBDao">ZEK DB Data Access Object</param>
        /// <param name="auskunftDao">Information Data Access Object</param>
        public AbstractZekBo(IZekWSDao zekWSDao, IZekDBDao zekDBDao, IAuskunftDao auskunftDao)
        {
            this.zekWSDao = zekWSDao;
            this.zekDBDao = zekDBDao;
            this.auskunftDao = auskunftDao;
        }

        /// <summary>
        /// New Credit Application
        /// </summary>
        /// <param name="inDto">ZEK Input Data Transfer Object</param>
        /// <returns>ZEK Output Data Transfer Object</returns>
        public abstract AuskunftDto kreditgesuchNeu(ZekInDto inDto);

        /// <summary>
        /// Informative Inquiry
        /// </summary>
        /// <param name="inDto">ZEK Input Data Transfer Object</param>
        /// <returns>ZEK Output Data Transfer Object</returns>
        public abstract AuskunftDto informativabfrage(ZekInDto inDto);

       /// <summary>
        /// informativabfrageLogDump
       /// </summary>
       /// <param name="inDto">inDto</param>
       /// <param name="area">area</param>
       /// <param name="sysAreaid">sysAreaid</param>
       /// <param name="syswfuser">syswfuser</param>
       /// <returns></returns>
        public abstract AuskunftDto informativabfrageLogDump(ZekInDto inDto, string area, long sysAreaid, long syswfuser);

        /// <summary>
        /// Credit Application Rejected
        /// </summary>
        /// <param name="inDto">ZEK Input Data Transfer Object</param>
        /// <returns>ZEK Output Data Transfer Object</returns>
        public abstract AuskunftDto kreditgesuchAblehnen(ZekInDto inDto);

        /// <summary>
        /// Update Address
        /// </summary>
        /// <param name="inDto">ZEK Input Data Transfer Object</param>
        /// <returns>ZEK Output Data Transfer Object</returns>
        public abstract AuskunftDto updateAddress(ZekInDto inDto);

        /// <summary>
        /// New Credit Application
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Output Data Access Object</returns>
        public abstract AuskunftDto kreditgesuchNeu(long sysAuskunft);

        /// <summary>
        /// Informative Inquiry
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Output Data Access Object</returns>
        public abstract AuskunftDto informativabfrage(long sysAuskunft);

        /// <summary>
        /// Reject Credit Application
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Output Data Access Object</returns>
        public abstract AuskunftDto kreditgesuchAblehnen(long sysAuskunft);

        /// <summary>
        /// Update Address
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Output Data Access Object</returns>
        public abstract AuskunftDto updateAddress(long sysAuskunft);

        /// <summary>
        /// register Cash Loan
        /// </summary>
        /// <param name="inDto">ZEK Input Data Transfer Object</param>
        /// <returns>ZEK Output Data Transfer Object</returns>
        public abstract AuskunftDto registerBardarlehen(ZekInDto inDto);

        /// <summary>
        /// Register Fixed Credit
        /// </summary>
        /// <param name="inDto">ZEK Input Data Transfer Object</param>
        /// <returns>ZEK Output Data Transfer Object</returns>
        public abstract AuskunftDto registerFestkredit(ZekInDto inDto);

        /// <summary>
        /// Register advance on current account
        /// </summary>
        /// <param name="inDto">ZEK Input Data Transfer Object</param>
        /// <returns>ZEK Output Data Transfer Object</returns>
        public abstract AuskunftDto registerKontokorrentkredit(ZekInDto inDto);

        /// <summary>
        /// Register Leasing or Rental contract
        /// </summary>
        /// <param name="inDto">ZEK Input Data Transfer Object</param>
        /// <returns>ZEK Output Data Transfer Object</returns>
        public abstract AuskunftDto registerLeasingMietvertrag(ZekInDto inDto);

        /// <summary>
        /// Register Downpay credit
        /// </summary>
        /// <param name="inDto">ZEK Input Data Transfer Object</param>
        /// <returns>ZEK Output Data Transfer Object</returns>
        public abstract AuskunftDto registerTeilzahlungskredit(ZekInDto inDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto registerTeilzahlungsvertrag(ZekInDto inDto);

        /// <summary>
        /// Report Card commitment
        /// </summary>
        /// <param name="inDto">ZEK Input Data Transfer Object</param>
        /// <returns>ZEK Output Data Transfer Object</returns>
        public abstract AuskunftDto meldungKartenengagement(ZekInDto inDto);

        /// <summary>
        /// Report Overdraft Credit
        /// </summary>
        /// <param name="inDto">ZEK Input Data Transfer Object</param>
        /// <returns>ZEK Output Data Transfer Object</returns>
        public abstract AuskunftDto meldungUeberziehungskredit(ZekInDto inDto);

        /// <summary>
        /// Register Cash Loan
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Output Data Access Object</returns>
        public abstract AuskunftDto registerBardarlehen(long sysAuskunft);

        /// <summary>
        /// Register Fixed Credit
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Output Data Access Object</returns>
        public abstract AuskunftDto registerFestkredit(long sysAuskunft);

        /// <summary>
        /// Register Downpay Credit
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Output Data Access Object</returns>
        public abstract AuskunftDto registerKontokorrentkredit(long sysAuskunft);

        /// <summary>
        /// Register Leasing or Rental contract
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Output Data Access Object</returns>
        public abstract AuskunftDto registerLeasingMietvertrag(long sysAuskunft);

        /// <summary>
        /// Register Downpay credit
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Output Data Access Object</returns>
        public abstract AuskunftDto registerTeilzahlungskredit(long sysAuskunft);

        /// <summary>
        /// registerTeilzahlungsvertrag
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto registerTeilzahlungsvertrag(long sysAuskunft);

        /// <summary>
        /// Report Card Commitment
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Output Data Access Object</returns>
        public abstract AuskunftDto meldungKartenengagement(long sysAuskunft);

        /// <summary>
        /// Report Overdraft Credit
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Output Data Access Object</returns>
        public abstract AuskunftDto meldungUeberziehungskredit(long sysAuskunft);

        /// <summary>
        /// updateBardarlehen
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateBardarlehen(ZekInDto inDto);

        /// <summary>
        /// updateFestkredit
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateFestkredit(ZekInDto inDto);

        /// <summary>
        /// updateKontokorrentkredit
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateKontokorrentkredit(ZekInDto inDto);

        /// <summary>
        /// updateLeasingMietvertrag
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateLeasingMietvertrag(ZekInDto inDto);

        /// <summary>
        /// updateTeilzahlungskredit
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateTeilzahlungskredit(ZekInDto inDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateTeilzahlungsvertrag(ZekInDto inDto);

        /// <summary>
        /// updateBardarlehen
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateBardarlehen(long sysAuskunft);

        /// <summary>
        /// updateFestkredit
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateFestkredit(long sysAuskunft);

        /// <summary>
        /// updateKontokorrentkredit
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateKontokorrentkredit(long sysAuskunft);

        /// <summary>
        /// updateLeasingMietvertrag
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateLeasingMietvertrag(long sysAuskunft);

        /// <summary>
        /// updateTeilzahlungskredit
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateTeilzahlungskredit(long sysAuskunft);

        /// <summary>
        /// updateTeilzahlungsvertrag
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateTeilzahlungsvertrag(long sysAuskunft);

        /// <summary>
        /// closeBardarlehen
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeBardarlehen(ZekInDto inDto);

        /// <summary>
        /// closeBardarlehen
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeBardarlehen(long sysAuskunft);

        /// <summary>
        /// closeLeasingMietvertrag
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeLeasingMietvertrag(ZekInDto inDto);

        /// <summary>
        /// closeLeasingMietvertrag
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeLeasingMietvertrag(long sysAuskunft);

        /// <summary>
        /// closeFestkredit
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeFestkredit(ZekInDto inDto);

        /// <summary>
        /// closeFestkredit
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeFestkredit(long sysAuskunft);

        /// <summary>
        /// closeTeilzahlungskredit
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeTeilzahlungskredit(ZekInDto inDto);

        /// <summary>
        /// closeTeilzahlungsvertrag
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeTeilzahlungsvertrag(ZekInDto inDto);

        /// <summary>
        /// closeTeilzahlungskredit
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeTeilzahlungskredit(long sysAuskunft);

        /// <summary>
        /// closeTeilzahlungsvertrag
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeTeilzahlungsvertrag(long sysAuskunft);

        /// <summary>
        /// closeKontokorrentkredit
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeKontokorrentkredit(ZekInDto inDto);

        /// <summary>
        /// closeKontokorrentkredit
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeKontokorrentkredit(long sysAuskunft);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        public abstract AuskunftDto eCode178Anmelden(ZekInDto inDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        public abstract AuskunftDto eCode178Anmelden(long sysAuskunft);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        public abstract AuskunftDto eCode178Abmelden(ZekInDto inDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        public abstract AuskunftDto eCode178Abmelden(long sysAuskunft);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        public abstract AuskunftDto eCode178Mutieren(ZekInDto inDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        public abstract AuskunftDto eCode178Mutieren(long sysAuskunft);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        public abstract AuskunftDto eCode178Abfrage(ZekInDto inDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        public abstract AuskunftDto eCode178Abfrage(long sysAuskunft);

        /// <summary>
        /// getARMs
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        public abstract AuskunftDto getARMs(ZekInDto inDto);

        /// <summary>
        /// getARMs
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        public abstract AuskunftDto getARMs(long sysAuskunft);

        //BNR11
        /// <summary>
        /// informativabfrageOL
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftOLDto informativabfrageOL(AuskunftOLDto inDto);
    }
}