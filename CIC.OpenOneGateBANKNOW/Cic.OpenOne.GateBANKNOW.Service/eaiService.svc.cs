using AutoMapper;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    /// <summary>
    /// Endpoint for EAI interfacing
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class eaiService : IeaiService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// internally used method to trigger a webservice-call
        /// internal version for job-service to fire an event
        /// </summary>
        /// <param name="sysEaiHOT"></param>
        /// <returns></returns>
        public oEAIExecDto execEAIHOT(int sysEaiHOT)
        {
            CredentialContext cctx = new CredentialContext();
            /*
             * Beispiel für Verarbeitung per Service
                EAIART.CODE = “CALL_BOS”

                EAIHOT.OLTABLE = VT
                EAIHOT.CODE = createOrUpdateDMSAkte
                EAIHOT.SYSOLTABLE = 2438
                */
            ServiceHandler<int, oEAIExecDto> ew = new ServiceHandler<int, oEAIExecDto>(sysEaiHOT);
            return ew.process(delegate(int input, oEAIExecDto rval)
            {

                
                    if (input == 0)
                        throw new ArgumentException("No valid input");
                
                    BOFactory.getInstance().createEaihotBo().execEAIHOT(input);
                    rval.success();
               
            });
        }

    }
}
