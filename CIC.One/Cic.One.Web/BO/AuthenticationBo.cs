using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.Util.SOAP;
using Cic.One.DTO;
using Cic.One.Web.DAO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.Util.Config;
using Cic.One.Web.DTO;
using System.Security;
using Cic.OpenOne.Common.Resources;
using Cic.One.Web.DAO.Mail;
using Cic.OpenOne.Common.Model.DdOw;
using AutoMapper;
using Cic.OpenOne.Common.Util;


namespace Cic.One.Web.BO
{


    /// <summary>
    /// BO for Autorization of User
    /// </summary>
    public class AuthenticationBo : IAuthenticationBo
    {



        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const String DEFKEY = "XAKLOP901ASDDDA";
        private IAppSettingsDao settingsDao;
        private IAuthenticationDao authDao;
        private const String DEFAULTSTRING = "default";
        /// <summary>
        /// Constructor
        /// </summary>
        public AuthenticationBo(IAppSettingsDao settingsDao, IAuthenticationDao authDao)
        {
            this.settingsDao = settingsDao;
            this.authDao = authDao;
        }

        /// <summary>
        /// /// Delivers all users in sight of field of the current user
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public WfuserDto[] getPermittedUsers(long sysperole)
        {

            WfuserDto[] rval = authDao.getWfusers(sysperole);
            foreach (WfuserDto wf in rval)
            {
                if (wf.sysperole == sysperole)
                    wf.name = "Meine";
            }
            return rval;

        }

        /// <summary>
        /// finds a certain wfuser
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public WfuserDto getWfuserDto(igetWfuserDto input)
        {
            return authDao.getWfuserDto(input);
        }

        /// <summary>
        /// login via eaihot
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rval"></param>
        /// <returns></returns>
        public ologinEaiHotDto loginEaihot(iloginEaiHotDto input, ref ologinEaiHotDto rval)
        {
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "guid", Value = input.guid });

                Cic.One.DTO.EaihotDto eaihot = ctx.ExecuteStoreQuery<Cic.One.DTO.EaihotDto>("select * from eaihot where computername=:guid", par.ToArray()).FirstOrDefault();
                if (eaihot == null) throw new Exception("EAIHOT not found");
                rval.wfuserCode = ctx.ExecuteStoreQuery<String>("select code from wfuser where syswfuser=" + eaihot.syswfuser, null).FirstOrDefault();
                rval.syswfuser = eaihot.syswfuser.GetValueOrDefault();
                rval.execExpression = eaihot.evalexpression;

                if ("9CD9C1A3-E5C4-4130-A797-A2911B564537".Equals(input.guid))//loadtest guid, random user
                {
                    rval.syswfuser = ctx.ExecuteStoreQuery<long>("select syswfuser from (select wfuser.syswfuser,round(dbms_random.value(100,999), 0) rnd  from wfuser,puser where puser.syswfuser=wfuser.syswfuser and wfuser.sysperson>0 and puser.sysdefaultperole>0 and puser.externeid is not null and puser.disabled=0 and wfuser.master=1 and maintmodeb2b=0 order by rnd ) where rownum<2", null).FirstOrDefault();
                    rval.wfuserCode = ctx.ExecuteStoreQuery<String>("select code from wfuser where syswfuser=" + rval.syswfuser, null).FirstOrDefault();
                }
                else
                {
                    if (!eaihot.startdate.HasValue || eaihot.startdate.Value == 0)// || !eaihot.starttime.HasValue)
                        throw new Exception("Login via EAIHOT timed out");
                    int nowday = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(DateTime.Now);
                    if (eaihot.startdate < nowday)
                        throw new Exception("Login via EAIHOT timed out");

                    if (eaihot.starttime.HasValue && eaihot.starttime.Value > 0)
                    {
                        int nowtime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                        if ((nowtime < (5 * 6000)))
                        {
                            if (eaihot.starttime.Value < 8610000)
                                throw new Exception("Login via EAIHOT timed out");
                        }
                        else if (nowtime - eaihot.starttime.Value > (5 * 6000))
                            throw new Exception("Login via EAIHOT timed out");
                    }
                }
                ctx.ExecuteStoreCommand("update eaihot set submitdate=startdate, submittime=starttime where syseaihot=" + eaihot.syseaihot,null);
                ctx.ExecuteStoreCommand("update eaihot set starttime=null, startdate=null, prozessstatus=2 where syseaihot=" + eaihot.syseaihot, null);
                 try { 
                    rval.sysvlm = long.Parse(eaihot.inputparameter2);
                    rval.vlmCode = ctx.ExecuteStoreQuery<String>("select code from vlmconf where sysvlmconf=" + rval.sysvlm, null).FirstOrDefault();
                    }
                    catch
                    {
                        _log.Debug("Inputparameter2 Falsch gesetzt");
                    }

