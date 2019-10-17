using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abstrakter BankNow Kalkulator
    /// </summary>
    public abstract class AbstractBankNowCalculator : IBankNowCalculator
    {
        /// <summary>
        /// Objekttyp DAO
        /// </summary>
        protected IObTypDao obTypDao;
        /// <summary>
        /// Zins DAO
        /// </summary>
        protected IZinsDao zinsDao;
        /// <summary>
        /// Prisma DAO
        /// </summary>
        protected IPrismaDao prismaDao;
        /// <summary>
        /// VG DAO
        /// </summary>
        protected IVGDao vgdao;
        /// <summary>
        /// Provision DAO
        /// </summary>
        protected IProvisionDao ProvisionDao;
        /// <summary>
        /// Subventions DAO
        /// </summary>
        protected ISubventionDao subventionDao;
        /// <summary>
        /// Versicherungs DAO
        /// </summary>
        protected IInsuranceDao insuranceDao;
        /// <summary>
        /// Mehrwertsteuer-Ermittlungs DAO
        /// </summary>
        protected IMwStDao MwStDao;
        /// <summary>
        /// Quote DAO
        /// </summary>
        protected IQuoteDao quoteDao;
        /// <summary>
        /// ISO Sprachen code
        /// </summary>
        protected string isoCode;

        /// <summary>
        /// Prisma Services Dao
        /// </summary>
        protected IPrismaServiceDao prismaServiceDao;

        /// <summary>
        /// Kalkulation Dao
        /// </summary>
        protected IKalkulationDao kalkulationDao;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="obTypDao">Objekttyp DAO</param>
        /// <param name="zinsDao">Zins DAO</param>
        /// <param name="prismaDao">Prisma DAO</param>
        /// <param name="vgdao">VG DAO</param>
        /// <param name="ProvisionDao">Provisions DAO</param>
        /// <param name="subventionDao">Suubventions DAO</param>
        /// <param name="insuranceDao">Versicherungs DAO</param>
        /// <param name="mwstDao">Mehrwertsteuer Ermittlungs DAO</param>
        /// <param name="quoteDao">Quote DAO</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="kalkulationDao">kalkulation DAO</param>
        public AbstractBankNowCalculator(IObTypDao obTypDao, IZinsDao zinsDao, IPrismaDao prismaDao, IVGDao vgdao, IProvisionDao ProvisionDao, ISubventionDao subventionDao, IInsuranceDao insuranceDao, IMwStDao mwstDao, IQuoteDao quoteDao, String isoCode, IKalkulationDao kalkulationDao, IPrismaServiceDao prismaServiceDao)
        {
            this.obTypDao = obTypDao;
            this.prismaDao = prismaDao;
            this.zinsDao = zinsDao;
            this.vgdao = vgdao;
            this.ProvisionDao = ProvisionDao;
            this.isoCode = isoCode;
            this.subventionDao = subventionDao;
            this.insuranceDao = insuranceDao;
            this.MwStDao = mwstDao;
            this.quoteDao = quoteDao;
            this.kalkulationDao = kalkulationDao;
            this.prismaServiceDao = prismaServiceDao;
        }


        /// <summary>
        /// calculates the calculation
        /// </summary>
        /// <param name="kalkulation">Kalkulations DTO</param>
        /// <param name="prodCtx">Produktkontext</param>
        /// <param name="kalkCtx">Berechnungs-Kontext</param>
        /// <param name="rateError">Fehler bei Ratenberechnung</param>
        /// <returns></returns>
        public abstract KalkulationDto calculate(KalkulationDto kalkulation, prKontextDto prodCtx, kalkKontext kalkCtx, ref byte rateError);


        /// <summary>
        /// Calculates Provisions for Expected Loss Calculations
        /// uses a minimum required input interfaces
        /// </summary>
        /// <param name="prodCtx"></param>
        /// <param name="kundenScore"></param>
        /// <param name="finanzierungsbetrag"></param>
        /// <param name="zinsertrag"></param>
        /// <returns></returns>
        public abstract List<AngAntProvDto> calculateProvisionsDirect(prKontextDto prodCtx, String kundenScore, double finanzierungsbetrag, double zinsertrag);
    }
}
