using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
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
    public abstract class AbstractTransactionRisikoBo : ITransactionRisikoBo
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
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IEurotaxDBDao eurotaxDBDao;

        /// <summary>
        /// 
        /// </summary>
        protected IEurotaxBo eurotaxBo;

        /// <summary>
        /// 
        /// </summary>
        protected IMwStDao mwStDao = OpenOne.Common.DAO.CommonDaoFactory.getInstance().getMwStDao();

        /// <summary>
        /// 
        /// </summary>
        protected TransactionRisikoDao trDao = new TransactionRisikoDao();

        /// <summary>
        /// 
        /// </summary>
        protected IEaihotDao eaihotDao = new EaihotDao();

       

        /// <summary>
        /// 
        /// </summary>
        protected ITranslateBo translateBo = new TranslateBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao());


        public AbstractTransactionRisikoBo(IAngAntDao angAntDao, IVGDao vgDao, IEurotaxDBDao eurotaxDBDao, IEurotaxBo eurotaxBo, IMwStDao mwStDao, ITranslateBo translateBo, TransactionRisikoDao trDao, IEaihotDao eaihotDao)
        {

            this.angAntDao = angAntDao;
            this.vgDao = vgDao;
            this.eurotaxDBDao = eurotaxDBDao;
            this.eurotaxBo = eurotaxBo;
            this.mwStDao = mwStDao;
            this.translateBo = translateBo;
            this.trDao = trDao;
            this.eaihotDao = eaihotDao;
        
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract ofinVorEinreichungDto processFinVorEinreichung(ifinVorEinreichungDto input);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="user"></param>
        /// <param name="isoCode"></param>
        /// <param name="kalkVariante"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public abstract ocheckTrRiskDto checkTrRisk(icheckTrRiskDto input, long user, string isoCode, bool kalkVariante, long syswfuser);

        /// <summary>
        /// checkTrRiskBySysid
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoCode"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public abstract ocheckTrRiskDto checkTrRiskBySysid(long sysid, long sysPEROLE, string isoCode, long syswfuser);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract orisikoSimDto risikoSim(irisikoSimDto input);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract osolveKalkVariantenDto solveKalkVarianten(isolveKalkVariantenDto input);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract ocheckTrRiskByIdDto checkTrRiskBySysid(icheckTrRiskByIdDto inDto);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoCode"></param>
        /// <param name="outputGUI"></param>
        /// <returns></returns>
        public abstract ocheckTrRiskDto checkTrRisk(icheckTrRiskDto input, long sysPEROLE, string isoCode, bool kalkVariante, ref ocheckTrRiskByIdDto outputGUI);

        /// <summary>
        /// getV_cluster
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public abstract VClusterDto getV_cluster(long sysid, bool saveMarktwerteInDB, long sysprproduct, long sysperole);

        /// <summary>
        /// deleteVarianteUndDERulsByAntrag
        /// </summary>
        /// <param name="sysid"></param>
        public abstract void deleteVarianteUndDERulsByAntrag(long sysid);

        /// <summary>
        /// verwendet die gespeicherte antkalkvar für die finanzierungsvorschlags-Variante, berechnet mit WS solve neu und liefert das AngAntKalkDto
        /// </summary>
        /// <param name="antrag">antrag</param>
        /// <param name="rang">rang</param>
        /// <param name="sysPEROLE">sysPEROLE</param>
        /// <param name="isoCode">isoCode</param>
        /// <returns></returns>
        public abstract AngAntKalkDto getVariante(AntragDto antrag, long rang, long sysPEROLE, string isoCode, bool mitRisikoFilter);

        /// <summary>
        /// REMO-Staffel wird  angelegt
        /// </summary>
        /// <param name="sysid"></param>
        public abstract void remostaffelAnlegen(long sysid, long sysprproduct, long sysperole);

        /// <summary>
        /// EL-Kalk soll aber nur dann erfolgen, wenn ein EAIPAR-Statement (EAIPAR:CODE = 'EL_KALK') zum Antrag eine 1 liefert 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public abstract bool getEL_KALKFlag(long sysid);
    }
}
