using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public abstract class AbstractKundenRisikoBo : IKundenRisikoBo
    {

      
        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IAngAntDao angAntDao;
        

        /// <summary>
        /// VG DAO
        /// </summary>
        protected IVGDao vgDao;

        /// <summary>
        /// Prisma Dao
        /// </summary>
        protected IPrismaDao pDao;

        /// <summary>
        /// 
        /// </summary>
        protected IPrismaProductBo prodBo;

        /// <summary>
        /// 
        /// </summary>
        protected IMwStBo mwStBo = OpenOne.Common.BO.CommonBOFactory.getInstance().createMwstBo();

        /// <summary>
        /// 
        /// </summary>
        protected IMwStDao mwStDao = OpenOne.Common.DAO.CommonDaoFactory.getInstance().getMwStDao();

        /// <summary>
        /// 
        /// </summary>
        protected KundenRisikoDao trDao = new KundenRisikoDao();

        protected IPrismaParameterBo parBo;

        /// <summary>
        /// 
        /// </summary>
        protected IEaihotDao eaihotDao = new EaihotDao();

       

        /// <summary>
        /// 
        /// </summary>
        protected ITranslateBo translateBo = new TranslateBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao());



        public AbstractKundenRisikoBo(IAngAntDao angAntDao, IVGDao vgDao, IPrismaProductBo pBo, IPrismaParameterBo parBo, IMwStBo mwStBo, IMwStDao mwStDao, ITranslateBo translateBo, KundenRisikoDao trDao, IEaihotDao eaihotDao)
        {

            this.angAntDao = angAntDao;
            this.vgDao = vgDao;
            this.prodBo = pBo;
            this.mwStBo = mwStBo;
            this.mwStDao = mwStDao;
            this.translateBo = translateBo;
            this.trDao = trDao;
            this.eaihotDao = eaihotDao;
            this.parBo = parBo;
        
        }

        /// <summary>
        /// Calculates the credit limits for the context
        /// </summary>
        /// <param name="kontext"></param>
        /// <param name="sysantrag"></param>
        /// <param name="sysvart"></param>
        /// <param name="isoCode"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public abstract List<ProductCreditInfoDto> getCreditLimits(prKontextDto kontext, long sysantrag, long sysvart, String isoCode, long syswfuser);


        /// <summary>
        /// Calculates Credit Expected Loss for the offer and product context
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="kontext"></param>
        /// <returns></returns>
        public abstract VClusterDto getELValues(long sysid, prKontextDto kontext);

        /// <summary>
        /// Calculates Credit Expected Loss for the offer and product context providing gui debug data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract ocheckKrRiskByIdDto checkKrRiskById(icheckKrRiskByIdDto input);


        /// <summary>
        /// Performs the EL Product Validation
        /// </summary>
        /// <param name="rval"></param>
        /// <param name="sysid"></param>
        /// <param name="kontext"></param>
        public abstract void performProductValidation(ocheckAntAngDto rval, long sysid, prKontextDto kontext, String isoCode,bool hasMA);

        /// <summary>
        /// getCreditLimits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract ogetCreditLimitsGUIDto getCreditLimits(igetCreditLimitsGUIDto input );

        /// <summary>
        /// EL_DEFlag  Es sollte kein TR berechnet werden, wenn das CLUSTERVALUE nicht von der DE übergeben wird
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public abstract bool KRBerechnen(long sysid);
                /// <summary>
        /// Returns true if Antrag has a MA
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public abstract bool hasMA(long sysid);
        
    
    }
}
