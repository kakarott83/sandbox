using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Resources;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.SOAP;
using System.ServiceModel.Web;
using System.Text;

namespace Cic.OpenOne.Common.Util.Security
{
    /// <summary>
    /// Validates the User and provides Information about his credentials
    /// </summary>
    public class CredentialContext
    {
        private static CacheDictionary<String, MembershipUserValidationInfo> credentialCache =
                        CacheFactory<String, MembershipUserValidationInfo>.getInstance().createCache(1000 * 10 * 30);
        private static CacheDictionary<String, long> peroleCache =
                        CacheFactory<String, long>.getInstance().createCache(1000 * 10 * 30);
        private DefaultMessageHeader messageHeader;
        private MembershipProvider provider;
        private String cacheKey;
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// CredentialContext
        /// </summary>
        /// <param name="messageHeader"></param>
        public CredentialContext(DefaultMessageHeader messageHeader)
        {
            init(messageHeader);
        }

        /// <summary>
        /// CredentialContext
        /// </summary>
        public CredentialContext()
        {
            MessageHeader<DefaultMessageHeader> mh = new MessageHeader<DefaultMessageHeader>();
            DefaultMessageHeader dmh = mh.ReadHeader();
            if (dmh == null || dmh.UserName==null)
            {
                try
                {
                    
                    dmh = (DefaultMessageHeader)Activator.CreateInstance(typeof(DefaultMessageHeader));

                    
                    dmh.UserName = WebOperationContext.Current.IncomingRequest.Headers.Get("UserName");
                    dmh.Password = WebOperationContext.Current.IncomingRequest.Headers.Get("Password");
                    if(dmh.UserName==null)//try Authorization Header
                    {
                        try
                        {
                            String auth = WebOperationContext.Current.IncomingRequest.Headers.Get("Authorization");
                            String[] authInfo = null;
                            if(auth!=null)
                                authInfo = auth.Split(' ');
                            if (authInfo!=null && authInfo.Length == 2)
                            {
                                if (!authInfo[0].Equals("Basic"))
                                {
                                    throw new Exception("HTTP Authentication " + authInfo[0] + " not supported, only Basic or Username+Password Header are supported");
                                }
                                else if (authInfo[1]!=null)
                                {
                                    byte[] data = Convert.FromBase64String(authInfo[1]);
                                    string decodeduserPw = Encoding.UTF8.GetString(data);
                                    if (decodeduserPw != null)
                                    {
                                        String[] userinfo = decodeduserPw.Split(':');
                                        dmh.UserName = userinfo[0];
                                        dmh.Password = userinfo[1];
                                    }
                                }
                            }
                            else throw new Exception("Authorization Header did not contain the correct Values");
                        }catch(Exception exc)
                        {
                            _log.Info("Neither DefaultMessageHeader(SOAP) nor HTTP-Headers Username and Password nor HTTP Authorization Header Basic was found: " + exc.Message);
                        }
                    }


                    try
                    {
                        dmh.SysPEROLE = long.Parse(WebOperationContext.Current.IncomingRequest.Headers.Get("SysPEROLE"));
                    }
                    catch (Exception e1) { }
                    try
                    {
                        dmh.ISOLanguageCode = WebOperationContext.Current.IncomingRequest.Headers.Get("ISOLanguageCode");
                    }
                    catch (Exception e1) { }
                    if(dmh.ISOLanguageCode==null)
                    {
                        try
                        {
                            dmh.ISOLanguageCode = WebOperationContext.Current.IncomingRequest.Headers.Get("Accept-Language");
                            if(dmh.ISOLanguageCode!=null)
                            {
                                _log.Info("Fetched Language from HTTP Header Accept-Language: " + dmh.ISOLanguageCode);
                            }
                        }
                        catch (Exception e1) { }
                    }
                    dmh.UserType = 0;
                    try
                    {
                        dmh.UserType = int.Parse(WebOperationContext.Current.IncomingRequest.Headers.Get("UserType"));
                    }
                    catch (Exception e2) { }

                    

                }catch(Exception exc)
                {
                    return;
                }
            }
            
            /*
            if (dmh.UserName == null || dmh.UserName.Length == 0)
            {
                //dmh = new DefaultMessageHeader();
                dmh = (DefaultMessageHeader)Activator.CreateInstance(typeof(DefaultMessageHeader));
                //default for development when soap header ist omitted:
                dmh.UserName = "Default";
                dmh.Password = "XAKLOP901ASDDDA";
                dmh.SysPEROLE = 2;
              
                dmh.ISOLanguageCode = "en";
                dmh.UserType = 0;
            }
             */
            init(dmh);
        }

