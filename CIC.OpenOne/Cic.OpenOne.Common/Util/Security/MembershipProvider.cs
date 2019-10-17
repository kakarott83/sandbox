using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Extension;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.Util.Security
{
    /// <summary>
    /// MembershipProvider
    /// </summary>
    [System.CLSCompliant(true)]
    public sealed class MembershipProvider //: System.Web.Security.MembershipProvider
    {
        /// <summary>
        /// USER_TYPE_PUSER
        /// </summary>
        public static int USER_TYPE_PUSER = 0;
        /// <summary>
        /// USER_TYPE_WFUSER
        /// </summary>
        public static int USER_TYPE_WFUSER = 1;

        private const String LOGAREA = ".NET-LOGIN";
        private DdOwExtended owEntities;
        private static string CnstMasterPassword = "XAKLOP901ASDDDA";
        private int _PasswordFormat = 0;
        private const int Encrypted = 1;
        private DateTime nullDate = new DateTime(1800, 1, 1);
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool SAMLENABLED = true;//when true and PUSER, always use default Masterpassword
        private static String QUERYPEROLE = @"select perole.*,roletype.typ roletypetyp,roletype.name roletypename from perole, roletype where perole.sysroletype=roletype.sysroletype(+)
                                          and (perole.validfrom is null or perole.validfrom<= trunc(sysdate)  or perole.validfrom<=to_date('01.01.0111' , 'dd.MM.yyyy')   )
                                          and (perole.validuntil is null or perole.validuntil>= trunc(sysdate)  or perole.validuntil<=to_date('01.01.0111' , 'dd.MM.yyyy'))
                                          and perole.sysperson=:sysperson";
        private static bool samlConfigured = false;

        /// <summary>
        /// MembershipProvider
        /// </summary>
        /// <param name="owEntities"></param>
        public MembershipProvider(DdOwExtended owEntities)
        {
            this.owEntities = owEntities;
            cfgSaml();
        }

        private static void cfgSaml()
        {
            if (samlConfigured) return;

            if (Cic.OpenOne.Common.Properties.Config.Default.SAML != null && "false".Equals(Cic.OpenOne.Common.Properties.Config.Default.SAML.ToLower()))
                SAMLENABLED = false;
            samlConfigured = true;
        }
       

        public static String getMasterPasswd(bool isWFUser)
        {
            // Master password
            string masterPassword = CnstMasterPassword;
            
            //SAML always uses the default masterPassword
            if (SAMLENABLED && !isWFUser) return masterPassword;

            try
            {
                // Get PreSharedKey-Params (default from Config.Settings or from web.config)
                string[] keyPath = Cic.OpenOne.Common.Properties.Config.Default.PreSharedKeyPath.Split('/');
                if (keyPath.Length > 2)
                {
                    // get the encoded Password from the database
                    masterPassword = AppConfig.Instance.GetEntry(keyPath[1], keyPath[2], CnstMasterPassword, keyPath[0]);
                    if (!Cic.OpenOne.Common.Properties.Config.Default.PreSharedKeyType.Equals(PUserUtil.CnstPreSharedKeyTypeTXT))
                        masterPassword = PUserUtil.DecryptPassword(masterPassword);
                }
            }
            catch
            {
                // Ignore exception
            }
            return masterPassword;
        }

        /// <summary>
        /// validateSAML
        /// embeds the PKI SAML auth info into the header-info
        /// </summary>
        /// <param name="messageHeader"></param>
        public static void validateSAML(Cic.OpenOne.Common.Util.SOAP.DefaultMessageHeader messageHeader)
        {
            cfgSaml();
            _log.Debug("SAML-ENABLED: " + SAMLENABLED);
            if (!SAMLENABLED) return;

            //validate CS PKI SAML-User Info
            try
            {
                var ci = Thread.CurrentPrincipal.Identity as Microsoft.IdentityModel.Claims.IClaimsIdentity;
                if (ci != null)
                {
                    _log.Debug("SAML-User-Information found.");
                    if (ci.IsAuthenticated)
                    {
                        messageHeader.UserName = ci.Name;
                        messageHeader.Password = getMasterPasswd(false);
                    }
                    _log.Debug("CurrentPrincipal.IsAuthenticated = " + ci.IsAuthenticated + ", CurrentPrincipal.Identity: " + ci.Name + ", using messageHeader.UserName = '" + messageHeader.UserName + "'.");
                }
                else
                    _log.Debug("CurrentPrincipal.Identity = null, messageHeader.UserName = '" + messageHeader.UserName + "'.");
            }
            catch (Exception)
            {
                _log.Debug("SAML-User-Information not found, using messageHeader.UserName = '" + messageHeader.UserName + "' and preshared Key.");
            }
            //----------------end
        }

        /// <summary>
        /// if input is the masterpassword returns the
        ///     puser-password for usertype puser
        ///     master-password für usertype wfuser
        /// else the given password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        private String getRealPassword(string username, string password, int userType)
        {
            try
            {
                string masterPassword = getMasterPasswd(userType == USER_TYPE_WFUSER);
                if (password.Equals(masterPassword))
                {
                    return masterPassword;
                    /*// If the Master password is valid the RealPassword is replaced by PUSER password
                    if (userType == USER_TYPE_PUSER)
                        return GetPortalUserPassword(username, userType);
                    else
                        return masterPassword;*/
                }
            }
            catch
            {
                // Ignore exception
            }
            return password;
        }

        /// <summary>
        /// validates the credentials
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public MembershipUserValidationInfo ValidateUser(string userName, string password, int userType)
        {
            MembershipUserValidationInfo membershipUserValidationInfo = new MembershipUserValidationInfo();

            password = getRealPassword(userName, password, userType);

            //Validate System locked state
            long disabled = owEntities.ExecuteStoreQuery<long>("select disabled from wfsys",null).FirstOrDefault();

            membershipUserValidationInfo.systemlocked = disabled == 1;
            _log.Debug("membershipUserValidationInfo.systemlocked : " + membershipUserValidationInfo.systemlocked);
           

            // Validate portal and workflow user
            PUSER puser = null;
            WFUSER wfuser = null;
            if (userType == USER_TYPE_PUSER)
            {
                puser = MyExtendedValidatePortalUser(userName, password, membershipUserValidationInfo, userType);
                if (puser != null)
                {
                    _log.Debug("PUser found: puser.SYSPUSER = " + puser.SYSPUSER);
                    membershipUserValidationInfo.sysPUSER = puser.SYSPUSER;
                    wfuser = MyExtendedValidateWorkflowUser(puser, membershipUserValidationInfo,true);
                    if (wfuser == null)
                        _log.Debug("WFUser not found. Username = " + userName + ", userType = USER_TYPE_PUSER, sysPUSER = " + membershipUserValidationInfo.sysPUSER + ".");
                }
                else
                    _log.Debug("PUser not found. Username = " + userName + ", userType = USER_TYPE_PUSER.");
            }
            else
            {
                wfuser = MyExtendedValidateWorkflowUser(userName, password, membershipUserValidationInfo, userType);
                //Case BNOW: Login with WFUSER(Default in webservice), but WFUSER-CODE is different from the entered PUSER-Externeid
                //So we try to look for a valid PUSER-Login with the given userName and Password
                if(wfuser==null)
                {
                    puser = MyExtendedValidatePortalUser(userName, password, membershipUserValidationInfo, USER_TYPE_PUSER);
                    if (puser != null)
                    {
                        _log.Debug("PUser found: puser.SYSPUSER = " + puser.SYSPUSER);
                        membershipUserValidationInfo.sysPUSER = puser.SYSPUSER;
                        wfuser = MyExtendedValidateWorkflowUser(puser, membershipUserValidationInfo,false);
                        if (wfuser == null)
                            _log.Debug("WFUser not found. Username = " + userName + ", userType = USER_TYPE_PUSER, sysPUSER = " + membershipUserValidationInfo.sysPUSER + ".");
                    }
                    else
                        _log.Debug("PUser not found. Username = " + userName + ", userType = USER_TYPE_PUSER.");
                    
                }
            }
            if (wfuser != null)
                _log.Debug("WFUser found: wfuser.SYSWFUSER = " + wfuser.SYSWFUSER);

            if (membershipUserValidationInfo.MembershipUserValidationStatus != MembershipUserValidationStatus.Valid)
            {
                // PUSER or WFUSER is not valid
                _log.Debug("PUSER or WFUSER is not valid. MembershipUserValidationStatusReason: " + membershipUserValidationInfo.MembershipUserValidationStatusReason);
                return membershipUserValidationInfo;
            }
            long? sysperson = (userType == USER_TYPE_PUSER) ? puser.SYSPERSON : wfuser.SYSPERSON;
            if (!sysperson.HasValue && wfuser!=null && wfuser.SYSPERSON.HasValue)
            {
                sysperson = wfuser.SYSPERSON.Value;
            }
            if (!sysperson.HasValue)
            {
                membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.ValidPersonNotFound;
                membershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidPersonNotFound";
                String logMessage = membershipUserValidationInfo.MembershipUserValidationStatusReason + " (" +
                    ((userType == USER_TYPE_PUSER) ? "PUSER.SYSPERSON" : "WFUSER.SYSPERSON") + " empty for user " + userName + ")";
                LogUtil.addWFLog(LOGAREA, logMessage,1);
                _log.Debug(logMessage);
                return membershipUserValidationInfo;
            }
            membershipUserValidationInfo.sysWFUSER = wfuser.SYSWFUSER;
            membershipUserValidationInfo.WFUSERCODE = wfuser.CODE;
            membershipUserValidationInfo.sysPERSON = (long)sysperson;
            _log.Debug("UserValidation successful. sysPERSON = " + membershipUserValidationInfo.sysPERSON);
            /*
            if (membershipUserValidationInfo.systemlocked)
            {
                
                membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.Systemlocked;
                membershipUserValidationInfo.MembershipUserValidationStatusReason = wfsys.DISABLEDREASON;
                _log.Debug("wfsys.DISABLEDREASON : " + wfsys.DISABLEDREASON);
            }*/

            //Wartungsmodus - BNRNEUN/BANKNOW-101
            try
            {
                long DISABLEB2B =
                    owEntities.ExecuteStoreQuery<long>("select DISABLEB2B from wfsys", null).FirstOrDefault();
                if (DISABLEB2B > 0) //Wartungsmodus aktiv
                {
                    if (puser != null && puser.MAINTMODEB2B.HasValue && puser.MAINTMODEB2B.Value != 1)
                    {
                        membershipUserValidationInfo.MembershipUserValidationStatus =
                            MembershipUserValidationStatus.Systemlocked;
                        membershipUserValidationInfo.MembershipUserValidationStatusReason = "Wartungsmodus aktiv";
                    }
                    /*    using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "mode", Value = wfsys.MaintGroup });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysWFUser", Value = membershipUserValidationInfo.sysWFUSER });

                        long sysrror = ctx.ExecuteStoreQuery<long>("select sysrrorgrnm from rgm,rgr,rrorgrnm where rgm.sysrgr=rgr.sysrgr and rgr.name = rrorgrnm.codergr and coderro=:mode and syswfuser=:sysWFUser", parameters.ToArray()).FirstOrDefault();
                        if (sysrror == 0)
                        {
                            membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.Systemlocked;
                            membershipUserValidationInfo.MembershipUserValidationStatusReason = "Wartungsmodus aktiv";
                        }
                    }*/

                }
            }
            catch (Exception e)
            {
                _log.Warn("Reading DISABLEB2B not possible: "+e.Message);
            }

            return membershipUserValidationInfo;
        }

        /// <summary>
        /// returns all valid roles for the person
        /// </summary>
        /// <param name="sysPERSON"></param>
        /// <returns></returns>
        public List<PeroleDto> getUserRoles(long sysPERSON) 
        {
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = sysPERSON });
                return ctx.ExecuteStoreQuery<PeroleDto>(QUERYPEROLE, parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// Validates the user credentials against the role and brand
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public MembershipUserValidationInfo ExtendedValidateUser(string userName, string password, long sysPEROLE, int userType)
        {
            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            MembershipUserValidationInfo membershipUserValidationInfo = ValidateUser(userName, password, userType);
           
            _log.Debug("Duration ValidateUser: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if (membershipUserValidationInfo.MembershipUserValidationStatus != MembershipUserValidationStatus.Valid)
                return membershipUserValidationInfo;

            PeroleDto perole = null;
            

            //Validate chosen PEROLE
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                if (sysPEROLE == 0)
                {
                    
                    sysPEROLE = ctx.ExecuteStoreQuery<long>("select sysperole from perole, roletype where roletype.sysroletype=perole.sysroletype and (perole.validuntil = TO_DATE ('01.01.0111','dd.mm.yyyy') OR perole.validuntil >= sysdate  OR perole.validuntil IS NULL) AND (perole.validfrom = TO_DATE ('01.01.0111','dd.mm.yyyy') OR perole.validfrom <= sysdate  OR perole.validfrom IS NULL) and perole.sysperson=" + membershipUserValidationInfo.sysPERSON, null).FirstOrDefault();
                    _log.Debug("no perole given, fetched " + sysPEROLE);
                }


                perole = ctx.ExecuteStoreQuery<PeroleDto>("select validfrom, validuntil, sysperole, roletype.typ from perole, roletype where roletype.sysroletype=perole.sysroletype and perole.sysperole="+sysPEROLE,null).FirstOrDefault();
               
                _log.Debug("Duration Perole: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                // Check PEROLE
                if (perole == null || !MyCheckIsValid(perole.VALIDFROM, perole.VALIDUNTIL))
                {
                    // ValidRoleNotFound
                    membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.ValidRoleNotFound;
                    membershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidRoleNotFound";
                    LogUtil.addWFLog(LOGAREA, membershipUserValidationInfo.MembershipUserValidationStatusReason +
                        " (perole.sysperole=" + sysPEROLE + " not found OR perole valid-range invalid for user " + userName + ")",1);
                    return membershipUserValidationInfo;
                }

                membershipUserValidationInfo.sysPEROLE = sysPEROLE;

                bool IsIM = false;
              
                IsIM = (IsIM || (
                    perole.ROLETYPETYP == (long)Cic.OpenOne.Common.Model.DdOl.RoleTypeTyp.BANKMITARBEITER));
                membershipUserValidationInfo.IsInternalMitarbeiter = IsIM;
                _log.Debug("Duration Roletype: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                // Deliver PERSON
                MembershipUserValidationInfo person = ctx.ExecuteStoreQuery<MembershipUserValidationInfo>("select person.sysperson, person.syspuser from perole, person where perole.sysperson=person.sysperson and perole.sysperole=" + sysPEROLE, null).FirstOrDefault();//Cic.OpenOne.Common.Model.DdOl.PERSON.getPERSONByPeRole(ctx, sysPEROLE);
                if (person == null)
                {
                    // ValidPersonNotFound
                    membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.ValidPersonNotFound;
                    membershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidPersonNotFound";
                    // PERSON not valid
                    LogUtil.addWFLog(LOGAREA, membershipUserValidationInfo.MembershipUserValidationStatusReason + " (person on perole " + sysPEROLE + " empty for user " + userName + ")",1);

                    return membershipUserValidationInfo;
                }
                _log.Debug("Duration Person: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                // Deliver TypPUser
                int TypPUser = ctx.ExecuteStoreQuery<int>("select PTYPPUSER from cicconf", null).FirstOrDefault();
                bool vpnf = (TypPUser == 0 && person.sysPERSON != membershipUserValidationInfo.sysPERSON) ||
                    (TypPUser == 1 && userType == USER_TYPE_PUSER && person.sysPUSER != membershipUserValidationInfo.sysPUSER);
                if (vpnf)
                {
                    // ValidPersonNotFound
                    membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.ValidPersonNotFound;
                    membershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidPersonNotFound";
                    LogUtil.addWFLog(LOGAREA, membershipUserValidationInfo.MembershipUserValidationStatusReason + " (" +
                        ((TypPUser == 0) ? "PERSON.SYSPERSON" : "PERSON.SYSPUSER") + " does not match for user " + userName + ")",1);

                    // PERSON not valid
                    return membershipUserValidationInfo;
                }
                if (TypPUser > 1)
                {
                    throw new ArgumentOutOfRangeException("TypPUser");
                }
                _log.Debug("Duration PType: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                /* if (sysBRAND > 0)//check for brand-validity only if brand given, brand is accesed through roletype 6
                 {
                     // Find the PeRole with VP typ in the perole tree
                     vpPEROLE = Cic.OpenOne.Common.Model.DdOl.PEROLE.FindRootPEROLEObjByRoleType(ctx, sysPEROLE, Cic.OpenOne.Common.Model.DdOl.PEROLE.CnstVPRoleTypeNumber);

                     if (vpPEROLE == null)
                     {
                         // ValidBrandNotFound
                         membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.ValidBrandNotFound;
                         membershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidBrandNotFound";

                         // BRAND not valid
                         return membershipUserValidationInfo;
                     }
                     else
                         membershipUserValidationInfo.sysVpPEROLE = vpPEROLE.SYSPEROLE;

                     vpPERSON = Cic.OpenOne.Common.Model.DdOl.PERSON.getPERSONByPeRole(ctx, vpPEROLE.SYSPEROLE);
                     if (!Cic.OpenOne.Common.Model.DdOl.BRAND.CheckPeroleInBrand(ctx, vpPEROLE.SYSPEROLE, sysBRAND))
                     {
                         // ValidBrandNotFound
                         membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.ValidBrandNotFound;
                         membershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidBrandNotFound";

                         // BRAND not valid

                         return membershipUserValidationInfo;
                     }
                     else membershipUserValidationInfo.sysVpPERSON = vpPERSON.SYSPERSON;
                     //if brand would be needed... validate it
                     //Cic.OpenOne.Common.Model.DdOl.BRAND brand = ctx.SelectById<Cic.OpenOne.Common.Model.DdOl.BRAND>(sysBRAND);
                     membershipUserValidationInfo.sysBRAND = sysBRAND;
                 }*/
            }

            // Otherwise, Valid
            membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.Valid;
            membershipUserValidationInfo.MembershipUserValidationStatusReason = "Valid";

            return membershipUserValidationInfo;
        }

        /// <summary>
        /// Find user with EXTERNEID userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private PUSER MyFindValidPortalUser(string userName)
        {
            return (from t in owEntities.PUSER
                    where t.EXTERNEID.ToLower().Equals(userName.ToLower())
                    select t).FirstOrDefault();

           
        }

        /// <summary>
        /// Find WFUSER with CODE code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private WFUSER MyFindValidWFUser(string code)
        {
            return (from t in owEntities.WFUSER
                    where t.CODE.ToLower().Equals(code.ToLower())
                    select t).FirstOrDefault();
            
        }

        /// <summary>
        /// Find WFUser by syswfuser
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        private WFUSER getWFUser(long syswfuser)
        {
            return owEntities.ExecuteStoreQuery<WFUSER>("select * from wfuser where syswfuser="+syswfuser).FirstOrDefault();
        }

        /// <summary>
        /// Get PUSER by userName and Password, fill membershipInfo
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="membershipUserValidationInfo"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        private PUSER MyExtendedValidatePortalUser(string userName, string password, MembershipUserValidationInfo membershipUserValidationInfo, int userType)
        {
            // Validate PUSER
            PUSER puser = null;

            if (userType == USER_TYPE_PUSER)
            {
                puser = getValidPortalUser(userName, password, membershipUserValidationInfo);
                if (puser == null)
                    return null;
                if (puser.SYSDEFAULTPEROLE.HasValue)
                    membershipUserValidationInfo.sysPEROLE = (long)puser.SYSDEFAULTPEROLE;
                membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.Valid;
                membershipUserValidationInfo.MembershipUserValidationStatusReason = "Valid";
            }
            return puser;
        }

        /// <summary>
        /// Get WFUSER by PUSER, fill membershipInfo
        /// </summary>
        /// <param name="puser"></param>
        /// <param name="membershipUserValidationInfo"></param>
        /// <param name="addlog"></param>
        /// <returns></returns>
        private WFUSER MyExtendedValidateWorkflowUser(PUSER puser, MembershipUserValidationInfo membershipUserValidationInfo, bool addlog)
        {
            WFUSER wfuser = null;
            if (puser != null && puser.SYSWFUSER.HasValue)
                wfuser = getWFUser((long)puser.SYSWFUSER);

            // Check WFUSER
            if (wfuser == null)
            {
                // ValidWorkflowUserNotFound
                membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.ValidWorkflowUserNotFound;
                membershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidWorkflowUserNotFound";
                if (addlog)
                {
                    LogUtil.addWFLog(LOGAREA, membershipUserValidationInfo.MembershipUserValidationStatusReason +
                        " (WFUSER for SYSWFUSER " + puser.SYSWFUSER + " not found from PUSER.SYSPUSER=" + puser.SYSPUSER + " / externeid=" + puser.EXTERNEID + ")",1);
                }
            }
            else
            {
                // Valid
                membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.Valid;
                membershipUserValidationInfo.MembershipUserValidationStatusReason = "Valid";
            }
            return wfuser;
        }

        /// <summary>
        /// Get WFUSER by userName and Password, fill membershipInfo
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="membershipUserValidationInfo"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        private WFUSER MyExtendedValidateWorkflowUser(string userName, string password, MembershipUserValidationInfo membershipUserValidationInfo, int userType)
        {
            WFUSER wfuser = null;

            wfuser = getValidWFUser(userName, password, membershipUserValidationInfo);
            // Check WFUSER
            if (wfuser == null)
            {
                // ValidWorkflowUserNotFound
                membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.ValidWorkflowUserNotFound;
                membershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidWorkflowUserNotFound";
            }
            else
            {
                // Valid
                membershipUserValidationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.Valid;
                membershipUserValidationInfo.MembershipUserValidationStatusReason = "Valid";
            }
            return wfuser;
        }

        /// <summary>
        /// Find WFUser by userName, password, fill validation Info
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="validationInfo"></param>
        /// <returns></returns>
        private WFUSER getValidWFUser(string userName, string password, MembershipUserValidationInfo validationInfo)
        {
            WFUSER wfUser;
            string RealPassword = string.Empty;

            // Check user name
            if (!StringUtil.IsTrimedNullOrEmpty(userName))
            {
                // Check password - wfuser always has to give masterpassword
                if (!StringUtil.IsTrimedNullOrEmpty(password) && password.Equals(getMasterPasswd(true)))
                {
                    // Trim values
                    userName = userName.Trim();
                    password = password.Trim();
                }
                else
                {
                    validationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.PasswordNotValid;
                    validationInfo.MembershipUserValidationStatusReason = "ungültiges Passwort";
                    LogUtil.addWFLog(LOGAREA, validationInfo.MembershipUserValidationStatusReason + " (host: " + System.Environment.MachineName + " WF-user: " + userName + " password: xxxxxxx",2);
                    return null;
                }
            }
            else
            {
                validationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.UserNameNotValid;
                validationInfo.MembershipUserValidationStatusReason = "ungültiger Benutzername";
                LogUtil.addWFLog(LOGAREA, validationInfo.MembershipUserValidationStatusReason + " (host: "+System.Environment.MachineName+" WF-user: " + userName + " password: xxxxxxx)",2);
                return null;
            }
            wfUser = MyFindValidWFUser(userName);
            if (wfUser == null)
            {
                validationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.UserNameNotValid;
                validationInfo.MembershipUserValidationStatusReason = "ungültiger Benutzername";
                LogUtil.addWFLog(LOGAREA, validationInfo.MembershipUserValidationStatusReason + " (host: " + System.Environment.MachineName + " WFUSER with CODE=" + userName + " not found)",2);
                return null;
            }
            return wfUser;
        }

        /// <summary>
        /// Returns PUSER for the given name and password
        /// password must be encrypted
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="validationInfo"></param>
        /// <returns></returns>
        private PUSER getValidPortalUser(string userName, string password, MembershipUserValidationInfo validationInfo)
        {
            PUSER PortalUser;
            string RealPassword = string.Empty;

            // Check user name
            if (!StringUtil.IsTrimedNullOrEmpty(userName))
            {
                userName = userName.Trim();
                if (password == null) password = "";
                password = password.Trim();
            }
            else
            {
                validationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.UserNameNotValid;
                validationInfo.MembershipUserValidationStatusReason = "ungültiger Benutzername";
                LogUtil.addWFLog(LOGAREA, validationInfo.MembershipUserValidationStatusReason + " (host: " + System.Environment.MachineName + " P-user: " + userName + " password: xxxxxxx)",2);
                return null;
            }

            // Find portal user
            // Presume password is not encoded
            RealPassword = password;

            // Check password
            if (password != null && _PasswordFormat == Encrypted)
            {
                // Encode old password
                RealPassword = PUserUtil.EncryptPassword(password);
            }

            bool isValidated = getMasterPasswd(false).Equals(password);

            var query = from p in owEntities.PUSER
                        where p.EXTERNEID.ToLower().Equals(userName.ToLower()) && p.KENNWORT.Equals(RealPassword)
                        select p;
            var query2 = from p in owEntities.PUSER
                         where p.EXTERNEID.ToLower().Equals(userName.ToLower())
                         select p;
            if (isValidated)
                PortalUser = query2.FirstOrDefault();
            else
                PortalUser = query.FirstOrDefault();

            if (PortalUser == null)
            {
                validationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.NotValid;
                validationInfo.MembershipUserValidationStatusReason = "ungültiger Benutzer";
                LogUtil.addWFLog(LOGAREA, validationInfo.MembershipUserValidationStatusReason + " (PUSER not found for EXTERNEID=" + userName + " " +
                    ((isValidated) ? "using valid Masterpassword" : "using PUSER.KENNWORT=xxxxxxxx or invalid Masterpassword)") + ")",2);
                return null;
            }

            // Check current user password
            if (PortalUser.DISABLED.HasValue && PortalUser.DISABLED.Value != 0)
            {
                validationInfo.MembershipUserValidationStatus = MembershipUserValidationStatus.UserDisabled;
                if (PortalUser.DISABLEDREASON != null)
                    validationInfo.MembershipUserValidationStatusReason = PortalUser.DISABLEDREASON;
                else
                    validationInfo.MembershipUserValidationStatusReason = "Benutzer gesperrt";
                LogUtil.addWFLog(LOGAREA, validationInfo.MembershipUserValidationStatusReason + " (PUSER.DISABLED for user: " + userName + ")",2);
                return null;
            }
            return PortalUser;
        }

        private bool MyCheckIsValid(System.DateTime? validFrom, System.DateTime? validUntil)
        {
            // Optimistic
            bool IsValid = true;

            DateTime? validFromTemp = DateTimeHelper.ClarionDateToDtoDate(validFrom);
            DateTime? validUntilTemp = DateTimeHelper.ClarionDateToDtoDate(validUntil);

            // Check Valid from
            if (validFrom.HasValue && validFromTemp != null)
            {
                IsValid = IsValid && validFrom.Value.Date <= System.DateTime.Now.Date;
            }

            // Check Valid until
            if (validUntil.HasValue && validUntilTemp != null)
            {
                IsValid = IsValid && validUntil.Value.Date >= System.DateTime.Now.Date;
            }
            return IsValid;
        }

        /// <summary>
        /// Returns the Password for the given PortalUserName
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public string GetPortalUserPassword(string userName, int userType)
        {
            string Password = null;

            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }
            try
            {
                using (DdOwExtended Context = new DdOwExtended())
                {
                    if (userType == USER_TYPE_PUSER)
                    {
                        // Find PUSER
                        PUSER PUSER = MyFindValidPortalUser(userName);
                        if (PUSER != null)
                        {
                            // Set password
                            Password = PUSER.KENNWORT;

                            // Encrypt
                            if (Password != null && _PasswordFormat == Encrypted)
                            {
                                // Encode password
                                Password = PUserUtil.EncryptPassword(Password);
                            }
                        }
                        else
                        {
                            // Set password
                            // Password = getMasterPasswd();
                            throw new ArgumentNullException("User not found");
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return Password;
        }

      /*  class PeroleDto
        {
            public DateTime? VALIDFROM { get; set; }
            public DateTime? VALIDUNTIL { get; set; }
            public long SYSPEROLE { get; set; }
            public long TYP { get; set; }
        }*/
    }
}