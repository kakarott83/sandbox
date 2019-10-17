using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenLease.Service.Provision
{
    /// <summary>
    /// Calculates Gebühr Provision for BMW
    /// </summary>
    [System.CLSCompliant(true)]
    public class GebuehrProvisionCalculator : AbstractProvisionCalculator
    {

        

        override
        public ProvisionDto calculate(DdOlExtended context, PROVDao prov, ProvisionDto param)
        {
            ProvisionDto rval = new ProvisionDto();
            rval.provision = 0;

            decimal maxPercent = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MAX_BEARBEITUNGSGEBUEHR);
            decimal maxgebuehr = param.finanzierungssumme * maxPercent / 100.0M;


            if (param.prparam != null)
            {
                //Change value depending on max value
                if (maxgebuehr > param.prparam.MAXVALN)
                {
                    maxgebuehr = param.prparam.MAXVALN.GetValueOrDefault();
                }
            }
            //nur bearbeitungsgebühr min maxgebühr erhält provision
            if (param.bearbeitungsgebuehr >= maxgebuehr)
            {
                PROVRATE tarif = prov.DeliverProvRateAdjusted(param.sysprproduct, param.sysobtyp, param.sysPerole, param.sysBrand, param.rank);
                if (tarif != null)
                {
                    rval.provision = (decimal)tarif.PROVRATE1 / 100 * param.bearbeitungsgebuehr;
                }
            }
            rval.provision = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.provision);

            return rval;
        }

    }
}