        private void init(DefaultMessageHeader messageHeader)
        {
            this.messageHeader = messageHeader;
            //embed CS PKI SAML auth info in header-info
            MembershipProvider.validateSAML(messageHeader);

            if (messageHeader.SysPEROLE == 0 && messageHeader.UserName!=null && peroleCache.ContainsKey(messageHeader.UserName))
            {
                messageHeader.SysPEROLE = peroleCache[messageHeader.UserName];
            }

            cacheKey = string.Join("_", new string[] { messageHeader.UserName, messageHeader.Password, messageHeader.SysPEROLE.ToString(), 
            messageHeader.ISOLanguageCode, messageHeader.UserType.ToString() });
            
			LogUtil.RegisterLoggedIn (messageHeader.SysPEROLE);		// rh 20170202
        }

		///// <summary>
		///// rh 20170202: register LoggedIn User to CICLOG
		///// </summary>
		///// <param name="SysPEROLE"></param>
		//private void RegisterLoggedIn(long SysPEROLE)
		//{
		//	// rh 20170202: SAMPLE Call LogLoggedInUse
		//	bool bLog = Cic.OpenOne.Common.Properties.Config.Default.LogLoggedInUser;
		//	if (bLog)											// FEATURE: SWITCH to START/STOPP LoggedInUser-Logging
		//	{
		//		int sessionTimeoutMin = 60;						// HINT: ENTER timeOut here, until we can GET it from DefaultMessageHeader  
		//		long appId = 717171;							// HINT: ENTER AppID here, until we can GET it from DefaultMessageHeader  
		//		string userSource = "sourceID";					// HINT: ENTER userSource here, until we can GET it from DefaultMessageHeader  
		//		string hostName = System.Net.Dns.GetHostEntry ("LocalHost").HostName;

		//		// LogUtil.RegisterLoggedIn (SysPEROLE, sessionTimeoutMin, appId, hostName, userSource);
				
		//		LogUtil.RegisterLoggedIn (SysPEROLE);
		//	}
		//}

        /// <summary>
        /// getMembershipInfo
        /// </summary>
        /// <returns></returns>
        public MembershipUserValidationInfo getMembershipInfo()
        {
            if (String.IsNullOrEmpty(messageHeader.UserName))
            {
                throw new ServiceBaseException("E_10001_UserNameNotValid", MembershipUserValidationStatus.UserNameNotValid.ToString());
            }
            if (!credentialCache.ContainsKey(cacheKey))
            {
                if (provider == null)
                    provider = new MembershipProvider(new DdOwExtended());
                credentialCache[cacheKey] = provider.ExtendedValidateUser(messageHeader.UserName, messageHeader.Password,
                                                                          messageHeader.SysPEROLE, messageHeader.UserType);
                credentialCache[cacheKey].ISOLanguageCode = messageHeader.ISOLanguageCode;
                if(messageHeader.UserName!=null)
                    peroleCache[messageHeader.UserName] = credentialCache[cacheKey].sysPEROLE;
            }
            return credentialCache[cacheKey];
        }

        /// <summary>
        /// Validates only username/password
        /// </summary>
        /// <returns></returns>
        public MembershipUserValidationInfo validateUser()
        {
            MembershipProvider provider = new MembershipProvider(new DdOwExtended());
            MembershipUserValidationInfo userInfo = provider.ValidateUser(messageHeader.UserName, messageHeader.Password, messageHeader.UserType);
            validateMembershipInfo(userInfo);
            return userInfo;
        }

        /// <summary>
        /// Validates the user from the soap header and returns the user membership info
        /// </summary>
        /// <returns>user membership info</returns>
        /// <exception cref="ServiceBaseException">when the user is not authorized or the soap header was missing</exception>
        public MembershipUserValidationInfo validateService()
        {
            MembershipUserValidationInfo userInfo = getMembershipInfo();
            validateMembershipInfo(userInfo);
            return userInfo;
        }

        /// <summary>
        /// validateMembershipInfo
        /// </summary>
        /// <param name="userInfo"></param>
        public void validateMembershipInfo(MembershipUserValidationInfo userInfo)
        {
            try
            {
                validateMembership(userInfo);
            }
            catch (Exception e)
            {
                credentialCache.Remove(cacheKey);
                throw e;
            }
        }

        /// <summary>
        /// validateMembership
        /// </summary>
        /// <param name="userInfo"></param>
        public static void validateMembership(MembershipUserValidationInfo userInfo)
        {
            if (userInfo.MembershipUserValidationStatus != MembershipUserValidationStatus.Valid)
            {
                String code = "";
                if (userInfo.MembershipUserValidationStatus == MembershipUserValidationStatus.NotValid)
                    code = "E_10001_UserNotValid";
                else if (userInfo.MembershipUserValidationStatus == MembershipUserValidationStatus.UserNameNotValid)
                    code = "E_10001_UserNameNotValid";
                else if (userInfo.MembershipUserValidationStatus == MembershipUserValidationStatus.PasswordNotValid)
                    code = "E_10001_PasswordNotValid";
                else if (userInfo.MembershipUserValidationStatus == MembershipUserValidationStatus.ValidWorkflowUserNotFound)
                    code = "E_10001_WorkflowUserNotFound";
                else if (userInfo.MembershipUserValidationStatus == MembershipUserValidationStatus.ValidRoleNotFound)
                    code = "E_10001_RoleNotFound";
                else if (userInfo.MembershipUserValidationStatus == MembershipUserValidationStatus.ValidPersonNotFound)
                    code = "E_10001_PersonNotFound";
                else if (userInfo.MembershipUserValidationStatus == MembershipUserValidationStatus.ValidBrandNotFound)
                    code = "E_10001_BrandNotFound";
                else if (userInfo.MembershipUserValidationStatus == MembershipUserValidationStatus.UserDisabled)
                    code = "E_10001_UserDisabled";
                else if (userInfo.MembershipUserValidationStatus == MembershipUserValidationStatus.Systemlocked)
                    code = "E_10001_SystemLocked";

                throw new ServiceBaseException(code, userInfo.MembershipUserValidationStatusReason);
            }
        }

