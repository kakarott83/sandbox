using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// abstract business object for setting simple fields
    /// </summary>
    public abstract class AbstractSimpleSetterBo : ISimpleSetterBo
    {
        /// <summary>
        /// data access object to use
        /// </summary>
        protected ISimpleSetterDao simpleSetterDao;

        /// <summary>
        /// constructs a abstractSimpleSetter business object
        /// </summary>
        /// <param name="simpleSetterDao">the data access object to use</param>
        public AbstractSimpleSetterBo(ISimpleSetterDao simpleSetterDao)
        {
            this.simpleSetterDao = simpleSetterDao;
        }

        /// <summary>
        /// set Geschaeftsart 
        /// </summary>
        /// <param name="sysprchannel">Key des Channels</param>
        /// <returns>osetGeschaeftsartDto</returns>
        public abstract string setGeschaeftsart(long sysprchannel);

        /// <summary>
        /// set Kontrollschild
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Kontrollschild">Kontollschild</param>
        /// <returns>osetKontrollschild</returns>
        public abstract void setKontrollschild(long sysID, string Kontrollschild);

        /// <summary>
        /// set Stammnummer
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Stammnummer">Stammnummer</param>
        /// <returns>osetStammnummerDto</returns>
        public abstract void setStammnummer(long sysID, string Stammnummer);

        /// <summary>
        /// set Farbe
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Farbe">Farbe</param>
        /// <returns>osetFarbeDto</returns>
        public abstract void setFarbe(long sysID, string Farbe);

        /// <summary>
        /// set Chassisnummer
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Chassisnummer">Chassisnummer</param>
        /// <returns>osetChassisnummer</returns>
        public abstract void setChassisnummer(long sysID, string Chassisnummer);

        /// <summary>
        /// set Ablieferdatum
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Ablieferdatum">Ablieferdatum</param>
        /// <returns>osetAblieferdatumDto</returns>
        public abstract void setAblieferdatum(long sysID, DateTime Ablieferdatum);
    }
}