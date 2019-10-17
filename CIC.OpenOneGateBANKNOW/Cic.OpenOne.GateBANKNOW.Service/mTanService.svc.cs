using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using AutoMapper;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    /// <summary>
    /// Service for accessing the BNOW B2BOL AD User Management Service
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class mTanService : ImTanService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Creates a BNOW User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ocreatemTanUserDto CreateUser(imTanUserDto user)
        {
            ocreatemTanUserDto rval = new ocreatemTanUserDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
				if(String.IsNullOrEmpty(user.user.language))
                user.user.language = cctx.getMembershipInfo().ISOLanguageCode;
                GateBANKNOW.Common.DTO.imTanUserDto svcuser = Mapper.Map<imTanUserDto, GateBANKNOW.Common.DTO.imTanUserDto>(user);
                GateBANKNOW.Common.DTO.ocreatemTanUserDto ruser = BOFactory.getInstance().createB2BOLBo().CreateUser(svcuser);
                Mapper.Map<GateBANKNOW.Common.DTO.ocreatemTanUserDto, ocreatemTanUserDto>(ruser, rval);

                rval.success();

                rval.message.detail = ruser.status.message;
                if (!"true".Equals(ruser.status.success))
                {
                    rval.message.type = OpenOne.Common.DTO.MessageType.Error;
                    rval.message.code = "403";
                }

                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shouldnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        ///  Reads a BNOW User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ogetmTanUserDataDto GetData(imTanUserDto user)
        {
            ogetmTanUserDataDto rval = new ogetmTanUserDataDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                cctx.validateService();

                if( (user.user.applicationId == null || user.user.applicationId.Length == 0) &&
                (user.user.userId==null||user.user.userId.Length==0) )
                {
                    rval.message.type = OpenOne.Common.DTO.MessageType.Error;
                    rval.message.code = "403";
                    rval.message.detail = "No applicationId or userId";
                    return rval;
                }


				if(String.IsNullOrEmpty(user.user.language))
                user.user.language = cctx.getMembershipInfo().ISOLanguageCode;

                GateBANKNOW.Common.DTO.imTanUserDto svcuser = Mapper.Map<imTanUserDto, GateBANKNOW.Common.DTO.imTanUserDto>(user);

                GateBANKNOW.Common.DTO.ogetmTanUserDataDto ruser = BOFactory.getInstance().createB2BOLBo().GetData(svcuser);
                Mapper.Map<GateBANKNOW.Common.DTO.ogetmTanUserDataDto, ogetmTanUserDataDto>(ruser, rval);

                rval.success();

                rval.message.detail = ruser.status.message;
                if (!"true".Equals(ruser.status.success))
                {
                    rval.message.type = OpenOne.Common.DTO.MessageType.Error;
                    rval.message.code = "403";
                }



                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shouldnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Writes a BNOW User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public osetmTanUserDataDto SetData(imTanUserDto user)
        {
            osetmTanUserDataDto rval = new osetmTanUserDataDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                cctx.validateService();

                if ((user.user.applicationId == null || user.user.applicationId.Length == 0) &&
                (user.user.userId == null || user.user.userId.Length == 0))
                {
                    rval.message.type = OpenOne.Common.DTO.MessageType.Error;
                    rval.message.code = "403";
                    rval.message.detail = "No applicationId and userId";
                    return rval;
                }

				if(String.IsNullOrEmpty(user.user.language))
                user.user.language = cctx.getMembershipInfo().ISOLanguageCode;

                GateBANKNOW.Common.DTO.imTanUserDto svcuser = Mapper.Map<imTanUserDto, GateBANKNOW.Common.DTO.imTanUserDto>(user);


                GateBANKNOW.Common.DTO.osetmTanUserDataDto ruser = BOFactory.getInstance().createB2BOLBo().SetData(svcuser);
                Mapper.Map<GateBANKNOW.Common.DTO.osetmTanUserDataDto, osetmTanUserDataDto>(ruser, rval);

                rval.success();
                rval.message.detail = ruser.status.message;
                if (!"true".Equals(ruser.status.success))
                {
                    rval.message.type = OpenOne.Common.DTO.MessageType.Error;
                    rval.message.code = "403";
                }




                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shouldnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Changes the BNOW User Password
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public osetPasswordDto SetPassword(isetmTanUserPasswordDto input)
        {
            osetPasswordDto rval = new osetPasswordDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                cctx.validateService();

                GateBANKNOW.Common.DTO.isetmTanUserPasswordDto svcuser = Mapper.Map<isetmTanUserPasswordDto, GateBANKNOW.Common.DTO.isetmTanUserPasswordDto>(input);


                GateBANKNOW.Common.DTO.mTanStatusDto ruser = BOFactory.getInstance().createB2BOLBo().SetPassword(svcuser);


                rval.success();
                rval.message.detail = ruser.message;
                if (!"true".Equals(ruser.success))
                {
                    if (ruser.message!=null && ruser.message.IndexOf("technical") > -1)
                    {
                        rval.message.type = OpenOne.Common.DTO.MessageType.Error;
                        rval.message.code = "403";
                    }
                    else
                    {
                        rval.message.type = OpenOne.Common.DTO.MessageType.Info;
                        rval.message.code = "1";
                    }
                
                }




                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shouldnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }
    }
}
