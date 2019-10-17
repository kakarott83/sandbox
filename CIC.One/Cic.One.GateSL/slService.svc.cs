using Cic.OpenOne.GateSL.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Cic.OpenOne.GateSL.Service.DTO;
using Cic.One.Web.BO;
using Cic.OpenOne.Common.Util.Security;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.One.Web.Service.DAO;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using AutoMapper;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.GateSL.Service
{
    /// <summary>
    /// Endpoint for SL Frontend Service Methods
    /// </summary>
    public class slService : IslService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// loads, recalculates and safes the offer
        /// </summary>
        /// <param name="sysId">offer id</param>
        /// <returns></returns>
        [WebInvoke(Method = "GET",RequestFormat = WebMessageFormat.Json,ResponseFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Bare,UriTemplate = "/recalculateOffer?sysId={sysId}")]
        public orecalculateOfferDto recalculateOffer(long sysId)
        {
            ServiceHandler<long, orecalculateOfferDto> ew = new ServiceHandler<long, orecalculateOfferDto>(sysId);
            return ew.process(delegate (long input, orecalculateOfferDto rval, CredentialContext ctx)
            {
                if (sysId == 0)
                    throw new ArgumentException("No valid offer id");

                //SL-DEMO Wizard Kalkulation

                //get business Objects
                ICalculationBo calcBO = BOFactoryFactory.getInstance().getCalculationBo();
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                //get offer data
                AntragDto antrag = bo.getAntragDetails(input);
                AntkalkDto kalkulation = bo.getAntkalkDetails(sysId);

                //recalc
                isolveKalkulationDto ikalk = calcBO.createIsolveKalkulationFromAntrag(antrag, kalkulation);
                osolveKalkulationDto okalk = new osolveKalkulationDto();
                BOFactoryFactory.getInstance().getCalculationBo().solveKalkulation(ikalk, okalk);
                kalkulation = okalk.antkalk;
                int i = 0;
                foreach (double rate in okalk.raten) kalkulation.zahlplan[i++].betrag = rate;

                //save data
                bo.createOrUpdateAntkalk(kalkulation);


                /*
                //BNOW Offer Kalkulation

                IAngAntBo angant = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createAngAntBo();
                Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto loadedAntrag = angant.getAntrag(sysId);

                IKalkulationBo kalkBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKalkulationBo(ctx.getUserLanguange());
                Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalkulationInput = loadedAntrag.kalkulation;
                Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext kKontext = new Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext();
                if (loadedAntrag.angAntObDto != null)
                {
                    kKontext.grundBrutto = loadedAntrag.angAntObDto.grundBrutto;
                    kKontext.zubehoerBrutto = loadedAntrag.angAntObDto.zubehoerBrutto;
                }
                byte rateError = 0;
                Cic.OpenOne.Common.DTO.Prisma.prKontextDto pKontext = MyCreateProductKontext(ctx, loadedAntrag);
                Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalk = kalkBo.calculate(kalkulationInput, pKontext, kKontext, ctx.getUserLanguange(), ref rateError);
                loadedAntrag.kalkulation = kalk;
                angant.createOrUpdateAntrag(loadedAntrag, ctx.getMembershipInfo().sysPEROLE);
                */


            },true);
        }

        /// <summary>
        /// Creates a ProductKontext for the Offer
        /// </summary>
        /// <param name="cctx"></param>
        /// <param name="antrag"></param>
        /// <returns></returns>
        private static OpenOne.Common.DTO.Prisma.prKontextDto MyCreateProductKontext(CredentialContext cctx, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antrag)
        {
            OpenOne.Common.DTO.Prisma.prKontextDto pKontext = new OpenOne.Common.DTO.Prisma.prKontextDto();

            pKontext.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
            pKontext.sysperole = cctx.getMembershipInfo().sysPEROLE;
            pKontext.sysprproduct = antrag.kalkulation.angAntKalkDto.sysprproduct;
            pKontext.sysbrand = 0;
            if (antrag.kunde != null)
            {
                pKontext.syskdtyp = antrag.kunde.syskdtyp;
            }
            if (antrag.angAntObDto != null)
            {
                pKontext.sysobart = antrag.angAntObDto.sysobart;
                pKontext.sysobtyp = antrag.angAntObDto.sysobtyp;
            }
            pKontext.sysprchannel = antrag.sysprchannel.HasValue? antrag.sysprchannel.Value:0;
            pKontext.sysprhgroup = antrag.sysprhgroup.HasValue ? antrag.sysprhgroup.Value : 0;
            pKontext.sysprusetype = antrag.kalkulation.angAntKalkDto.sysobusetype;

            return pKontext;
        }

        [WebInvoke(Method = "POST",RequestFormat = WebMessageFormat.Json,ResponseFormat = WebMessageFormat.Json,BodyStyle =WebMessageBodyStyle.Bare,UriTemplate = "/recalcOffer")]
        public oBaseDto recalcOffer(recalcInput inp)
        {

            ServiceHandler<recalcInput, oBaseDto> ew = new ServiceHandler<recalcInput, oBaseDto>(inp);
            return ew.process(delegate (recalcInput inputData, oBaseDto rval, CredentialContext ctx)
            {
                if (inputData == null||inputData.nummer==null)
                    throw new ArgumentException("No valid offer number");


                //get business Objects
                ICalculationBo calcBO = BOFactoryFactory.getInstance().getCalculationBo();
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(null);

                //get technical id from antrag number
                SearchDao<long> sd = new SearchDao<long>();
                long sysId = sd.getFirstOrDefault("select sysid from antrag where antrag=:antrag", new { antrag = inputData.nummer });

                if(sysId==0)
                    throw new ArgumentException("No valid offer number");

                //get offer data
                AntragDto antrag = bo.getAntragDetails(sysId);
                AntkalkDto kalkulation = bo.getAntkalkDetails(sysId);

                //recalc
                isolveKalkulationDto ikalk = calcBO.createIsolveKalkulationFromAntrag(antrag, kalkulation);
                osolveKalkulationDto okalk = new osolveKalkulationDto();
                BOFactoryFactory.getInstance().getCalculationBo().solveKalkulation(ikalk, okalk);
                kalkulation = okalk.antkalk;
                int i = 0;
                foreach (double rate in okalk.raten)
                {
                    kalkulation.zahlplan[i++].betrag = rate;
                }

                //save data
                bo.createOrUpdateAntkalk(kalkulation);

            }, true);
        }
    }
    
    public class recalcInput
    {
        
        public String nummer;
        
        public String info;
        
    }
}
