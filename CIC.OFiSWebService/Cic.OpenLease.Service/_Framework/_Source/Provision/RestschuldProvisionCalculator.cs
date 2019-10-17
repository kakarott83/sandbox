using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenLease.Service.Provision
{
    /// <summary>
    /// Calculates Restschuld Provision for BMW
    /// </summary>
    [System.CLSCompliant(true)]
    public class RestschuldProvisionCalculator : AbstractProvisionCalculator
    {

        
        override
        public ProvisionDto calculate(DdOlExtended context, PROVDao prov, ProvisionDto param)
        {
            ProvisionDto rval = new ProvisionDto();
            rval.provision = 0;

            decimal quote = MyDeliverSteuer_Personenbezogen(context);
            
            PROVRATE tarif = prov.DeliverProvRateAdjusted(param.sysprproduct, param.sysobtyp, param.sysPerole, param.sysBrand, param.rank);
            if(tarif!=null)
                rval.provision = param.versicherungspraemiegesamt / ( 1+(quote/100) ) * (decimal)tarif.PROVRATE1/100;

            if (param.sysVstyp > 0)
            {
                VSTYP vsType = new VSTYPDao(context).getVsTyp(param.sysVstyp);
                if (vsType.ANTEILLS.HasValue)
                {
                    rval.provision *= vsType.ANTEILLS.Value / 100.0M;
                }
            }
            bool leasing = isLeasing(context, param.sysprproduct);
            int lz = 1;
            if (leasing)
                lz = param.laufzeit;
            rval.provision = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.provision*lz);

            return rval;
        }

        protected decimal MyDeliverSteuer_Personenbezogen(DdOlExtended context)
        {
            return QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_VSSTEUERPERSONENBEZOGEN);
        }

    }
}