using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.Util.Exceptions;

namespace Cic.One.Web.BO
{
    public class ServiceHandlerSimple
    {
        public static T process<B, T>(B bo, Func<B, T> func)
            where T : oBaseDto, new()
        {
            CredentialContext cctx = new CredentialContext();
            T rval = new T();
            try
            {
                rval = func(bo);
                rval.success();
            }
            catch (Microsoft.Exchange.WebServices.Data.ServiceResponseException e)
            {
                //TODO Fehlercode hinzufügen
                cctx.fillBaseDto(rval, e, "Exchange_ServiceError");
            }
            catch (System.Data.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
            }
            catch (AutoMapper.AutoMapperMappingException e)
            {
                cctx.fillBaseDto(rval, e.InnerException, "AutoMapperError");
                return rval;
            }
            catch (System.Data.UpdateException e)
            {
                cctx.fillBaseDto(rval, e.InnerException, "UpdateException");
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
            }
            return rval;
        }


        public static T process<B, T>(B bo, Action<B, T> action)
            where T : oBaseDto, new()
        {
            CredentialContext cctx = new CredentialContext();
            T rval = new T();
            try
            {
                action(bo,rval);
                rval.success();
            }
            catch (Microsoft.Exchange.WebServices.Data.ServiceResponseException e)
            {
                //TODO Fehlercode hinzufügen
                cctx.fillBaseDto(rval, e, "Exchange_ServiceError");
            }
            catch (System.Data.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
            }
            return rval;
        }
    }
}