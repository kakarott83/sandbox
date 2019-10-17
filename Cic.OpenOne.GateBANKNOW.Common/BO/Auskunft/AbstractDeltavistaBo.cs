using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Abstract Class: Deltavista Business Object
    /// </summary>
    public abstract class AbstractDeltavistaBo : IDeltavistaBo
    {
        /// <summary>
        /// Deltavista Web Service Business Object
        /// </summary>
        protected IDeltavistaWSDao dvWSDao;

        /// <summary>
        /// Deltavista DB Data Access Object
        /// </summary>
        protected IDeltavistaDBDao dvDBDao;

        /// <summary>
        /// Information Data Access Object
        /// </summary>
        protected IAuskunftDao auskunftDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dvWSDao">Deltavista Web Service Business Object</param>
        /// <param name="dvDBDao">Deltavista DB Data Access Object</param>
        /// <param name="auskunftDao">Information Data Access Object</param>
        public AbstractDeltavistaBo(IDeltavistaWSDao dvWSDao, IDeltavistaDBDao dvDBDao, IAuskunftDao auskunftDao)
        {
            this.dvWSDao = dvWSDao;
            this.dvDBDao = dvDBDao;
            this.auskunftDao = auskunftDao;
        }

        /// <summary>
        /// Get Udentified Address
        /// </summary>
        /// <param name="inDto">InputData</param>
        /// <exception cref="System.NotImplementedException">if not implemented</exception>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto getIdentifiedAddress(DeltavistaInDto inDto);


        /// <summary>
        /// Get Udentified Address Arbeitgeber
        /// </summary>
        /// <param name="inDto">InputData</param>
        /// <exception cref="System.NotImplementedException">if not implemented</exception>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto getIdentifiedAddressArb(DeltavistaInDto inDto);

        /// <summary>
        /// Get Company Details
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <exception cref="System.NotImplementedException">if not implemented</exception>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto getCompanyDetailsByAddressId(DeltavistaInDto inDto);

        /// <summary>
        /// Get Debt Details by Address
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <exception cref="System.NotImplementedException">if not implemented</exception>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto getDebtDetailsByAddressId(DeltavistaInDto inDto);

        /// <summary>
        /// Oder Cresura Report
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto orderCresuraReport(DeltavistaInDto inDto);

        /// <summary>
        /// Get report
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto getReport(DeltavistaInDto inDto);

        /// <summary>
        /// Get Identified Address by Address ID
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto getIdentifiedAddress(long sysAuskunft);


        /// <summary>
        /// Get Identified Address by Address ID
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto getIdentifiedAddressArb(long sysAuskunft);

        /// <summary>
        /// Get Company Details by Address ID
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto getCompanyDetailsByAddressId(long sysAuskunft);

        /// <summary>
        /// Get Debt Details by Address ID
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto getDebtDetailsByAddressId(long sysAuskunft);

        /// <summary>
        /// Order Cresura report by Address ID
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto orderCresuraReport(long sysAuskunft);

        /// <summary>
        /// Get Report by Address ID
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto getReport(long sysAuskunft);
    }
}