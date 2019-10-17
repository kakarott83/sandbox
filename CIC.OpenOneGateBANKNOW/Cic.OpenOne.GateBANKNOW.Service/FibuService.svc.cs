using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Service.Contract;

using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO;
using AutoMapper;

namespace Cic.OpenOne.GateBANKNOW.Service
{

    /// <summary>
    /// FibuService
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class FibuService : IFibuService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public oMessagingDto FibuTest(string input)
        {

            oMessagingDto rval = new oMessagingDto();
            if (input == "TR")
            {
                rval.message.code = "True";
            }
            else
            {
                rval.message.code = "False";
            }
           
            rval.message.detail = "FibuTest OK!";
            rval.Output = true;
            rval.success();

            return rval;
        }

        /// <summary>
        /// UploadAccountMasterdata
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public oMessagingDto UploadAccountMasterdata(iFibuAccountMasterDTO input)
        {
            oMessagingDto rval = new oMessagingDto();

            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();

                if (input.fibuAccountList == null)
                {
                    throw new ArgumentException("UploadAccountMasterdata: no data to import 1");
                }

                if (input.fibuAccountList.Count == 0)
                {
                    throw new ArgumentException("UploadAccountMasterdata: no data to import 2");
                }

                FibuAccountMasterDTO fibuMasterDto = new FibuAccountMasterDTO();


                fibuMasterDto = Mapper.Map<iFibuAccountMasterDTO, FibuAccountMasterDTO>(input);


                FibuIFBookDao fibuDao = new FibuIFBookDao(fibuMasterDto);

                string ret = fibuDao.SaveDataInDB();

                if (ret == "")
                {
                    rval.message.code = "True";
                    rval.message.detail = "UploadAccountMasterdata OK!";
                    rval.Output = true;
                    rval.success();
                }
                else
                {
                    rval.success();

                    rval.message.code = "False";
                    rval.message.detail = "UploadAccountMasterdata NOK!";
                    rval.Output = false;
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
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }
    }
}

