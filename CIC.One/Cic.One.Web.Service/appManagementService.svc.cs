using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.One.DTO;
using Cic.One.Web.BO;


using Cic.One.Web.DAO;

using System.Reflection;
using Cic.One.Web.Service.DAO;
using System.Xml.Serialization;
using Cic.OpenOne.Common.Util;
using Cic.One.Web.Contract;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.SOAP;
using Cic.One.Web.DAO.Mail;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.Util.Serialization;
using System.Activities;
using Cic.One.Workflow.BO;
using Cic.OpenOne.Common.Util.Collection;
using Cic.One.GateOEM.Service;
using Cic.OpenOne.Common.Util.Extension;
using Dapper;
using System.Timers;
using CIC.ASS.Common.BO;

namespace Cic.One.Web.Service
{
	[ServiceBehavior (Namespace = "http://cic-software.de/One")]
	[Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
	public class appManagementService : IappManagementService
	{

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		/// <summary>
		/// Flushes the caches
		/// </summary>
		public void flushCache ()
		{
            _log.Info("Triggered flushCache");
			CacheManager.getInstance ().flush (0);
			WFVDao.flushCache ();
            AppConfig.Instance.reloadCFG();
            SqlMapper.PurgeQueryCache();
            ProcessUpdater.updateLoglevelFromDB();
            var rebuild = AppConfig.Instance.getValueFromDb("SETUP.NET", "LUCENE", "REBUILD");
            _log.Info("SETUP.NET/LUCENE/REBUILD="+rebuild);
            if (rebuild != null && "1".Equals(rebuild) && LuceneFactory.getInstance().getIndexUpdateInterval() > 0)
            {
                _log.Info("Rebuilding Index Now");
                Timer timer = new Timer();
                timer.Elapsed += new ElapsedEventHandler(updateIndex);
                timer.Start();//start immediately
            }
            
		}

        private void updateIndex(object source, ElapsedEventArgs eArgs)
        {
            try
            {
                ((System.Timers.Timer)source).Stop();
                LuceneBO.getInstance().rebuild();
            }catch(Exception )
            {

            }
        }

		/// <summary>
		/// Helper to fetch the header structure needed for the ws
		/// </summary>
		/// <returns></returns>
		public DefaultMessageHeader getHeader ()
		{
			return new DefaultMessageHeader ();
		}

		/// <summary>
		/// returns a list of all web vlms
		/// </summary>
		/// <returns></returns>
		public ogetVlmDto getVlmList ()
		{
			ServiceHandler<long, ogetVlmDto> ew = new ServiceHandler<long, ogetVlmDto> (0);

			return ew.process (delegate (long input, ogetVlmDto rval)
			{

				IWorkflowBo bo = BOFactoryFactory.getInstance ().getWorkflowBo ();
				rval.vlm = bo.getVlmList ();

			});
		}

		/// <summary>
		/// delivers vlm config
		/// </summary>
		/// <param name="vlmid"></param>
		/// <returns></returns>
		public ogetVlmConfigDto getVlmConfig (String vlmid)
		{
			ServiceHandler<String, ogetVlmConfigDto> ew = new ServiceHandler<String, ogetVlmConfigDto> (vlmid);

			return ew.process (delegate (String input, ogetVlmConfigDto rval)
			{

				IWorkflowBo bo = BOFactoryFactory.getInstance ().getWorkflowBo ();
				rval.entries = bo.getVlmConfig (input);
				rval.menus = bo.getMenus (input);
			});
		}

		/// <summary>
		/// Processes a workflow until
		///  - finished
		///  - or until bookmark reached (by UserIntarction-Activity)
		///  
		/// Uses the given input context and returns the modified context
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		public oprocessWorkflowDto processWorkflow (iprocessWorkflowDto dto)
		{
			ServiceHandler<iprocessWorkflowDto, oprocessWorkflowDto> ew = new ServiceHandler<iprocessWorkflowDto, oprocessWorkflowDto> (dto);

			return ew.process (delegate (iprocessWorkflowDto input, oprocessWorkflowDto rval, CredentialContext ctx)
			{

				IWorkflowBo bo = BOFactoryFactory.getInstance ().getWorkflowBo ();
				if (input.workflowContext != null)
				{
					input.workflowContext.sysWFUSER = ctx.getMembershipInfo ().sysWFUSER;
					input.workflowContext.sysPEROLE = ctx.getMembershipInfo ().sysPEROLE;
				}
				bo.processWorkflow (input, rval, DAOFactoryFactory.getInstance ().getWorkflowDao (), Cic.One.Web.BO.BOFactoryFactory.getInstance ().getCASBo ());
			});
		}

		/// <summary>
		/// Autorizes the user and delivers user settings
		/// </summary>
		/// <returns></returns>
		public ogetValidateUserDto getValidateUser (igetValidateUserDto dto)
		{
			ServiceHandler<igetValidateUserDto, ogetValidateUserDto> ew = new ServiceHandler<igetValidateUserDto, ogetValidateUserDto> (dto);

			return ew.process (delegate (igetValidateUserDto input, ogetValidateUserDto rval)
			{
				IAuthenticationBo authBo = BOFactoryFactory.getInstance ().getAuthenticationBo ();

				authBo.authenticate(input, input.userType, ref rval);

				////————————————————————————————————————————————————
				//// rh 20170131: SAMPLE CALL register LoggedIn User
				////————————————————————————————————————————————————
				
				iLoggedInListDto iloggedInList = new iLoggedInListDto ();
				iloggedInList.loggedInSearchPattern = new CicLogDto ();
				// TEST user 07333377 
				iloggedInList.loggedInSearchPattern.cicbenutzer = "07333377";
				// TEST CURRENT DATE: 
				iloggedInList.loggedInSearchPattern.logindate = DateTime.Now;
				// TEST LAST 24h
				iloggedInList.loggedInSearchPattern.logindate = DateTime.Now.AddDays (-1);
				// TEST specific DATE: 
				iloggedInList.loggedInSearchPattern.logindate = DateTime.Now.AddDays (-4);
				// iloggedInList.loggedInSearchPattern.id = 771177;

				// TEST DIV (all) fields:
				iloggedInList.loggedInSearchPattern.id = 363;
				iloggedInList.loggedInSearchPattern.id = 771177;

				string strDate = "2017-02-03 17:00:00";							// TEST specific DATE: 
				DateTime toDate = DateTime.Parse (strDate);
				iloggedInList.loggedInSearchPattern.logoutdate = toDate;

				iloggedInList.loggedInSearchPattern.maschine = "CIC";			// AnyHost / CIC (source not combined with orabenutzer)
				iloggedInList.loggedInSearchPattern.maschine = "AnyHost";		// DummyHost/ AnyHost / CIC (source not combined with orabenutzer)

				iloggedInList.loggedInSearchPattern.orabenutzer = "CIC";		// null / CIC (maschine not combined with orabenutzer)
				iloggedInList.loggedInSearchPattern.orabenutzer = null;			// null / CIC (maschine not combined with orabenutzer)

				iloggedInList.loggedInSearchPattern.source = null;				// AnyDummySource / AnySource / null
				iloggedInList.loggedInSearchPattern.source = "AnyDummySource";	// AnyDummySource / AnySource / null
				iloggedInList.loggedInSearchPattern.source = "AnySource";		// AnyDummySource / AnySource / null

				iloggedInList.loggedInSearchPattern.sysciclog = 584957;			// do NOT not combine: 584957 --> login 2017-02-06 08:54:40 logOut = null

				oLoggedInListDto ologgedInList = listLoggedIn (iloggedInList);


	
				////////// DUMMY: rh 20161220: CALL TEST
				////////oemService oems = new oemService ();
				////////{
				////////	// authBo.resetPasswordByDeepLink (input.username);
				////////	oems.resetPassword (input.username);
				////////}
				
				// Startet die Exchange Subscription (Asynchron)
				MailDaoFactory.getInstance().CheckCreateSubscriptionAsync(rval.userData.getEntityId());
			});
		}

		/// <summary>
		/// Changes the user's password
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public ochangeUserPasswordDto changeUserPassword (ichangeUserPasswordDto dto)
		{
			ServiceHandler<ichangeUserPasswordDto, ochangeUserPasswordDto> ew = new ServiceHandler<ichangeUserPasswordDto, ochangeUserPasswordDto> (dto);

			return ew.process (delegate (ichangeUserPasswordDto input, ochangeUserPasswordDto rval, CredentialContext ctx)
			{
				IAuthenticationBo authBo = BOFactoryFactory.getInstance ().getAuthenticationBo ();

				authBo.changeUserPassword (ctx.getMembershipInfo ().sysWFUSER, input.newPassword);


			}, true);
		}

		/// <summary>
		/// login for EAIHOT
		/// </summary>
		/// <returns></returns>
		public ologinEaiHotDto loginEaiHot (iloginEaiHotDto dto)
		{
			ServiceHandler<iloginEaiHotDto, ologinEaiHotDto> ew = new ServiceHandler<iloginEaiHotDto, ologinEaiHotDto> (dto);

			return ew.process (delegate (iloginEaiHotDto input, ologinEaiHotDto rval)
			{
				IAuthenticationBo authBo = BOFactoryFactory.getInstance ().getAuthenticationBo ();

				authBo.loginEaihot (input, ref rval);
			});
		}

		/// <summary>
		/// Log loggedIn users
		/// rh 20170203
		/// </summary>
		/// <returns></returns>
		public ologLoggedInDto logLoggedIn (ilogLoggedInDto dto)
		{
			ServiceHandler<ilogLoggedInDto, ologLoggedInDto> ew = new ServiceHandler<ilogLoggedInDto, ologLoggedInDto> (dto);

			return ew.process (delegate (ilogLoggedInDto input, ologLoggedInDto rval)
			{
				// LogUtil.RegisterLoggedIn (input.sysperole, input.timeOutMin, input.appId, input.hostID, input.userSource);
				LogUtil.RegisterLoggedIn (input.sysperole);
			});
		}

		///// <summary>
		///// Switch Log loggedIn users ON / OFF
		///// rh 20170203
		///// </summary>
		///// <returns></returns>
		//public void switchLoggedInLog (bool bLog)
		//{
		//	LogUtil.switchLoggedInLog (bLog);
		//}


		/// <summary>
		/// UpdateLoggedInLogProperties
		/// rh 20170209
		/// </summary>
		/// <returns></returns>
		public void UpdateLoggedInLogProperties (iLoggedInLogPropertiesDto dto)
		{
			LogUtil.UpdateLoggedInLogProperties (dto);
		}


		/// <summary>
		/// List loggedIn users
		/// rh 20170203
		/// </summary>
		/// <returns></returns>
		public oLoggedInListDto listLoggedIn (iLoggedInListDto dto)
		{

			if (dto == null)	// rh 20170206 Feature: support empty LoggedInSearchPattern-List
				dto = new iLoggedInListDto ();

			ServiceHandler<iLoggedInListDto, oLoggedInListDto> ew = new ServiceHandler<iLoggedInListDto, oLoggedInListDto> (dto);
			return ew.process (delegate (iLoggedInListDto input, oLoggedInListDto rval)
			{
				rval.loggedInUsers = LogUtil.ListLoggedIn (input.loggedInSearchPattern);
			});
		}

		/// <summary>
		/// Delivers all users in sight of field of the current user
		/// </summary>
		/// <returns></returns>
		public ogetPermittedUsersDto getPermittedUsers ()
		{
			ServiceHandler<long, ogetPermittedUsersDto> ew = new ServiceHandler<long, ogetPermittedUsersDto> (0);

			return ew.process (delegate (long input, ogetPermittedUsersDto rval, CredentialContext ctx)
			{
				IAuthenticationBo authBo = BOFactoryFactory.getInstance ().getAuthenticationBo ();

				rval.users = authBo.getPermittedUsers (ctx.getMembershipInfo ().sysPEROLE);


			});


		}

		/// <summary>
		/// Delivers the Wfuser for the given person 
		/// 
		/// </summary>
		/// <returns></returns>
		public ogetWfuserDto getWfuser (igetWfuserDto input)
		{
			ServiceHandler<igetWfuserDto, ogetWfuserDto> ew = new ServiceHandler<igetWfuserDto, ogetWfuserDto> (input);

			return ew.process (delegate (igetWfuserDto iinput, ogetWfuserDto rval, CredentialContext ctx)
			{
				IAuthenticationBo authBo = BOFactoryFactory.getInstance ().getAuthenticationBo ();

				rval.wfuser = authBo.getWfuserDto (iinput);


			});


		}

		/// <summary>
		/// Delivers Service state information
		/// </summary>
		/// <returns></returns>
		public ServiceInfoDto DeliverServiceInformation ()
		{
			ServiceHandler<long, ServiceInfoDto> ew = new ServiceHandler<long, ServiceInfoDto> (0);
			return ew.process (delegate (long input, ServiceInfoDto rval)
			{
				IStateServiceBo bo = BOFactoryFactory.getInstance ().getStateServiceBo ();
				bo.getServiceInformation (rval);
				rval.logLevel = ProcessUpdater.logLevel;
			});


		}

		/// <summary>
		/// Delivers Service state information
		/// </summary>
		/// <returns></returns>
		public LogInfoDto getLogData ()
		{
			ServiceHandler<long, LogInfoDto> ew = new ServiceHandler<long, LogInfoDto> (0);
			return ew.process (delegate (long input, LogInfoDto Info)
			{

				Info.data = "SETUP.NET - LOG - LOGDATASIZE is 0";
				int CHUNKSIZE = 0;

				String defSize = AppConfig.Instance.getValueFromDb ("SETUP.NET", "LOG", "LOGDATASIZE");
				if (defSize != null)
				{
					CHUNKSIZE = int.Parse (defSize);
					if (CHUNKSIZE > 0)
						Info.data = LogUtil.getLogFileEnd (CHUNKSIZE);
				}

				Info.success ();

			});


		}


		/// <summary>
		/// AppSettingsItems state information
		/// </summary>
		/// <returns></returns>
		public ogetAppSettingsServiceDto getAppSettingsItems (igetAppSettingsItemsDto input)
		{
			ServiceHandler<igetAppSettingsItemsDto, ogetAppSettingsServiceDto> ew = new ServiceHandler<igetAppSettingsItemsDto, ogetAppSettingsServiceDto> (input);


			return ew.process (delegate (igetAppSettingsItemsDto dto, ogetAppSettingsServiceDto rval)
			{

				if (input.bezeichnung == "" && input.syswfuser == 0 && input.sysreg == 0 && input.sysregsec == 0 && input.sysregvar == 0 && input.sysregvar == 0)
					throw new ArgumentException ("No User");

				;

				IAppSettingsBo appBo = BOFactoryFactory.getInstance ().getAppSettingsBO ();

				rval.item = appBo.getAppSettingsItems (input);

			});


		}

		/// <summary>
		/// AppSettingsItems state information
		/// </summary>
		/// <returns></returns>
		public ogetAppSettingsServiceDto CreateorUpdateAppSettingsItems (icreateOrUpdateAppSettingsItemsDto input)
		{
			ServiceHandler<icreateOrUpdateAppSettingsItemsDto, ogetAppSettingsServiceDto> ew = new ServiceHandler<icreateOrUpdateAppSettingsItemsDto, ogetAppSettingsServiceDto> (input);


			return ew.process (delegate (icreateOrUpdateAppSettingsItemsDto dto, ogetAppSettingsServiceDto rval)
			{

				if (input.sysWfuser == 0)
					throw new ArgumentException ("No User");


				IAppSettingsBo appBo = BOFactoryFactory.getInstance ().getAppSettingsBO ();

				rval.item = new ogetAppSettingsItemsDto ();
				rval.item.dtos = appBo.createOrUpdateAppSettingsItems (input);
				ogetAppSettingsItemsDto temp = rval.item;
			});


		}

		/// <summary>
		/// AppSettingsItems state information
		/// </summary>
		/// <returns></returns>
		public ogetAppSettingsServiceDto CreateorUpdateAppSettingsItem (icreateOrUpdateAppSettingsItemDto input)
		{
			ServiceHandler<icreateOrUpdateAppSettingsItemDto, ogetAppSettingsServiceDto> ew = new ServiceHandler<icreateOrUpdateAppSettingsItemDto, ogetAppSettingsServiceDto> (input);


			return ew.process (delegate (icreateOrUpdateAppSettingsItemDto dto, ogetAppSettingsServiceDto rval)
			{

				if (input.regVar.bezeichnung == "" && input.sysWfuser == 0 && input.regVar.sysRegVar == 0)
					throw new ArgumentException ("No User");

				IAppSettingsBo appBo = BOFactoryFactory.getInstance ().getAppSettingsBO ();
				rval.item = new ogetAppSettingsItemsDto ();
				rval.item.dto = appBo.createOrUpdateAppSettingsItem (input);
				ogetAppSettingsItemsDto temp = rval.item;
			});


		}


		/// <summary>
		/// Liefert die Version der ITA WebSearch zurück
		/// </summary>
		/// <param name="info"></param>
		/// <returns>Information</returns>
		public ogetVersionInfo getDocumentSearchVersionInfo (igetVersionInfo info)
		{
			ServiceHandler<igetVersionInfo, ogetVersionInfo> ew = new ServiceHandler<igetVersionInfo, ogetVersionInfo> (info);
			return ew.process (delegate (igetVersionInfo input, ogetVersionInfo rval)
			{
				if (input == null)
					throw new ArgumentException ("No valid input");

				Cic.One.Web.BO.Search.IDocumentSearchBo bo = BOFactoryFactory.getInstance ().getDocumentSearchBO ();
				ogetVersionInfo result = bo.getVersionInfo (input);
				rval.Copyright = result.Copyright;
				rval.ITA_Frameware = result.ITA_Frameware;
				rval.Version = result.Version;
				rval.WFL_Frameware = result.WFL_Frameware;
			});
		}

		/// <summary>
		/// Evaluiert die übergebenen Clarion Expressions
		/// </summary>
		/// <param name="eval"></param>
		/// <returns></returns>
		public oCASEvaluateDto getEvaluation (iCASEvaluateDto eval)
		{
			ServiceHandler<iCASEvaluateDto, oCASEvaluateDto> ew = new ServiceHandler<iCASEvaluateDto, oCASEvaluateDto> (eval);
			return ew.process (delegate (iCASEvaluateDto input, oCASEvaluateDto rval, CredentialContext ctx)
			{
				if (input == null)
					throw new ArgumentException ("No valid input");

				ICASBo bo = BOFactoryFactory.getInstance ().getCASBo ();
				if (bo != null)
				{
					rval.evaluationResults = bo.getEvaluation (input, ctx.getMembershipInfo ().sysWFUSER);
				}


			});
		}

		/// <summary>
		/// evaluates the expression for the given area and areaid
		/// supports the following languages with the given prefix:
		/// c#:   = csharp
		/// vb:   = visual basic
		/// cw:   = clarion (CAS)
		///       = return the expression
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		public oEvalExpressionDto evaluate (iEvalExpressionDto dto)
		{
			ServiceHandler<iEvalExpressionDto, oEvalExpressionDto> ew = new ServiceHandler<iEvalExpressionDto, oEvalExpressionDto> (dto);
			return ew.process (delegate (iEvalExpressionDto input, oEvalExpressionDto rval, CredentialContext ctx)
			{
				if (input == null)
					throw new ArgumentException ("No valid input");
				if (input.area != "NONE")
				{
					ctx.validateService ();

					if (input.context == null)
					{
						input.context = new WorkflowContext ();

						input.context.sysWFUSER = ctx.getMembershipInfo ().sysWFUSER;
						input.context.sysPEROLE = ctx.getMembershipInfo ().sysPEROLE;

					}
					if (input.context.sysWFUSER == 0)
					{
						input.context.sysWFUSER = ctx.getMembershipInfo ().sysWFUSER;
					}
					if (input.context.sysPEROLE == 0)
					{
						input.context.sysPEROLE = ctx.getMembershipInfo ().sysPEROLE;
					}
					input.context.isocode = ctx.getMembershipInfo ().ISOLanguageCode;
				}
				else
				{
					if (input.context == null)
						input.context = new WorkflowContext ();
				}
				WorkflowService ws = new WorkflowService ();
				ws.setWorkflowDao (DAOFactoryFactory.getInstance ().getWorkflowDao ());
				ws.setCASBo (Cic.One.Web.BO.BOFactoryFactory.getInstance ().getCASBo ());
				rval.result = ws.evaluate (input);
				rval.context = input.context;

			}, false);
		}


		//public void doVorgang(String action, EntityD
		/// <summary>
		/// Synchronizes the database with the available gui-templates
		/// </summary>
		/// <param name="viewConfig"></param>
		public void synchronizeViewConfig (isyncViewConfigDto viewConfig)
		{

			IWorkflowBo bo = BOFactoryFactory.getInstance ().getWorkflowBo ();
			bo.synchronizeViewConfig (viewConfig.configs);

		}

		/// <summary>
		/// returns all GUI-Field Translations
		/// by this query
		/// select ctfoid.frontid,typ,verbaldescription MASTER, replaceterm TRANSLATION,replaceblob,isocode 
		/// FROM ctfoid,cttfoid,ctlang where ctfoid.frontid=cttfoid.frontid and ctlang.sysctlang=cttfoid.sysctlang and flagtranslate=1 order by ctfoid.frontid;
		/// </summary>
		/// <returns>oTranslationDto</returns>
		public ogetTranslationsDto getTranslations ()
		{
			ServiceHandler<long, ogetTranslationsDto> ew = new ServiceHandler<long, ogetTranslationsDto> (0);
			return ew.process (delegate (long input, ogetTranslationsDto rval, CredentialContext ctx)
			{


				ITranslateBo Translator = new TranslateBo (Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance ().getTranslateDao ());
				rval.result = Translator.GetStaticList2 ();

			}, false);

		}

        /// <summary>
        /// Returns all CFG/USRCFG key/values
        /// </summary>
        /// <returns></returns>
        public ogetCFGDto getCFG()
        {
            ServiceHandler<long, ogetCFGDto> ew = new ServiceHandler<long, ogetCFGDto>(0);
            return ew.process(delegate(long input, ogetCFGDto rval, CredentialContext ctx)
            {


                rval.cfgs = new UtilityDAO().getConfigList();

            }, false);
        }

		/// <summary>
		/// Liefert alle Quoten incl. deren zeitlicher Gültigkeiten
		/// </summary>
		/// <returns></returns>
		public ogetQuotesDto getQuotes ()
		{
			ServiceHandler<long, ogetQuotesDto> ew = new ServiceHandler<long, ogetQuotesDto> (0);
			return ew.process (delegate (long input, ogetQuotesDto rval, CredentialContext ctx)
			{
				rval.quotes = CommonDaoFactory.getInstance ().getQuoteDao ().getQuotes ();

			}, true);
		}

		/// <summary>
		/// Adds the given id to the queue of future index-updates for the given area
		/// </summary>
		/// <param name="area">case insensitive area</param>
		/// <param name="id"></param>
		public void queueForIndexUpdate (String area, long id)
		{
			LuceneBO.getInstance ().queueForIndexUpdate (area, id);
		}


		/// <summary>
		/// Creates a Deeplink to a password change gui for the current user and sends an email to this user
		/// </summary>
		/// <param name="inp">the wfusername</param>
		/// <returns></returns>
		public ocreatePasswordDeepLink createPasswordDeepLink (icreatePasswordDeepLink inp)
		{
			ServiceHandler<icreatePasswordDeepLink, ocreatePasswordDeepLink> ew = new ServiceHandler<icreatePasswordDeepLink, ocreatePasswordDeepLink> (inp);
			return ew.process (delegate (icreatePasswordDeepLink input, ocreatePasswordDeepLink rval, CredentialContext ctx)
			{
				IAuthenticationBo authBo = BOFactoryFactory.getInstance ().getAuthenticationBo ();
				authBo.createPasswordDeepLink (input.username);
			}, false);
		}

	}

}
