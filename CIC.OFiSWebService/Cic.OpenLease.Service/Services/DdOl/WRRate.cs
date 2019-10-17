using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Class to calculate the BMW specific Service and Repair rate and charges
    /// </summary>
    [System.CLSCompliant(true)]
    public class WRRate
    {
        private const string _CnstWR_KORR_VARIABLE = "WR_KORR_VARIABLE";
        
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Private variables
        private DdOlExtended _context;
        private decimal _excessKMCharge;
        private decimal _recessKMCharge;
        private decimal _KMCharge;
        private decimal _Rate;
        
        #endregion

        #region Properties
        public decimal excessKMCharge
        {
            get { return _excessKMCharge; }
            set { _excessKMCharge = value; }
        }
        public decimal recessKMCharge
        {
            get { return _recessKMCharge; }
            set { _recessKMCharge = value; }
        }
        public decimal KMCharge
        {
            get { return _KMCharge; }
            set { _KMCharge = value; }
        }
        public decimal Rate
        {
            get { return _Rate; }
            set { _Rate = value; }
        }
        #endregion
        

       
        #region Constructors
        public WRRate(DdOlExtended context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public void calculateWRRate(long sysObtyp, long lz, decimal ll, bool serviceVariabel, decimal rn, DateTime perDate, decimal tax)
        {
            KORREKTURDao kh = new KORREKTURDao(_context);

            string op = "+";

            
            try
            {
                _Log.Debug("Calc Wartung  Reifen: " + rn + " LL: " + ll + " LZ: " + lz + " Variabel: " + serviceVariabel + " Date: " + perDate + " OBTYP: " + sysObtyp );

                //get the WR Tab Id from the obtyp table
                //var dbData = from p in _context.OBTYP where p.SYSOBTYP == sysObtyp select p.SYSVGWR;
                //long sysVGWR =  (long)dbData.Single();

                //WR-Tab Interpolation
                ADJDto vgParam = VGADJDao.deliverVGWRAdjValue(_context, sysObtyp, perDate);


                //_Rate = VGDao.deliverVGValue(_context, sysVGWR, perDate, lz.ToString(), ll.ToString(), 1);
                _Rate = VGDao.deliverVGValue(_context, vgParam.sysvg, perDate, lz.ToString(), ll.ToString(), 1);
                if (vgParam.adjvalue != 0)
                    _Rate += (_Rate / 100 * vgParam.adjvalue);

                decimal debugrate = _Rate;
                if (serviceVariabel)
                {
                    decimal addVariableCharge = kh.Correct(_CnstWR_KORR_VARIABLE, 0, op, perDate, _Rate.ToString(), "0.0");
                    if(addVariableCharge!=0)
                        _Rate += _Rate / 100 * addVariableCharge;
                }

                

                //Additional km
                decimal ratebrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(_Rate, tax);
                decimal reifenbrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rn, tax);
                 // Fixed #2763 ll not ll gesamt, but llpa
                if (ll == 0)
                    _excessKMCharge = 0;
                else 
                    _excessKMCharge = (ratebrutto + reifenbrutto) / (ll / 12);
                _Log.Debug("Wartung Rate: " + _Rate + " RateBrutto: " + ratebrutto + " Reifen: " + rn + " LL: " + ll + " LZ: " + lz + " Variabel: " + serviceVariabel + " SYSVGWR: " + vgParam.sysvg + " OBTYP: " + sysObtyp + " VGRate: " + debugrate);
                //Lesser km
                decimal lessKMQuote = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MINDER_KM_SATZ);
                if (lessKMQuote == 0)
                    _recessKMCharge = 0;
                else
                    _recessKMCharge = excessKMCharge / 100 * lessKMQuote;
                //Charge/Km
                if (ll == 0)
                    _KMCharge = 0;
                else 
                    _KMCharge = lz * ratebrutto / ll;


                /*if (serviceVariabel)
                {
                    _excessKMCharge = 0;
                    _recessKMCharge = 0;
                    _KMCharge = 0;
                }*/
                

               
            }
            catch (System.InvalidOperationException e)
            {
                // TODO Exceptionhandling with Resources
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralSelect, "Can't calculate WR Rate, invalid SYSOBTYP or perDate "+e.Message);
            }


        }
        #endregion

    }
}
