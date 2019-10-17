using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.OpenLease.Service.Provision
{
    /// <summary>
    /// Calculates Provision for a new Contract for BMW
    /// </summary>
    [System.CLSCompliant(true)]
    public class AbschlussProvisionCalculator : AbstractProvisionCalculator
    {

        override
        public ProvisionDto calculate(DdOlExtended context, PROVDao prov, ProvisionDto param)
        {

            decimal gtAufschlag = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_AufschlagGT) / 100.0M;
            decimal ltAufschlag = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_AufschlagGT) / 100.0M;

            ProvisionDto rval = new ProvisionDto();
            /*
            List<PROVTARIF> tarife = prov.DeliverProvTarife(param.sysPerole, param.sysBrand, param.sysPrhgroup, param.rank);
            if(tarife.Count==0)
                throw new ApplicationException(ExceptionMessages.E10010_NoProvTarif); 

            var tQuery = from t in tarife
                         where t.STANDARDFLAG == 1
                         select t;
            if (tQuery.Count() == 0) throw new ApplicationException(ExceptionMessages.E10013_NoDefaultProvTarif);

            PROVTARIF standardTarif = tQuery.First<PROVTARIF>();

            tQuery = from t in tarife
                         where t.SYSPROVTARIF == param.sysProvTarif
                         select t;

            if (tQuery.Count() == 0 && param.sysProvTarif != 0) throw new ApplicationException(String.Format(ExceptionMessages.E10011_InvlidProvTarif, param.sysProvTarif)); 
            */
            decimal stdRate = 0.00M;// (decimal)standardTarif.PROVRATE;
            decimal selRate = 0.00M;//PROVTARIF selectedTarif = tQuery.First<PROVTARIF>().PROVRATE;
            decimal standardprovision = (decimal)stdRate / 100 * param.finanzierungssumme;//Standardprovision - Bezugsgrösse
            decimal variantenprovision = 0;
            rval.sfAufschlag = 0;

            if ((param.wunschprovision >= 0 && param.sysProvTarif == 0) || (param.wunschprovision > 0 ))
            {
                variantenprovision = param.wunschprovision;
                param.sysProvTarif = 0;
            }
            else
            {
                
                decimal tarifadj = prov.DeliverProvRateAdjusted((decimal)selRate, param.sysprproduct, param.sysobtyp, param.sysPerole, param.sysBrand, param.rank);
                variantenprovision = tarifadj / 100 * param.finanzierungssumme;//Provision der gewählten Variante
            }
            if(variantenprovision>standardprovision)
                rval.sfAufschlag = gtAufschlag * (variantenprovision - standardprovision);
            if (variantenprovision < standardprovision)
                rval.sfAufschlag = ltAufschlag * (variantenprovision - standardprovision);


            rval.deltaStandard = variantenprovision - standardprovision;
            rval.provision = variantenprovision;
            rval.zugangsProvision = 0;

            /*PROVRATE zugangsTarif = prov.DeliverProvRateAdjusted(param.sysprproduct, param.sysobtyp, param.sysPerole, param.sysBrand, (int)ProvisionTypeConstants.Zugang);
            if (zugangsTarif != null && zugangsTarif.PROVRATE1.HasValue)
                rval.zugangsProvision = (decimal)zugangsTarif.PROVRATE1/100 * param.finanzierungssumme;
            */
            rval.provision = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.provision);
            rval.zugangsProvision = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.zugangsProvision);
            rval.deltaStandard = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.deltaStandard);
            
            return rval;
        }
    }
}