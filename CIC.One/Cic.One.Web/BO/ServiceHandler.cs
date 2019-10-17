using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.Util.Exceptions;

namespace Cic.One.Web.BO
{
    /// <summary>
    /// Wraps common Service Endpoint Processing 
    ///  - Exception-Management
    ///  - Error-Handling
    ///  - Credential-Management
    ///  
    ///  All Endpoint Processing should occur inside the ProcessServiceMethod-Delegate
    /// 
    ///  Example:
    ///  ServiceHandler<icreateOrUpdatePtaskDto, ocreateOrUpdatePtaskDto> ew = new ServiceHandler<icreateOrUpdatePtaskDto, ocreateOrUpdatePtaskDto>(ptask);
    ///        return ew.process(delegate(icreateOrUpdatePtaskDto input, ocreateOrUpdatePtaskDto rval, CredentialContext ctx)
    ///        {
    ///
    ///            if (input == null || input.ptask == null)
    ///                throw new ArgumentException("No valid input");
    ///
    ///            rval.ptask = BOFactory.getInstance().getCRMEntityMailBO(input.ptask.sysOwner).createOrUpdatePtask(input);
    ///
    ///        });
    /// </summary>
    /// <typeparam name="X">The Webservice Input Type</typeparam>
    /// <typeparam name="Y">The Webservice Output Type</typeparam>
    public class ServiceHandler<X, Y> where Y : oBaseDto
    {
        private X input;
        private Y rval;

        public delegate void ProcessServiceMethod<in S, in T>(X input, Y output);
        public delegate void ProcessServiceMethod<in S, in T, in Z>(X input, Y output, Z cctx) where Z: CredentialContext;

        public ServiceHandler(X input, Y output)
        {
            this.input = input;
            this.rval = output;
        }

        public ServiceHandler(X input)
        {
            this.input = input;
            rval = (Y)Activator.CreateInstance(typeof(Y));
        }

        public ServiceHandler()
        {
            rval = (Y)Activator.CreateInstance(typeof(Y));
        }

        public Y process(ProcessServiceMethod<X, Y> m)
        {
            CredentialContext cctx = new CredentialContext();
            try
            {
                
                m(input, rval);
                rval.success();
                return rval;
            }
            catch (System.Data.EntityException e)
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
                cctx.fillBaseDto(rval, e.InnerException!=null?e.InnerException:e, "F_00001_GeneralError");
                return rval;
            }
        }

        public Y process(ProcessServiceMethod<X, Y, CredentialContext> m)
        {
            return process(m,true);
        }
        public Y process(ProcessServiceMethod<X, Y, CredentialContext> m, bool validate)
        {
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (validate) 
                    cctx.validateService();
                m(input, rval, cctx);
                rval.success();
                return rval;
            }
            catch (System.Data.EntityException e)
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
                cctx.fillBaseDto(rval, e.InnerException != null ? e.InnerException : e, "F_00001_GeneralError");
                return rval;
            }
        }
    }
}