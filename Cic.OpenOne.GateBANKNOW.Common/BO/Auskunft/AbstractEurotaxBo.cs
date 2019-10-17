using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Abstract Class: Eurotax Business Object
    /// </summary>
    [System.CLSCompliant(false)]
    public abstract class AbstractEurotaxBo : IEurotaxBo
    {
        /// <summary>
        /// Eurotax Web Service Business Object
        /// </summary>
        protected IEurotaxWSDao eurotaxWSDao;

        /// <summary>
        /// Eurotax DB Data Access Object
        /// </summary>
        protected IEurotaxDBDao eurotaxDBDao;

        /// <summary>
        /// Information Data Access Object
        /// </summary>
        protected IAuskunftDao auskunftDao;

        /// <summary>
        /// Information Data Access Object
        /// </summary>
        protected IVGDao vgDao;

        /// <summary>
        /// Information Data Access Object
        /// </summary>
        protected IObTypDao obtypDao;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eurotaxWSDao">Eurotax Web Service Business Object</param>
        /// <param name="eurotaxDBDao">Eurotax DB Data Access Object</param>
        /// <param name="auskunftDao">Information Data Access Object</param>
        /// <param name="vgDao">Wertegruppen DAO</param>
        /// <param name="obtypDao">Objekttyp DAO</param>
        public AbstractEurotaxBo(IEurotaxWSDao eurotaxWSDao, IEurotaxDBDao eurotaxDBDao, IAuskunftDao auskunftDao, IVGDao vgDao, IObTypDao obtypDao)
        {
            this.eurotaxWSDao = eurotaxWSDao;
            this.eurotaxDBDao = eurotaxDBDao;
            this.auskunftDao = auskunftDao;
            this.vgDao = vgDao;
            this.obtypDao = obtypDao;
        }

        // <summary>
        // Get Forecast
        // </summary>
        // <param name="inDto">Input Data</param>
        // <returns>Output Data</returns>
        // public abstract EurotaxOutDto GetForecast(EurotaxInDto inDto);

        /// <summary>
        /// Get Forecast
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>Output Data List</returns>
        public abstract List<EurotaxOutDto> GetForecast(EurotaxInDto inDto);

        /// <summary>
        /// Get Remo
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>Output Data List</returns>
        public abstract List<EurotaxOutDto> GetRemo(EurotaxInDto inDto);

        /// <summary>
        /// Get Forecast
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto GetForecast(long sysAuskunft);

        /// <summary>
        /// get Valuation
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>Output Data</returns>
        public abstract EurotaxOutDto GetValuation(EurotaxInDto inDto);

        /// <summary>
        /// get Valuation
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto GetValuation(long sysAuskunft);

        /// <summary>
        /// Ermitteln des Restwertes eines Fahrzeugs
        /// </summary>
        /// <param name="input">Eingabedaten</param>
        /// <param name="sysvg">Gruppenparameter</param>
        /// <returns>Restwert in Prozent</returns>
        public abstract double evaluateRestwert(RestwertRequestDto input, long sysvg);

        /// <summary>
        /// Ermittlen des Neuwerts eines Fahrzeugs.
        /// </summary>
        /// <param name="sysobtyp">Objekttyp</param>
        /// <returns>Netto Neupreis Betrag</returns>
        public abstract double evaluateNeupreis(long sysobtyp);


        /// <summary>
        /// GetVinDecode
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract EurotaxVinOutDto GetVinDecode(EurotaxVinInDto inDto);


       /// <summary>
       /// 
       /// </summary>
       /// <param name="sysobtyp"></param>
       /// <param name="laufzeit"></param>
       /// <param name="neupreis"></param>
       /// <param name="neupreisDefault"></param>
       /// <param name="neupreisIW"></param>
       /// <param name=""></param>
       /// <param name="kmStand"></param>
       /// <param name="zubehoer"></param>
       /// <param name="erstzulassung"></param>
       /// <param name="schwacke"></param>
       /// <param name="jahresKm"></param>
       /// <returns></returns>
        public abstract List<EurotaxOutDto> getEurotaxOutList(long sysobtyp, int laufzeit, double neupreis, double neupreisDefault, double neupreisIW,double neupreisVGREF, double kmStand, double zubehoer, DateTime? erstzulassung, string schwacke, long jahresKm);


         /// <summary>
        /// Fetch the eurotax forecast value
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract EurotaxOutDto getEurotaxForecast(EurotaxInDto inDto);

        /// <summary>
        /// Provides access data (username, password, signature)
        /// </summary>
        public abstract Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ETGHeaderType MyGetHeaderType(EurotaxVinInDto inDto);

        /// <summary>
        /// Fills Setting Type
        /// </summary>
        /// <param name="inDto"></param>
        public abstract Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ETGsettingType MyGetSettingType(EurotaxVinInDto inDto);

        public abstract string MyGetServiceId(EurotaxVinInDto inDto);

        /// <summary>
        /// Gets username and password from db and fills header
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract DAO.Auskunft.EurotaxValuationRef.ETGHeaderType MyGetHeaderTypeForValuation(EurotaxInDto inDto);

        /// <summary>
        /// Gets setting for Valuation
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract DAO.Auskunft.EurotaxValuationRef.ETGsettingType MyGetSettingTypeForValuation(EurotaxInDto inDto);
    }
}
