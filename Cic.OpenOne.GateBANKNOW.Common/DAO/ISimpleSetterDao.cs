using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// defines interface for data access objects setting simple fields
    /// </summary>
    public interface ISimpleSetterDao
    {
        /// <summary>
        /// set Geschaeftsart property of antrag
        /// </summary>
        string setGeschaeftsart(long Sysprchannel); 

        /// <summary>
        /// set Farbe property of antrag
        /// </summary>
        void setFarbe(long sysID, string Farbe); 

        /// <summary>
        /// set Kontrollschild property of antrag
        /// </summary>
        void setKontrollschild(long sysID, string Kontrollschild); 

        /// <summary>
        /// set Stammnummer property of antrag
        /// </summary>
        void setStammnummer(long sysID, string Stammnummer);

        /// <summary>
        /// set Chassisnummer property of antrag
        /// </summary>
        void setChassisnummer(long sysID, string Chassisnummer);

        /// <summary>
        /// set Ablieferdatum property of antrag
        /// </summary>
        void setAblieferdatum(long sysID, DateTime Ablieferdatum); 
    }
}