                if (rval.sysvlm == 0)//  Fallback auf von aussen gesetzte Default VLM
                {
                    input.vlmCode = input.vlmCode.Replace("+", " ");
                    par = new List<Devart.Data.Oracle.OracleParameter>();
                    par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = input.vlmCode });
                    rval.sysvlm = ctx.ExecuteStoreQuery<long>("select sysvlmconf from vlmconf where code=:code", par.ToArray()).FirstOrDefault();
                    rval.vlmCode = input.vlmCode;
                }

            }
            return rval;
        }
        /// <summary>
        /// Validates if the perole or its parents is not inactive
        /// throws an SecurityException if sysperole is not active
        /// </summary>
        /// <param name="sysperole">sysperole of the user</param>
        public static void validateActivePerole(long sysperole)
        {
            PeRoleUtil.clearisActiveCache(sysperole);
            if (!PeRoleUtil.isActive(sysperole)||sysperole==0)//also when user has currently no role at all
                throw new SecurityException("F_00008_Perole_Inactive");
        }


        /// <summary>
        /// Creates a deeplink for changing the password for the user, sent by email
        /// needs a configured deeplink like:
        /// insert into deeplnk(area,code,clienttype,targettype,paramsign,evalparam01,evalparam02,evalparam04,paramexpression) values('WFUSER','CHGPASSWD',3,1,':','{{$object.entities.eaihot.computername}}','WEB-KIA DEALER','{{$object.areaid}','extlogin.xhtml?g=:p01&v=:p02&p1=PAGE&p2=pwdchg');
        /// </summary>
        /// <param name="wfusername"></param>
        public void createPasswordDeepLink(String wfusername)
        {
            
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                WfuserDto wfuser = authDao.getWfuserByCode(wfusername);
                if (wfuser == null)
                    throw new Exception(" ");//intentionally left blank
                if (wfuser.extmailaddress == null || wfuser.extmailaddress.Trim().Length < 1)
                    throw new Exception("Keine EMail-Adresse hinterlegt");
                
                /*
                 * insert into deeplnk(area,code,execexpression,clienttype,targettype,paramsign,evalparam01,evalparam02,evalparam04,paramexpression)
                values('WFUSER','CHGPASSWD','',3,1,':','{{$object.entities.eaihot.computername}}','WEB-KIA DEALER','{{$object.areaid}}','extlogin.xhtml?g=:p01&v=:p02&p1=PAGE&p2=pwdchg');
                */
                long sysbrand = ctx.ExecuteStoreQuery<long>("select brand.sysbrand from brand,prhgroupm,prbrandm where brand.sysbrand=prbrandm.sysbrand and prbrandm.sysprhgroup=prhgroupm.sysprhgroup and prhgroupm.sysperole in (select sysperole from perole,roletype where perole.sysroletype=roletype.sysroletype and roletype.typ=6 and roletype.code='HD' connect by PRIOR sysparent = sysperole start with sysperson=" + wfuser.sysperson + ")", null).FirstOrDefault();
                String brand = ctx.ExecuteStoreQuery<String>("select name from brand where sysbrand=" + sysbrand, null).FirstOrDefault();
                //Deeplink into GUI:
                Cic.One.Workflow.DAO.WorkflowDao wfd = new Cic.One.Workflow.DAO.WorkflowDao();
                String deeplink = wfd.createDeeplink("WFUSER", wfuser.syswfuser, wfuser.syswfuser, "CHGPASSWD_"+brand);
                if (deeplink == null)
                    throw new Exception("CHGPASSWD Deeplink nicht konfiguriert");
                deeplink = deeplink.Replace(" ", "+");

                //Send Email
                //EAIHOT
                //CODE FO_PW_CHANGE 
                //eaiart 202
                //OLTABLE SYSTEM
                Cic.One.DTO.EaihotDto eaihot = new Cic.One.DTO.EaihotDto();
                eaihot.syseaiart = 202;
                eaihot.oltable = "SYSTEM";
                eaihot.sysoltable = 0;
                eaihot.code = "FO_PW_CHANGE";
                eaihot.syswfuser = wfuser.syswfuser;
                eaihot.inputparameter1 = deeplink;
                eaihot.inputparameter2 = wfuser.extmailaddress;
                eaihot.inputparameter3 = ""+wfuser.syswfuser;
                eaihot.eve = 1;
                eaihot.submitdate = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                eaihot.submittime = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                eaihot.clientart = 0;
                eaihot.prozessstatus = 0;
                eaihot.hostcomputer = "*";

                EAIHOT rval = new EAIHOT();
                rval = Mapper.Map<Cic.One.DTO.EaihotDto, EAIHOT>(eaihot, rval);
                rval.SYSEAIHOT = 0;
                rval.EAIARTReference.EntityKey = ctx.getEntityKey(typeof(EAIART),eaihot.syseaiart.Value);
                ctx.AddToEAIHOT(rval);
                ctx.SaveChanges();
                
            }
            
        }

        /// <summary>
        /// Changes the Users' password
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="newpassword"></param>
        public void changeUserPassword(long syswfuser, String newpassword)
        {
            authDao.changeUserPassword(syswfuser, newpassword);
        }

        /// <summary>
        /// Authentifizieren
        /// </summary>
        /// <param name="input">Benutzerlogindaten</param>
        /// <param name="loginType">Anmeldetyp</param>
        /// <param name="authInfo">Authentifizierungsdaten</param>
        /// <returns>Authentifizierungsdaten Ausgang</returns>
        public ogetValidateUserDto authenticate(igetValidateUserDto input, int loginType, ref ogetValidateUserDto authInfo)
        {
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                if(String.IsNullOrEmpty(input.username))
                    throw new Exception("invalid login");

                MembershipProvider prov = new MembershipProvider(ctx);
                //fill user information
                DefaultMessageHeader defMsgHeader = new DefaultMessageHeader();
                defMsgHeader.UserName = input.username;
                defMsgHeader.Password = input.presharedKey;//provided preshared-key -> dont check password
                if (defMsgHeader.Password == null)//no presharedKey given -> needs user pw check!
                {
                    defMsgHeader.Password = MembershipProvider.getMasterPasswd(loginType == MembershipProvider.USER_TYPE_WFUSER);//default preshared key
                }
                String password = input.password;

                if(loginType == MembershipProvider.USER_TYPE_WFUSER)
                { 
                    String ssoUser = authDao.getSSOWfuser(input.username);
                    if (ssoUser != null)
                    {
                        _log.Info("Found User " + ssoUser + " in WFSSO Table for User " + input.username);
                        defMsgHeader.UserName = ssoUser;
                    }
                }

                _log.Info("Username to authenticate: " + defMsgHeader.UserName + ", userType: " +
                           loginType + " (" + (loginType == MembershipProvider.USER_TYPE_PUSER ? "USER_TYPE_PUSER" : "USER_TYPE_WFUSER") + ")");

                MembershipUserValidationInfo info = prov.ValidateUser(defMsgHeader.UserName, defMsgHeader.Password, loginType);
                if (loginType == MembershipProvider.USER_TYPE_WFUSER && info.MembershipUserValidationStatus != MembershipUserValidationStatus.Valid)
                {
                    if (defMsgHeader.Password != null && defMsgHeader.Password.Length > 0)
                    {
                        info = prov.ValidateUser(defMsgHeader.UserName, defMsgHeader.Password, MembershipProvider.USER_TYPE_PUSER);
                        _log.Warn("Fallback to PUSER-Login for " + defMsgHeader.UserName);
                    }
                }

                //validate the login and throw exception if no valid user
                CredentialContext.validateMembership(info);
                authInfo.userData = authDao.getWfuser(info.sysWFUSER);

                //now also try puser, if puser-role is inactive or puser.disabled, set login to invalid
                if(loginType == MembershipProvider.USER_TYPE_WFUSER && info.MembershipUserValidationStatus == MembershipUserValidationStatus.Valid)
                {
                    long syspuser = authDao.getPUser(info.sysWFUSER);
                    if (syspuser > 0)//WFUSER has PUSER, check if allowed! //BANKNOW-EPOS has this behaviour!
                    {
                        String puserExterneid = authDao.getPUserId(info.sysWFUSER);
                        //login with the puser's id accociated to the wfuser, given the preshared-key, we dont want to check password but that the user is available and valid
                        MembershipUserValidationInfo info2 = prov.ValidateUser(puserExterneid, defMsgHeader.Password, MembershipProvider.USER_TYPE_PUSER);

                        if (info2.MembershipUserValidationStatus != MembershipUserValidationStatus.Valid)
                        {
                            info.MembershipUserValidationStatus = info2.MembershipUserValidationStatus;
                            info.MembershipUserValidationStatusReason = info2.MembershipUserValidationStatusReason;

                        }
                        else if (info2.sysPERSON > 0 && info2.sysWFUSER > 0)
                        {
                            if (info2.sysPEROLE == 0)
                            {
                                List<PeroleDto> roles = prov.getUserRoles(info2.sysPERSON);
                                if (roles != null && roles.Count > 0)
                                    info2.sysPEROLE = (from f in roles
                                                       where f.INACTIVEFLAG == 0
                                                       select f.SYSPEROLE).FirstOrDefault();
                            }
                            validateActivePerole(info2.sysPEROLE);
                        }
                    }
                    else//HCE DOES NOT HAVE A PUSER
                    {
                        if(authInfo.userData.disabled>0)
                        {
                            info.MembershipUserValidationStatus = MembershipUserValidationStatus.UserDisabled;
                            info.MembershipUserValidationStatusReason = ""+MembershipUserValidationStatus.UserDisabled;
                        }
                    }
                }
                CredentialContext.validateMembership(info);

                authInfo.userData.sysperole = info.sysPEROLE;
                authInfo.userType = loginType;


                //we wont reach this place when provided preshared key was wrong!
                

                bool loginOK = false;
                if (input.presharedKey != null)//we where provided with preshared key, so allow login by setting current password to the one we compare with
                {
                    String pkeyDatabase = MembershipProvider.getMasterPasswd(loginType == MembershipProvider.USER_TYPE_WFUSER);
                    if (pkeyDatabase!=null && pkeyDatabase.Equals(input.presharedKey))
                        loginOK = true;
                    pkeyDatabase = MembershipProvider.getMasterPasswd(loginType == MembershipProvider.USER_TYPE_PUSER);
                    if (pkeyDatabase != null && pkeyDatabase.Equals(input.presharedKey))
                        loginOK = true;
                }
                else
                {
                    String wfpassword = authDao.getWfuserPassword(info.sysWFUSER);
                    String cpw = RpwComparator.Encode(input.password);
                    if (cpw!=null && cpw.Equals(wfpassword))
                        loginOK = true;
                }

                
                if (authInfo.userData.sysperole == 0)
                {
                    authInfo.userData.sysperole = authDao.getDefaultperole(defMsgHeader.UserName);
                }

                if (!loginOK)
                {
                    _log.Debug("Login Failed: PKEY: "+(input.presharedKey!=null)+" PW: "+(input.password!=null)+" WFUSER: "+info.sysWFUSER);
                    throw new Exception("invalid login");
                }

                
                if (info.sysPERSON == 0 && info.sysPEROLE > 0)
                {
                    info.sysPERSON = authDao.getSysPerson(info.sysPEROLE, true);
                }
                if (info.sysPERSON == 0 && info.sysPUSER > 0)
                {
                    info.sysPERSON = authDao.getSysPerson(info.sysPUSER, false);
                }

                //ROLES-------------------------------------
                _log.Debug("Loading Roles..");
                List<PeroleDto> peroles = prov.getUserRoles(info.sysPERSON);

                bool hasDefault = false;
                authInfo.rolemap = new List<RoleMapDto>();

                long haendlerSysPerole = 0;

                // Eigentlich ist die Schleife hier überflüssig, weil der angemeldete User nur eine Role haben darf...
                foreach (PeroleDto role in peroles)
                {
                    if (authInfo.userData.sysperole == 0)//set default roles when nothing set
                    {
                        authInfo.userData.sysperole = role.SYSPEROLE;
                        info.sysPEROLE = role.SYSPEROLE;
                    }
                    RoleMapDto rm = new RoleMapDto();
                    rm.sysPerole = role.SYSPEROLE;
                    rm.inactive = !PeRoleUtil.isActive(role.SYSPEROLE) ? 1 : 0;

                    if (role.SYSPEROLE == info.sysPEROLE)
                    {
                        rm.isDefault = 1;
                        hasDefault = true;
                    }
                    try
                    {
                       
                            rm.roleName = role.ROLETYPENAME;
                            rm.roletyp = role.ROLETYPETYP;
                            // wenn die role ein Verkäufer ist, dann hole BildWelten für seinen sysParent (Händler)
                            if (role.ROLETYPETYP == (int?)RoleTypeTyp.VERKAEUFER && role.SYSPARENT >0)
                                haendlerSysPerole = (long)role.SYSPARENT;
                       
                    }
                    catch (Exception ex)
                    {
                        // Entsorgt die nicht verwendung der Exception
                        String message = ex.Message;
                    }//no roletype available

                    authInfo.rolemap.Add(rm);
                }

                if (!hasDefault && authInfo.rolemap.Count > 0)
                    authInfo.rolemap.FirstOrDefault().isDefault = 1;

                authInfo.userData.typ = authDao.getRoletypeTypByPerole(authInfo.userData.sysperole);

                //---------Fill Attributes--------------------------------------
                authInfo.attributmap = new List<AttributeMapDto>();

                //language attributes-----------------------------

                _log.Debug("Loading Languages..");
                AttributeValue defLang = authDao.getIsoCode(info.sysPERSON);

                AttributeMapDto langAtt = new AttributeMapDto();
                langAtt.attributeName = AttributeName.ISOLanguageCode;
                langAtt.attributeValues = authDao.getAllIsocodes();
                langAtt.defaultValue = defLang != null ? defLang.value : langAtt.attributeValues.FirstOrDefault().value;
                authInfo.attributmap.Add(langAtt);

                //KAM attributes
                try { 
                authInfo.attributmap.Add(authDao.getKamAttributes(authInfo.userData.sysperole));
                }
                catch (Exception) { }
                //BRAND attributes
                try { 
                authInfo.attributmap.Add(authDao.getBrandAttributes(authInfo.userData.sysperole));
                }
                catch (Exception) { }

                //EPOS_ADMIN attributes
                try { 
                AttributeMapDto epos = authDao.getEposAdminAttribute(authInfo.userData.sysperole);
                if(epos!=null)
                    authInfo.attributmap.Add(epos);
                }
                catch (Exception) { }

                //ABWICKLUNGSORT attributes
                try
                {
                    AttributeMapDto awo = authDao.getAbwicklungsortAttribute(authInfo.userData.syswfuser);
                    if (awo != null)
                        authInfo.attributmap.Add(awo);
                }
                catch (Exception) { }

                //EPOSBEDINGUNG attribute
                try
                {
                    AttributeMapDto awo = authDao.getEposBedingungen(authInfo.userData.sysperole);
                    if (awo != null)
                        authInfo.attributmap.Add(awo);
                }
                catch (Exception) { }

                //BPEROLES attribute
                try
                {
                    AttributeMapDto awo = authDao.getBPERoles(authInfo.userData.syswfuser);
                    if (awo != null)
                        authInfo.attributmap.Add(awo);
                }
                catch (Exception) { }

                //BPELANE attribute
                try
                {
                    AttributeMapDto awo = authDao.getBPELanes (authInfo.userData.syswfuser);
                    if (awo != null)
                        authInfo.attributmap.Add(awo);
                }
                catch (Exception) { }

				//USERFILIALEN attribute
				try
				{
					// AttributeMapDto awo = authDao.getUsersFilialen (authInfo.userData.syswfuser);
					AttributeMapDto awo = authDao.getUsersFilialen (authInfo.userData.sysperson);
					
					if (awo != null)
						authInfo.attributmap.Add (awo);
				}
				catch (Exception) { }


                // Bildwelten -----------
                _log.Debug("Loading Bildwelten..");
                try { 
                // Ein Händler kann maximal einer Bildwelt zugeordnet sein
                BildweltInfoDto bildWeltInfo = authDao.getBildwelten(haendlerSysPerole);

                AttributeMapDto bwAttributeMap = new AttributeMapDto();
                bwAttributeMap.attributeName = AttributeName.BildweltCode;
                bwAttributeMap.attributeValues = new List<AttributeValue>();
                bwAttributeMap.defaultValue = DEFAULTSTRING;

                AttributeValue bwAttributeValue = new AttributeValue();
                if (bildWeltInfo == null)
                {
                    bwAttributeValue.value = DEFAULTSTRING;
                    BildweltInfoDto dbw = authDao.getDefaultBildwelt();
                    if(dbw!=null)
                    bwAttributeValue.id = ""+dbw.SYSPRBILDWELT;

                    AttributeValue bwres = new AttributeValue();
                    bwres.id = "THEME";
                    bwres.value = dbw.RESSOURCE;
                    bwAttributeMap.attributeValues.Add(bwres);
                }
                else
                {
                    bwAttributeValue.value = bildWeltInfo.NAME;
                    bwAttributeValue.id = ""+bildWeltInfo.SYSPRBILDWELT;

                    bwAttributeMap.defaultValue = bildWeltInfo.NAME;

                    AttributeValue bwres = new AttributeValue();
                    bwres.id = "THEME";
                    bwres.value = bildWeltInfo.RESSOURCE;
                    bwAttributeMap.attributeValues.Add(bwres);
                }
                bwAttributeMap.attributeValues.Add(bwAttributeValue);

                authInfo.attributmap.Add(bwAttributeMap);
                }
                catch (Exception) { }
                // Bildwelten ende

                //Vertriebskanäle
                _log.Debug("Loading Channels..");
                try { 
                List<long> channels = authDao.getChannels(authInfo.userData.sysperole);
                if(channels!=null&&channels.Count>0)
                { 
                    AttributeMapDto channelAttributeMap = new AttributeMapDto();
                    channelAttributeMap.attributeName = AttributeName.Channels;
                    channelAttributeMap.attributeValues = new List<AttributeValue>();
                    foreach(long chnl in channels)
                    {
                        AttributeValue att = new AttributeValue();
                        att.id = ""+chnl;
                        att.value = ""+chnl;
                        channelAttributeMap.attributeValues.Add(att);
                    }
                    authInfo.attributmap.Add(channelAttributeMap);
                }
                }
                catch (Exception) { }


                //PRHGROUPS
                try
                {
                    AttributeMapDto am = new AttributeMapDto();
                    am.attributeName=AttributeName.STANDARD_HG;
                    String stdhgname = AppConfig.Instance.getValueFromDb("ANGEBOTSSASSISTENT", "FESTE_HANDELSGRUPPEN", "STANDARD_HG");
                    long sysprhgroup = ctx.ExecuteStoreQuery<long>("select sysprhgroup from prhgroup where name='"+stdhgname+"'",null).FirstOrDefault();
                    am.defaultValue=""+sysprhgroup;
                    authInfo.attributmap.Add(am);

                    am = new AttributeMapDto();
                    am.attributeName=AttributeName.MA_HG;
                    stdhgname = AppConfig.Instance.getValueFromDb("ANGEBOTSSASSISTENT", "FESTE_HANDELSGRUPPEN", "MA_HG");
                    sysprhgroup = ctx.ExecuteStoreQuery<long>("select sysprhgroup from prhgroup where name='"+stdhgname+"'",null).FirstOrDefault();
                    am.defaultValue=""+sysprhgroup;
                    authInfo.attributmap.Add(am);
                }
                catch (Exception) { }

                //chat attributes

               /* RegVarDto[] items = settingsDao.deliverAppSettingsItems(new igetAppSettingsItemsDto() { area = "CHAT", bezeichnung = RegVarPaths.getInstance().CHAT, syswfuser = -1 });
                String chatValue = "false";
                if (items != null && items.Length == 1)
                {
                    chatValue = items.FirstOrDefault().wert;
                }*/
                //Sollte nur erreicht werden, falls die Registry resettet wurde.
                    //geht aber nicht weil das aus CFG kommt und so nicht dort angelegt wird
                    //bitte sowas nächstes mal vorher wenigstens einmal testen!
                    /*items = settingsDao.createOrUpdateAppSettingsItem(new icreateOrUpdateAppSettingsItemDto()
                        {
                             regVar = new RegVarDto()
                             {
                                completePath = RegVarPaths.getInstance().CHAT+ "Enabled",
                                code = "Enabled",
                                wert = "false",
                                area = "CHAT",
                                syswfuser = -1
                             },
                             sysWfuser = -1
                        });*/
                
                /*AttributeMapDto chatAtt = new AttributeMapDto();
                chatAtt.attributeName = AttributeName.ChatCode;
                chatAtt.attributeValues = new List<AttributeValue>()
                {
                    new AttributeValue()
                    {
                        id = "1",
                        value = chatValue
                    }
                };
                authInfo.attributmap.Add(chatAtt);*/

                _log.Debug("Loading Rights..");
                IRightsMapBo rightsMapBo = BOFactoryFactory.getInstance().createRightsMapBo();
                List<Cic.OpenOne.Common.DTO.RightsMap> rights = rightsMapBo.getRightsForWFUser(info.sysWFUSER);
                
                Dictionary<String, RightMapDto> workMap = new Dictionary<string, RightMapDto>();
                foreach(Cic.OpenOne.Common.DTO.RightsMap r in rights)
                {
                    if(!workMap.ContainsKey(r.rightsMapId))
                    {
                        RightMapDto rm = new RightMapDto();
                        rm.codeRFU = r.codeRFU;
                        rm.codeRMO = r.codeRMO;
                        rm.rechte=r.rechte;
                        workMap[r.rightsMapId] = rm;
                        continue;
                    }
                    //doublette, or the rights
                    RightMapDto rmap = workMap[r.rightsMapId];
                    int l1 = Convert.ToInt32(rmap.rechte,2);
                    int l2 = Convert.ToInt32(r.rechte, 2);
                    int l3 = l1 | l2;
                    rmap.rechte = Convert.ToString(l3, 2); 
                }
                authInfo.rightsmap = workMap.Values.ToList();
                
                authInfo.username = defMsgHeader.UserName;

                return authInfo;
            }
        }
    }
}