        /// <summary>
        /// getUserLanguange
        /// </summary>
        /// <returns></returns>
        public string getUserLanguange()
        {
            return messageHeader.ISOLanguageCode;
        }

        /// <summary>
        /// Fills the service return object type with exception details from this exception
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="exception"></param>
        public void fillBaseDto(oBaseDto obj, ServiceBaseException exception)
        {
            obj.success(); //for setting duration
            obj.message.stacktrace = exception.StackTrace;
            obj.message.code = exception.code;
            //System.Globalization.CultureInfo resourceCulture = CultureInfo.CreateSpecificCulture(getUserLanguange()); 

            obj.message.message = ExceptionMessages.ResourceManager.GetString(exception.code);//, resourceCulture);
            obj.message.detail = exception.Message;
            if (obj.message.message == null && exception.Message != null)//avoid null message
                obj.message.message = exception.Message;

            if (exception.InnerException != null)
                obj.message.detail += " InnerException: " + exception.InnerException.Message;

            obj.message.type = exception.type;

            if(exception.type==MessageType.Error)
                _log.Error(exception.code, exception);
            else if (exception.type == MessageType.Warn)
                _log.Warn(exception.code, exception);
            else _log.Info(exception.code, exception);
            if (exception.InnerException != null)
            {
                ILog mylog = _log;
                try
                {
                    var st = new StackTrace(exception.InnerException, true); // create the stack trace
                    var query = st.GetFrames()         // get the frames
                          .Select(frame => new
                          {                   // get the info
                              FileName = frame.GetFileName(),
                              LineNumber = frame.GetFileLineNumber(),
                              ColumnNumber = frame.GetFileColumnNumber(),
                              Method = frame.GetMethod(),
                              Class = frame.GetMethod().DeclaringType,
                          });
                    mylog = Log.GetLogger(query.FirstOrDefault().Class);
                }
                catch (Exception)
                {
                }
                if (exception.type == MessageType.Error)
                    mylog.Error("InnerException: ", exception.InnerException);
                else if (exception.type == MessageType.Warn)
                    mylog.Warn("InnerException: ", exception.InnerException);
                else mylog.Info("InnerException: ", exception.InnerException);
               
            }
        }

        /// <summary>
        /// Fills the service return object type with exception details from this exception
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        /// <param name="code"></param>
        public void fillBaseDto(oBaseDto obj, Exception e, String code)
        {
            if (e != null && e.GetType() == typeof(ServiceBaseException))//handle servicebaseexception correctly
            {
                fillBaseDto(obj, (ServiceBaseException)e);
                return;
            }
            obj.success(); //for setting duration
            obj.message.stacktrace = e.StackTrace;
            obj.message.code = code;

            //System.Globalization.CultureInfo resourceCulture = CultureInfo.CreateSpecificCulture(getUserLanguange()); 
            //System.Resources.ResourceManager rm = ResourceManager.CreateFileBasedResourceManager("ExceptionMessages", ".", null);
            // string test = rm.GetString(code, resourceCulture); 
            obj.message.message = ExceptionMessages.ResourceManager.GetString(code);
            obj.message.detail = e.Message;
            if (obj.message.message == null && e.Message != null)//avoid null message
                obj.message.message = e.Message;

            if (e.InnerException != null)
                obj.message.detail += " InnerException: " + e.InnerException.Message;

            obj.message.type = MessageType.Fatal;

            ILog mylog = _log;
            try
            {
                var st = new StackTrace(e, true); // create the stack trace
                var query = st.GetFrames()         // get the frames
                      .Select(frame => new
                       {                   // get the info
                           FileName = frame.GetFileName(),
                           LineNumber = frame.GetFileLineNumber(),
                           ColumnNumber = frame.GetFileColumnNumber(),
                           Method = frame.GetMethod(),
                           Class = frame.GetMethod().DeclaringType,
                       });
                mylog = Log.GetLogger(query.FirstOrDefault().Class);
            }
            catch (Exception)
            {
            }

            mylog.Error(code, e);
            if (e.InnerException != null)
                mylog.Error("InnerException: ", e.InnerException);
        }
    }
}