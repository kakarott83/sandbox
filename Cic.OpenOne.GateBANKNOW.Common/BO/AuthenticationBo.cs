using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.Util.SOAP;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.Resources;
using Cic.OpenOne.Common.Model.Prisma;


namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Type of validation for data set validation
    /// </summary>
    public enum ValidationType
    {
        /// <summary>
        /// ANGEBOT
        /// </summary>
        /// 
        ANGEBOT,
        /// <summary>
        /// ANTRAG
        /// </summary>
        ANTRAG,

        /// <summary>
        /// Interessent
        /// </summary>
        IT,

        /// <summary>
        /// Vertrag
        /// </summary>
        VT
    }

    class BildweltInfo
    {
        public String NAME { get; set; }
        public int DEFAULTFLAG { get; set; }
        public long SYSPRBILDWELT { get; set; }
    }

    /// <summary>
    /// BO for User Autorization
    /// </summary>
    public class AuthenticationBo
    {

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const String GETISOCODE = "select ctlang.isocode value, ctlang.sysctlang id from person,ctlang where ctlang.sysctlang=person.sysctlang and person.sysperson=:sysperson";
        private const String GETISOCODES = "select ctlang.isocode value, ctlang.sysctlang id from ctlang where flagtranslate=1";

        private const String QUERYBILDWELTEN =
                                        "SELECT DEFAULTFLAG,  NAME,  prbildwelt.SYSPRBILDWELT " +
                                        " FROM prbildweltM,  prbildwelt " +
                                        " WHERE prbildweltM.sysprbildwelt = prbildwelt.sysprbildwelt AND prbildweltM.sysPerole = :haendlerSysPerole" +
                                            " AND prbildweltM.sysperole  > 0 AND prbildweltM.activeflag   = 1 AND prbildwelt.activeflag    = 1 " +
                                            " AND ( prbildweltM.validfrom  IS NULL " +
                                                    " OR prbildweltM.validfrom    <= :perDate " +
                                                    " OR prbildweltM.validfrom     = to_date('01.01.0111' , 'dd.MM.yyyy') ) " +
                                            " AND ( prbildweltM.validuntil IS NULL " +
                                                    " OR prbildweltM.validuntil   >= :perDate " +
                                                    " OR prbildweltM.validuntil    = to_date('01.01.0111' , 'dd.MM.yyyy')) " +
                                        " ORDER BY prbildweltM.rank ";

        private const String QUERYDEFAULTBILDWELT = "select sysprbildwelt from prbildwelt where activeflag=1 and defaultflag=1";

        private const String DEFAULTSTRING = "default";

        private const String QUERYPERMISSION = "SELECT CIC.CIC_PEROLE_UTILS.ChkObjInPEUNI(:sysPUSER, :type, :sysid, :perDate) from dual";
        

        /// <summary>
        /// Constructor
        /// </summary>
        public AuthenticationBo()
        {
        }

        /// <summary>
        /// Validates if the perole or its parents is not inactive
        /// throws an SecurityException if sysperole is not active
        /// </summary>
        /// <param name="sysperole">sysperole of the user</param>
        public static void validateActivePerole(long sysperole)
        {
            PeRoleUtil.clearisActiveCache(sysperole);
            if (!PeRoleUtil.isActive(sysperole) || sysperole == 0)
            {
                throw new SecurityException(ExceptionMessages.F_00008_Perole_Inactive, ExceptionMessages.F_00008_Perole_Inactive, OpenOne.Common.DTO.MessageType.Info);
            }
        }

        /// <summary>
        ///  Validates the access permission of the user to the data
        /// </summary>
        /// <param name="type">Validation Type</param>
        /// <param name="sysPUSER">id of puser</param>
        /// <param name="sysid">id of entity</param>
        /// <param name="perDate">date</param>
        public static void validateUserPermission(ValidationType type, long sysPUSER, long sysid, DateTime perDate)
        {
            if (sysid < 1)
            {
                return;
            }
            if (sysPUSER < 1)
            {
                return;
            }

            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "sysPUSER", Value = sysPUSER } ,
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "type", Value = type.ToString() } ,
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value =sysid } ,
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(perDate) } 
                                    };
                long permission = ctx.ExecuteStoreQuery<long>(QUERYPERMISSION, pars).FirstOrDefault();

                if (permission > 0)
                {
                    return;
                }

                throw new SecurityException(ExceptionMessages.E_30002_NoPermission, ExceptionMessages.E_30002_NoPermission, OpenOne.Common.DTO.MessageType.Info);
            }
        }

        /// <summary>
        /// Returns the dealers sysperole
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public long getDealerId(String userName, String password)
        {
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                MembershipProvider prov = new MembershipProvider(ctx);

                 List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "externeid", Value = userName });
                long sysperson = ctx.ExecuteStoreQuery<long>("select sysperson from puser where externeid=:externeid", parameters.ToArray()).FirstOrDefault();

                
                //ROLES-------------------------------------
                List<Cic.OpenOne.Common.DTO.PeroleDto> peroles = prov.getUserRoles(sysperson);
                // Eigentlich ist die Schleife hier überflüssig, weil der angemeldete User nur eine Role haben darf...
                foreach (Cic.OpenOne.Common.DTO.PeroleDto role in peroles)
                {
                    if (!PeRoleUtil.isActive(role.SYSPEROLE))
                    {
                        continue;
                    }
                    return role.SYSPEROLE;

                }

                return 0;

            }
        }
        /// <summary>
        /// Authentifizieren
        /// </summary>
        /// <param name="userName">benutzername</param>
        /// <param name="password">Passwort</param>
        /// <param name="loginType">Anmeldetyp</param>
        /// <param name="authInfo">Authentifizierungsdaten</param>
        /// <returns>Authentifizierungsdaten Ausgang</returns>
        public oExtendedUserDto authenticate(String userName, String password, int loginType, ref oExtendedUserDto authInfo)
        {
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                MembershipProvider prov = new MembershipProvider(ctx);

                //fill user information
                DefaultMessageHeader defMsgHeader = new DefaultMessageHeader();
                defMsgHeader.UserName = userName;
                defMsgHeader.Password = password;
                if (String.IsNullOrEmpty(userName))
                {
                    throw new Exception("invalid login: empty username");
                }
                // da wird username und password mit CurrentPrincipal.Identity und MasterPassword überschrieben, wenn sie gefunden wurden
                MembershipProvider.validateSAML(defMsgHeader);

                _log.Info("Username to authenticate: " + defMsgHeader.UserName + ", userType: " + 
                           loginType + " (" + (loginType == MembershipProvider.USER_TYPE_PUSER ? "USER_TYPE_PUSER" : "USER_TYPE_WFUSER") + ")");

                MembershipUserValidationInfo info = prov.ValidateUser(defMsgHeader.UserName, defMsgHeader.Password, loginType);

                //validate the login and throw exception if no valid user
                CredentialContext.validateMembership(info);
                if (info.sysPEROLE == 0)
                {
                    List<Cic.OpenOne.Common.DTO.PeroleDto> roles = prov.getUserRoles(info.sysPERSON);
                    if (roles != null && roles.Count > 0)
                    {
                        info.sysPEROLE = (from f in roles
                                          where  f.INACTIVEFLAG == 0
                                          select f.SYSPEROLE).FirstOrDefault();
                    }
                }
                validateActivePerole(info.sysPEROLE);

                //ROLES-------------------------------------
                List<Cic.OpenOne.Common.DTO.PeroleDto> peroles = prov.getUserRoles(info.sysPERSON);

                bool hasDefault = false;
                authInfo.rolemap = new List<RoleMap>();

                long haendlerSysPerole = 0;

                // Eigentlich ist die Schleife hier überflüssig, weil der angemeldete User nur eine Role haben darf...
                foreach (Cic.OpenOne.Common.DTO.PeroleDto role in peroles)
                {
                    RoleMap rm = new RoleMap();
                    rm.sysPerole = role.SYSPEROLE;
                    rm.inactive = !PeRoleUtil.isActive(role.SYSPEROLE);

                    if (role.SYSPEROLE == info.sysPEROLE)
                    {
                        rm.isDefault = true;
                        hasDefault = true;
                    }
                    try
                    {
                       
                            rm.roleName = role.ROLETYPENAME;
                            // wenn die role ein Verkäufer ist, dann hole BildWelten für seinen sysParent (Händler)
                            if (role.ROLETYPETYP == (int?)RoleTypeTyp.VERKAEUFER && role.SYSPARENT >0)
                            {
                                haendlerSysPerole = (long)role.SYSPARENT;
                            }
                    }
                    catch (Exception ex)
                    {
                        // Entsorgt die nicht verwendung der Exception
                        String message = ex.Message;
                    }//no roletype available

                    authInfo.rolemap.Add(rm);
                }

                if (!hasDefault && authInfo.rolemap.Count > 0)
                {
                    authInfo.rolemap.FirstOrDefault().isDefault = true;
                }

                //---------Fill Attributes--------------------------------------
                authInfo.attributmap = new List<AttributeMap>();

                //language attributes-----------------------------

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = info.sysPERSON });

                AttributeValue defLang = ctx.ExecuteStoreQuery<AttributeValue>(GETISOCODE, parameters.ToArray()).FirstOrDefault();

                AttributeMap langAtt = new AttributeMap();
                langAtt.attributeName = AttributeName.ISOLanguageCode;
                langAtt.attributeValues = ctx.ExecuteStoreQuery<AttributeValue>(GETISOCODES, null).ToList();
                langAtt.defaultValue = defLang != null ? defLang.value : langAtt.attributeValues.FirstOrDefault().value;
                authInfo.attributmap.Add(langAtt);

                // Bildwelten -----------
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) },
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "haendlerSysPerole", Value = haendlerSysPerole } };
                // Ein Händler kann maximal einer Bildwelt zugeordnet sein
                BildweltInfo bildWeltInfo = ctx.ExecuteStoreQuery<BildweltInfo>(QUERYBILDWELTEN, pars).FirstOrDefault();

                AttributeMap bwAttributeMap = new AttributeMap();
                bwAttributeMap.attributeName = AttributeName.BildweltCode;
                bwAttributeMap.attributeValues = new List<AttributeValue>();
                bwAttributeMap.defaultValue = DEFAULTSTRING;

                AttributeValue bwAttributeValue = new AttributeValue();
                if (bildWeltInfo == null)
                {
                    bwAttributeValue.value = DEFAULTSTRING;
                    bwAttributeValue.id = ctx.ExecuteStoreQuery<long>(QUERYDEFAULTBILDWELT, null).FirstOrDefault();
                }
                else
                {
                    bwAttributeValue.value = bildWeltInfo.NAME;
                    bwAttributeValue.id = bildWeltInfo.SYSPRBILDWELT;

                    bwAttributeMap.defaultValue = bildWeltInfo.NAME;
                }
                bwAttributeMap.attributeValues.Add(bwAttributeValue);

                authInfo.attributmap.Add(bwAttributeMap);
                // Bildwelten ende

                IRightsMapBo rightsMapBo = BOFactory.getInstance().createRightsMapBo();
                authInfo.rightsmap = rightsMapBo.getRightsForWFUser(info.sysWFUSER);
                authInfo.username = defMsgHeader.UserName;

                return authInfo;
            }
        }
    }
}