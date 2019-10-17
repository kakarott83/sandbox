using AutoMapper;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service
{
   
    /// <summary>
    /// Service providing trigger access from DMS into OL
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class dmsService : IdmsService
    {
        /// <summary>
        /// interface from DMS to OL for new incoming Documents
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public oDMSUploadDto execDMSUploadTrigger(iDMSUploadDto input)
        {
            
            
            ServiceHandler<iDMSUploadDto, oDMSUploadDto> ew = new ServiceHandler<iDMSUploadDto, oDMSUploadDto>(input);
            return ew.process(delegate(iDMSUploadDto inputdata, oDMSUploadDto rval, CredentialContext ctx)
            {
                if (inputdata == null)
                    throw new ArgumentException("No valid input");
                
				ctx.validateService();
				
                BOFactory.getInstance().createDMSBo().execDMSUploadTrigger(ctx.getMembershipInfo().sysPEROLE, ctx.getMembershipInfo().sysWFUSER, Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.iDMSUploadDto, Cic.OpenOne.GateBANKNOW.Common.DTO.iDMSUploadDto>(inputdata));
                rval.success();
            });            
        }
       
    }
}
