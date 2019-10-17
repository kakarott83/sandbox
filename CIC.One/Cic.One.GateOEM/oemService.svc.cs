using AutoMapper;
using Cic.OpenLease.Service;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using System.Data.Entity;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateOEM.Service.Contract;
using Cic.OpenOne.GateOEM.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.One.GateOEM.BO;
using Cic.One.Web.BO;
using Cic.One.DTO;

namespace Cic.One.GateOEM.Service
{
    /// <summary>
    /// Endpoint for HCE VAP Frontend Interfaces 
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateOEM")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class oemService : IoemService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

     
        /// <summary>
        /// Creates an approval from the external VAP System, returning a unique new approval id and a deeplink into the frontoffice
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateApprovalDto createApproval(icreateApprovalDto inp)
        {
            ServiceHandler<icreateApprovalDto, ocreateApprovalDto> ew = new ServiceHandler<icreateApprovalDto, ocreateApprovalDto>(inp);
            return ew.process(delegate(icreateApprovalDto input, ocreateApprovalDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No input");

                VAPBo vap = new VAPBo();
                ocreateApprovalDto rv = vap.createApproval(input);
                rval.guarantor = rv.guarantor;
                rval.customer = rv.customer;
                rval.deepLink = rv.deepLink;
                rval.obj = rv.obj;
                rval.status = rv.status;
                rval.statusCode = rv.statusCode;
                rval.sysid = rv.sysid;

            },false);

          
        }

        
        /// <summary>
        /// returns the approvalinformation previously created with createApproval for the given approval id approval id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ogetApprovalInformationDto getApprovalInformation(igetApprovalInformationDto inp)
        {

            ServiceHandler<igetApprovalInformationDto, ogetApprovalInformationDto> ew = new ServiceHandler<igetApprovalInformationDto, ogetApprovalInformationDto>(inp);
            return ew.process(delegate(igetApprovalInformationDto input, ogetApprovalInformationDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No input");
               
                VAPBo vap = new VAPBo();
                ogetApprovalInformationDto rv = vap.getApprovalInformation(input);
                rval.guarantor = rv.guarantor;
                rval.customer = rv.customer;
                rval.deepLink = rv.deepLink;
                rval.calculation = rv.calculation;
                rval.obj = rv.obj;
                rval.status = rv.status;
                rval.statusCode = rv.statusCode;
                rval.sysid = rv.sysid;

            },false);
            
        }

		/// <summary>
		/// returns deepLink only
		/// rh 20170213
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public ogetInformationDto getInformation (igetInformationDto inp)
		{
			ServiceHandler<igetInformationDto, ogetInformationDto> ew = new ServiceHandler<igetInformationDto, ogetInformationDto> (inp);
			return ew.process (delegate (igetInformationDto input, ogetInformationDto rval, CredentialContext ctx)
			{
				if (input == null)
					throw new ArgumentException ("No input");

				VAPBo vap = new VAPBo ();
				ogetInformationDto rv = vap.getInformation (input);
				rval.deepLink = rv.deepLink;

			}, false);
		}

		/// <summary>
        /// Processes an invoice from external provider
        /// </summary>
        /// <param name="sXmlInputDataSet"></param>
        /// <returns></returns>
        public string sendInvoice(string sXmlInputDataSet)
        {
            
            ServiceHandler<string, oBaseDto> ew = new ServiceHandler<string, oBaseDto>(sXmlInputDataSet);
            oBaseDto ret= ew.process(delegate(string inputdata, oBaseDto rval, CredentialContext ctx)
            {
                if (inputdata == null)
                    throw new ArgumentException("No valid input");

                new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Santander.InvoiceBo().processInvoice(inputdata);

                rval.success();
            },true);

            return Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Santander.InvoiceBo.getErrorObject(ret);
        }


        /// <summary>
        /// Validates User with Password
        /// </summary>
        /// <param name="sXmlInputDataSet"></param>
        /// <returns>true when Login accepted, false otherwise</returns>
        public bool validateUser(String user, String password)
        {
            IAuthenticationBo authBo = BOFactoryFactory.getInstance().getAuthenticationBo();
            igetValidateUserDto input = new igetValidateUserDto();
            if (password != null)
            {
                password = password.Replace("&quot;", @"""");
            }
            input.userType = 0;
            input.username = user;
            input.password = password;
            ogetValidateUserDto rval = new ogetValidateUserDto();
            try
            {
                authBo.authenticate(input, input.userType, ref rval);
                return true;
            }catch(Exception e)
            {
                _log.Warn("oemService-Login validateUser(" + user + ",xxx) failed with " + e.Message);
                return false;
            }
        }

		/// <summary>
		/// Creates a Deeplink to a password change GUI for the current user and sends an email to this user
		/// (rh: 20161219)
		/// </summary>
		/// <param name="username">the username</param>
		/// <returns></returns>
		public void resetPassword (String username)
		{
			try
			{
				IAuthenticationBo authBo = BOFactoryFactory.getInstance ().getAuthenticationBo ();
				authBo.createPasswordDeepLink (username);
			}
			catch (Exception e)
			{
				_log.Warn ("Password Reset of User(" + username + ") failed with " + e.Message);
			}
		}

        /// <summary>
        /// Validates Guid and returns User Login
        /// </summary>
        /// <param name="sXmlInputDataSet"></param>
        /// <returns>The User Login or null if no Entry found</returns>
        public String validateGuid(String guid)
        {
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "guid", Value = guid });

                Cic.One.DTO.EaihotDto eaihot = ctx.ExecuteStoreQuery<Cic.One.DTO.EaihotDto>("select * from eaihot where computername=:guid", par.ToArray()).FirstOrDefault();
                if (eaihot == null)
                {
                    _log.Warn("oemService-Login validateGuid(" + guid + ") failed with EAIHOT not found");
                    return null;
                }
                if (!eaihot.startdate.HasValue || eaihot.startdate.Value == 0)
                    throw new Exception("oemService-Login validateGuid(" + guid + ") failed: timed out");
                int nowday = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(DateTime.Now);
                if (eaihot.startdate < nowday)
                    throw new Exception("oemService-Login validateGuid(" + guid + ") failed: timed out");

                if (eaihot.starttime.HasValue && eaihot.starttime.Value > 0)
                {
                    int nowtime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                    if ((nowtime < (5 * 6000)))
                    {
                        if (eaihot.starttime.Value < 8610000)
                            throw new Exception("oemService-Login validateGuid(" + guid + ") failed: timed out");
                    }
                    else if (nowtime - eaihot.starttime.Value > (5 * 6000))
                        throw new Exception("oemService-Login validateGuid(" + guid + ") failed: timed out");
                }

                String user = ctx.ExecuteStoreQuery<String>("select code from wfuser where syswfuser=" + eaihot.syswfuser, null).FirstOrDefault();
                if(user==null)
                {
                    _log.Warn("oemService-Login validateGuid(" + guid + ") failed with User for EAIHOT not found");
                }
                return user;
            }
        }
    }
}
