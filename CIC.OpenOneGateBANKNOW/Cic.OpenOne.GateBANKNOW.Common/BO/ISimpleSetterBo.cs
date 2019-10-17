using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// defines interface for business objects setting simple fields
    /// </summary>
    public interface ISimpleSetterBo
    {
        /// <summary>
        /// set Geschaeftsart 
        /// </summary>
        /// <param name="sysprchannel">key des Channels</param>
        /// <returns>osetGeschaeftsartDto</returns>
        string setGeschaeftsart(long sysprchannel);

        /// <summary>
        /// set Kontrollschild
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Kontrollschild">Kontrollschild</param>
        /// <returns>osetKontrollschild</returns>
        void setKontrollschild(long sysID, string Kontrollschild);

        /// <summary>
        /// set Stammnummer
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Stammnummer">Stammnummer</param>
        /// <returns>osetStammnummerDto</returns>
        void setStammnummer(long sysID, string Stammnummer);

        /// <summary>
        /// set Farbe
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Farbe">Farbe</param>
        /// <returns>osetFarbeDto</returns>
        void setFarbe(long sysID, string Farbe);

        /// <summary>
        /// set Chassisnummer
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Chassisnummer">Chassisnummer</param>
        /// <returns>osetChassisnummer</returns>
        void setChassisnummer(long sysID, string Chassisnummer);

        /// <summary>
        /// set Ablieferdatum
        /// </summary>
        /// <param name="sysID">sysID des Antrags</param>
        /// <param name="Ablieferdatum">Ablieferdatum</param>
        /// <returns>osetAblieferdatumDto</returns>
        void setAblieferdatum(long sysID, DateTime Ablieferdatum);
    }
}