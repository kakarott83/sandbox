using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// business object for setting simple fields
    /// </summary>
    public class SimpleSetterBo : AbstractSimpleSetterBo
    {
        /// <summary>
        /// contructs a simpleSetter business object
        /// </summary>
        /// <param name="simpleSetterDao">the data access object to use</param>
        public SimpleSetterBo(ISimpleSetterDao simpleSetterDao)
            : base(simpleSetterDao)
        {
        }

        /// <summary>
        /// set Geschaeftsart 
        /// </summary>
        /// <param name="sysprchannel">Key des Channels</param>
        /// <returns>osetGeschaeftsartDto</returns>
        public override string setGeschaeftsart(long sysprchannel)
        {
            return simpleSetterDao.setGeschaeftsart(sysprchannel);
        }

        /// <summary>
        /// set Kontrollschild
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Kontrollschild">Kontrollschild</param>
        /// <returns>osetKontrollschild</returns>
        public override void setKontrollschild(long sysID, string Kontrollschild)
        {
            simpleSetterDao.setKontrollschild(sysID, Kontrollschild);
        }

        /// <summary>
        /// set Stammnummer
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Stammnummer">Stammnummer</param>
        /// <returns>osetStammnummerDto</returns>
        public override void setStammnummer(long sysID, string Stammnummer)
        {
            simpleSetterDao.setStammnummer(sysID, Stammnummer);
        }

        /// <summary>
        /// set Farbe 
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Farbe">Farbe</param>
        /// <returns>osetFarbeDto</returns>
        public override void setFarbe(long sysID, string Farbe)
        {
            simpleSetterDao.setFarbe(sysID, Farbe);
        }

        /// <summary>
        /// set Chassisnummer
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Chassisnummer">Chassisnummer</param>
        /// <returns>osetChassisnummer</returns>
        public override void setChassisnummer(long sysID, string Chassisnummer)
        {
            simpleSetterDao.setChassisnummer(sysID, Chassisnummer);
        }

        /// <summary>
        /// set Ablieferdatum
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Ablieferdatum">Ablieferdatum</param>
        /// <returns>osetAblieferdatumDto</returns>
        public override void setAblieferdatum(long sysID, DateTime Ablieferdatum)
        {
            simpleSetterDao.setAblieferdatum(sysID, Ablieferdatum);
        }
    }
}