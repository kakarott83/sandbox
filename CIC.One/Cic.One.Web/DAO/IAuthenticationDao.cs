using Cic.One.DTO;
using Cic.One.Web.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.Web.DAO
{
    /// <summary>
    /// DAO Interface for user authentication and logon infos
    /// </summary>
    public interface IAuthenticationDao
    {
        /// <summary>
        /// determine Brand attributes
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        AttributeMapDto getBrandAttributes(long sysperole);

        /// <summary>
        /// determine KAM attributes
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        AttributeMapDto getKamAttributes(long sysperole);

        /// <summary>
        /// determine NAMEBPLANES
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        AttributeMapDto getBPELanes(long syswfuser);

        /// <summary>
		/// determine User's Filialen
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
		AttributeMapDto getUsersFilialen (long syswfuser);

        /// <summary>
        /// determine EPOS_ADMIN rgr 
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        AttributeMapDto getEposAdminAttribute(long sysperole);

        /// <summary>
        /// Returns all parent workflow users for the perole
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        WfuserDto[] getWfusers(long sysperole);

        /// <summary>
        /// finds a certain wfuser
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        WfuserDto getWfuserDto(igetWfuserDto input);

        /// <summary>
        /// gets the wfuser password
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        String getWfuserPassword(long syswfuser);

         /// <summary>
        /// gets the wfuser code from the wfsso table by looking for the kerberos userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        String getSSOWfuser(String userName);

        /// <summary>
        /// returns the workflow user
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        WfuserDto getWfuser(long syswfuser);

        /// <summary>
        /// returns the pusers defaultperole
        /// </summary>
        /// <param name="extid"></param>
        /// <returns></returns>
        long getDefaultperole(String extid);

        /// <summary>
        /// get the puser password by extid
        /// </summary>
        /// <param name="extid"></param>
        /// <returns></returns>
        String getPasswordByExtId(String extid);

        /// <summary>
        /// gets the sysperson by the given value
        /// </summary>
        /// <param name="value">syspuser or sysperole</param>
        /// <param name="sysperole">when false value is syspuser else sysperole</param>
        /// <returns></returns>
        long getSysPerson(long value, bool sysperole);

        /// <summary>
        /// gets the roletype typ for the perole
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        long getRoletypeTypByPerole(long sysperole);

        /// <summary>
        /// gets the isocode fro the sysperson
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        AttributeValue getIsoCode(long sysperson);

        /// <summary>
        /// gets all available Isocodes
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        List<AttributeValue> getAllIsocodes();

        /// <summary>
        /// returns the puser of the wfuser
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        long getPUser(long syswfuser);

        /// <summary>
        /// returns the bildwelt for the haendler
        /// </summary>
        /// <param name="haendlerSysPerole"></param>
        /// <returns></returns>
        BildweltInfoDto getBildwelten(long haendlerSysPerole);


        /// <summary>
        /// return the default bildwelt
        /// </summary>
        /// <returns></returns>
        BildweltInfoDto getDefaultBildwelt();


        /// <summary>
        /// fetch all Vertriebskanäle
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        List<long> getChannels(long sysperole);

        /// <summary>
        /// returns the workflow user by code
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        WfuserDto getWfuserByCode(String code);

         /// <summary>
        /// returns the puser EXTERNEID for the wfuser
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        String getPUserId(long syswfuser);

        /// <summary>
        /// determine ABWICKLUNGSORT Attribute
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        AttributeMapDto getAbwicklungsortAttribute(long syswfuser);

        /// <summary>
        /// Changes the Users' password
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="newpassword"></param>
        void changeUserPassword(long syswfuser, String newpassword);

        /// <summary>
        /// determine EPOS_BEDINGUNGEN Attribute
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        AttributeMapDto getEposBedingungen(long sysperole);


        /// <summary>
        /// determine BPEROLES
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        AttributeMapDto getBPERoles(long syswfuser);

    }
}
