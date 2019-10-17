using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util.SOAP;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Extension;


namespace Cic.One.Web.Contract
{
     /// <summary>
    /// Methods for managing the application (views, rights, users, status, logs, ...)
    /// </summary>
    [ServiceContract(Name = "IappManagementService", Namespace = "http://cic-software.de/One")]
    public interface IappManagementService
    {
        /// <summary>
        /// Flushes the cache
        /// </summary>
        [OperationContract]
        void flushCache();

        /// <summary>
        /// returns a list of all web vlms
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ogetVlmDto getVlmList();

        /// <summary>
        /// Processes a workflow until
        ///  - finished
        ///  - or until bookmark reached (by UserIntarction-Activity)
        ///  
        /// Uses the given input context and returns the modified context
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [OperationContract]
        oprocessWorkflowDto processWorkflow(iprocessWorkflowDto dto);
     
        /// <summary>
        /// Delivers all users in sight of field of the current user
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ogetPermittedUsersDto getPermittedUsers();

        [OperationContract]
        DefaultMessageHeader getHeader();

        /// <summary>
        /// delivers vlm config
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ogetVlmConfigDto getVlmConfig(String vlmid);


         /// <summary>
        /// Delivers Service state information
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ServiceInfoDto DeliverServiceInformation();


         /// <summary>
        /// Delivers Service state information
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        LogInfoDto getLogData();


        /// <summary>
        /// Autorizes the user and delivers user settings
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ogetValidateUserDto getValidateUser(igetValidateUserDto dto);
        
        /// <summary>
        /// login for EAIHOT
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ologinEaiHotDto loginEaiHot(iloginEaiHotDto dto);

        /// <summary>
        ///Delivers user settings
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ogetAppSettingsServiceDto getAppSettingsItems(igetAppSettingsItemsDto input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ogetAppSettingsServiceDto CreateorUpdateAppSettingsItem(icreateOrUpdateAppSettingsItemDto input);

         /// <summary>
        /// AppSettingsItems state information
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ogetAppSettingsServiceDto CreateorUpdateAppSettingsItems(icreateOrUpdateAppSettingsItemsDto input);


        /// <summary>
        /// Liefert die Version der ITA WebSearch zurück
        /// </summary>
        /// <param name="info"></param>
        /// <returns>Information</returns>
        [OperationContract]
        ogetVersionInfo getDocumentSearchVersionInfo(igetVersionInfo info);

         /// <summary>
        /// Evaluiert die übergebenen Clarion Expressions
        /// </summary>
        /// <param name="eval"></param>
        /// <returns></returns>
        [OperationContract]
        oCASEvaluateDto getEvaluation(iCASEvaluateDto eval);


         /// <summary>
        /// Synchronizes the database with the available gui-templates
        /// </summary>
        /// <param name="viewConfig"></param>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        void synchronizeViewConfig(isyncViewConfigDto viewConfig);

        /// <summary>
        /// Evaluates a openlease expression 
        ///  prefix: none - will be returned as string
        ///  prefix: vb: will be evaluated as visual basic
        ///  
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [OperationContract]
        oEvalExpressionDto evaluate(iEvalExpressionDto dto);

         /// <summary>
        /// returns all GUI-Field Translations
        /// by this query
        /// select ctfoid.frontid,typ,verbaldescription MASTER, replaceterm TRANSLATION,replaceblob,isocode 
        /// FROM ctfoid,cttfoid,ctlang where ctfoid.frontid=cttfoid.frontid and ctlang.sysctlang=cttfoid.sysctlang and flagtranslate=1 order by ctfoid.frontid;
        /// </summary>
        /// <returns>oTranslationDto</returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ogetTranslationsDto getTranslations();


        /// <summary>
        /// Adds the given id to the queue of future index-updates for the given area
        /// </summary>
        /// <param name="area">case insensitive area</param>
        /// <param name="id"></param>
        [OperationContract]
        void queueForIndexUpdate(String area, long id);

        /// <summary>
        /// Delivers the Wfuser for the given person 
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ogetWfuserDto getWfuser(igetWfuserDto input);


        /// <summary>
        /// Changes the user's password
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ochangeUserPasswordDto changeUserPassword(ichangeUserPasswordDto input);

        /// <summary>
        /// Creates a Deeplink to a password change gui for the current user and sends an email to this user
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ocreatePasswordDeepLink createPasswordDeepLink(icreatePasswordDeepLink inp);

        /// <summary>
        /// Liefert alle Quoten incl. deren zeitlicher Gültigkeiten
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ogetQuotesDto getQuotes();


		/// <summary>
		/// Log loggedIn users
		/// rh 20170203
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		ologLoggedInDto logLoggedIn (ilogLoggedInDto dto);

		/// <summary>
		/// List loggedIn users
		/// rh 20170203
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		oLoggedInListDto listLoggedIn (iLoggedInListDto dto);

		/// <summary>
		/// SetLoggedInLogProperties
		/// rh 20170209
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		void UpdateLoggedInLogProperties (iLoggedInLogPropertiesDto dto);

		/// <summary>
        /// Returns all CFG/USRCFG key/values
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ogetCFGDto getCFG();

		///// <summary>
		///// Switch Log loggedIn users ON / OFF
		///// rh 20170209
		///// </summary>
		///// <returns></returns>
		//[OperationContract]
		//void switchLoggedInLog (bool bLog);
	}
}