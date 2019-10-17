using Cic.OpenLease.ServiceAccess;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.Model.DdEurotax;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.ET.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service.Services.DdOl
{


    public class Tires
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private const string CnstSummerTire = "S";
        private const string CnstWinterTire = "W";
        

        private static CacheDictionary<String, TiresEurotaxDto> etTiresCache = CacheFactory<String, TiresEurotaxDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        private DdOlExtended _context;

        public Tires(DdOlExtended context)
        {
            _context = context;
        }

        public MehrMinderKmDto deliverMehrMinderKm(decimal listenPreis, decimal sonderausstattung, decimal paketeBrutto, long sysprproduct)
        {
            decimal SatzMinderKMPValue;
            decimal SatzMehrKMPValue;
            decimal MerhKMToleranzgrenze;
            decimal MinderKMToleranzgrenze;

           

            MehrMinderKmDto MehrMinderKmDto = new MehrMinderKmDto();

            SatzMinderKMPValue = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MINDER_KM_SATZ);
            SatzMehrKMPValue = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MEHR_KM_SATZ);
            MerhKMToleranzgrenze = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MehrKMToleranzgrenzeBezeichnung);
            MinderKMToleranzgrenze = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MinderKMToleranzgrenzeBezeichnung);

            PrismaDao pd = new CachedPrismaDao();
            long sysvart = pd.getVertragsart(sysprproduct).SYSVART;

            // Get the tax rate
            decimal TaxRate = LsAddHelper.GetTaxRate(_context, sysvart);

            MehrMinderKmDto.SatzMehrKm = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(KalkulationHelper.CalculateMehrKMSatz(listenPreis, sonderausstattung, paketeBrutto, SatzMehrKMPValue));
            MehrMinderKmDto.SatzMinderKm = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(KalkulationHelper.CalculateMinderKMSatz(MehrMinderKmDto.SatzMehrKm, SatzMinderKMPValue));

            MehrMinderKmDto.SatzMehrKmBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(MehrMinderKmDto.SatzMehrKm, TaxRate));
            MehrMinderKmDto.SatzMinderKmBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(MehrMinderKmDto.SatzMinderKm, TaxRate));

            MehrMinderKmDto.SatzMehrKmUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(MehrMinderKmDto.SatzMehrKmBrutto, TaxRate));
            MehrMinderKmDto.SatzMinderKmUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(MehrMinderKmDto.SatzMinderKmBrutto, TaxRate));

            MehrMinderKmDto.MehrKMToleranzgrenze = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(MerhKMToleranzgrenze);
            MehrMinderKmDto.MinderKMToleranzgrenze = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(MinderKMToleranzgrenze);

            return MehrMinderKmDto;
                
           
        }

        public MehrMinderKmDto deliverMehrMinderKmVorvertrag(long sysvorvt)
        {
            decimal SatzMinderKMPValue;
            decimal SatzMehrKMPValue;
            decimal MerhKMToleranzgrenze;
            decimal MinderKMToleranzgrenze;



            MehrMinderKmDto MehrMinderKmDto = new MehrMinderKmDto();

            SatzMinderKMPValue = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MINDER_KM_SATZ);
            SatzMehrKMPValue = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MEHR_KM_SATZ);
            MerhKMToleranzgrenze = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MehrKMToleranzgrenzeBezeichnung);
            MinderKMToleranzgrenze = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MinderKMToleranzgrenzeBezeichnung);


            long sysvart = 100;
            ANGEBOTDto vorvtInfo = null;

            using (DdOlExtended Context = new DdOlExtended())
            {

                vorvtInfo = Context.ExecuteStoreQuery<ANGEBOTDto>("select ob.satzmehrkm ANGOBSATZMEHRKM, ob.satzminderkm ANGOBSATZMINDERKM, vertrag vorvertragsnummer,kalk.syskalktyp as SYSKALKTYPVORVT from vt,kalk,ob where kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=" + sysvorvt, null).FirstOrDefault();
                if(vorvtInfo.SYSKALKTYPVORVT.HasValue)
                    sysvart = Context.ExecuteStoreQuery<long>("select sysvart from prproduct where syskalktyp=" + vorvtInfo.SYSKALKTYPVORVT.Value, null).FirstOrDefault();
                            
            }

            // Get the tax rate
            decimal TaxRate = LsAddHelper.GetTaxRate(_context, sysvart);

            MehrMinderKmDto.SatzMehrKm = vorvtInfo.ANGOBSATZMEHRKM.HasValue?vorvtInfo.ANGOBSATZMEHRKM.Value:0;
            MehrMinderKmDto.SatzMinderKm = vorvtInfo.ANGOBSATZMINDERKM.HasValue?vorvtInfo.ANGOBSATZMINDERKM.Value:0;

            MehrMinderKmDto.SatzMehrKmBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(MehrMinderKmDto.SatzMehrKm, TaxRate));
            MehrMinderKmDto.SatzMinderKmBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(MehrMinderKmDto.SatzMinderKm, TaxRate));

            MehrMinderKmDto.SatzMehrKmUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(MehrMinderKmDto.SatzMehrKmBrutto, TaxRate));
            MehrMinderKmDto.SatzMinderKmUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(MehrMinderKmDto.SatzMinderKmBrutto, TaxRate));

            MehrMinderKmDto.MehrKMToleranzgrenze = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(MerhKMToleranzgrenze);
            MehrMinderKmDto.MinderKMToleranzgrenze = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(MinderKMToleranzgrenze);

            return MehrMinderKmDto;


        }

        /// <summary>
        /// returns all tireinformation for the vehicle
        /// if no tirecodes given, will use the first available type for initialization
        /// </summary>
        /// <param name="eurotaxNr"></param>
        /// <param name="reifencodeVorne"></param>
        /// <param name="reifencodeHinten"></param>
        /// <param name="reifencodeVorneSommer"></param>
        /// <param name="reifencodeHintenSommer"></param>
        /// <returns></returns>
        public TireInfoDto getTireData(String eurotaxNr, String reifencodeVorne, String reifencodeHinten, String reifencodeVorneSommer, String reifencodeHintenSommer)
        {
            return getTireData(eurotaxNr, reifencodeVorne, reifencodeHinten, reifencodeVorneSommer, reifencodeHintenSommer, false,null,null);
        }
        public TireInfoDto getTireData(String eurotaxNr, String reifencodeVorne, String reifencodeHinten, String reifencodeVorneSommer, String reifencodeHintenSommer, bool netto, String felgenCodeVorne, String felgenCodeHinten)
        {
            decimal ust = 0;
            if(!netto)
                 ust = LsAddHelper.GetTaxRate(_context,null);

            TireInfoDto rval = new TireInfoDto();
            rval.tiresFsPrices = FsPreisHelper.GetNebenKostenPriceParameters();
            //Spalte Dimensionen für Reifen vorne/hinten, Felgen vorne/hinten
            rval.eurotaxTires = getEurotaxTires(eurotaxNr, ref reifencodeVorne, ref reifencodeHinten);

            reifencodeVorne = reifencodeVorne.Replace("R1", "R 1");
            reifencodeVorne = reifencodeVorne.Replace("R2", "R 2");

            reifencodeHinten = reifencodeHinten.Replace("R1", "R 1");
            reifencodeHinten = reifencodeHinten.Replace("R2", "R 2");

            if (felgenCodeVorne == null)
                felgenCodeVorne = reifencodeVorne;
            if (felgenCodeHinten == null)
                felgenCodeHinten = reifencodeHinten;
            felgenCodeVorne = felgenCodeVorne.Replace("R1", "R 1");
            felgenCodeHinten = felgenCodeHinten.Replace("R2", "R 2");
            if (reifencodeVorneSommer != null)
            {
                reifencodeVorneSommer = reifencodeVorneSommer.Replace("R1", "R 1");
                reifencodeVorneSommer = reifencodeVorneSommer.Replace("R2", "R 2");
            }
            if (reifencodeHintenSommer != null)
            {
                reifencodeHintenSommer = reifencodeHintenSommer.Replace("R1", "R 1");
                reifencodeHintenSommer = reifencodeHintenSommer.Replace("R2", "R 2");
            }

            //Spalte Marke/Durchschnitt - Felgen
            rval.frontRims = getRims(felgenCodeVorne);
            rval.rearRims = getRims(felgenCodeHinten);

            if (reifencodeVorneSommer == null || reifencodeVorneSommer.Length == 0)
                reifencodeVorneSommer = reifencodeVorne;
            if (reifencodeHintenSommer == null || reifencodeHintenSommer.Length == 0)
                reifencodeHintenSommer = reifencodeHinten;

            rval.frontPricesSummer = getTiresPricesFromBauart(reifencodeVorneSommer, CnstSummerTire, ust);
            rval.rearPricesSummer = getTiresPricesFromBauart(reifencodeHintenSommer, CnstSummerTire, ust);

            rval.frontPricesWinter = getTiresPricesFromBauart(reifencodeVorne, CnstWinterTire, ust);
            rval.rearPricesWinter = getTiresPricesFromBauart(reifencodeHinten, CnstWinterTire, ust);

            rval.reifencodeHinten = reifencodeHinten;
            rval.reifencodeVorne = reifencodeVorne;
            rval.reifencodeVorneSommer = reifencodeVorneSommer;
            rval.reifencodeHintenSommer = reifencodeHintenSommer;

            return rval;
        }

        /// <summary>
        /// Calculates the current tire-configuration
        /// </summary>
        /// <param name="tiresAndRimsCalculationDto"></param>
        /// <returns></returns>
        public TiresAndRimsCalculationDto calculateTires(TiresAndRimsCalculationDto tiresAndRimsCalculationDto)
        {

            
                string op = "+";
                tiresAndRimsCalculationDto.AufschlagP = 100;


                using (DdOlExtended Context = new DdOlExtended())
                {
                    KORREKTURDao kh = new KORREKTURDao(Context);

                    try
                    {
                        decimal saetze = 0;

                        if (tiresAndRimsCalculationDto.Reifensaetze == 0)//triggers satz-kalkulation
                        {
                            decimal maxkm = kh.Correct("TIRE_LAUFLEISTUNG", 0, op, DateTime.Now, tiresAndRimsCalculationDto.Leistung.ToString(), string.Empty);
                            decimal ll = tiresAndRimsCalculationDto.Laufleistung / 12 * tiresAndRimsCalculationDto.Laufzeit;
                            saetze = (ll - maxkm) / maxkm;
                            if (saetze <= 1)
                            {
                                saetze = 1;//min 1 satz

                            }

                        }
                        if (tiresAndRimsCalculationDto.ReifensaetzeChanged == 1)//triggers satz-kalkulation
                        {
                            saetze = tiresAndRimsCalculationDto.Reifensaetze;
                        }
                        if (saetze > 0)//when one of both triggers
                        {
                            saetze = Math.Round(saetze, 0);
                            decimal rest = saetze % 2;
                            decimal floor = Math.Floor(saetze / 2);
                            decimal winter = (floor + rest);//reifensaetze winter

                            decimal sommer = floor;//reifensaetze sommer
                            if (saetze == 1)
                            { winter = 1; sommer = 0; }

                            tiresAndRimsCalculationDto.SRCount = (int)sommer * 2;
                            tiresAndRimsCalculationDto.SFCount = (int)sommer * 2;
                            tiresAndRimsCalculationDto.WRCount = (int)winter * 2;
                            tiresAndRimsCalculationDto.WFCount = (int)winter * 2;
                            // bmwTiresAndRimsCalculationDto.Reifensaetze = (int)saetze;

                            tiresAndRimsCalculationDto.RRimsCount = 2;
                            tiresAndRimsCalculationDto.FRimsCount = 2;

                        }
                        //if (bmwTiresAndRimsCalculationDto.ReifensaetzeChanged == 0)
                        {
                            tiresAndRimsCalculationDto.Reifensaetze = (tiresAndRimsCalculationDto.WRCount + tiresAndRimsCalculationDto.WFCount + tiresAndRimsCalculationDto.SFCount + tiresAndRimsCalculationDto.SRCount) / 4;
                        }

                    }
                    catch (Exception et)
                    {
                        // Log the exception
                        _Log.Error("Korrekturttyp TIRE_LAUFLEISTUNG für Default Reifensatzanzahl-Berechnung nicht vorhanden", et);
                    }

                    //Calculate SRPriceTotal
                    tiresAndRimsCalculationDto.SRPriceTotal = tiresAndRimsCalculationDto.SRPrice * tiresAndRimsCalculationDto.SRCount;

                    //Calculate WFPrice
                    tiresAndRimsCalculationDto.WFPriceTotal = tiresAndRimsCalculationDto.WFPrice * tiresAndRimsCalculationDto.WFCount;

                    //Calculate WRPrice
                    tiresAndRimsCalculationDto.WRPriceTotal = tiresAndRimsCalculationDto.WRPrice * tiresAndRimsCalculationDto.WRCount;

                    //Calculate SFPrice
                    tiresAndRimsCalculationDto.SFPriceTotal = tiresAndRimsCalculationDto.SFPrice * tiresAndRimsCalculationDto.SFCount;

                    //Calculate rRimsPrice
                    tiresAndRimsCalculationDto.RRimsPriceTotal = tiresAndRimsCalculationDto.RRimsPrice * tiresAndRimsCalculationDto.RRimsCount;

                    // Get the tax rate
                    decimal TaxRate = LsAddHelper.GetTaxRate(null);

                    tiresAndRimsCalculationDto.NebenKostenPrice = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(tiresAndRimsCalculationDto.NebenKostenPriceNetto, TaxRate);

                    //Calculate fRimsPrice
                    tiresAndRimsCalculationDto.FRimsPriceTotal = tiresAndRimsCalculationDto.FRimsPrice * tiresAndRimsCalculationDto.FRimsCount;
                    tiresAndRimsCalculationDto.Gesamtreifenanzahl = (tiresAndRimsCalculationDto.SRCount + tiresAndRimsCalculationDto.WFCount + tiresAndRimsCalculationDto.WRCount + tiresAndRimsCalculationDto.SFCount);
                    tiresAndRimsCalculationDto.NebenKostenTotal = tiresAndRimsCalculationDto.NebenKostenPrice * (tiresAndRimsCalculationDto.Gesamtreifenanzahl);
                    tiresAndRimsCalculationDto.ReifenGesamt = tiresAndRimsCalculationDto.SRPriceTotal + tiresAndRimsCalculationDto.WFPriceTotal + tiresAndRimsCalculationDto.WRPriceTotal + tiresAndRimsCalculationDto.SFPriceTotal + tiresAndRimsCalculationDto.FRimsPriceTotal + tiresAndRimsCalculationDto.RRimsPriceTotal;

                    decimal risikoaufschlag = 3;
                    try
                    {
                        risikoaufschlag = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_RisikoaufschlagReifen);
                    }
                    catch (Exception)
                    {
                        _Log.Error("Risikoaufschlag Reifen Quote " + QUOTEDao.QUOTE_RisikoaufschlagReifen + " not configured, using 3%");
                    }
                    tiresAndRimsCalculationDto.ReifenGesamt = tiresAndRimsCalculationDto.ReifenGesamt + (tiresAndRimsCalculationDto.ReifenGesamt * risikoaufschlag / 100);


                    tiresAndRimsCalculationDto.ReifenGesamt += tiresAndRimsCalculationDto.NebenKostenTotal;

                    tiresAndRimsCalculationDto.ReifenRateBrutto = tiresAndRimsCalculationDto.ReifenGesamt / tiresAndRimsCalculationDto.Laufzeit;
                    tiresAndRimsCalculationDto.ReifenRateNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.ReifenRateBrutto, TaxRate);

                    switch (tiresAndRimsCalculationDto.CalcMode)
                    {
                        case TiresAndRimsCalcModes.FixUnlimitiert:
                            tiresAndRimsCalculationDto.AufschlagP = kh.Correct("SRV_KORR_REIFENFIX", 0, op, DateTime.Now, string.Empty, string.Empty) + kh.Correct("SRV_KORR_REIFENLIM", 0, op, DateTime.Now, string.Empty, string.Empty);
                            tiresAndRimsCalculationDto.Aufschlag = tiresAndRimsCalculationDto.ReifenRateNetto * (tiresAndRimsCalculationDto.AufschlagP / 100);
                            tiresAndRimsCalculationDto.ReifenRateNetto += (tiresAndRimsCalculationDto.ReifenRateNetto * tiresAndRimsCalculationDto.AufschlagP / 100);


                            break;
                        case TiresAndRimsCalcModes.FixLimitiert:
                            tiresAndRimsCalculationDto.AufschlagP = kh.Correct("SRV_KORR_REIFENFIX", 0, op, DateTime.Now, string.Empty, string.Empty);
                            tiresAndRimsCalculationDto.Aufschlag = tiresAndRimsCalculationDto.ReifenRateNetto * (tiresAndRimsCalculationDto.AufschlagP / 100);
                            tiresAndRimsCalculationDto.ReifenRateNetto += tiresAndRimsCalculationDto.ReifenRateNetto * (tiresAndRimsCalculationDto.AufschlagP / 100);
                            break;
                    }

                    tiresAndRimsCalculationDto.ReifenRateBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(tiresAndRimsCalculationDto.ReifenRateNetto, TaxRate));

                    tiresAndRimsCalculationDto.ReifenRate_Default = tiresAndRimsCalculationDto.ReifenRateBrutto;
                    tiresAndRimsCalculationDto.ReifenRateBrutto -= tiresAndRimsCalculationDto.Nachlass;



                    //Calculate netto and ust values
                    tiresAndRimsCalculationDto.FRimsPriceNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.FRimsPrice, TaxRate);
                    tiresAndRimsCalculationDto.FRimsPriceUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.FRimsPrice, TaxRate);
                    tiresAndRimsCalculationDto.FRimsPriceTotalNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.FRimsPriceTotal, TaxRate);
                    tiresAndRimsCalculationDto.FRimsPriceTotalUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.FRimsPriceTotal, TaxRate);

                    tiresAndRimsCalculationDto.RRimsPriceNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.RRimsPrice, TaxRate);
                    tiresAndRimsCalculationDto.RRimsPriceUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.RRimsPrice, TaxRate);
                    tiresAndRimsCalculationDto.RRimsPriceTotalNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.RRimsPriceTotal, TaxRate);
                    tiresAndRimsCalculationDto.RRimsPriceTotalUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.RRimsPriceTotal, TaxRate);

                    tiresAndRimsCalculationDto.SFPriceNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.SFPrice, TaxRate);
                    tiresAndRimsCalculationDto.SFPriceUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.SFPrice, TaxRate);
                    tiresAndRimsCalculationDto.SFPriceTotalNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.SFPriceTotal, TaxRate);
                    tiresAndRimsCalculationDto.SFPriceTotalUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.SFPriceTotal, TaxRate);

                    tiresAndRimsCalculationDto.SRPriceNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.SRPrice, TaxRate);
                    tiresAndRimsCalculationDto.SRPriceUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.SRPrice, TaxRate);
                    tiresAndRimsCalculationDto.SRPriceTotalNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.SRPriceTotal, TaxRate);
                    tiresAndRimsCalculationDto.SRPriceTotalUSt = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.SRPriceTotal, TaxRate);

                    tiresAndRimsCalculationDto.WFPriceNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.WFPrice, TaxRate);
                    tiresAndRimsCalculationDto.WFPriceUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.WFPrice, TaxRate);
                    tiresAndRimsCalculationDto.WFPriceTotalNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.WFPriceTotal, TaxRate);
                    tiresAndRimsCalculationDto.WFPriceTotalUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.WFPriceTotal, TaxRate);

                    tiresAndRimsCalculationDto.WRPriceNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.WRPrice, TaxRate);
                    tiresAndRimsCalculationDto.WRPriceUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.WRPrice, TaxRate);
                    tiresAndRimsCalculationDto.WRPriceTotalNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.WRPriceTotal, TaxRate);
                    tiresAndRimsCalculationDto.WRPriceTotalUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.WRPriceTotal, TaxRate);

                    tiresAndRimsCalculationDto.ReifenRateNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.ReifenRateBrutto, TaxRate);
                    tiresAndRimsCalculationDto.ReifenRateUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.ReifenRateBrutto, TaxRate);

                    tiresAndRimsCalculationDto.NebenKostenPriceNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.NebenKostenPrice, TaxRate);
                    tiresAndRimsCalculationDto.NebenKostenPriceUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.NebenKostenPrice, TaxRate);
                    tiresAndRimsCalculationDto.NebenKostenTotalNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(tiresAndRimsCalculationDto.NebenKostenTotal, TaxRate);
                    tiresAndRimsCalculationDto.NebenKostenTotalUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(tiresAndRimsCalculationDto.NebenKostenTotal, TaxRate);

                    //Rounding

                    tiresAndRimsCalculationDto.FRimsPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.FRimsPrice);
                    tiresAndRimsCalculationDto.FRimsPriceNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.FRimsPriceNetto);
                    tiresAndRimsCalculationDto.FRimsPriceUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.FRimsPriceUst);
                    tiresAndRimsCalculationDto.FRimsPriceTotal = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.FRimsPriceTotal);
                    tiresAndRimsCalculationDto.FRimsPriceTotalNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.FRimsPriceTotalNetto);
                    tiresAndRimsCalculationDto.FRimsPriceTotalUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.FRimsPriceTotalUst);

                    tiresAndRimsCalculationDto.RRimsPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.RRimsPrice);
                    tiresAndRimsCalculationDto.RRimsPriceNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.RRimsPriceNetto);
                    tiresAndRimsCalculationDto.RRimsPriceUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.RRimsPriceUst);
                    tiresAndRimsCalculationDto.RRimsPriceTotal = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.RRimsPriceTotal);
                    tiresAndRimsCalculationDto.RRimsPriceTotalNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.RRimsPriceTotalNetto);
                    tiresAndRimsCalculationDto.RRimsPriceTotalUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.RRimsPriceTotalUst);

                    tiresAndRimsCalculationDto.SFPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SFPrice);
                    tiresAndRimsCalculationDto.SFPriceNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SFPriceNetto);
                    tiresAndRimsCalculationDto.SFPriceUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SFPriceUst);
                    tiresAndRimsCalculationDto.SFPriceTotal = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SFPriceTotal);
                    tiresAndRimsCalculationDto.SFPriceTotalNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SFPriceTotalNetto);
                    tiresAndRimsCalculationDto.SFPriceTotalUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SFPriceTotalUst);

                    tiresAndRimsCalculationDto.SRPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SRPrice);
                    tiresAndRimsCalculationDto.SRPriceNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SRPriceNetto);
                    tiresAndRimsCalculationDto.SRPriceUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SRPriceUst);
                    tiresAndRimsCalculationDto.SRPriceTotal = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SRPriceTotal);
                    tiresAndRimsCalculationDto.SRPriceTotalNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SRPriceTotalNetto);
                    tiresAndRimsCalculationDto.SRPriceTotalUSt = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.SRPriceTotalUSt);

                    tiresAndRimsCalculationDto.WFPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WFPrice);
                    tiresAndRimsCalculationDto.WFPriceNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WFPriceTotalNetto);
                    tiresAndRimsCalculationDto.WFPriceUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WFPriceUst);
                    tiresAndRimsCalculationDto.WFPriceTotal = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WFPriceTotal);
                    tiresAndRimsCalculationDto.WFPriceTotalNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WFPriceTotalNetto);
                    tiresAndRimsCalculationDto.WFPriceTotalUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WFPriceTotalUst);

                    tiresAndRimsCalculationDto.WRPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WRPrice);
                    tiresAndRimsCalculationDto.WRPriceNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WRPriceNetto);
                    tiresAndRimsCalculationDto.WRPriceUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WRPriceUst);
                    tiresAndRimsCalculationDto.WRPriceTotal = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WRPriceTotal);
                    tiresAndRimsCalculationDto.WRPriceTotalNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WRPriceTotalNetto);
                    tiresAndRimsCalculationDto.WRPriceTotalUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.WRPriceTotalUst);

                    tiresAndRimsCalculationDto.ReifenRateBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.ReifenRateBrutto);
                    tiresAndRimsCalculationDto.ReifenRateNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.ReifenRateNetto);
                    tiresAndRimsCalculationDto.ReifenRateUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.ReifenRateUst);

                    tiresAndRimsCalculationDto.NebenKostenPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.NebenKostenPrice);
                    tiresAndRimsCalculationDto.NebenKostenPriceNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.NebenKostenPriceNetto);
                    tiresAndRimsCalculationDto.NebenKostenPriceUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.NebenKostenPriceUst);
                    tiresAndRimsCalculationDto.NebenKostenTotal = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.NebenKostenTotal);
                    tiresAndRimsCalculationDto.NebenKostenTotalNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.NebenKostenTotalNetto);
                    tiresAndRimsCalculationDto.NebenKostenTotalUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(tiresAndRimsCalculationDto.NebenKostenTotalUst);


                    return tiresAndRimsCalculationDto;
                }
           
        }
        /// <summary>
        /// Liefert Marke/Durchschnitt für Reifen vorne/hinten
        /// 
        /// AIDA-Usage:
        /// 
        /// Reifen:
        /// Dimensionen: VTIRE_DIM_List / HTIRE_DIM_List - deliverEurotaxTires(eurotaxnr)
        /// gewählte Dimension ==== code für Felgen
        /// Marke/Durchschnitt: WVTIRE_MAKE_List / WHTIRE_MAKE_List / SVTIRE_MAKE_List / SHTIRE_MAKE_List - deliverTirePricesFromBauart(code, bauart)
        /// Felgen:
        /// Dimensionen: HRIM_DIM_List / VRIM_DIM_List - deliverEurotaxRims(eurotax, code);
        /// Marke/Durchschnitt: HRIM_MAKE_List / VRIM_MAKE_List - deliverRims(code);
        /// 
        /// </summary>
        /// <param name="code">Dimension z.B. 225/35 R17</param>
        /// <param name="bauart">S oder W für Sommer/Winter</param>
        /// <returns></returns>
        /// 
        private TireDto[] getTiresPricesFromBauart(string code, string bauart, decimal Ust)
        {
            Dictionary<string, decimal> HerstellerPriceAvg = null;

            List<TireDto> TireDtoList = null;
           



            HerstellerPriceAvg = RestOfTheHelpers.GetTiresFromCodeAndBauartAVG(_context, code, bauart);


            //Check in reiftyplist is not null
            TireDtoList = new List<TireDto>();

            if (HerstellerPriceAvg.Count > 0)
            {
                //put Average to front
                decimal ds = HerstellerPriceAvg[RestOfTheHelpers.DURCHSCHNITT];
                HerstellerPriceAvg.Remove(RestOfTheHelpers.DURCHSCHNITT);
                TireDto TireDto = new TireDto();
                TireDto.Code = code;
                TireDto.Manufacturer = RestOfTheHelpers.DURCHSCHNITT;
                TireDto.Price = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(ds);
                TireDto.Price = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue((decimal)TireDto.Price, Ust);
                TireDto.Bauart = bauart;
                TireDtoList.Add(TireDto);

                foreach (KeyValuePair<string, decimal> hp in HerstellerPriceAvg)
                {
                    //Create new assembler
                    TireDto = new TireDto();
                    TireDto.Code = code;
                    TireDto.Manufacturer = hp.Key;
                    TireDto.Price = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(hp.Value);
                    TireDto.Price = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue((decimal)TireDto.Price, Ust);
                    TireDto.Bauart = bauart;
                    TireDtoList.Add(TireDto);
                }


            }

            if (TireDtoList != null)
            {
                return TireDtoList.ToArray();
            }
            else
            {
                return null;
            }

        }


        /// <summary>
        /// Delivers Marke/Durchschnitt für Felgen - Spalte zwei in Gui bei Felgen
        /// </summary>
        /// <param name="code">Code from deliverEurotaxTires e.g. 225/40 R17</param>
        /// <returns></returns>
        private RimDto[] getRims(string code)
        {
            int diameter = RestOfTheHelpers.GetDiameterFromCode(code);

            return _context.ExecuteStoreQuery<RimDto>("select felgen.bezeichnung code, felgen.hersteller manufacturer, felgen.preis price, felgtyp.durchmesser diameter, felgtyp.breite width from felgtyp,felgen where felgen.sysfelgtyp=felgtyp.sysfelgtyp and felgtyp.durchmesser="+diameter,null).ToArray();
/*
            List<RimDto> RimDtoList = null;
            List<FELGEN> FELGENList = null;
            FELGENAssembler FELGENAssembler = null;

            FELGENList = FELGENHelper.GetFelgenFromCode(_context, code);

            if ((FELGENList != null) && (FELGENList.Count > 0))
            {
                //Create new assembler
                FELGENAssembler = new FELGENAssembler();

                //Create new RimDtoList
                RimDtoList = new List<RimDto>();

                foreach (FELGEN FELGENLoop in FELGENList)
                {
                    RimDtoList.Add(FELGENAssembler.ConvertToDto(FELGENLoop));
                }
            }

            if (RimDtoList != null)
            {
                return RimDtoList.ToArray();
            }
            else
            {
                return null;
            }
            */
        }

        /// <summary>
        /// Delivers Dimension information for front and rear axis Tires, including the xxx/xx Rxx dimension displayed for the car
        /// </summary>
        /// <param name="eurotaxNr"></param>
        /// <param name="reifencodeVorne"></param>
        /// <param name="reifencodeHinten"></param>
        /// <returns></returns>
        private TiresEurotaxDto getEurotaxTires(String eurotaxNr, ref String reifencodeVorne, ref String reifencodeHinten)
        {
            TiresEurotaxDto TiresRimsEurotaxDto = null;
            List<ETGTYRES> EtgTyresFront = null;
            List<ETGTYRES> EtgTyresRear = null;

            List<TireDto> TireDtoList = null;
            List<RimDto> RimDtoList = null;

            ETGTYRESAssembler ETGTYRESAssembler;
            ETGRIMSAssembler ETGRIMSAssembler;
            List<ETGRIMS> EtgRimsFront = null;
            List<ETGRIMS> EtgRimsRear = null;


            using (DdEurotaxExtended Context = new DdEurotaxExtended())
            {
                //Get lists of tires and rims
                EtgTyresFront = Cic.OpenLease.Service.DdEurotax.ETGTYPEHelper.GetFrontTires(Context, eurotaxNr);
                EtgTyresRear = Cic.OpenLease.Service.DdEurotax.ETGTYPEHelper.GetRearTires(Context, eurotaxNr);



                //Check if list is null
                if ((EtgTyresFront != null) && (EtgTyresFront.Count > 0))
                {



                    //Create new assembler
                    ETGTYRESAssembler = new ETGTYRESAssembler();


                    //Create new TiresRimsEurotaxDto if is null
                    if (TiresRimsEurotaxDto == null)
                    {
                        TiresRimsEurotaxDto = new TiresEurotaxDto();
                    }

                    //Add to TiresRimsEurotaxDto
                    TiresRimsEurotaxDto.TiresFront = ETGTYRESAssembler.ConvertToListDto(EtgTyresFront).ToArray();
                }

                //Check if list is null
                if ((EtgTyresRear != null) && (EtgTyresRear.Count > 0))
                {
                    //Create new tire dto list
                    TireDtoList = new List<TireDto>();

                    //Create new assembler
                    ETGTYRESAssembler = new ETGTYRESAssembler();


                    //Create new TiresRimsEurotaxDto if is null
                    if (TiresRimsEurotaxDto == null)
                    {
                        TiresRimsEurotaxDto = new TiresEurotaxDto();
                    }

                    //Add to TiresRimsEurotaxDto
                    TiresRimsEurotaxDto.TiresRear = ETGTYRESAssembler.ConvertToListDto(EtgTyresRear).ToArray();

                }

                if ((reifencodeVorne == null || reifencodeVorne.Length == 0) && TiresRimsEurotaxDto != null && TiresRimsEurotaxDto.TiresFront != null && TiresRimsEurotaxDto.TiresFront.Length>0)
                    reifencodeVorne = TiresRimsEurotaxDto.TiresFront[0].Code;
                if ((reifencodeHinten == null || reifencodeHinten.Length == 0) && TiresRimsEurotaxDto != null && TiresRimsEurotaxDto.TiresRear != null && TiresRimsEurotaxDto.TiresRear.Length > 0)
                    reifencodeHinten = TiresRimsEurotaxDto.TiresRear[0].Code;

                 if (reifencodeVorne != null && reifencodeVorne.Length > 0)
                EtgRimsFront = Cic.OpenLease.Service.DdEurotax.ETGTYPEHelper.GetFrontRims(Context, eurotaxNr, reifencodeVorne);
                 if (reifencodeHinten != null && reifencodeHinten.Length > 0)
                EtgRimsRear = Cic.OpenLease.Service.DdEurotax.ETGTYPEHelper.GetRearRims(Context, eurotaxNr, reifencodeHinten);

            }
            //Check if list is null
            if ((EtgRimsFront != null) && (EtgRimsFront.Count > 0))
            {
                //Create new rim dto list
                RimDtoList = new List<RimDto>();

                //Create new assembler
                ETGRIMSAssembler = new ETGRIMSAssembler();

                //Loop through list and annd to tire dto
                foreach (ETGRIMS ETGRIMSLoop in EtgRimsFront)
                {
                    RimDtoList.Add(ETGRIMSAssembler.ConvertToDto(ETGRIMSLoop));
                }

                //Create new TiresRimsEurotaxDto if is null
                if (TiresRimsEurotaxDto == null)
                {
                    TiresRimsEurotaxDto = new TiresEurotaxDto();
                }

                //Add to TiresRimsEurotaxDto
                TiresRimsEurotaxDto.RimsFront = RimDtoList.ToArray();
            }

            //Check if list is null
            if ((EtgRimsRear != null) && (EtgRimsRear.Count > 0))
            {
                //Create new rim dto list
                RimDtoList = new List<RimDto>();

                //Create new assembler
                ETGRIMSAssembler = new ETGRIMSAssembler();

                //Loop through list and annd to tire dto
                foreach (ETGRIMS ETGRIMSLoop in EtgRimsFront)
                {
                    RimDtoList.Add(ETGRIMSAssembler.ConvertToDto(ETGRIMSLoop));
                }

                //Create new TiresRimsEurotaxDto if is null
                if (TiresRimsEurotaxDto == null)
                {
                    TiresRimsEurotaxDto = new TiresEurotaxDto();
                }

                //Add to TiresRimsEurotaxDto
                TiresRimsEurotaxDto.RimsRear = RimDtoList.ToArray();
            }

            //return
            return TiresRimsEurotaxDto;


        }
    }
}