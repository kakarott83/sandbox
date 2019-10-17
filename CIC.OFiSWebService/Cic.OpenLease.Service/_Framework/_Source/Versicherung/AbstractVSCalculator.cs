using Cic.OpenLease.Service.Properties;
using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenLease.Service.Services.DdOl.DTO;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service.Versicherung
{
    /// <summary>
    /// Abstract Base Class of all ensurance calculators
    /// QUOTE VERSICHERUNGSSTEUER 
    /// </summary>
    public abstract class AbstractVSCalculator : IVSCalculator
    {


        protected static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<String, List<VsTypPosDto>> vsCache = CacheFactory<String, List<VsTypPosDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        #region Constants

        #endregion

        /// <summary>
        /// Returns all vstp positions for the vstyp
        /// </summary>
        /// <param name="sysvstyp"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected List<VsTypPosDto> getVSTypPos(long sysvstyp, DdOlExtended context)
        {
            if(!vsCache.ContainsKey("VSTYP"))
            {
                List<VsTypPosDto> positions = context.ExecuteStoreQuery<VsTypPosDto>("select * from vstyppos order by rang", null).ToList();
                vsCache["VSTYP"] = positions;
            }
            return (from f in vsCache["VSTYP"]
                    where f.sysVSTyp == sysvstyp
                    select f).ToList();
            
        }

        #region My Methods
        protected decimal MyDeliverSteuer(DdOlExtended context)
        {
            return QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_VSSTEUER);
        }
        protected decimal MyDeliverSteuerPers(DdOlExtended context)
        {
            return QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_VSSTEUERPERSONENBEZOGEN);
        }

        /// <summary>
        /// Returns the sum of all insurance tax positions
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected decimal getSteuerSum(List<ServiceAccess.DdOl.InsuranceResultDto> pos)
        {
            if (pos == null) return 0;
            return (from f in pos
                    select f.Versicherungssteuer).Sum();

        }
        /// <summary>
        /// Return true if the product has a vart of kind leasing
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        protected bool isLeasing(DdOlExtended context, long sysprproduct)
        {
            CalculationDao calcDao = new CalculationDao(context);
            VartDTO va = calcDao.getVART(sysprproduct);
            
            if(va==null) return false;
            return (va.CODE.IndexOf("LEASING") > -1);
        }
        protected decimal MyDeliverSubvention(DdOlExtended context, decimal defaultValue, long sysprproduct, int area, long areaid, long term)
        {
            Subvention sub = new Subvention(context);
            PrismaDao pd = new CachedPrismaDao();
            long sysvart = pd.getVertragsart(sysprproduct).SYSVART;// Cic.OpenLease.Model.DdOl.PRPRODUCTHelper.DeliverSYSVART(context, sysprproduct);
            decimal Ust = LsAddHelper.GetTaxRate(context, sysvart);
            return sub.deliverSubvention(defaultValue, sysprproduct, area, areaid, term, Ust);
        }

        /// <summary>
        /// Berechnet Subvention explizit (durch Konfiguration) sowie implizit durch Nachlass. Versicherungsprämie wird entsprechend reduziert
        /// </summary>
        /// <param name="context"></param>
        /// <param name="param"></param>
        /// <param name="rval"></param>
        /// <param name="versicherungsSteuerProzent"></param>
        protected void MySetSubventionNachlass(DdOlExtended context, InsuranceParameterDto param, InsuranceResultDto rval, decimal versicherungsSteuerProzent)
        {


            //Expl. Subvention:
            decimal pvorSub = rval.Praemie_Default * param.Laufzeit;
            decimal subbrutto = MyDeliverSubvention(context, pvorSub, param.sysPrProduct, Subvention.CnstAREA_INSURANCE, param.SysVSTYP, param.Laufzeit);
            rval.Praemie_Subvention = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(pvorSub - subbrutto);
            rval.Praemie_Subvention /= (1 + versicherungsSteuerProzent / 100);

            //Implicit Subvention by Nachlass
            //rval.Netto = (subbrutto / param.Laufzeit) / (1 + versicherungsSteuerProzent / 100);
            rval.Netto = ((subbrutto / param.Laufzeit) - rval.Motorsteuer) / (1 + (decimal)(versicherungsSteuerProzent / 100));

            if (param.Nachlass != 0 && param.Zahlmodus != 0)
                rval.Netto -= (param.Nachlass * (12 / param.Zahlmodus));
            rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
            rval.Praemie_Default = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.Praemie_Default);
            //
            /*rval.Praemie -rval.Versicherungssteuer -rval.Motorsteuer = rval.Netto;
            Nachlass =(orgnetto - Subvention) - nettoneu;
            rval.Praemie_Default /(1+versicherungsSteuerProzent / 100)= rval.NettoORG
             * Nachlass =((rval.Praemie_Default /(1+versicherungsSteuerProzent / 100)) - Subvention) - nettoneu;
             * 
             Nachlass =((praemie_default /(1+versicherungsSteuerProzent / 100)) - praemie_default -(rval.Netto * versicherungsSteuerProzent / 100)) - Subvention - netto;
            rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
            (rval.Versicherungssteuer *100)/Netto= rversicherungsSteuerProzent;
            Nachlass =((praemie_default /(1+versicherungsSteuerProzent / 100)) - praemie_default -(rval.Netto * versicherungsSteuerProzent / 100)) - Subvention - netto;
            */

            //subnetto - rval.Praemie + rval.Versicherungssteuer + rval.Motorsteuer = (param.Nachlass * (12 / param.Zahlmodus));

            //Round all
            rval.Netto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.Netto);


            rval.Versicherungssteuer = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.Versicherungssteuer);
            rval.Motorsteuer = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.Motorsteuer);

            rval.Praemie = rval.Netto + rval.Versicherungssteuer + rval.Motorsteuer;
        }

        protected decimal MyDeliverProvision(DdOlExtended context, int CnstCALCPROVISION, long sysBrand, long sysPerole, decimal rvalNetto, decimal rvalPraemie, long sysprproduct, long sysobtyp, bool calcProv, long sysvstyp)
        {
            decimal provision;
            try
            {
                ProvisionDto paramprovision = new ProvisionDto();
                paramprovision.rank = CnstCALCPROVISION;
                paramprovision.sysBrand = sysBrand;
                paramprovision.sysPerole = sysPerole;
                paramprovision.versicherungspraemieexkl = rvalNetto;
                paramprovision.versicherungspraemiegesamt = rvalPraemie;
                paramprovision.sysprproduct = sysprproduct;
                paramprovision.sysobtyp = sysobtyp;
                paramprovision.noProvision = !calcProv;
                paramprovision.sysVstyp = sysvstyp;
                PROVDao dao = new PROVDao(context);

                paramprovision = dao.DeliverProvision(paramprovision);
                provision = paramprovision.provision;
                provision = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(provision);
            }
            catch (Exception ie)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.E10012_ProvNotConfigured, CnstCALCPROVISION) + ": " + ie.Message, ie);
            }
            return provision;
        }
        #endregion


        public abstract ServiceAccess.DdOl.InsuranceResultDto calculate(DdOlExtended context, long sysPerole, long sysBrand, VSTYP vstyp, ServiceAccess.DdOl.InsuranceParameterDto param);
    }
}