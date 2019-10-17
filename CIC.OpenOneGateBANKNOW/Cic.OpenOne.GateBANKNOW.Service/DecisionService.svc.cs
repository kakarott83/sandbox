using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Reflection;
using System.ServiceModel;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    using AutoMapper;

    using Common.BO;

    /// <summary>
    /// Der Service AuskunftService ruft die im Auskunfttypen angegebene Schnittstellenmethode auf
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
    public class DecisionService : IDecisionService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  INT 2: Guardean Decision Engine Result Interface 
        ///  
        ///  receives the CreditDecision result, asynchronously from SHS Guardean
        /// </summary>
        /// <param name="input"></param>
        public osetDecisionResultDto setCreditDecisionResult(isetDecisionResultDto input)
        {
            osetDecisionResultDto rval = new osetDecisionResultDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DecisionEngineGuardeanExecute ex = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DecisionEngineGuardeanExecute();
                ex.setCreditDecisionResult(input.input);
                rval.success();
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

        /// <summary>
        /// INT 3: Guardean aggregation Interface
        /// Performs an aggregation for the given Customer for an external decision Engine
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public getAggregationDto getAggregation(igetAggregationDto input)
        {
            getAggregationDto rval = new getAggregationDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DecisionEngineGuardeanExecute ex = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DecisionEngineGuardeanExecute();
                rval.response = ex.getAggregation(input.req);
                rval.success();
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

        /// <summary>
        /// INT 5: returns a certain Document. This method is only implemented for SHS MUW
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        public ogetDocumentDto getDocument(long sysdmsdoc)
        {
            ogetDocumentDto rval = new ogetDocumentDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                IDocumentServiceBo doc = BOFactory.getInstance().createDocumentServiceBo();
                Common.DTO.DmsDocDto dmsdoc = doc.getDocument(sysdmsdoc);
                Common.DTO.DocumentTypeDto dmsdoctype = null;


                if (dmsdoc != null)
                {
                    // cctx.getMembershipInfo().ISOLanguageCode
                    dmsdoctype = doc.getDocumentType("de-DE", dmsdoc.syswftx);
                }

                rval.document = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto, Cic.OpenOne.GateBANKNOW.Service.DTO.DmsDocDto>(dmsdoc);
                rval.documentType = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.DocumentTypeDto, Cic.OpenOne.GateBANKNOW.Service.DTO.DocumentTypeDto>(dmsdoctype);
                rval.success();
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
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// INT 6: Guardean liability chain Interface
        /// Returns all KNE that are contained in the liability chain of the corresponding person.
        /// First we search for all GvK A/B and
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ogetLiabilityChainDto getLiabilityChain(igetLiabilityChainDto input)
        {
            ogetLiabilityChainDto rval = new ogetLiabilityChainDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DecisionEngineGuardeanExecute ex = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DecisionEngineGuardeanExecute();
                rval.response = ex.getLiabilityChain(input.req);
                rval.success();
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

        /// <summary>
        /// INT 7: Sets the customer check result.
        /// This was removed from INT 2 and put into it's own API.
        /// It is used for setting the customer check result for mainApplicant, CoApplicant, Guarantor, authorizedRepresentative, UBO, CBUProspect, CEO
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public osetCustomerCheckResult setCustomerCheckResult(isetCustomerCheckResult input)
        {
            osetCustomerCheckResult rval = new osetCustomerCheckResult();
            CredentialContext cctx = new CredentialContext();
            try
            {
                Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DecisionEngineGuardeanExecute ex = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DecisionEngineGuardeanExecute();
                rval.response = ex.setCustomerCheckResult(input.req);
                rval.success();
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