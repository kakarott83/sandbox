using AutoMapper;
using Cic.One.DTO;
using Cic.One.DTO.BN;
using Cic.One.GateBANKNOW.BO;
using Cic.One.GateBANKNOW.Contract;
using Cic.One.Web.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Linq;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.One.Web.DAO;
using Cic.One.GateBANKNOW.DTO;

namespace Cic.One.GateBANKNOW
{
    /// <summary>
    /// Service-Endpoint for BANKNOW Porsche Specific Functions
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class BNPartnerService : IBNPartnerService
    {

        /// <summary>
		/// Delivers Service state information
		/// </summary>
		/// <returns></returns>
		public ogetStatusDto getStatus()
        {
            
            ServiceHandler<long, ogetStatusDto> ew = new ServiceHandler<long, ogetStatusDto>(0);
            ogetStatusDto rvalue= ew.process(delegate (long input, ogetStatusDto rval, CredentialContext cctx)
            {
                IStateServiceBo bo = BOFactoryFactory.getInstance().getStateServiceBo();
                ServiceInfoDto sinfo = new ServiceInfoDto();
                bo.getServiceInformation(sinfo);
            });
            
            return rvalue;
        }

        /// <summary>
        /// Delivers a deeplink for the given offer
        /// </summary>
        /// <returns></returns>
        public ogetLinkDto getLink(igetLinkDto input)
        {
            ServiceHandler<igetLinkDto, ogetLinkDto> ew = new ServiceHandler<igetLinkDto, ogetLinkDto>(input);
            ogetLinkDto rvalue= ew.process(delegate (igetLinkDto inp, ogetLinkDto rval, CredentialContext cctx)
            {
                PartnerBO bo = new PartnerBO();
                rval.deeplink = bo.getLink(inp);
            });
            return rvalue;
        }

        /// <summary>
        /// creates a new offer including calculation, customer, objectdata
        /// </summary>
        /// <returns></returns>
        public ocreateAntragDto createAntrag(icreateAntragDto input)
        {
            ServiceHandler<icreateAntragDto, ocreateAntragDto> ew = new ServiceHandler<icreateAntragDto, ocreateAntragDto>(input);
            ServiceBaseException ferr=null;
            ocreateAntragDto rvalue= ew.process(delegate (icreateAntragDto inp, ocreateAntragDto rval,CredentialContext cctx)
            {
                PartnerBO bo = new PartnerBO();
                ferr = bo.createAntrag(inp );

                igetLinkDto ilink = new igetLinkDto();
                ilink.extdealerid = inp.extdealerid;
                ilink.extuserid = inp.extuserid;
                ilink.extreferenz = inp.antrag.extreferenz;

                rval.deeplink = bo.getLink(ilink); 

               
            });

            if (ferr != null)
            {
                rvalue.message.code = ferr.code;
                rvalue.message.type = ferr.type;
            }

            return rvalue;
        }
    }
       
}
