using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;
using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;
using CIC.Database.OL.EF6.Model;
using Cic.OpenOne.Common.Model.DdCt;
using CIC.Database.IC.EF6.Model;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Eurotax DB Data Access Object
    /// </summary>
    [System.CLSCompliant(false)]
    public class EurotaxDBDao : IEurotaxDBDao
    {
        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const String RWFLAGGEDINTERNALQUERY = "select FLAGLOCKEDVG external from obtyp  where sysobtyp = :sysobtyp";

        

        const String VGQUERY = @"SELECT * FROM VG WHERE SYSVG =
                                  (SELECT OBTYP.SYSVGLGD FROM OBTYP WHERE OBTYP.SYSVGLGD > 0 START WITH OBTYP.SYSOBTYP = (SELECT SYSOBTYP FROM ANTOB WHERE SYSANTRAG=:p1 ) 
                                  CONNECT BY PRIOR OBTYP.SYSOBTYPP = OBTYP.SYSOBTYP AND ROWNUM = 1) ";

        
        const String LEVELONEQUERY = "SELECT count(*) found FROM vc_obtyp1 where id1 = :sysobtyp";

        const String OBJECTDATAQUERY = "select vc_obtyp5.*, id5 id, vc_obtyp1.bezeichnung fahrzeugart, vc_obtyp2.bezeichnung marke, vc_obtyp3.bezeichnung baureihe, " +
                                       " vc_obtyp4.bezeichnung modell " +
                                       " from vc_obtyp1, vc_obtyp2, vc_obtyp3, vc_obtyp4, vc_obtyp5 " +
                                       " where vc_obtyp1.id1=vc_obtyp2.id1 and vc_obtyp2.id2=vc_obtyp3.id2 and vc_obtyp3.id3=vc_obtyp4.id3 and " +
                                             " vc_obtyp4.id4=vc_obtyp5.id4 and vc_obtyp5.schwacke='{0}'";

        const String AWFLAGGEDINTERNALQUERY = "select NOEXTID external from obtyp  where sysobtyp=:sysobtyp";


        const String QUERYRWFAKTOR = " SELECT quotedat.PROZENT FROM quotedat, quote WHERE quotedat.sysquote = quote.sysquote AND quotedat.gueltigab <= sysdate AND quote.bezeichnung LIKE 'MAX_RW_FAKTOR' ORDER BY quotedat.gueltigab desc ";
        const String QUERYMWFAKTOR = " SELECT quotedat.PROZENT FROM quotedat, quote WHERE quotedat.sysquote = quote.sysquote AND quotedat.gueltigab <= sysdate AND quote.bezeichnung LIKE 'MW_FAKTOR' ORDER BY quotedat.gueltigab desc ";

        const String QUERYVGREF = @"select vgref.sysvg from vgreftype, vgref, (select sysobtyp,level  lvl from obtyp start with sysobtyp = :sysobtyp connect by prior sysobtypp = sysobtyp) obttable
                                    where vgreftype.arearef = 'OBTYP' and vgref.sysidref=obttable.sysobtyp and vgref.codevgreftype=vgreftype.code  and vgreftype.type=:vgreftype  
                                    AND ( vgref.validfrom  IS NULL   OR vgref.validfrom    <= :perDate  OR vgref.validfrom     = to_date('01.01.0111' , 'dd.MM.yyyy') )  
                                    AND ( vgref.validuntil IS NULL   OR vgref.validuntil   >= :perDate OR vgref.validuntil    = to_date('01.01.0111' , 'dd.MM.yyyy')) 
                                    and (
                                    (( vgreftype.areacontext='BRAND' and vgref.sysidcontext=:sysbrand and :sysbrand>0) or ( vgreftype.areacontext='BRAND' and 0=:sysbrand) )
                                    or 
                                    (( vgreftype.areacontext='PRHGROUP' and vgref.sysidcontext=:sysprhgroup ) or( vgreftype.areacontext='PRHGROUP' and 0=:sysprhgroup ))
                                    or
                                    vgreftype.areacontext is null
                                    )
                                    order by vgreftype.rank desc, obttable.lvl";// obttable.lvl, vgreftype.rank";

        const double RWFAKTOR = 90;

        /// <summary>
        /// Creates a new ETGINPFC and links it with AUSKUNFT
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>SYSETGINPFC</returns>-
        public long SaveEurotaxInpFc(long sysAuskunft)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    ETGINPFC inputGetForecast = new ETGINPFC();
                    inputGetForecast.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ETGINPFC.Add(inputGetForecast);
                    context.SaveChanges();
                    return inputGetForecast.SYSETGINPFC;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von EurotaxInpFc. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Creates new ETGHeader, ETGSettings and ETGVehicleInp and links it with ETGINPFC
        /// </summary>
        /// <param name="inDto"></param>
        /// <param name="sysEtgInp"></param>
        public void SaveEurotaxInDto(EurotaxInDto inDto, long sysEtgInp)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    ETGINPFC etginp = context.ETGINPFC.Where(par => par.SYSETGINPFC == sysEtgInp).Single();

                    ETGHEADER header = new ETGHEADER();
                    header.ETGINPFC = etginp;
                    //header.VERSIONREQUEST = inDto.Version;
                    context.ETGHEADER.Add(header);

                    ETGSETTINGS settings = new ETGSETTINGS();
                    settings.ETGINPFC = etginp;
                    settings.ISOCOUNTRYCODE = inDto.ISOCountryCode.ToString();
                    settings.ISOCURRENCYCODE = inDto.ISOCurrencyCode.ToString();
                    settings.ISOLANGUAGECODE = inDto.ISOLanguageCode.ToString();
                    context.ETGSETTINGS.Add(settings);

                    ETGVEHINP vehicleInp = new ETGVEHINP();
                    vehicleInp.ETGINPFC = etginp;
                    vehicleInp.ANNUALMILEAGEVAL = (int)inDto.EstimatedAnnualMileageValue;
                    vehicleInp.NATCODE = inDto.NationalVehicleCode;
                    vehicleInp.FORECASTPERIOND = inDto.ForecastPeriodFrom;
                    if (inDto.RegistrationDate.Year > 1900)
                        vehicleInp.REGISTRATIONDATE = inDto.RegistrationDate;
                    if (inDto.CurrentMileageValue != null)
                        vehicleInp.CURRENTMILEAGEVAL = (int)inDto.CurrentMileageValue;
                    if (inDto.TotalListPriceOfEquipment > 0)
                        vehicleInp.TOTALLPEQUIPMENT = (decimal)inDto.TotalListPriceOfEquipment;
                    context.ETGVEHINP.Add(vehicleInp);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von EurotaxForecastInput. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Save Eurotax Valuation Input
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <param name="inDto">Input Data</param>
        public void SaveEurotaxValuationInput(long sysAuskunft, EurotaxInDto inDto)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    ETGINPGVAL inputGetValuation = new ETGINPGVAL();
                    inputGetValuation.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ETGINPGVAL.Add(inputGetValuation);

                    ETGHEADER header = new ETGHEADER();
                    header.ETGINPGVAL = inputGetValuation;
                    //header.VERSIONREQUEST = inDto.Version;
                    context.ETGHEADER.Add(header);

                    ETGSETTINGS settings = new ETGSETTINGS();
                    settings.ETGINPGVAL = inputGetValuation;
                    settings.ISOCOUNTRYCODE = inDto.ISOCountryCodeValuation.ToString();
                    settings.ISOCURRENCYCODE = inDto.ISOCurrencyCodeValuation.ToString();
                    settings.ISOLANGUAGECODE = inDto.ISOLanguageCodeValuation.ToString();
                    context.ETGSETTINGS.Add(settings);

                    ETGVALINP valuation = new ETGVALINP();
                    valuation.ETGINPGVAL = inputGetValuation;
                    valuation.MILEAGE = inDto.Mileage;
                    valuation.NATCODE = inDto.NationalVehicleCode;
                    valuation.REGISTRATIONDATE = inDto.RegistrationDate;
                    context.ETGVALINP.Add(valuation);

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von EurotaxValuationInput. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Save Eurotax Valuation Output
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <param name="outDto">Output Data</param>
        public void SaveEurotaxValuationOutput(long sysAuskunft, EurotaxOutDto outDto)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    ETGOUTGVAL etgOut;
                    // check if ETGOUTFC already exists
                    AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();

                    if (Auskunft != null && !context.Entry(Auskunft).Collection(f => f.ETGOUTGVALList).IsLoaded)
                        context.Entry(Auskunft).Collection(f => f.ETGOUTGVALList).Load();
                    if (Auskunft.ETGOUTGVALList.Count() == 0)
                    {
                        // New ETGOUTGVAL
                        etgOut = new ETGOUTGVAL();
                        etgOut.AUSKUNFT = Auskunft;
                        context.ETGOUTGVAL.Add(etgOut);
                        // New ETGVALOUT
                        ETGVALOUT valuation = new ETGVALOUT();
                        valuation.ETGOUTGVAL = etgOut;
                        valuation.ACTUALNEWPRICEINDICATOR = outDto.ActualNewPriceIndicator;
                        valuation.ACTUALNEWPRICEVALUE = (decimal?)outDto.ActualNewPrice;
                        valuation.BASICRESB2BAMOUNT = (decimal?)outDto.BasicResidualB2BAmount;
                        valuation.BASICRESRETAILAMOUNT = (decimal?)outDto.BasicResidualRetailAmount;
                        valuation.BASICRESTRADEAMOUNT = (decimal?)outDto.BasicResidualTradeAmount;
                        valuation.MILEAGEADJB2BAMOUNT = (decimal?)outDto.MileageAdjustmentB2BAmount;
                        valuation.MILEAGEADJRETAILAMOUNT = (decimal?)outDto.MileageAdjustmentRetailAmount;
                        valuation.MILEAGEADJTRADEAMOUNT = (decimal?)outDto.MileageAdjustmentTradeAmount;
                        valuation.MONTHLYADJB2BAMOUNT = (decimal?)outDto.MonthlyAdjustmentB2BAmount;
                        valuation.MONTHLYADJRETAILAMOUNT = (decimal?)outDto.MonthlyAdjustmentRetailAmount;
                        valuation.MONTHLYADJTRADEAMOUNT = (decimal?)outDto.MonthlyAdjustmentTradeAmount;
                        valuation.AVERAGEMILAGE = (int?)outDto.AverageMileage;
                        context.ETGVALOUT.Add(valuation);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von EurotaxValuationOutput. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Creates or updates ETGOUTFC and links it with AUSKUNFT
        /// Creates or updates ETGVEHOUT and links it with ETGOUTFC
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveEurotaxForecastOutput(long sysAuskunft, EurotaxOutDto outDto)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    ETGOUTFC outfc;
                    // check if ETGOUTFC already exists
                    AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();

                    if (Auskunft != null && !context.Entry(Auskunft).Collection(f => f.ETGOUTFCList).IsLoaded)
                        context.Entry(Auskunft).Collection(f => f.ETGOUTFCList).Load();

                    if (Auskunft.ETGOUTFCList.Count() > 0)
                    {
                        outfc = Auskunft.ETGOUTFCList.First();
                        // Update ETGvehicleOut
                        ETGVEHOUT vehout = context.ETGVEHOUT.Where(par => par.ETGOUTFC.SYSETGOUTFC == outfc.SYSETGOUTFC).Single();
                        vehout.RETAILAMOUNT = (decimal?)outDto.RetailAmount;
                        vehout.RETAILVALUEINPERCENT = (decimal?)outDto.RetailValueInPercentage;
                        vehout.TRADEAMOUNT = (decimal?)outDto.TradeAmount;
                        vehout.TRADEVALUEINPERCENT = (decimal?)outDto.TradeValueInPercentage;
                        vehout.ERRORCODE = outDto.ErrorCode;
                        vehout.ERRORDESCRIPTION = outDto.ErrorDescription;
                        context.SaveChanges();
                    }
                    else
                    {
                        // New ETGOutfc
                        outfc = new ETGOUTFC();
                        outfc.AUSKUNFT = Auskunft;
                        context.ETGOUTFC.Add(outfc);
                        context.SaveChanges();
                        // New ETGvehicleOut
                        ETGVEHOUT vehout = new ETGVEHOUT();
                        vehout.ETGOUTFC = context.ETGOUTFC.Where(par => par.SYSETGOUTFC == outfc.SYSETGOUTFC).Single();
                        vehout.RETAILAMOUNT = (decimal?)outDto.RetailAmount;
                        vehout.RETAILVALUEINPERCENT = (decimal?)outDto.RetailValueInPercentage;
                        vehout.TRADEAMOUNT = (decimal?)outDto.TradeAmount;
                        vehout.TRADEVALUEINPERCENT = (decimal?)outDto.TradeValueInPercentage;
                        vehout.ERRORCODE = outDto.ErrorCode;
                        vehout.ERRORDESCRIPTION = outDto.ErrorDescription;
                        context.ETGVEHOUT.Add(vehout);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von EurotaxForecastOutput. ", ex);
                throw ex;
            }
        }



        /// <summary>
        /// Gets ETGVEHINP and ETGSettings by SysAuskunft and returns new EurotaxInDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>EurotaxInDto, filled with data from ETGsettings and ETGVehicleInp</returns>
        public EurotaxInDto FindBySysId(long sysAuskunft)
        {
            EurotaxInDto inDto = new EurotaxInDto();
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    var eurotaxQuery = from auskunft in context.AUSKUNFT
                                       from etgVehInp in auskunft.ETGINPFCList
                                       where auskunft.SYSAUSKUNFT == sysAuskunft
                                       select etgVehInp;
                    ETGINPFC etgInput = eurotaxQuery.Single();
                    if (!context.Entry(etgInput).Collection(f => f.ETGVEHINPList).IsLoaded)
                        context.Entry(etgInput).Collection(f => f.ETGVEHINPList).Load();
                    if (!context.Entry(etgInput).Collection(f => f.ETGSETTINGSList).IsLoaded)
                        context.Entry(etgInput).Collection(f => f.ETGSETTINGSList).Load();
                    if (!context.Entry(etgInput).Collection(f => f.ETGHEADERList).IsLoaded)
                        context.Entry(etgInput).Collection(f => f.ETGHEADERList).Load();

                    ETGVEHINP vehicle = etgInput.ETGVEHINPList.Single();
                    ETGSETTINGS etgSettings = etgInput.ETGSETTINGSList.Single();
                    ETGHEADER etgHeader = etgInput.ETGHEADERList.Single();

                    //inDto.Version = etgHeader.VERSIONREQUEST;
                    inDto.NationalVehicleCode = (long)vehicle.NATCODE;
                    inDto.ForecastPeriodFrom = vehicle.FORECASTPERIOND;
                    inDto.EstimatedAnnualMileageValue = (uint)vehicle.ANNUALMILEAGEVAL;
                    if (vehicle.CURRENTMILEAGEVAL != null)
                        inDto.CurrentMileageValue = (uint?)vehicle.CURRENTMILEAGEVAL;
                    if (vehicle.REGISTRATIONDATE != null)
                        inDto.RegistrationDate = (DateTime)vehicle.REGISTRATIONDATE;
                    if (vehicle.TOTALLPEQUIPMENT != null)
                        inDto.TotalListPriceOfEquipment = (double)vehicle.TOTALLPEQUIPMENT;
                    //inDto.ISOcountryCode = etgSettings.ISOCOUNTRYCODE;
                    //inDto.ISOcurrencyCode = etgSettings.ISOCURRENCYCODE;
                    //inDto.ISOlanguageCode = etgSettings.ISOLANGUAGECODE;
                    if (!string.IsNullOrEmpty(etgSettings.ISOCOUNTRYCODE))
                        inDto.ISOCountryCode = (EurotaxRef.ISOcountryType)Enum.Parse(typeof(EurotaxRef.ISOcountryType), etgSettings.ISOCOUNTRYCODE);
                    if (!string.IsNullOrEmpty(etgSettings.ISOCURRENCYCODE))
                        inDto.ISOCurrencyCode = (EurotaxRef.ISOcurrencyType)Enum.Parse(typeof(EurotaxRef.ISOcurrencyType), etgSettings.ISOCURRENCYCODE);
                    if (!string.IsNullOrEmpty(etgSettings.ISOLANGUAGECODE))
                        inDto.ISOLanguageCode = (EurotaxRef.ISOlanguageType)Enum.Parse(typeof(EurotaxRef.ISOlanguageType), etgSettings.ISOLANGUAGECODE);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Laden von Eurotaxdaten. Error Message. ", ex);
                throw ex;
            }
            return inDto;
        }

        /// <summary>
        /// Find Valuation Input By Sys ID
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Eurotax Information</returns>
        public EurotaxInDto FindValuationInputBySysId(long sysAuskunft)
        {
            EurotaxInDto inDto = new EurotaxInDto();
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    var eurotaxQuery = from auskunft in context.AUSKUNFT
                                       from etgValInp in auskunft.ETGINPGVALList
                                       where auskunft.SYSAUSKUNFT == sysAuskunft
                                       select etgValInp;
                    ETGINPGVAL etgInput = eurotaxQuery.Single();
                    if (!context.Entry(etgInput).Collection(f => f.ETGVALINPList).IsLoaded)
                        context.Entry(etgInput).Collection(f => f.ETGVALINPList).Load();

                    ETGVALINP valuation = etgInput.ETGVALINPList.Single();
                    if (!context.Entry(etgInput).Collection(f => f.ETGSETTINGSList).IsLoaded)
                        context.Entry(etgInput).Collection(f => f.ETGSETTINGSList).Load();
                    if (!context.Entry(etgInput).Collection(f => f.ETGHEADERList).IsLoaded)
                        context.Entry(etgInput).Collection(f => f.ETGHEADERList).Load();

                    ETGSETTINGS etgSettings = etgInput.ETGSETTINGSList.Single();
                    ETGHEADER etgHeader = etgInput.ETGHEADERList.Single();
                    //inDto.Version = etgHeader.VERSIONREQUEST;
                    inDto.NationalVehicleCode = (long)valuation.NATCODE;
                    inDto.Mileage = valuation.MILEAGE;
                    inDto.RegistrationDate = (DateTime)valuation.REGISTRATIONDATE;
                    if (!string.IsNullOrEmpty(etgSettings.ISOCOUNTRYCODE))
                        inDto.ISOCountryCodeValuation = (EurotaxValuationRef.ISOcountryType)Enum.Parse(typeof(EurotaxValuationRef.ISOcountryType), etgSettings.ISOCOUNTRYCODE);
                    if (!string.IsNullOrEmpty(etgSettings.ISOCURRENCYCODE))
                        inDto.ISOCurrencyCodeValuation = (EurotaxValuationRef.ISOcurrencyType)Enum.Parse(typeof(EurotaxValuationRef.ISOcurrencyType), etgSettings.ISOCURRENCYCODE);
                    if (!string.IsNullOrEmpty(etgSettings.ISOLANGUAGECODE))
                        inDto.ISOLanguageCodeValuation = (EurotaxValuationRef.ISOlanguageType)Enum.Parse(typeof(EurotaxValuationRef.ISOlanguageType), etgSettings.ISOLANGUAGECODE);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Laden von Eurotaxdaten. Error Message. ", ex);
                throw ex;
            }
            return inDto;
        }

        /// <summary>
        /// Ermitteln des Restwert-Settings
        /// </summary>
        /// <param name="sysobtyp">Objecttyp</param>
        /// <param name="perDate">perDate</param>
        /// <returns>Einstellungen</returns>
        public RestWertSettingsDto getRestwertSettings(long sysobtyp, DateTime perDate)
        {
            //Ticket#2012010910000253 — Hotfix 5785-RW: Anpassung Webservices 
            RestWertSettingsDto rval = new RestWertSettingsDto();


            // getWertegruppe gibt obTyp.sysvgrw zurück (Eigene Restwert-Gruppe)
            rval.sysvgrw = getWertegruppe(sysobtyp, perDate);
            if (rval.sysvgrw == 0)
                rval.External = true;
            else
                rval.External = false;

            return rval;

        }

        /// <summary>
        /// getRestwertSettings2
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <param name="perDate">perDate</param>
        /// <returns></returns>
        public RestWertSettingsDto getRestwertSettings2(long sysobtyp, DateTime perDate)
        {
            RestWertSettingsDto rval = new RestWertSettingsDto();


            // getWertegruppe2 gibt obTyp.sysvgrw2 zurück (Remo-Restwert-Gruppe)
            rval.sysvgrw = getWertegruppe2(sysobtyp, perDate);
            if (rval.sysvgrw == 0)
                rval.External = true;
            else
                rval.External = false;
            return rval;

        }

        /// <summary>
        /// Ermitteln des Aktwert-Settings
        /// </summary>
        /// <param name="sysobtyp">Objecttyp</param>
        /// <param name="perDate">perDate</param>
        /// <returns>Einstellungen</returns>
        public NeuwertSettingsDto getAktwertSettings(long sysobtyp, DateTime perDate)
        {
            NeuwertSettingsDto rval = new NeuwertSettingsDto();

            using (DdCtExtended ctx = new DdCtExtended())
            {
                DbParameter[] pars =
                        {
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysobtyp", Value = sysobtyp},
                        };
                //Execute Query
                NeuwertSettingsDBDto vgrw = ctx.ExecuteStoreQuery<NeuwertSettingsDBDto>(AWFLAGGEDINTERNALQUERY, pars).FirstOrDefault();

                if (vgrw == null)
                    throw new Exception("Ungültiger Objekttyp!");
                rval.External = vgrw.External == 1 ? false : true;
                if (!rval.External)
                    rval.sysvgrw = getWertegruppe(sysobtyp,perDate);

                return rval;
            }
        }

        /// <summary>
        /// Ermitteln der Wertegruppe des 
        /// </summary>
        /// <param name="sysobtyp">Objecttyp</param>
        /// <param name="perDate">per Datum</param>
        /// <returns>Einstellungen</returns>
        private long getWertegruppe(long sysobtyp, DateTime perDate)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                
                String dayStr = "to_date('" + perDate.Year + "-" + perDate.Month + "-" + perDate.Day + "', 'yyyy-mm-dd')";
                String validStr = One.Utils.Util.SQLDateUtil.CheckDate(dayStr, "vgvalid");
                return ctx.ExecuteStoreQuery<long>("select sysvgrw from obtyp where sysvgrw != 0 and exists(select vg.sysvg from vg,vgvalid where vg.sysvg = sysvgrw and vgvalid.sysvg=vg.sysvg and " + validStr + ") start with sysobtyp = " + sysobtyp + " connect by prior sysobtypp = sysobtyp").FirstOrDefault();

                
            }
        }

        /// <summary>
        /// Ermitteln der Wertegruppe des 
        /// </summary>
        /// <param name="sysobtyp">Objecttyp</param>
        /// <param name="perDate">per Datum</param>
        /// <returns>Einstellungen</returns>
        private long getWertegruppe2(long sysobtyp, DateTime perDate)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                String dayStr = "to_date('" + perDate.Year + "-" + perDate.Month + "-" + perDate.Day + "', 'yyyy-mm-dd')";
                String validStr = One.Utils.Util.SQLDateUtil.CheckDate(dayStr, "vgvalid");
                return ctx.ExecuteStoreQuery<long>("select sysvgrw2 from obtyp where sysvgrw2 != 0 and exists(select vg.sysvg from vg,vgvalid where vg.sysvg = sysvgrw2 and vgvalid.sysvg=vg.sysvg and " + validStr + ") start with sysobtyp = " + sysobtyp + " connect by prior sysobtypp = sysobtyp").FirstOrDefault();
            }
        }

        /// <summary>
        /// Ermitteln der Wertegruppe des Antrags
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        private VG getSysVGByAntrag(long sysantrag)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                DbParameter[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = sysantrag }, };

                //Execute Query
                return ctx.ExecuteStoreQuery<VG>(VGQUERY, pars).FirstOrDefault();
            }
        }

        /// <summary>
        /// Ermitteln des Restwertes eines Fahrzeugs
        /// </summary>
        /// <param name="input">EingabeDaten</param>
        /// <returns>Restwert in Prozent</returns>
        public double evaluateFzRestwert(RestwertRequestDto input)
        {
            double retval = 0.0;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                OBTYPFZADD Data = (from obtypfzadd in ctx.OBTYPFZADD
                                   where obtypfzadd.SYSOBTYP == input.sysobtyp
                                   select obtypfzadd).FirstOrDefault();

                ANGOB Item = Data.OBTYP.ANGOBList.FirstOrDefault();

                return retval;
            }
        }

        /// <summary>
        /// Ermitteln des Neuwerts eines Fahrzeugs.
        /// </summary>
        /// <param name="sysobtyp">Objecttyp</param>
        /// <returns>Netto Neupreis Betrag</returns>
        public double evaluateFzNeupreis(long sysobtyp)
        {
            IAngAntDao angAntDao = CommonDaoFactory.getInstance().getAngAntDao();
            AngAntObDto Data = angAntDao.getObjektdaten(sysobtyp);
            return Data.grundBrutto;
        }

        /// <summary>
        /// returns true, if the obtyp is a root-vehicle-tree id
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public bool isLevelOne(long sysobtyp)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                DbParameter[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp }, };

                //Execute Query
                long found = ctx.ExecuteStoreQuery<long>(LEVELONEQUERY, pars).FirstOrDefault();

                return found > 0;
            }
        }


        /// <summary>
        /// getELInDtoAusDB
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        public ELInDto getELInDtoAusAntrag(long sysantrag, AngAntKalkDto angAntKalkDto)
        {
            //--vt-Tabelle wird für Mapping zum Tilgungsverlauf zum Antrag benötigt (neu sollte der Buchwertverlauf ebenfalls in der antobslpos-Tabelle aufgeführt werden --> siehe CR)
            const String QUERYANTRAGOBJEKTDATEN = "  with Leasing_Antrag as ( " +
                                                  "  select antrag.antrag, antrag.sysid, antrag.sysantrag,  antrag.sysvart, ANTRAG.SYSMWST sysmwst, decode(antrag.beginn,to_date('01.01.0111','DD.MM.YY'),sysdate,beginn) Beginn, sysVM " +
                                                  "  from antrag   " +
                                                  "  where   " +
                                                  // "  ((antrag.sysvart in(3,5,8)  and antrag.sysprchannel = 1) OR antrag.sysvart = 1) and "+
                                                  "   antrag.sysid = :sysAntrag  " +
                                                  "  )  " +
                                                  "  , Fhz_Objekt as ( " +
                                                  "  select Leasing_Antrag.antrag,  Leasing_Antrag.sysVM, ANTOB.ZUBEHOERBRUTTO zubehoer, ANTOB.HERSTELLER Marke, ANTOB.FABRIKAT Modell, decode(ANTOB.ERSTZULASSUNG,to_date('01.01.0111','DD.MM.YY'),sysdate,ANTOB.ERSTZULASSUNG) Erstzulassung, ANTOB.SYSOBTYP sysobtyp, ANTOB.UBNAHMEKM kmStand, ANTOB.JAHRESKM jahreskm, ANTOB.SYSOB sysob, ANTOB.SYSOBART sysobart, ANTOB.AHKBRUTTO ahkBrutto " +
                                                  "  from Leasing_Antrag, antob " +
                                                  "  where Leasing_Antrag.sysid = antob.sysantrag " +
                                                  "  )  " +
                                                  "  select   Leasing_Antrag.antrag antrag, Leasing_Antrag.beginn beginn, Leasing_Antrag.sysVM, Fhz_Objekt.ERSTZULASSUNG Erstzulassung, Fhz_Objekt.zubehoer zubehoer, " +
                                                  "  CASE " +
                                                  "  WHEN to_date(Fhz_Objekt.ERSTZULASSUNG,'dd.MM.yy') <= add_months(to_date(sysdate, 'dd.MM.yy'), -6) AND Fhz_Objekt.SYSOBART=12 " +
                                                  "  THEN round(months_between (to_date(sysdate,'dd.MM.yy'), add_months(to_date(sysdate, 'dd.MM.yy'), -1))) " +
                                                  "  ELSE ceil(months_between (to_date(sysdate,'dd.MM.yy'), to_date(Fhz_Objekt.ERSTZULASSUNG,'dd.MM.yy'))) " +
                                                  "  END alter_Fhz_Mt, " +
                                                  "  Fhz_Objekt.sysobtyp, Fhz_Objekt.sysob, Fhz_Objekt.ahkBrutto, Fhz_Objekt.sysobart, Leasing_Antrag.antrag, Leasing_Antrag.sysid, Leasing_Antrag.sysvart  " +
                                                  "  from Leasing_Antrag,  Fhz_Objekt " +
                                                  "  where " +
                                                  "  Leasing_Antrag.antrag = Fhz_Objekt.antrag  ";


            const String QUERYSCOREBEZEICHNUNG = " SELECT SCOREBEZEICHNUNG  scorebezeichnung " +
                                              " FROM risikokl rk, " +
                                              " deoutexec dx, " +
                                              " dedetail dd " +
                                              " WHERE dx.SYSDEOUTEXEC = dd.SYSDEOUTEXEC " +
                                              " AND dd.RISIKOKLASSEID =rk.sysrisikokl " +
                                              " AND dd.ANTRAGSTELLER = 1 " +
                                              " AND dx.sysauskunft    = " +
                                              " (SELECT max(auskunft.sysauskunft) " +
                                              " FROM auskunft, " +
                                              " deenvinp, " +
                                              " deinpexec " +
                                              " WHERE auskunft.statusnum         = 0 " +
                                              " AND auskunft.area                  = 'ANTRAG' " +
                                              " AND auskunft.sysauskunfttyp        = 3 " +
                                              " AND deenvinp.flagbonitaetspruefung = 1 " +
                                              " AND auskunft.sysauskunft           =deinpexec.sysauskunft " +
                                              " AND deenvinp.sysdeinpexec          =deinpexec.sysdeinpexec " +
                                              " AND auskunft.sysid                 = :psysid " +
                                              " )";


            const String QUERYSCORETOTAL = " SELECT SCORETOTAL scoretotal " +
                                                " FROM risikokl rk, " +
                                                " deoutexec dx, " +
                                                " dedetail dd " +
                                                " WHERE dx.SYSDEOUTEXEC = dd.SYSDEOUTEXEC " +
                                                " AND dd.RISIKOKLASSEID =rk.sysrisikokl " +
                                                " AND dd.ANTRAGSTELLER = 1 " +
                                                " AND dx.sysauskunft    = " +
                                                " (SELECT max(auskunft.sysauskunft) " +
                                                " FROM auskunft, " +
                                                " deenvinp, " +
                                                " deinpexec " +
                                                " WHERE auskunft.statusnum         = 0 " +
                                                " AND auskunft.area                  = 'ANTRAG' " +
                                                " AND auskunft.sysauskunfttyp        = 3 " +
                                                " AND deenvinp.flagbonitaetspruefung = 1 " +
                                                " AND auskunft.sysauskunft           =deinpexec.sysauskunft " +
                                                " AND deenvinp.sysdeinpexec          =deinpexec.sysdeinpexec " +
                                                " AND auskunft.sysid                 = :psysid " +
                                                " )";


            const String QUERYAUSFALLWVG = "select v.sysvg from vg v,vgtype t where v.sysvgtype=t.sysvgtype and t.NAME='PD'";

            const String QUERYMWST = " select mwst.prozent from mwst where sysmwst in (select sysmwst from antrag where sysid = :psysid) ";


            ELInDto eLIndto = new ELInDto();
            eLIndto.sysid = sysantrag;

            using (DdCtExtended context = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysantrag });

                eLIndto = context.ExecuteStoreQuery<ELInDto>(QUERYANTRAGOBJEKTDATEN, parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = eLIndto.sysid });
                eLIndto.scorebezeichnung = context.ExecuteStoreQuery<String>(QUERYSCOREBEZEICHNUNG, parameters.ToArray()).FirstOrDefault();
                if (eLIndto.scorebezeichnung == null) eLIndto.scorebezeichnung = "FPN"; 

                eLIndto.schwacke = getSchwacke(eLIndto.sysobtyp);

                if (eLIndto.erstzulassung == null || eLIndto.erstzulassung > DateTime.Now)
                    eLIndto.erstzulassung = (DateTime)DateTime.Now;


                VG vg = getSysVGByAntrag(sysantrag);
                if (vg != null)
                {
                    eLIndto.sysvg = vg.SYSVG;
                    eLIndto.vgName = vg.NAME;
                }

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = eLIndto.sysid });
                eLIndto.scorewert = context.ExecuteStoreQuery<double>(QUERYSCORETOTAL, parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = eLIndto.sysid });
                eLIndto.mwst = context.ExecuteStoreQuery<double>(QUERYMWST, parameters.ToArray()).FirstOrDefault();

                parameters.Clear();

                eLIndto.ausfallwvg = context.ExecuteStoreQuery<double>(QUERYAUSFALLWVG, null).FirstOrDefault();


                eLIndto.barkaufpreis = angAntKalkDto.bginternbrutto;
                eLIndto.anzahlung = angAntKalkDto.szBrutto;
                eLIndto.anz_bkp = System.Math.Round(eLIndto.anzahlung / eLIndto.barkaufpreis, 4);
                eLIndto.finanzierungsbetrag = eLIndto.barkaufpreis - angAntKalkDto.szBrutto;
                eLIndto.laufzeit = angAntKalkDto.lz;
                eLIndto.jahresKm = angAntKalkDto.ll;
                eLIndto.rate = angAntKalkDto.rateBrutto;
                eLIndto.restwert = angAntKalkDto.rwBrutto;
                eLIndto.zins = angAntKalkDto.zins;
                eLIndto.zinskosten = angAntKalkDto.calcZinskosten;

                eLIndto.rwFaktor = context.ExecuteStoreQuery<double>(QUERYRWFAKTOR, null).FirstOrDefault();
                eLIndto.mwFaktor = context.ExecuteStoreQuery<double>(QUERYMWFAKTOR, null).FirstOrDefault();

                if (eLIndto.rwFaktor == 0) eLIndto.rwFaktor = RWFAKTOR;
                eLIndto.saveMarktwerteInDb = false;


                if (eLIndto.sysobart == 12)
                {

                    eLIndto.neupreis4DoRemo = eLIndto.ahkBrutto;
                    eLIndto.neupreis4DoRemoDefault = eLIndto.ahkBrutto;
                    eLIndto.neupreis4DoRemoIW = eLIndto.ahkBrutto;
                    eLIndto.neupreis4DoRemoVGREF = eLIndto.ahkBrutto;
                }
                else
                {
                    //BNRSIZE - 1145  LP wird neu immer aus der gleichen Quelle kommen wie die Restwertverläufe.
                    eLIndto.lp_prozentIW = getLp_prozent(sysantrag, "RWVG", angAntKalkDto.sysprproduct,0);
                    eLIndto.lp_prozent = getLp_prozent(sysantrag, "RWVG2", angAntKalkDto.sysprproduct, 0);
                    eLIndto.lp_prozentVGREF = getLp_prozent(sysantrag, "RWVGREF", angAntKalkDto.sysprproduct, 0);
                    eLIndto.neupreis4DoRemo = getNeupreis(sysantrag, eLIndto.lp_prozent, eLIndto, EurotaxSource.EurotaxForecast);
                    eLIndto.neupreis4DoRemoIW = getNeupreis(sysantrag, eLIndto.lp_prozentIW, eLIndto, EurotaxSource.InternalTableRW);
                    eLIndto.neupreis4DoRemoVGREF = getNeupreis(sysantrag, eLIndto.lp_prozentVGREF, eLIndto, EurotaxSource.InternalTableVGREF_RW);
                    eLIndto.neupreis4DoRemoDefault = getNeupreis(sysantrag, eLIndto.lp_prozent, eLIndto, EurotaxSource.InternalTableRemo);



                }


                eLIndto.neupreis = getNeupreisoriginal(sysantrag);

                eLIndto.sysprproduct = angAntKalkDto.sysprproduct;

                eLIndto.sysCreate = DateTime.Now;

                eLIndto.zinscust = angAntKalkDto.zinscust;

                return eLIndto;
            }
        }




        /// <summary>
        /// getELInDtoAusDB
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        public ELInDto getELInDtoAusDB(long sysantrag, long sysprproduct, long sysperole)
        {
            //--vt-Tabelle wird für Mapping zum Tilgungsverlauf zum Antrag benötigt (neu sollte der Buchwertverlauf ebenfalls in der antobslpos-Tabelle aufgeführt werden --> siehe CR)
            const String QUERYANTRAGOBJEKTDATEN = "  with Leasing_Antrag as (  " +
                                   "select antrag.antrag, antrag.sysid, antrag.sysantrag,  antrag.sysvart, ANTRAG.SYSMWST sysmwst, decode(antrag.beginn,to_date('01.01.0111','DD.MM.YY'),sysdate,null,sysdate,beginn) Beginn, sysVM " +
                                   "from antrag    " +
                                   "where   " +
                                   //"((antrag.sysvart in(3,5,8)  and antrag.sysprchannel = 1) OR antrag.sysvart = 1)  and" +
                                   " antrag.sysid = :sysAntrag  " +
                                   ")  " +
                                   ", Fhz_Objekt as (  " +
                                   "select Leasing_Antrag.antrag,  Leasing_Antrag.sysVM, ANTOB.ZUBEHOERBRUTTO zubehoer, ANTOB.HERSTELLER Marke, ANTOB.FABRIKAT Modell, decode(ANTOB.ERSTZULASSUNG,to_date('01.01.0111','DD.MM.YY'),sysdate,null,sysdate,ANTOB.ERSTZULASSUNG) Erstzulassung, ANTOB.SYSOBTYP sysobtyp, ANTOB.UBNAHMEKM kmStand, ANTOB.JAHRESKM jahreskm, ANTOB.SYSOB sysob, ANTOB.SYSOBART sysobart, ANTOB.AHKBRUTTO ahkBrutto " +
                                   "from Leasing_Antrag, antob  " +
                                   "where Leasing_Antrag.sysid = antob.sysantrag " +
                                   ")  " +
                                   "select ANTKALK.zinscust, ANTKALK.syscreate, ANTKALK.sysprproduct sysprproduct, ANTKALK.zinskosten zinskosten, ANTKALK.BGINTERNBRUTTO  Barkaufpreis, antkalk.szbrutto Anzahlung, round((antkalk.szbrutto/ANTKALK.BGINTERNBRUTTO),4) Anz_BKP, (ANTKALK.BGINTERNBRUTTO-antkalk.szbrutto) Finanzierungsbetrag, antkalk.LZ Laufzeit, Fhz_Objekt.jahresKm jahresKm, Fhz_Objekt.kmStand kmStand, antkalk.RATEBRUTTO rate, Leasing_Antrag.antrag antrag, Leasing_Antrag.beginn beginn, Leasing_Antrag.sysVM, Fhz_Objekt.ERSTZULASSUNG Erstzulassung, Fhz_Objekt.zubehoer zubehoer, " +
                                   "CASE " +
                                   "WHEN to_date(Fhz_Objekt.ERSTZULASSUNG,'dd.MM.yy') <= add_months(to_date(sysdate, 'dd.MM.yy'), -6) AND Fhz_Objekt.SYSOBART=12 " +
                                   "THEN round(months_between (to_date(sysdate,'dd.MM.yy'), add_months(to_date(sysdate, 'dd.MM.yy'), -1))) " +
                                   "ELSE ceil(months_between (to_date(sysdate,'dd.MM.yy'), to_date(Fhz_Objekt.ERSTZULASSUNG,'dd.MM.yy'))) " +
                                   "END alter_Fhz_Mt," +
                                   "Fhz_Objekt.sysobtyp, Fhz_Objekt.sysob, Fhz_Objekt.ahkBrutto, Fhz_Objekt.sysobart, Fhz_Objekt.zubehoer zubehoer, " +
                                   "antkalk.syskalk, Leasing_Antrag.antrag, Leasing_Antrag.sysid, Leasing_Antrag.sysvart, (ANTKALK.RWBRUTTO) Restwert, (ANTKALK.ZINS) zins " +
                                   "from Leasing_Antrag, antkalk, Fhz_Objekt " +
                                   "where Leasing_Antrag.sysid = antkalk.syskalk " +
                                   "and  Leasing_Antrag.antrag = Fhz_Objekt.antrag ";


            const String QUERYSCOREBEZEICHNUNG = " SELECT SCOREBEZEICHNUNG  scorebezeichnung " +
                                                " FROM risikokl rk, " +
                                                " deoutexec dx, " +
                                                " dedetail dd " +
                                                " WHERE dx.SYSDEOUTEXEC = dd.SYSDEOUTEXEC " +
                                                " AND dd.RISIKOKLASSEID =rk.sysrisikokl " +
                                                " AND dd.ANTRAGSTELLER = 1 " +
                                                " AND dx.sysauskunft    = " +
                                                " (SELECT max(auskunft.sysauskunft) " +
                                                " FROM auskunft, " +
                                                " deenvinp, " +
                                                " deinpexec " +
                                                " WHERE auskunft.statusnum         = 0 " +
                                                " AND auskunft.area                  = 'ANTRAG' " +
                                                " AND auskunft.sysauskunfttyp        = 3 " +
                                                " AND deenvinp.flagbonitaetspruefung = 1 " +
                                                " AND auskunft.sysauskunft           =deinpexec.sysauskunft " +
                                                " AND deenvinp.sysdeinpexec          =deinpexec.sysdeinpexec " +
                                                " AND auskunft.sysid                 = :psysid " +
                                                " )";


            const String QUERYSCORETOTAL = " SELECT SCORETOTAL scoretotal " +
                                                " FROM risikokl rk, " +
                                                " deoutexec dx, " +
                                                " dedetail dd " +
                                                " WHERE dx.SYSDEOUTEXEC = dd.SYSDEOUTEXEC " +
                                                " AND dd.RISIKOKLASSEID =rk.sysrisikokl " +
                                                " AND dd.ANTRAGSTELLER = 1 " +
                                                " AND dx.sysauskunft    = " +
                                                " (SELECT max(auskunft.sysauskunft) " +
                                                " FROM auskunft, " +
                                                " deenvinp, " +
                                                " deinpexec " +
                                                " WHERE auskunft.statusnum         = 0 " +
                                                " AND auskunft.area                  = 'ANTRAG' " +
                                                " AND auskunft.sysauskunfttyp        = 3 " +
                                                " AND deenvinp.flagbonitaetspruefung = 1 " +
                                                " AND auskunft.sysauskunft           =deinpexec.sysauskunft " +
                                                " AND deenvinp.sysdeinpexec          =deinpexec.sysdeinpexec " +
                                                " AND auskunft.sysid                 = :psysid " +
                                                " )";


            const String QUERYAUSFALLWVG = "select v.sysvg from vg v,vgtype t where v.sysvgtype=t.sysvgtype and t.NAME='PD'";

            const String QUERYMWST = "select mwst.prozent from mwst where sysmwst in (select sysmwst from antrag where sysid = :psysid) ";

            ELInDto eLIndto = new ELInDto();
            using (DdCtExtended context = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysantrag });

                eLIndto = context.ExecuteStoreQuery<ELInDto>(QUERYANTRAGOBJEKTDATEN, parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = eLIndto.sysid });
                eLIndto.scorebezeichnung = context.ExecuteStoreQuery<String>(QUERYSCOREBEZEICHNUNG, parameters.ToArray()).FirstOrDefault();
                if (eLIndto.scorebezeichnung == null) eLIndto.scorebezeichnung = "FPN"; 


                eLIndto.schwacke = getSchwacke(eLIndto.sysobtyp);

                if (eLIndto.erstzulassung == null || eLIndto.erstzulassung > DateTime.Now)
                    eLIndto.erstzulassung = (DateTime)DateTime.Now;




                VG vg = getSysVGByAntrag(sysantrag);
                if (vg != null)
                {
                    eLIndto.sysvg = vg.SYSVG;
                    eLIndto.vgName = vg.NAME;
                }

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = eLIndto.sysid });
                eLIndto.scorewert = context.ExecuteStoreQuery<double>(QUERYSCORETOTAL, parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = eLIndto.sysid });
                eLIndto.mwst = context.ExecuteStoreQuery<double>(QUERYMWST, parameters.ToArray()).FirstOrDefault();

                parameters.Clear();

                eLIndto.ausfallwvg = context.ExecuteStoreQuery<double>(QUERYAUSFALLWVG, null).FirstOrDefault();

                eLIndto.rwFaktor = context.ExecuteStoreQuery<double>(QUERYRWFAKTOR, null).FirstOrDefault();
                eLIndto.mwFaktor = context.ExecuteStoreQuery<double>(QUERYMWFAKTOR, null).FirstOrDefault();

                if (eLIndto.rwFaktor == 0) eLIndto.rwFaktor = RWFAKTOR;
                eLIndto.saveMarktwerteInDb = false;

                if (eLIndto.sysobart == 12)
                {

                    eLIndto.neupreis4DoRemo = eLIndto.ahkBrutto;
                    eLIndto.neupreis4DoRemoDefault = eLIndto.ahkBrutto;
                    eLIndto.neupreis4DoRemoIW = eLIndto.ahkBrutto;
                    eLIndto.neupreis4DoRemoVGREF = eLIndto.ahkBrutto;
                }
                else
                {
                    //BNRSIZE - 1145  LP wird neu immer aus der gleichen Quelle kommen wie die Restwertverläufe.
                    eLIndto.lp_prozentIW = getLp_prozent(sysantrag, "RWVG",sysprproduct,sysperole);
                    eLIndto.lp_prozentVGREF = getLp_prozent(sysantrag, "RWVGREF", sysprproduct, sysperole);
                    eLIndto.lp_prozent = getLp_prozent(sysantrag, "RWVG2", sysprproduct, sysperole);
                    eLIndto.neupreis4DoRemo = getNeupreis(sysantrag, eLIndto.lp_prozent, eLIndto, EurotaxSource.EurotaxForecast);
                    eLIndto.neupreis4DoRemoIW = getNeupreis(sysantrag, eLIndto.lp_prozentIW, eLIndto, EurotaxSource.InternalTableRW);
                    eLIndto.neupreis4DoRemoVGREF = getNeupreis(sysantrag, eLIndto.lp_prozentVGREF, eLIndto, EurotaxSource.InternalTableVGREF_RW );
                    eLIndto.neupreis4DoRemoDefault = getNeupreis(sysantrag, eLIndto.lp_prozent, eLIndto, EurotaxSource.InternalTableRemo);



                }




                eLIndto.neupreis = getNeupreisoriginal(sysantrag);

                return eLIndto;
            }
        }

        public double getRwFaktor()
        {
            double rwfaktor = 0;
            using (DdCtExtended context = new DdCtExtended())
            {
                rwfaktor = context.ExecuteStoreQuery<double>(QUERYRWFAKTOR, null).FirstOrDefault();
            }

            return rwfaktor;
        }


        /// <summary>
        /// Ermitteln der Objekttypdaten eines Fahrzeugs nach Nationalem Fahrzeugcode
        /// </summary>
        /// <param name="nationalVC">Nationaler Fahrzeugcode</param>
        /// <returns>Daten</returns>
        public ObtypDataRestwertDto getObTypDataByNVCByString(string nationalVC)
        {
            string QUERYSCHWACKEOBJTYPDATA = "SELECT sysobtyp FROM obtyp o where o.schwacke = :nationalVC order by sysobtyp asc";
            ObtypDataRestwertDto retval = new ObtypDataRestwertDto();
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "nationalVC", Value = nationalVC });
                retval.sysobtyp = ctx.ExecuteStoreQuery<long>(QUERYSCHWACKEOBJTYPDATA, parameters.ToArray()).FirstOrDefault();
                return retval;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="rwvg"></param>
        /// <returns></returns>
        private decimal getLp_prozent(long sysantrag, string rwvg, long sysprproduct, long sysperole)
        {
            const String QUERYDEFAULT_RWVG = "select sysvgrw from (select level lev, sysvgrw from obtyp where sysvgrw >0  start with sysobtyp=:sysobtyp connect by prior sysobtypp = sysobtyp order by level asc) where rownum  =1";

            const String QUERYDEFAULT_RWVG2 = "select sysvgrw2 from (select level lev, sysvgrw2 from obtyp where sysvgrw2 >0  start with sysobtyp=:sysobtyp connect by prior sysobtypp = sysobtyp order by level asc) where rownum  =1";
            const String QUERYLP_PROZENT = "select cic.CIC_COMMON_UTILS.CICVALUE2(SYSDATE, :DEFAULT_RWVG, :FZ_ALTER,  :ANTOB_UBNAHMEKM ,1,'','') from dual";
            const String QUERY_FHZALTER_4REMO = "select greatest (0,ceil(months_between(sysdate,decode(nvl(cic.cic_sys.to_cladate(ERSTZULASSUNG),0),0,SYSDATE,ERSTZULASSUNG)))) FZ_ALTER from ANTOB where sysantrag = :psysantrag";
            string queryRwvg = QUERYDEFAULT_RWVG2;
            
            if ("RWVGREF".Equals(rwvg))
            {
               

                using (DdOlExtended ctx = new DdOlExtended())
                {
                    //ATTENTION - misuses sysprjoker für ubnahmekm to avoid declaring a new container-dto for this query, sysprjoker wont be needed for autoassignprhgroup!
                    prKontextDto prodKontext = ctx.ExecuteStoreQuery<prKontextDto>("select antrag.sysprproduct,antrag.sysbrand,antob.ubnahmekm sysprjoker,perole.sysperole,antob.sysobtyp from antrag,perole,antob where antrag.sysvk=perole.sysperson and antob.sysantrag=antrag.sysid and antrag.sysid=" + sysantrag).FirstOrDefault();
                    IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                    BankNowCalculator.autoAssignPrhgroup(prodKontext, obDao, PrismaDaoFactory.getInstance().getPrismaDao());
                    RestWertSettingsDto rwSettings = getSysVGForVGREFType(VGRefType.RW, prodKontext.sysobtyp, prodKontext.sysbrand, prodKontext.sysprhgroup, CfgDate.verifyPerDate(null));
                    if (rwSettings.External)
                        return 0;


                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysantrag", Value = sysantrag });
                    int fz_alter = ctx.ExecuteStoreQuery<int>(QUERY_FHZALTER_4REMO, parameters.ToArray()).FirstOrDefault();

                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "DEFAULT_RWVG", Value = rwSettings.sysvgrw });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "FZ_ALTER", Value = fz_alter });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ANTOB_UBNAHMEKM", Value = prodKontext.sysprjoker});
                    decimal lp_prozent = ctx.ExecuteStoreQuery<decimal>(QUERYLP_PROZENT, parameters.ToArray()).FirstOrDefault();



                    return lp_prozent;


                }
            }


            if (rwvg == "RWVG")
            {
                queryRwvg = QUERYDEFAULT_RWVG;
            }
            using (DdOlExtended ctx = new DdOlExtended())
            {
                int fz_alter = 0;
                ANTOB antOb = (from a in ctx.ANTOB
                               where a.SYSANTRAG == sysantrag
                               select a).FirstOrDefault();
                if (antOb.OBTYP == null)
                    ctx.Entry(antOb).Reference(f => f.OBTYP).Load();

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = antOb.OBTYP.SYSOBTYP });
                double default_rwvg = ctx.ExecuteStoreQuery<double>(queryRwvg, parameters.ToArray()).FirstOrDefault();


                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysantrag", Value = sysantrag });
                fz_alter = ctx.ExecuteStoreQuery<int>(QUERY_FHZALTER_4REMO, parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "DEFAULT_RWVG", Value = default_rwvg });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "FZ_ALTER", Value = fz_alter });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ANTOB_UBNAHMEKM", Value = antOb.UBNAHMEKM });
                decimal lp_prozent = ctx.ExecuteStoreQuery<decimal>(QUERYLP_PROZENT, parameters.ToArray()).FirstOrDefault();



                return lp_prozent;


            }


        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="lp_prozent"></param>
        /// <param name="elindto"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private double getNeupreis(long sysantrag, decimal lp_prozent, ELInDto elindto, EurotaxSource source)
        {
            double neupreis = 0;
            decimal? listenpreis = 0;

            using (DdOlExtended ctx = new DdOlExtended())
            {

                ANTOB antOb = (from a in ctx.ANTOB
                               where a.SYSANTRAG == sysantrag
                               select a).FirstOrDefault();
                


                //BNRSIZE - 1145  LP wird neu immer aus der gleichen Quelle kommen wie die Restwertverläufe.
                if (antOb.LPBRUTTO.GetValueOrDefault() == 0 || source == EurotaxSource.InternalTableRW || source == EurotaxSource.InternalTableRemo||source == EurotaxSource.InternalTableVGREF_RW)
                {


                    listenpreis = lp_prozent * (antOb.GRUNDBRUTTO + antOb.ZUBEHOERBRUTTO.GetValueOrDefault()) / 100;
                    if (source == EurotaxSource.InternalTableRW)
                    {
                        elindto.listenpreis4DoRemoIW = (double)listenpreis;
                    }
                    else if (source == EurotaxSource.InternalTableVGREF_RW)
                    {
                        elindto.listenpreis4DoRemoVGREF = (double)listenpreis;
                    }
                    else if (source == EurotaxSource.InternalTableRemo)
                        elindto.listenpreis4DoRemoDefault = (double)listenpreis;

                    if (antOb.LPBRUTTO == 0)
                    {
                        elindto.listenpreis4DoRemo = (double)listenpreis;

                    }

                }

                else
                {
                    listenpreis = (antOb.GRUNDBRUTTO + antOb.ZUBEHOERBRUTTO.GetValueOrDefault()) * antOb.LPBRUTTO / antOb.GRUNDBRUTTO;
                    elindto.listenpreis4DoRemo = (double)listenpreis;

                }
                if (listenpreis > 0)
                {

                    neupreis = (double)((antOb.GRUNDBRUTTO + antOb.ZUBEHOERBRUTTO.GetValueOrDefault()) * antOb.AHKBRUTTO / listenpreis);

                }



                return neupreis;


            }

        }

        /// <summary>
        /// getNeupreisoriginal
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        private double getNeupreisoriginal(long sysantrag)
        {
            double neupreis = 0;

            using (DdOlExtended ctx = new DdOlExtended())
            {
                ANTOB antOb = (from a in ctx.ANTOB
                               where a.SYSANTRAG == sysantrag
                               select a).FirstOrDefault();

                if (antOb.OBTYP == null)
                    ctx.Entry(antOb).Reference(f => f.OBTYP).Load();
                double zubehoerbrutto = (antOb.ZUBEHOERBRUTTO == null) ? 0 : (double)antOb.ZUBEHOERBRUTTO;
                double grundbrutto = (antOb.GRUNDBRUTTO == null) ? 0 : (double)antOb.GRUNDBRUTTO;
                neupreis = grundbrutto + zubehoerbrutto;
            }

            return neupreis;
        }


        private string getSchwacke(long sysobtyp)
        {
            string schwacke = "";
            const string QUERYSCHWACKE = "select schwacke from obtyp where sysobtyp = :psysobtyp";
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysobtyp", Value = sysobtyp });

                schwacke = ctx.ExecuteStoreQuery<string>(QUERYSCHWACKE, parameters.ToArray()).FirstOrDefault();

            }
            return schwacke;
        }

        public EurotaxLoginDataDto GetEurotaxAccessData(string bez)
        {
            return new OpenOne.Common.DAO.Auskunft.AuskunftCfgDao().GetEurotaxAccessData(bez);
        }

        /// <summary>
        /// Returns the vgref value group for the given context
        /// </summary>
        /// <param name="vgtype"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="sysbrand"></param>
        /// <param name="sysprhgroup"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public RestWertSettingsDto getSysVGForVGREFType(VGRefType vgtype, long sysobtyp, long sysbrand, long sysprhgroup, DateTime perDate)
        {
            RestWertSettingsDto rval = new RestWertSettingsDto();

            using (DdCtExtended context = new DdCtExtended())
            {

                if(sysbrand==0 && sysprhgroup>0)
                {
                    //auto-determine-sysbrand
                    sysbrand = context.ExecuteStoreQuery<long>("select prbrandm.sysbrand from prbrandm, brand where prbrandm.sysbrand=brand.sysbrand and prbrandm.sysprhgroup=" + sysprhgroup).FirstOrDefault();
                }

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysbrand", Value = sysbrand });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprhgroup", Value = sysprhgroup });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vgreftype", Value = vgtype.ToString() });
                rval.sysvgrw = context.ExecuteStoreQuery<long>(QUERYVGREF, parameters.ToArray()).FirstOrDefault();
                rval.External = rval.sysvgrw > 0 ? false : true;
                rval.sysbrand = sysbrand;
                return rval;
            }


        }
    }

}