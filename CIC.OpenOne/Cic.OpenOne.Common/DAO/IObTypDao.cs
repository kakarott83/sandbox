using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF4.Model;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Object type Data Access Object Interface
    /// </summary>
    public interface IObTypDao
    {
        /// <summary>
        /// Object Type Descendenants
        /// </summary>
        /// <param name="sysobtyp">Object type</param>
        /// <returns>List of Object Descendant types</returns>
        List<long> getObTypDescendants(long sysobtyp);

        /// <summary>
        /// Object Type Ancestors
        /// </summary>
        /// <param name="sysobtyp">Object Type</param>
        /// <returns>List of Object Types Ancestors</returns>
        List<long> getObTypAscendants(long sysobtyp);

        /// <summary>
        /// Prhgroups for the role and obtyp
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        List<long> getPrhGroups(long sysperole, long sysobtyp, DateTime perDate);

        /// <summary>
        /// Prhgroups for the role
        /// </summary>
        /// <param name="sysperole">Personenrollen ID</param>
        /// <returns>Handelsgruppenliste</returns>
        List<long> getPrhGroupsbyPerole(long sysperole);

        /// <summary>
        /// Schlüssel des Händlers aus dem des Angestellten ermittlen.
        /// </summary>
        /// <param name="sysperole">Angestellten Schlüssel</param>
        /// <returns>Händlerschlüssel</returns>
        long getHaendlerByEmployee(long sysperole);

        /// <summary>
        /// Schlüssel des PersonID aus PEROLE ermittlen.
        /// </summary>
        /// <param name="sysperole">Perole</param>
        /// <returns>PersonID</returns>
        long getPersonIDByPEROLE(long sysperole);

        /// <summary>
        ///  Person aus PEROLE ermittlen.
        /// </summary>
        /// <param name="sysperole">Perole</param>
        /// <returns>PersonID</returns>
        PERSON getPersonByPEROLE(long sysperole);
       

        /// <summary>
        /// Ermitteln der Objekttypdaten eines Fahrzeugs
        /// </summary>
        /// <param name="sysobtyp">Objecttyp</param>
        /// <returns>Daten</returns>
        ObtypDataRestwertDto getObTypData(long sysobtyp);

        /// <summary>
        /// Ermitteln der Objekttypdaten eines Fahrzeugs nach Nationalem Fahrzeugcode
        /// </summary>
        /// <param name="nationalVC">Nationaler Fahrzeugcode</param>
        /// <returns>Daten</returns>
        ObtypDataRestwertDto getObTypDataByNVC(long nationalVC);

        /// <summary>
        /// Ermitteln der Objekttypdaten eines Fahrzeugs nach Nationalem Fahrzeugcode
        /// </summary>
        /// <param name="nationalVC"></param>
        /// <returns></returns>
        ObtypDataRestwertDto getObTypDataByNVCByString(string nationalVC);
    }
}
