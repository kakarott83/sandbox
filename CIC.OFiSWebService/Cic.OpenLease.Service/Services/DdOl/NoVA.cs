using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenLease.ServiceAccess;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdEurotax;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    class FuelInfo
    {
        public long SYSPRMART { get; set; }
        public String CODE { get; set; }
        public String MART { get; set; }
    }
    class MartDto
    {
        public long SYSPRMART { get; set; }
        public String NAME { get; set; }
        public String DESCRIPTION { get; set; }
        public String CODE { get; set; }
        public int RANK { get; set; }
    }
    /// <summary>
    /// Class to calculate the NoVA Value
    /// </summary>
    [System.CLSCompliant(true)]
    public class NoVA
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static CacheDictionary<String, MartDto> martcache = CacheFactory<String, MartDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, FuelTypeConstants> fuelCache = CacheFactory<long, FuelTypeConstants>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);


        #region Constants
        private const string _CnstNOVA_KORR_CO2 = "NOVA_KORR_CO2";
        private const string _CnstNOVA_KORR_NOX_B = "NOVA_KORR_NOX_B";
        private const string _CnstNOVA_KORR_HYBRID = "NOVA_KORR_HYBRID";
        private const string _CnstNOVA_KORR_NOX_D = "NOVA_KORR_NOX_D";
        private const string _CnstNOVA_KORR_DPF = "NOVA_KORR_DPF";
        private const string _CnstNOVA_KORR_MAXBETRAG = "NOVA_KORR_MAXBETRAG";

        private const string _CnstKRAFTSTOFF_BENZIN = "1";
        private const string _CnstKRAFTSTOFF_DIESEL = "2";
        private const string _CnstKRAFTSTOFF_HYBRID = "1";

        private const long _CnstKRAFTSTOFF_BENZIN_HYBRID = 11;
        private const long _CnstKRAFTSTOFF_BENZIN_STANDARD = 10;
        private const long _CnstKRAFTSTOFF_DIESEL_HYBRID = 21;
        private const long _CnstKRAFTSTOFF_DIESEL_STANDARD = 20;
        #endregion

        #region Private variables
        private DdOlExtended _context;
        private decimal _novabonusmalus;
        private decimal _nova;
        #endregion

        #region Properties
        /// <summary>
        /// returns the nova
        /// </summary>
        public decimal nova
        {
            get { return _nova; }
            set { _nova = value; }
        }
        /// <summary>
        /// returns the bonus/malus contained in the nova value, netto
        /// </summary>
        public decimal novabonusmalus
        {
            get { return _novabonusmalus; }
            set { _novabonusmalus = value; }
        }
        #endregion


        #region Constructors
        public NoVA(DdOlExtended context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        /// <summary>
        /// calculates the NoVA incl. novabonusmalus
        /// </summary>
        /// <param name="grund">Grundpreis</param>
        /// <param name="sonzub">Sonderzubehör</param>
        /// <param name="paket">Zubehör-Paketpreis</param>
        /// <param name="novasatz">Nova-Steuersatz</param>
        /// <param name="kraftstoff">Kraftstoffart 00|01|11|10</param>
        /// <param name="co2">CO2 Partikel</param>
        /// <param name="nox">NOx Partikel</param>
        /// <param name="particles">Feinstaubpartikel</param>
        /// <param name="perDate">per Datum</param>
        /// <returns></returns>
       /* public decimal calculateNovaAndBonus(decimal grund, decimal sonzub, decimal paket, decimal novasatz, string kraftstoff, decimal co2, decimal nox, decimal particles, DateTime perDate)
        {
            //NoVA Base
            _nova = (grund + sonzub + paket) * novasatz;

            //NoVA Eco-Bonus/Malus
            _novabonusmalus = calculateNovaBonusMalus(kraftstoff, co2, nox, particles, perDate);

            _nova += _novabonusmalus;

            return _nova;
        }*/

        public decimal validateQuoteByPercentage(String quoteStr, decimal input, decimal defValue)
        {
            decimal quote = QUOTEDao.deliverQuotePercentValueByName(quoteStr);
            decimal lowBorder = defValue - quote;
            decimal highBorder = defValue + quote;
            if (input < lowBorder)
                return lowBorder;
            if (input > highBorder)
                return highBorder;
            return input;
        }

        public decimal validateQuoteByValue(String quoteStr, decimal input, decimal defValue, decimal quoteDef)
        {
            decimal quote = quoteDef;
            try
            {
                QUOTEDao qd = new QUOTEDao();
                if(qd.exists(quoteStr, DateTime.Now))
                    quote = QUOTEDao.deliverQuotePercentValueByName(quoteStr);
                
            }
            catch (Exception)
            {
               //quote not defined yet
            }
            decimal lowBorder = defValue - defValue / 100 * quote;
            decimal highBorder = defValue + defValue / 100 * quote;
            if (input < lowBorder)
                return lowBorder;
            if (input > highBorder)
                return highBorder;
            return input;
        }

        public decimal validateQuoteByValue(String quoteStr, decimal input, decimal defValue)
        {
            decimal quote = QUOTEDao.deliverQuotePercentValueByName(quoteStr);
            decimal lowBorder = defValue - defValue / 100 * quote;
            decimal highBorder = defValue + defValue / 100 * quote;
            if (input < lowBorder)
                return lowBorder;
            if (input > highBorder)
                return highBorder;
            return input;
        }

        /// <summary>
        /// returns the configured Fueltype Database id for the vehicle
        /// CACHED
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public long getAntriebsartId(long sysobtyp)
        {
            FuelTypeConstants mart = getAntriebsart(sysobtyp);
            String martCode = mart.ToString();
            if (!martcache.ContainsKey(martCode))
            {
                List<MartDto> marts = _context.ExecuteStoreQuery<MartDto>("select * from prmart", null).ToList();
                foreach (MartDto mDto in marts)
                {
                    martcache[mDto.CODE] = mDto;
                }
            }
            return martcache[martCode].SYSPRMART;
        }

        /// <summary>
        /// Fetches the FuelType, independet of the vehicle being from eurotax or manually configured
        /// CACHED
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public FuelTypeConstants getAntriebsart(long sysobtyp)
        {
            if (!fuelCache.ContainsKey(sysobtyp))
            {
                FuelTypeConstants val;
                try
                {

                    String fuelQuery = "select fztyp.sysprmart, fztyp.mart, etgtxttabel.code from obtyp left outer join fztyp on fztyp.sysfztyp=obtyp.sysfztyp left outer join etgtype on etgtype.natcode=obtyp.schwacke left outer join etgtxttabel on etgtxttabel.code=txtfueltypecd2 where sysobtyp=";
                    FuelInfo fi = _context.ExecuteStoreQuery<FuelInfo>(fuelQuery + sysobtyp, null).FirstOrDefault();
                    if (fi.CODE != null && fi.CODE.Length > 0)
                        val= getAntriebsartEurotax(fi.CODE);
                        //TODO11 AIDA MART
                    else val = getAntriebsartMART(fi.SYSPRMART);
                    //else val = getAntriebsartFZ(fi.MART);


                }
                catch (Exception ex)
                {
                    _Log.Error("Fetching FuelType failed: ", ex);
                    val = FuelTypeConstants.Undefined;
                }
                fuelCache[sysobtyp] = val;
            }
            return fuelCache[sysobtyp];
        }
        /*
        /// <summary>
        /// @Deprecated
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        private decimal calcNovaBonusMalus(long sysobtyp)
        {
            try
            {

                decimal co2 = 0, particles = 0, nox = 0;
                String kraftstoff = "";
                bool hybrid = false;
                FuelTypeConstants antriebsart = FuelTypeConstants.Undefined;
                var CurrentObTyp = (from ObTyp in _context.OBTYP
                                    where ObTyp.SYSOBTYP == sysobtyp
                                    select ObTyp).FirstOrDefault();
                if (CurrentObTyp == null) return 0;
                int aklasse = Convert.ToInt32(CurrentObTyp.AKLASSE);
                if (aklasse == 40) return 0;//Ticket 3077
                if (CurrentObTyp.SCHWACKE != null && CurrentObTyp.SCHWACKE.Length > 0 && CurrentObTyp.IMPORTTABLE == "ETGTYPE")
                {
                    using (EurotaxEntities Entities = new EurotaxExtendedEntities())
                    {
                        // Query ETGCONSUMER
                        var CurrentConsumer = (from Consumer in Entities.ETGCONSUMER
                                               where Consumer.NATCODE == CurrentObTyp.SCHWACKE
                                               select Consumer).FirstOrDefault();

                        if (CurrentConsumer != null)
                        {
                            //= CurrentConsumer.PART.ToString();
                            co2 = (decimal)CurrentConsumer.CO2EMI.GetValueOrDefault();
                            nox = 1000 * (decimal)CurrentConsumer.NOX.GetValueOrDefault();
                            // (decimal)CurrentConsumer.CONSTOT.GetValueOrDefault();
                            particles = (decimal)CurrentConsumer.PART.GetValueOrDefault();
                        }
                        // Query ETGTYPE
                        var CurrentType = (from Type in Entities.ETGTYPE
                                           where Type.NATCODE == CurrentObTyp.SCHWACKE
                                           select Type).FirstOrDefault();

                        // Query ETGTXTTABEL
                        var CurrentFuel = (from Fuel in Entities.ETGTXTTABEL
                                           where Fuel.CODE == CurrentType.TXTFUELTYPECD2
                                           select Fuel).FirstOrDefault();
                        hybrid = isHybrid(CurrentType.SECFUELTYPCD2);

                        antriebsart = getAntriebsartEurotax(CurrentFuel.CODE);


                    }
                }
                else
                {
                    // Check if FzTyp reference is loaded
                    if (!CurrentObTyp.FZTYPReference.IsLoaded)
                    {
                        // Load the reference
                        CurrentObTyp.FZTYPReference.Load();
                    }

                    kraftstoff = string.Empty;

                    // Check if FzTyp exists
                    if (CurrentObTyp.FZTYP != null)
                    {
                        co2 = CurrentObTyp.FZTYP.CO2EMI.GetValueOrDefault();
                        particles = CurrentObTyp.FZTYP.PARTIKEL.GetValueOrDefault();
                        nox = CurrentObTyp.FZTYP.NOX.GetValueOrDefault();
                        //CurrentObTyp.FZTYP.VERBRAUCH.GetValueOrDefault();



                        if (CurrentObTyp.FZTYP.MART != null)
                        {
                            String mart = CurrentObTyp.FZTYP.MART;
                            if (mart.Contains("Hybrid"))
                                hybrid = true;
                            antriebsart = getAntriebsartFZ(mart);

                        }
                    }


                }

                if (antriebsart == FuelTypeConstants.Diesel)
                {
                    kraftstoff += "2";
                }
                else
                {
                    kraftstoff += "1";
                }

                if (hybrid)
                {
                    kraftstoff += "1";
                }
                else
                {
                    kraftstoff += "0";
                }


                return calculateNovaBonusMalus(kraftstoff, co2, nox, particles, System.DateTime.Now);

            }
            catch (Exception ex)
            {
                _Log.Error("Fetching BonusMalus failed: ", ex);
                return 0;
            }
        }
        */
        #endregion

        /// <summary>
        /// Fetches FuelType for Manually configured FZTYPE Vehicles vie PRMART
        /// </summary>
        /// <param name="sysprmart"></param>
        /// <returns></returns>
        public FuelTypeConstants getAntriebsartMART(long sysprmart)
        {
            if (martcache.Count==0)
            {
                List<MartDto> marts = _context.ExecuteStoreQuery<MartDto>("select * from prmart", null).ToList();
                foreach (MartDto mDto in marts)
                {
                    martcache[mDto.CODE] = mDto;
                }
            }
            foreach (String mart in martcache.Keys)
            {
				if (martcache[mart].SYSPRMART != sysprmart) continue;
				
                if (FuelTypeConstants.Undefined.ToString().Equals(mart))
                    return FuelTypeConstants.Undefined;
                if (FuelTypeConstants.UnleadedPetrolAndEthanol.ToString().Equals(mart))
                    return FuelTypeConstants.UnleadedPetrolAndEthanol;
                if (FuelTypeConstants.Diesel.ToString().Equals(mart))
                    return FuelTypeConstants.Diesel;
                if (FuelTypeConstants.UnleadedPetrol.ToString().Equals(mart))
                    return FuelTypeConstants.UnleadedPetrol;
                if (FuelTypeConstants.Petrol.ToString().Equals(mart))
                    return FuelTypeConstants.Petrol;
                if (FuelTypeConstants.Gas.ToString().Equals(mart))
                    return FuelTypeConstants.Gas;
                if (FuelTypeConstants.Electricity.ToString().Equals(mart))
                    return FuelTypeConstants.Electricity;
            }



            return FuelTypeConstants.Undefined;
        }

        /// <summary>
        /// Fetch Motortype-String for ANGOBINIMOTORTYP
        /// </summary>
        /// <param name="fuelType"></param>
        /// <returns></returns>
        public static String getMotortyp(FuelTypeConstants fuelType)
        {
            String rval = "UNDEFINED";
            if (fuelType.Equals(FuelTypeConstants.UnleadedPetrolAndEthanol))
                rval = "UNLEADED_PETROL_AND_ETHANOL";
            else if (fuelType.Equals(FuelTypeConstants.Diesel))
                rval = "DIESEL";
            else if (fuelType.Equals(FuelTypeConstants.UnleadedPetrol))
                rval = "UNLEADED_PETROL";
            else if (fuelType.Equals(FuelTypeConstants.Petrol))
                rval = "PETROL";
            else if (fuelType.Equals(FuelTypeConstants.Gas))
                rval = "GAS";
            else if (fuelType.Equals(FuelTypeConstants.Electricity))
                rval = "ELECTRICITY";
            return rval;
        }

        /// <summary>
        /// Fetch Fueltype from ANGOBINIMOTORTYP
        /// </summary>
        /// <param name="fuelType"></param>
        /// <returns></returns>
        public static FuelTypeConstants getFueltypeFromMotortyp(String motortyp)
        {
            FuelTypeConstants rval = FuelTypeConstants.Undefined;
            if (motortyp == null) return rval;
            if (motortyp.Equals("UNLEADED_PETROL_AND_ETHANOL"))
                rval = FuelTypeConstants.UnleadedPetrolAndEthanol;
            else if (motortyp.Equals("DIESEL"))
                rval = FuelTypeConstants.Diesel;
            else if (motortyp.Equals("UNLEADED_PETROL"))
                rval = FuelTypeConstants.UnleadedPetrol;
            else if (motortyp.Equals("PETROL"))
                rval = FuelTypeConstants.Petrol;
            else if (motortyp.Equals("GAS"))
                rval = FuelTypeConstants.Gas;
            else if (motortyp.Equals("ELECTRICITY"))
                rval = FuelTypeConstants.Electricity;
            return rval;
        }

        /// <summary>
        /// Fetches FuelType for Manually configured FZTYPE Vehicles
        /// </summary>
        /// <param name="mart"></param>
        /// <returns></returns>
        public static FuelTypeConstants getAntriebsartFZ(String mart)
        {
            FuelTypeConstants antriebsart = FuelTypeConstants.Undefined;
            // Get the appropriate fuel type constant
            if (mart.Contains("Benzin bleifrei+Eth"))
                antriebsart = FuelTypeConstants.UnleadedPetrolAndEthanol;
            else if (mart.Contains("Diesel"))
                antriebsart = FuelTypeConstants.Diesel;

            else if (mart.Contains("Benzin bleifrei"))
                antriebsart = FuelTypeConstants.UnleadedPetrol;

            else if (mart.Contains("Benzin"))
                antriebsart = FuelTypeConstants.Petrol;

            else if (mart.Contains("Gas"))
                antriebsart = FuelTypeConstants.Gas;

            else if (mart.Contains("Elektro"))
                antriebsart = FuelTypeConstants.Electricity;

            return antriebsart;
        }

        /// <summary>
        /// Fetches FuelType for Eurotax Vehicles
        /// </summary>
        /// <param name="fuelCode"></param>
        /// <returns></returns>
        public static FuelTypeConstants getAntriebsartEurotax(String fuelCode)
        {
            // Get the appropriate fuel type constant
            FuelTypeConstants antriebsart = FuelTypeConstants.Undefined;
            switch (fuelCode)
            {
                case "00100012":
                    antriebsart = FuelTypeConstants.UnleadedPetrolAndEthanol;
                    break;

                case "00100003":
                    antriebsart = FuelTypeConstants.Diesel;
                    break;

                case "00100001":
                    antriebsart = FuelTypeConstants.UnleadedPetrol;
                    break;

                case "00100002":
                    antriebsart = FuelTypeConstants.Petrol;
                    break;

                case "00100011":
                    antriebsart = FuelTypeConstants.Gas;
                    break;

                case "00100004":
                    antriebsart = FuelTypeConstants.Electricity;
                    break;

                default:
                    antriebsart = FuelTypeConstants.Undefined;
                    break;
            }
            return antriebsart;
        }

        /// <summary>
        /// See also Pflichtenheft Konfiguration, Ticket #1576, bmw nova ökologisierung feinspezifikation
        /// 
        /// Nova 2014 Malus für PKW über 250g - DONT call this for Motorcycles
        /// </summary>
        /// <param name="kraftstoff"></param>
        /// <param name="co2"></param>
        /// <param name="nox"></param>
        /// <param name="particles"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public decimal calculateNovaBonusMalus(string kraftstoff, decimal co2, decimal nox, decimal particles, DateTime perDate)
        {
            decimal rval = 0;
            decimal tempval = 0;
            decimal malus = 0;
            decimal bonus = 0;
            KORREKTURDao kh = new KORREKTURDao(_context);
            string op = "+";

            

            try
            {
                tempval = kh.Correct(_CnstNOVA_KORR_CO2, 0, op, perDate, co2.ToString(), "0");
                if (tempval < 0) bonus = tempval; else malus = tempval;

               /* if (_CnstKRAFTSTOFF_BENZIN.Equals(kraftstoff.Substring(0, 1)))
                {
                    tempval = kh.Correct(_CnstNOVA_KORR_NOX_B, 0, op, perDate, nox.ToString(), "0");
                    if (tempval < 0) bonus += tempval; else malus += tempval;
                }
                else
                {
                    tempval = kh.Correct(_CnstNOVA_KORR_NOX_D, 0, op, perDate, nox.ToString(), particles.ToString());
                    if (tempval < 0) bonus += tempval; else malus += tempval;
                    tempval = kh.Correct(_CnstNOVA_KORR_DPF, 0, op, perDate, particles.ToString(), "0");
                    if (tempval < 0) bonus += tempval; else malus += tempval;
                }
                tempval = kh.Correct(_CnstNOVA_KORR_HYBRID, 0, op, perDate, kraftstoff.Substring(1, 1), "0");
                if (tempval < 0) bonus += tempval; else malus += tempval;

                bonus = kh.Correct(_CnstNOVA_KORR_MAXBETRAG, bonus, op, perDate, bonus.ToString(), "0");*/
                rval = malus + bonus;
            }
            catch (System.InvalidOperationException)
            {
                // TODO Exceptionhandling with Resources
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralSelect, "Can't calculate RV, invalid SYSOBTYP or perDate.");
            }
            return rval;
        }


        /// <summary>
        /// Calculates the Nova amount, NO Bonus/Malus (ökologisierungsbetrag)
        /// uses the gross values and calculates the gross exl. nova addition
        /// </summary>
        /// <param name="technicalDataDto"></param>
        /// <param name="Ust"></param>
        /// <param name="noNovaCalc"></param>
        /// <param name="lieferdatum"></param>
        public void calculateNova(TechnicalDataDto technicalDataDto, decimal Ust, bool noNovaCalc, DateTime lieferdatum)
        {
            decimal sonderminderung = 0;
            if (!technicalDataDto.isMotorrad && technicalDataDto.NovaSatz > 0)
            {
                sonderminderung += calculateSonderminderung(lieferdatum, technicalDataDto.Antriebsart,
                    technicalDataDto.Hybrid);
                
                //sonderminderung darf max dem novabetrag entsprechen
                NovaType ntmp = new NovaType(Ust, technicalDataDto.NovaSatz, NovaType.fetchNovaQuote(), 0);
                ntmp.setBruttoInklNova(technicalDataDto.PaketeBrutto);
                decimal maxsm = ntmp.nova;
                ntmp.setBruttoInklNova(technicalDataDto.SonderausstattungBrutto);
                maxsm += ntmp.nova;
                ntmp.setNetto(technicalDataDto.ListenpreisNettoNetto);
                maxsm += ntmp.nova;
                if (maxsm + sonderminderung < 0)
                    sonderminderung = -1*maxsm;
               
            //--------------------------------------------------------
            }

            //NOVANEU
            NovaType nt = new NovaType(Ust, technicalDataDto.NovaSatz, NovaType.fetchNovaQuote(), sonderminderung);
            if (technicalDataDto.ListenpreisNettoNetto > 0)
            {
                nt.setNetto(technicalDataDto.ListenpreisNettoNetto);
                technicalDataDto.ListenpreisBruttoexklNoVa = nt.bruttoExklNova;// BmwTechnicalDataDto.ListenpreisBrutto / (100 + BmwTechnicalDataDto.NovaSatz) * 100;
                technicalDataDto.ListenpreisBrutto = nt.bruttoInklNova;
            }
            else
            {
                nt.setBruttoInklNova(technicalDataDto.ListenpreisBrutto);
                technicalDataDto.ListenpreisBruttoexklNoVa = nt.bruttoExklNova;// BmwTechnicalDataDto.ListenpreisBrutto / (100 + BmwTechnicalDataDto.NovaSatz) * 100;
                technicalDataDto.ListenpreisNettoNetto = nt.netto;
            }

            nt = new NovaType(Ust, technicalDataDto.NovaSatz, NovaType.fetchNovaQuote(), 0);
            nt.setBruttoInklNova(technicalDataDto.PaketeBrutto);
            technicalDataDto.PaketeBruttoexklNoVa = nt.bruttoExklNova; //BmwTechnicalDataDto.PaketeBrutto / (100 + BmwTechnicalDataDto.NovaSatz) * 100;
            nt.setBruttoInklNova(technicalDataDto.SonderausstattungBrutto);
            technicalDataDto.SonderausstattungBruttoexklNoVa = nt.bruttoExklNova; //BmwTechnicalDataDto.SonderausstattungBrutto / (100 + BmwTechnicalDataDto.NovaSatz) * 100;

            technicalDataDto.ListenpreisBruttoexklNoVa = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(technicalDataDto.ListenpreisBruttoexklNoVa);
            technicalDataDto.PaketeBruttoexklNoVa = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(technicalDataDto.PaketeBruttoexklNoVa);
            technicalDataDto.SonderausstattungBruttoexklNoVa = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(technicalDataDto.SonderausstattungBruttoexklNoVa);

            technicalDataDto.NovaBrutto = 0;
            technicalDataDto.NovaNetto = 0;
            technicalDataDto.NovaUst = 0;
            if (noNovaCalc) return;

            if (!technicalDataDto.NovaBefreiung)//keine novabefreiung
            {
                nt = new NovaType(Ust, technicalDataDto.NovaSatz, NovaType.fetchNovaQuote(), sonderminderung);
                nt.setBruttoInklNova(technicalDataDto.PaketeBrutto + technicalDataDto.SonderausstattungBrutto + technicalDataDto.ListenpreisBrutto);
                technicalDataDto.NovaBrutto = nt.nova + nt.novaZuschlag;//(BmwTechnicalDataDto.ListenpreisBruttoexklNoVa + BmwTechnicalDataDto.SonderausstattungBruttoexklNoVa + BmwTechnicalDataDto.PaketeBruttoexklNoVa) * BmwTechnicalDataDto.NovaSatz / 100;

               
                if (technicalDataDto.NovaBrutto < 0)
                    technicalDataDto.NovaBrutto = 0;


                technicalDataDto.NovaNetto = technicalDataDto.NovaBrutto;// Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(BmwTechnicalDataDto.NovaBrutto, Ust);
                technicalDataDto.NovaUst = 0;// Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(BmwTechnicalDataDto.NovaBrutto, Ust);
                //Rounding

                technicalDataDto.NovaBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(technicalDataDto.NovaBrutto);
                technicalDataDto.NovaNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(technicalDataDto.NovaNetto);
                technicalDataDto.NovaUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(technicalDataDto.NovaUst);
            }
        }


        /// <summary>
        /// calculates the Sonderminderung for PKW - DONT CALL FOR MOTORCYCLES!
        /// Der Abzug beträgt bis Ende 2014 € 450.- für Benzinfahrzeuge und € 350.- für Dieselfahrzeuge. 
        /// Ab 1.1.2015 beträgt der Abzug für alle Fahrzeuge € 400.- und ab 1.1.2016 € 300.- 
        /// Kein Abzugsposten, wenn ein Bonus für umweltfreundliche Antriebe anzuwenden ist.
        /// - Für Fahrzeuge mit umweltfreundlichen Antrieben wurde der Bonus auf € 600.- erhöht und bis Ende 2015 verlängert. 
        /// Bei Fahrzeugen mit einem umweltfreundlichen Antrieb (siehe nachfolgende Auflistung) ergibt sich eine Sonderminderung bis 31.12.2014 von EUR 600,-, allerdings nicht additiv zu den oben angeführten Abzügen. Diese Sonderminderung gilt für folgende Antriebsarten:
        /// - Hybridantrieb
        /// - E85 Kraftstoff
        /// - Erdgas/Biogas
        /// - Flüssiggas
        /// - Wasserstoff
        /// Aus keinem der genannten Abzüge kann sich eine Steuergutschrift ergeben. Das heißt, die NoVA kann minimal EUR 0,-- betragen.
        /// </summary>
        /// <param name="lieferdatum"></param>
        /// <param name="antriebsart"></param>
        /// <param name="hybrid"></param>
        /// <returns></returns>
        public static decimal calculateSonderminderung(DateTime lieferdatum, FuelTypeConstants antriebsart, bool hybrid)
        {
            QUOTEDao quote = new QUOTEDao();
            decimal rval = 0;
            if (antriebsart == FuelTypeConstants.Electricity) return rval;
            if (hybrid || antriebsart == FuelTypeConstants.Gas ||antriebsart == FuelTypeConstants.UnleadedPetrolAndEthanol)
            {
                if(quote.exists(QUOTEDao.QUOTE_NOVAABZUG_HYBRID, lieferdatum))
                    rval = (decimal)quote.getQuote(QUOTEDao.QUOTE_NOVAABZUG_HYBRID, lieferdatum);
                if(rval!=0) return rval;//Dieser Bonus bevorzugt vor den anderen
            }
            if (antriebsart == FuelTypeConstants.Diesel)
            {
                rval = (decimal)quote.getQuote(QUOTEDao.QUOTE_NOVAABZUG_DIESEL, lieferdatum);
            }
            else if (antriebsart != FuelTypeConstants.Diesel)
            {
                rval = (decimal)quote.getQuote(QUOTEDao.QUOTE_NOVAABZUG_OTHER, lieferdatum);
            }

            return rval;
         
           
        }


        public void fetchTechnicalDataFromFzTyp(TechnicalDataDto technicalDataDto, OBTYP CurrentObTyp, long sysperole, DdOlExtended context)
        {
            technicalDataDto.reset();
            if (CurrentObTyp.FZTYP == null)
                context.Entry(CurrentObTyp).Reference(f => f.FZTYP).Load();
           

            // Check if FzTyp exists
            if (CurrentObTyp.FZTYP == null) return;

            technicalDataDto.CO2Emission = CurrentObTyp.FZTYP.CO2EMI.GetValueOrDefault();
            technicalDataDto.Particles = CurrentObTyp.FZTYP.PARTIKEL.GetValueOrDefault();
            technicalDataDto.NOXEmission = CurrentObTyp.FZTYP.NOX.GetValueOrDefault();
            technicalDataDto.Verbrauch = CurrentObTyp.FZTYP.VERBRAUCH.GetValueOrDefault();


            // BmwTechnicalDataDto.AKLASSE = Convert.ToInt32(CurrentObTyp.AKLASSE);

            // Get the technical data
            technicalDataDto.Ccm = (long)CurrentObTyp.FZTYP.HUBRAUM.GetValueOrDefault();
            technicalDataDto.Kw = (long)CurrentObTyp.FZTYP.LEISTUNG.GetValueOrDefault();
            technicalDataDto.Ps = (long)Math.Round((double)technicalDataDto.Kw * 1.36, 0);
            technicalDataDto.Leistung = technicalDataDto.Kw + "/" + technicalDataDto.Ps;
            technicalDataDto.DPF = CurrentObTyp.FZTYP.PARTIKEL.GetValueOrDefault().ToString();

            technicalDataDto.NovaSatz = (decimal)CurrentObTyp.FZTYP.NOVA;

            decimal Ust = LsAddHelper.getGlobalUst(sysperole);


            // Get Price if not provided
            if (technicalDataDto.ListenpreisBrutto == 0)
            {
                //NOVANEU
               /* NovaType nt = new NovaType(Ust, BmwTechnicalDataDto.NovaSatz, NovaType.fetchNovaQuote(),0);
                nt.setNetto(CurrentObTyp.FZTYP.GRUND.GetValueOrDefault());
                BmwTechnicalDataDto.ListenpreisBrutto = nt.bruttoInklNova;
                BmwTechnicalDataDto.ListenpreisBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(BmwTechnicalDataDto.ListenpreisBrutto);
                */
                
                technicalDataDto.PaketeBrutto = 0;
                technicalDataDto.SonderausstattungBrutto = 0;
            }
            if (technicalDataDto.fetchListenpreis)//false for sa3
                technicalDataDto.ListenpreisBrutto = 0; //NOVA 2014 we let it do the deliverPurchasePrice WS!


            // Get Price if not provided
            if (technicalDataDto.ListenpreisBrutto == 0 || technicalDataDto.fetchListenpreis)
            {
                technicalDataDto.ListenpreisNettoNetto = CurrentObTyp.FZTYP.GRUND.GetValueOrDefault();
            }


            technicalDataDto.Verbrauch = CurrentObTyp.FZTYP.VERBRAUCH.GetValueOrDefault();
            technicalDataDto.Particles = CurrentObTyp.FZTYP.PARTIKEL.GetValueOrDefault();
            technicalDataDto.Hybrid = false;
            technicalDataDto.Getriebeart = CurrentObTyp.FZTYP.GART;

            
            //AIDA11 TODO11
            long sysprmart =  CurrentObTyp.FZTYP.SYSPRMART.GetValueOrDefault();
           
            if (sysprmart > 0) 
            {
           
                //if (mart.Contains("Hybrid"))
                    //BmwTechnicalDataDto.Hybrid = true;
                //
                technicalDataDto.Antriebsart = getAntriebsartMART(sysprmart);
                

            }
           /* if (CurrentObTyp.FZTYP.MART!=null)
            {
                BmwTechnicalDataDto.Antriebsart = getAntriebsartFZ(CurrentObTyp.FZTYP.MART);
            }*/
            
            // Get the automatic transmission flag
            bool AutoTransmission = CurrentObTyp.FZTYP.GART == "Automatik";

            technicalDataDto.Automatic = AutoTransmission ? 1 : 0;

            try
            {
                using (DdOlExtended Entities = new DdOlExtended())
                {
                    ExtTechInfo eti = Entities.ExecuteStoreQuery<ExtTechInfo>("select kwe,pse,kwgesamt,psgesamt,eek,reichweite,kwh from obtypfzadd where sysobtyp=" + technicalDataDto.sysobtyp, null).FirstOrDefault();
                    if (eti != null)
                    {
                        technicalDataDto.eek = eti.eek;
                        technicalDataDto.pse = eti.pse;
                        technicalDataDto.kwh = eti.kwh;
                        technicalDataDto.kwgesamt = eti.kwgesamt;
                        technicalDataDto.psgesamt = eti.psgesamt;
                        technicalDataDto.kwe = eti.kwe;
                        technicalDataDto.reichweite = eti.reichweite;
                    }
                }
            }
            catch (Exception e)
            {
                _Log.Error("Extended technical Data for manual car like kwe,pse,kwgesamt,psgesamt,eek,reichweite,kwh not available: " + e.Message);
            }

            setKraftstoff(technicalDataDto);

        }

        public static String getKraftstoff(FuelTypeConstants fuel, bool isHybrid)
        {
            String rval = string.Empty;
            if (fuel == FuelTypeConstants.Diesel)
                rval += "2";
            else
                rval += "1";


            if (isHybrid)
                rval += "1";
            else
                rval += "0";

            return rval;
        }

        private static void setKraftstoff(TechnicalDataDto dto)
        {
            dto.Kraftstoff = getKraftstoff(dto.Antriebsart, dto.Hybrid);
        }

        public FahrzeugArt getFahrzeugArtBySysObTyp(long sysobtyp)
        {
            String aklasse = _context.ExecuteStoreQuery<String>("select aklasse from obtyp where sysobtyp=" + sysobtyp, null).FirstOrDefault();
            return getFahrzeugArt(aklasse);
        }

        public static FahrzeugArt getFahrzeugArt(string aklasse)
        {
            switch (aklasse)
            {
                case ("10"): return FahrzeugArt.PKW;
                case ("20"): return FahrzeugArt.LKW;
                case ("40"): return FahrzeugArt.MOTORRAD;
                default: return FahrzeugArt.PKW;
            }
        }

        /// <summary>
        /// Liefert technische Daten+Preise über Eurotax
        /// </summary>
        /// <param name="eurotaxNr"></param>
        /// <param name="sysObArt"></param>
        /// <param name="technicalDataDto"></param>
        /// <returns></returns>
        public void fetchTechnicalDataFromEurotax(string eurotaxNr, long sysObArt, Cic.OpenLease.ServiceAccess.DdOl.TechnicalDataDto technicalDataDto, long sysperole)
        {


            try
            {
                technicalDataDto.reset();
                // Create the entities
                using (DdEurotaxExtended Entities = new DdEurotaxExtended())
                {

                    // Query ETGTYPE
                    var CurrentType = (from Type in Entities.ETGTYPE
                                       where Type.NATCODE == eurotaxNr
                                       select Type).FirstOrDefault();

                    // Check if the type was found
                    if (CurrentType == null)
                    {
                        _Log.Error("The eurotax number " + eurotaxNr + " was not found in ETGTYPE");
                        throw new System.ApplicationException("The eurotax number " + eurotaxNr + " was not found in ETGTYPE");
                    }

                    // Query ETGTYPEAT
                    var CurrentTypeAt = (from TypeAt in Entities.ETGTYPEAT
                                         where TypeAt.NATCODE == eurotaxNr
                                         select TypeAt).FirstOrDefault();

                    // Check if the austrian type was found
                    if (CurrentTypeAt == null)
                    {
                        //_Log.Warn("Could not retrieve the Austria specific data for " + eurotaxNr + " in ETGTYPEAT");
                        // Throw an exception
                       // throw new ApplicationException("Could not retrieve the Austria specific data for " + eurotaxNr + " in ETGTYPEAT");
                    }

                    // Query ETGCONSUMER
                    var CurrentConsumer = (from Consumer in Entities.ETGCONSUMER
                                           where Consumer.NATCODE == eurotaxNr
                                           select Consumer).FirstOrDefault();

                    // BmwTechnicalDataDto.AKLASSE = CurrentType.VEHTYPE;

                    // Check if consumer data was found
                    if (CurrentConsumer == null)
                    {
                        if (CurrentType.VEHTYPE == 40)
                            _Log.Debug("Consumer Data for " + eurotaxNr + " was not found!");
                        else
                        {
                            _Log.Warn("Could not retrieve Consumer Data for " + eurotaxNr + " in ETGCONSUMER");
                            throw new ApplicationException("Could not retrieve Consumer Data for " + eurotaxNr + " in ETGCONSUMER");
                        }
                        // Throw an exception

                    }

                    // Query ETGPRICE
                    var CurrentPrice = (from Price in Entities.ETGPRICE
                                        where Price.NATCODE == eurotaxNr
                                        select Price).FirstOrDefault();

                    // Check if the price was found
                    if (CurrentPrice == null)
                    {
                        // Throw an exception
                        _Log.Error("Price for " + eurotaxNr + " not found in ETGPRICE");
                        throw new ApplicationException("Price for " + eurotaxNr + " not found in ETGPRICE");
                    }

                    // Query ETGTXTTABEL
                    var CurrentFuel = (from Fuel in Entities.ETGTXTTABEL
                                       where Fuel.CODE == CurrentType.TXTFUELTYPECD2
                                       select Fuel).FirstOrDefault();

                    // Check if the fuel was found
                    if (CurrentFuel == null)
                    {
                        _Log.Error("Fuel for " + eurotaxNr + " could not be found in ETGTXTTABEL.");
                        // Throw an exception
                        throw new ApplicationException("Fuel for " + eurotaxNr + " could not be found in ETGTXTTABEL.");
                    }

                    // Query ETGTXTTABEL
                    var CurrentTransmission = (from Transmission in Entities.ETGTXTTABEL
                                               where Transmission.CODE == CurrentType.TXTTRANSTYPECD2
                                               select Transmission).FirstOrDefault();

                    // Check if the transmission data was found
                    if (CurrentTransmission == null)
                    {
                        _Log.Error("Transmission type for " + eurotaxNr + " could not be found in ETGTXTTABEL.");
                        // Throw an exception
                        throw new ApplicationException("Transmission type for " + eurotaxNr + " could not be found in ETGTXTTABEL.");
                    }




                    // Get the technical data
                    technicalDataDto.Ccm = (long)Math.Round(CurrentType.CAPTECH.GetValueOrDefault());
                    technicalDataDto.Kw = (long)Math.Round(CurrentType.KW.GetValueOrDefault());
                    technicalDataDto.Ps = (long)Math.Round(CurrentType.HP.GetValueOrDefault());
                    technicalDataDto.Leistung = CurrentType.KW.GetValueOrDefault() + "/" + CurrentType.HP.GetValueOrDefault();
                    if (CurrentConsumer != null)
                    {
                        technicalDataDto.DPF = CurrentConsumer.PART.ToString();
                        technicalDataDto.CO2Emission = (decimal)CurrentConsumer.CO2EMI.GetValueOrDefault();
                        technicalDataDto.NOXEmission = (decimal)CurrentConsumer.NOX.GetValueOrDefault();
                        technicalDataDto.Verbrauch = (decimal)CurrentConsumer.CONSTOT.GetValueOrDefault();
                        technicalDataDto.Particles = (decimal)CurrentConsumer.PART.GetValueOrDefault();
                    }
                    technicalDataDto.NovaSatz = 0;
                    if(CurrentTypeAt!=null)
                        technicalDataDto.NovaSatz = (decimal)CurrentTypeAt.NOVA2.GetValueOrDefault();

                    //Change Nox grom grams to miligrams
                    technicalDataDto.NOXEmission = (technicalDataDto.NOXEmission * 1000);
                    decimal Ust = LsAddHelper.getGlobalUst(sysperole);

                    // Get Price if not provided
                    if (technicalDataDto.ListenpreisBrutto == 0 || technicalDataDto.fetchListenpreis)
                    {
                        if (technicalDataDto.ListenpreisBrutto == 0)
                        {

                            technicalDataDto.PaketeBrutto = 0;
                            technicalDataDto.SonderausstattungBrutto = 0;
                        }

                        //value is netto
                        //NOVANEU
                        /*
                        NovaType nt = new NovaType(Ust, BmwTechnicalDataDto.NovaSatz, NovaType.fetchNovaQuote(),0);
                        nt.setNetto((decimal)CurrentPrice.NP2);
                        BmwTechnicalDataDto.ListenpreisBrutto = nt.bruttoInklNova;
                        BmwTechnicalDataDto.ListenpreisBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(BmwTechnicalDataDto.ListenpreisBrutto);
                        */
                        if(technicalDataDto.fetchListenpreis)//false for sa3 and VAP
                            technicalDataDto.ListenpreisBrutto = 0; //NOVA 2014 we let it do the deliverPurchasePrice WS!
                        technicalDataDto.ListenpreisNettoNetto = (decimal) CurrentPrice.NP2;

                    }

                    technicalDataDto.Hybrid = isHybrid(CurrentType.SECFUELTYPCD2);


                    technicalDataDto.Getriebeart = CurrentTransmission.TEXTLONG;

                    technicalDataDto.Antriebsart = getAntriebsartEurotax(CurrentFuel.CODE);

                    technicalDataDto.Automatic = hasAutoTransmission(CurrentType.TXTTRANSTYPECD2) ? 1 : 0;

                    try
                    {
                        ExtTechInfo eti = Entities.ExecuteStoreQuery<ExtTechInfo>(
                           /* @"select case when txtfueltypecd2!='00100004' then kw else seckw end as kw, 
      case when txtfueltypecd2!='00100004' then kw*1.36 else seckw*1.36 end as ps,
      case when txtfueltypecd2='00100004' then kw else seckw end as kwe,
      case when txtfueltypecd2='00100004' then kw*1.36 else seckw*1.36 end as pse,
  kw+seckw kwgesamt, kw*1.36+seckw*1.36 psgesamt, 0 eek, 0 reichweite from etgtype where natcode=" + eurotaxNr, null).FirstOrDefault();*/
                            "select kwe,pse,kwgesamt,psgesamt,eek,reichweite,kwh from obtypfzadd where sysobtyp=" + technicalDataDto.sysobtyp, null).FirstOrDefault();
                        if (eti != null)
                        {
                            technicalDataDto.eek = eti.eek;
                            technicalDataDto.pse = eti.pse;
                            technicalDataDto.kwh = eti.kwh;
                            technicalDataDto.kwgesamt = eti.kwgesamt;
                            technicalDataDto.psgesamt = eti.psgesamt;
                            technicalDataDto.kwe = eti.kwe;
                            technicalDataDto.reichweite = eti.reichweite;
                        }
                    }
                    catch (Exception e)
                    {
                        _Log.Error("Extended technical Data like kwe,pse,kwgesamt,psgesamt,eek,reichweite,kwh not available: " + e.Message);
                    }

                    setKraftstoff(technicalDataDto);
                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBmwTechnicalDataFailed, exception);


                // Log the exception
                _Log.Warn(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBmwTechnicalDataFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBmwTechnicalDataFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public static bool hasAutoTransmission(String txttranstypecd2)
        {
            // Get the automatic transmission flag
            return txttranstypecd2 == "00180007"
                || txttranstypecd2 == "00180005"
                || txttranstypecd2 == "00180004"
                || txttranstypecd2 == "00180010";

        }

        public static bool isHybrid(String secfueltypcd2)
        {
            bool isHybrid = secfueltypcd2 == "00180010";
            if (isHybrid) return true;

            switch (secfueltypcd2)
            {
                case "00100012":
                case "00100003":
                case "00100001":
                case "00100002":
                case "00100011":
                case "00100004":
                    return true;

                default:
                    return false;
            }
        }

    }
}
