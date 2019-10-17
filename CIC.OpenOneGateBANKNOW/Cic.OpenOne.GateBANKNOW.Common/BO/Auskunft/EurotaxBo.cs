using AutoMapper;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Eurotax Business Object
    /// </summary>
    [System.CLSCompliant(false)]
    public class EurotaxBo : AbstractEurotaxBo
    {
        DAO.Auskunft.EurotaxRef.GetForecastRequest1 request1;
        DAO.Auskunft.EurotaxValuationRef.GetValuationRequest request;

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected IMwStDao MwStDao = OpenOne.Common.DAO.CommonDaoFactory.getInstance().getMwStDao();


        IRounding round = RoundingFactory.createRounding();

        long callCounter = 0;

        private const long codeTechExc = -2;
        private const long codeSerAufExc = -1;
        private const String KORRTYP_RW = "RW_KORR";

        /// <summary>
        /// Constructor for EurotaxBo
        /// </summary>
        /// <param name="eurotaxwsdao">Eurotax Webservice DAO</param>
        /// <param name="eurotaxdbdao">Eurotax Datenbank DAO</param>
        /// <param name="auskunftdao">Auskunft DAO</param>
        /// <param name="vgDao">Wertegruppen DAO</param>
        /// <param name="obtypDao">Objekttyp DAO</param>
        public EurotaxBo(IEurotaxWSDao eurotaxwsdao, IEurotaxDBDao eurotaxdbdao, IAuskunftDao auskunftdao, IVGDao vgDao, IObTypDao obtypDao)
            : base(eurotaxwsdao, eurotaxdbdao, auskunftdao, vgDao, obtypDao)
        {
        }

        private bool isBlockedCall()
        {
            String blocked = AppConfig.getValueFromDb("SETUP.NET", "EUROTAX", "BLOCKED");
            bool blockCall = false;
            if (blocked != null)
            {
                blocked = blocked.ToUpper();
                blockCall = "TRUE".Equals(blocked);
            }
            return blockCall;
        }

        public List<EurotaxOutDto> GetForecastHCE(EurotaxInDto inDto)
        {
            ObtypDataRestwertDto input = null;
            
            input = this.eurotaxDBDao.getObTypDataByNVCByString(inDto.NationalVehicleCode.ToString());
            input.Schwacke = inDto.NationalVehicleCode;

            inDto.sysobtyp = input.sysobtyp;
            

            RestWertSettingsDto rwSettings = eurotaxDBDao.getRestwertSettings(input.sysobtyp, inDto.prodKontext != null ? inDto.prodKontext.perDate : CfgDate.verifyPerDate(null));
            double Neupreis = 0;
            double RestwertPercent = 0;

            Int32 periodeVon = 0;
            Int32 periodeBis = 0;
            Int32.TryParse(inDto.ForecastPeriodFrom, out periodeVon);
            Int32.TryParse(inDto.ForecastPeriodUntil, out periodeBis);

            // if only PeriodeVon is filled, then fetch only one Forecast 
            if (periodeVon > periodeBis)
            {
                periodeBis = periodeVon;
            }

            try
            {
                Neupreis = evaluateNeupreis(input.sysobtyp) + inDto.TotalListPriceOfEquipment;
            }
            catch (Exception ex)
            {
                //sonderfall: wenn sysobtyp von ebene 1(vc_obtyp1), dann fehler ignorieren (neupreis=0, sonst werfen)
                if (!isLevelOne(input.sysobtyp))
                {
                    throw ex;
                }
            }

            DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
            long Years = aktuell.Year - inDto.RegistrationDate.Year;
            long Months = aktuell.Month - inDto.RegistrationDate.Month;
            long Laufzeit = (Years * 12 + Months);

            List<EurotaxOutDto> outDtoList = new List<EurotaxOutDto>();
            IKorrekturBo korr = BOFactory.getInstance().createKorrekturBo();
            List<long> obtypascendants = this.obtypDao.getObTypAscendants(input.sysobtyp);
            obtypascendants.Reverse();
            bool doRWTAB = false;

            try
            {
                request1 = new GetForecastRequest1();
                request1.ETGHeader = MyGetHeaderType(inDto);
                request1.Settings = MyGetSettingType(inDto);

                if (isBlockedCall())
                {
                    callCounter = 0;
                    getBlockedEurotaxForecast(inDto, request1, outDtoList, periodeVon, periodeBis, Laufzeit);
                    _log.Info("getBlockedEurotaxForecast hat " + callCounter + " mal den Eurotax-Webservice aufgerufen.");

                    var t = from o in outDtoList
                            orderby o.ForecastPeriod
                            select o;
                    outDtoList = t.ToList();
                }
                else
                {
                    for (Int32 counter = periodeVon; counter <= periodeBis; counter++)
                    {
                        inDto.ForecastPeriodFrom = counter.ToString();

                        request1 = new GetForecastRequest1();
                        request1.ETGHeader = MyGetHeaderType(inDto);
                        request1.Settings = MyGetSettingType(inDto);
                        request1.Vehicle = MyGetVehicleType(inDto);
                        eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(inDto.sysobtyp, "EurotaxGetForecast"));
                        eurotaxWSDao.getForecast(ref request1, inDto.sysobtyp);
                        EurotaxOutDto outDto = MyMapVehicleToOutDto(request1.Vehicle, request1.ETGHeader);
                        outDtoList.Add(outDto);


                    }
                }
                //CR 24777
                foreach (EurotaxOutDto outDto in outDtoList)
                {
                    //BR10 / BNRZEHN-1669
                    if (obtypascendants.Count > 0)
                    {
                        if (obtypascendants.Count == 1)
                        {
                            outDto.RetailValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.RetailValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[0]);
                            outDto.TradeValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.TradeValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[0]);
                        }
                        else
                        {
                            outDto.RetailValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.RetailValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1]);
                            outDto.TradeValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.TradeValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1]);
                        }
                    }

                    outDto.RetailAmount = Math.Round(Neupreis * (outDto.RetailValueInPercentage / 100), 2);
                    outDto.TradeAmount = Math.Round(Neupreis * (outDto.TradeValueInPercentage / 100), 2);
                    outDto.source = EurotaxSource.EurotaxForecast;
                }
            }
            catch (Exception)
            {
                doRWTAB = true;
            }
            if(outDtoList.Count==0)
            {
                doRWTAB = true;
            }
            if (rwSettings.sysvgrw > 0&&doRWTAB)
                {
                    for (Int32 counter = periodeVon; counter <= periodeBis; counter++)
                    {
                        inDto.ForecastPeriodFrom = counter.ToString();

                        request1 = new GetForecastRequest1();
                        request1.ETGHeader = MyGetHeaderType(inDto);
                        request1.Settings = MyGetSettingType(inDto);
                        request1.Vehicle = MyGetVehicleType(inDto);

                        RestwertRequestDto Data = new RestwertRequestDto();
                        Data.Laufleistung = ((int)(inDto.CurrentMileageValue + (inDto.EstimatedAnnualMileageValue * (counter / 12.0)))).ToString();
                        Data.Laufzeit = (counter + Laufzeit).ToString();
                        Data.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                        Data.sysobtyp = input.sysobtyp;
                        RestwertPercent = evaluateRestwert(Data, rwSettings.sysvgrw);
                        //CR 24777 Korrektur RestwertProzent
                        //BR10 /     BNRZEHN-1669
                        if (obtypascendants.Count > 0)
                        {
                            if (obtypascendants.Count == 1)
                            {
                                RestwertPercent = korr.Correct(KORRTYP_RW, (decimal)RestwertPercent, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[0]);
                            }
                            else
                            {
                                RestwertPercent = korr.Correct(KORRTYP_RW, (decimal)RestwertPercent, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1]);
                            }
                        }

                        request1.Vehicle.ForecastData[0] = new ETGforecastDataType();
                        request1.Vehicle.ForecastData[0].ForecastedValue = new ETGforecastedValueType();
                        request1.Vehicle.ForecastData[0].Item = counter.ToString();
                        request1.Vehicle.ForecastData[0].ForecastedValue.RetailAmount = Math.Round(Neupreis * (RestwertPercent / 100), 2);
                        request1.Vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentage = Math.Round(RestwertPercent, 2);
                        request1.Vehicle.ForecastData[0].ForecastedValue.TradeAmount = Math.Round(Neupreis * (RestwertPercent / 100), 2);
                        request1.Vehicle.ForecastData[0].ForecastedValue.TradeValueInPercentage = Math.Round(RestwertPercent, 2);
                        request1.Vehicle.ForecastData[0].ForecastedValue.RetailAmountSpecified = true;
                        request1.Vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentageSpecified = true;
                        request1.Vehicle.ForecastData[0].ForecastedValue.TradeAmountSpecified = true;
                        request1.Vehicle.ForecastData[0].ForecastedValue.TradeValueInPercentageSpecified = true;

                        EurotaxOutDto outDto = MyMapVehicleToOutDto(request1.Vehicle, request1.ETGHeader);
                        outDto.source = EurotaxSource.InternalTableRW;
                        outDtoList.Add(outDto);
                    }
                
            }
            return outDtoList;
        }

       
        /// <summary>
        /// Gets input by filled inDto, saves input, calls GetForecast WS and saves output
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override List<EurotaxOutDto> GetForecast(EurotaxInDto inDto)
        {
            if(inDto.ISOCountryCode==ISOcountryType.DE)
            {
                return GetForecastHCE(inDto);
            }
            ObtypDataRestwertDto input = null;
            if (inDto.sysobtyp > 0)
            {
                try
                {
                    input = this.obtypDao.getObTypData(inDto.sysobtyp);
                    inDto.NationalVehicleCode = input.Schwacke;
                }
                catch(Exception)
                {
                    input = new ObtypDataRestwertDto();
                    input.sysobtyp = inDto.sysobtyp;
                }
                
            }
            else
            {
                //BR10
                input = this.eurotaxDBDao.getObTypDataByNVCByString(inDto.NationalVehicleCode.ToString());
                if (input.sysobtyp > 0)
                {
                    input.Schwacke = inDto.NationalVehicleCode;
                }

                inDto.sysobtyp = input.sysobtyp;
            }
            EurotaxSource internalSource = EurotaxSource.InternalTableVGREF_RW;
            if (inDto.prodKontext == null)
                inDto.prodKontext = new prKontextDto();
            BankNowCalculator.autoAssignPrhgroup(inDto.prodKontext, obtypDao, PrismaDaoFactory.getInstance().getPrismaDao());
            RestWertSettingsDto rwSettings = eurotaxDBDao.getSysVGForVGREFType(VGRefType.RW, input.sysobtyp, inDto.prodKontext.sysbrand, inDto.prodKontext.sysprhgroup, inDto.prodKontext.perDate);
            inDto.prodKontext.sysbrand = rwSettings.sysbrand; //autoassignment of brand if not given
            if (rwSettings.External)//no vgreftable found
            {
                internalSource = EurotaxSource.InternalTableRW;
                rwSettings = eurotaxDBDao.getRestwertSettings(input.sysobtyp, inDto.prodKontext != null ? inDto.prodKontext.perDate : CfgDate.verifyPerDate(null));
                if(rwSettings.External)//no vg table found
                {
                    internalSource = EurotaxSource.EurotaxForecast;
                }
                
            }
            double Neupreis = 0;
            double RestwertPercent = 0;

            Int32 periodeVon = 0;
            Int32 periodeBis = 0;
            Int32.TryParse(inDto.ForecastPeriodFrom, out periodeVon);
            Int32.TryParse(inDto.ForecastPeriodUntil, out periodeBis);

            // if only PeriodeVon is filled, then fetch only one Forecast 
            if (periodeVon > periodeBis)
            {
                periodeBis = periodeVon;
            }

            try
            {
                Neupreis = evaluateNeupreis(input.sysobtyp) + inDto.TotalListPriceOfEquipment;
            }
            catch (Exception ex)
            {
                //sonderfall: wenn sysobtyp von ebene 1(vc_obtyp1), dann fehler ignorieren (neupreis=0, sonst werfen)
                if (!isLevelOne(input.sysobtyp))
                {
                    throw ex;
                }
            }
            
            DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
            long Years = aktuell.Year - inDto.RegistrationDate.Year;
            long Months = aktuell.Month - inDto.RegistrationDate.Month;
            long Laufzeit = (Years * 12 + Months);

            _log.Debug("GetForecast: sysobtyp:" + input.sysobtyp + " using sysvg:" + rwSettings.sysvgrw + " Laufzeit:" + Laufzeit);

            List<EurotaxOutDto> outDtoList = new List<EurotaxOutDto>();
            IKorrekturBo korr = BOFactory.getInstance().createKorrekturBo();
            List<long> obtypascendants = this.obtypDao.getObTypAscendants(input.sysobtyp);
            obtypascendants.Reverse();
            if (!rwSettings.External)
            {
                for (Int32 counter = periodeVon; counter <= periodeBis; counter++)
                {
                    inDto.ForecastPeriodFrom = counter.ToString();

                    request1 = new GetForecastRequest1();
                    request1.Vehicle = MyGetVehicleType(inDto);

                    RestwertRequestDto Data = new RestwertRequestDto();
                    Data.Laufleistung = ((int)(inDto.CurrentMileageValue + (inDto.EstimatedAnnualMileageValue * (counter / 12.0)))).ToString();
                    Data.Laufzeit = (counter + Laufzeit).ToString();
                    Data.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                    Data.sysobtyp = input.sysobtyp;
                    RestwertPercent = evaluateRestwert(Data, rwSettings.sysvgrw);
                    //CR 24777 Korrektur RestwertProzent
                    //BR10 /     BNRZEHN-1669
                    if (obtypascendants.Count > 0)
                    {
                        if (obtypascendants.Count == 1)
                        {
                            RestwertPercent = korr.Correct(KORRTYP_RW, (decimal)RestwertPercent, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[0], ""+inDto.prodKontext.sysbrand, ""+inDto.prodKontext.sysprhgroup, internalSource.ToString(),"internal");
                        }
                        else
                        {
                            RestwertPercent = korr.Correct(KORRTYP_RW, (decimal)RestwertPercent, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1], ""+inDto.prodKontext.sysbrand, ""+inDto.prodKontext.sysprhgroup, internalSource.ToString(), "internal");
                        }
                    }

                    request1.Vehicle.ForecastData[0] = new ETGforecastDataType();
                    request1.Vehicle.ForecastData[0].ForecastedValue = new ETGforecastedValueType();
                    request1.Vehicle.ForecastData[0].Item = counter.ToString();
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailAmount = Math.Round(Neupreis * (RestwertPercent / 100), 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentage = Math.Round(RestwertPercent, 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeAmount = Math.Round(Neupreis * (RestwertPercent / 100), 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeValueInPercentage = Math.Round(RestwertPercent, 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailAmountSpecified = true;
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentageSpecified = true;
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeAmountSpecified = true;
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeValueInPercentageSpecified = true;

                    EurotaxOutDto outDto = MyMapVehicleToOutDto(request1.Vehicle, null);
                    outDto.source = internalSource;
                    outDtoList.Add(outDto);
                }
            }
            else //External - EUROTAX
            {
                request1 = new GetForecastRequest1();
                request1.ETGHeader = MyGetHeaderType(inDto);
                request1.Settings = MyGetSettingType(inDto);

                if (isBlockedCall())
                {
                    callCounter = 0;
                    getBlockedEurotaxForecast(inDto, request1, outDtoList, periodeVon, periodeBis, Laufzeit);
                    _log.Info("getBlockedEurotaxForecast hat " + callCounter + " mal den Eurotax-Webservice aufgerufen.");

                    var t = from o in outDtoList
                            orderby o.ForecastPeriod
                            select o;
                    outDtoList = t.ToList();
                }
                else
                {
                    for (Int32 counter = periodeVon; counter <= periodeBis; counter++)
                    {
                        inDto.ForecastPeriodFrom = counter.ToString();

                        request1 = new GetForecastRequest1();
                        request1.ETGHeader = MyGetHeaderType(inDto);
                        request1.Settings = MyGetSettingType(inDto);
                        request1.Vehicle = MyGetVehicleType(inDto);
                        eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(inDto.sysobtyp, "EurotaxGetForecast"));
                        eurotaxWSDao.getForecast(ref request1, inDto.sysobtyp);
                        EurotaxOutDto outDto = MyMapVehicleToOutDto(request1.Vehicle, request1.ETGHeader);
                        outDtoList.Add(outDto);
                    }
                }
                //CR 24777
                foreach (EurotaxOutDto outDto in outDtoList)
                {
                    //BR10 / BNRZEHN-1669
                    if (obtypascendants.Count>0)
                    {
                        if (obtypascendants.Count == 1)
                        { 
                            outDto.RetailValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.RetailValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[0], "" + inDto.prodKontext.sysbrand, "" + inDto.prodKontext.sysprhgroup, internalSource.ToString(), "external");
                            outDto.TradeValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.TradeValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[0], "" + inDto.prodKontext.sysbrand, "" + inDto.prodKontext.sysprhgroup, internalSource.ToString(), "external");
                        }
                        else
                        {
                            outDto.RetailValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.RetailValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1], "" + inDto.prodKontext.sysbrand, "" + inDto.prodKontext.sysprhgroup, internalSource.ToString(), "external");
                            outDto.TradeValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.TradeValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1], "" + inDto.prodKontext.sysbrand, "" + inDto.prodKontext.sysprhgroup, internalSource.ToString(), "external");
                        }
                    }

                    outDto.RetailAmount = Math.Round(Neupreis * (outDto.RetailValueInPercentage / 100), 2);
                    outDto.TradeAmount = Math.Round(Neupreis * (outDto.TradeValueInPercentage / 100), 2);
                    outDto.source = EurotaxSource.EurotaxForecast;
                }
            }
            return outDtoList;
        }

        /// <summary>
        /// getBlockedEurotaxForecast
        /// </summary>
        /// <param name="inDto"></param>
        /// <param name="request1"></param>
        /// <param name="outDtoList"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="laufzeit"></param>
        private void getBlockedEurotaxForecast(EurotaxInDto inDto, GetForecastRequest1 request1, List<EurotaxOutDto> outDtoList, int from, int to, long laufzeit)
        {
            _log.Debug("getBlockedEurotaxForecast started for periods " + from + "-" + to);

            if (from == to)
            {
                getBlockedEurotaxForecastFetch(inDto, request1, outDtoList, from, to);
            }
            else
            {
                bool binarySearchNeeded = true;

                // 3 <= FZ-Alter <= 120
                // Annahme: from ist immer kleiner als to
                if ((laufzeit + from) >= 3 && (laufzeit + to) <= 120 && (inDto.CurrentMileageValue + to / 12 * inDto.EstimatedAnnualMileageValue <= 200000))
                {
                    // Wenn getBlockedEurotaxForecastFetch fehlschlägt, dann muss binary search gemacht werden
                    binarySearchNeeded = !getBlockedEurotaxForecastFetch(inDto, request1, outDtoList, from, to);
                }
                if (binarySearchNeeded)
                {
                    int middle = (from + to) / 2;
                    getBlockedEurotaxForecast(inDto, request1, outDtoList, from, middle, laufzeit);
                    getBlockedEurotaxForecast(inDto, request1, outDtoList, middle + 1, to, laufzeit);
                }
            }
        }

        /// <summary>
        /// getBlockedEurotaxForecastFetch
        /// </summary>
        /// <param name="inDto"></param>
        /// <param name="request1"></param>
        /// <param name="outDtoList"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>false, when call to eurotax fails (so binary search may be necessary)</returns>
        private bool getBlockedEurotaxForecastFetch(EurotaxInDto inDto, GetForecastRequest1 request1, List<EurotaxOutDto> outDtoList, int from, int to)
        {
            // blocked call does binary search for valid periods, but doesn't deliver proper error-messages
            // so at this point we once check if the typenr is valid after all
            try
            {
                request1.Vehicle = MyGetVehicleType(inDto, from, to);
                eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(inDto.sysobtyp, "EurotaxGetForecast"));
                eurotaxWSDao.getForecast(ref request1, inDto.sysobtyp);
                callCounter++;
                MyMapVehicleToOutDto(outDtoList, request1.Vehicle, request1.ETGHeader);
            }
            catch (EuroTaxCommException ete)//some error occured, so this block failed
            {
                _log.Info("getBlockedEurotaxForecastFetch EuroTaxCommException: " + ete.Message);
                if (ete.getErrorType() == EuroTaxErrorType.TYPENR_NOT_FOUND)//no typenumber, so blocked search will also fail
                {
                    return true;
                }
                else if(ete.getErrorType() == EuroTaxErrorType.PROCESSING_ERROR)
                {
                    return false;//eurotax-error because blocksize too big, so try blocked calling
                }
            }
            catch (Exception ex)
            {
                _log.Info("getBlockedEurotaxForecastFetch for " + from + "-" + to + " failed: " + ex.Message);
                if (from == to)
                {
                    return true;//no need to perform binary search anymore
                }
                //other error, dont try blocked calling, we might end up in 120 calls with errors!
                //return false;
            }
            _log.Info("getBlockedEurotaxForecastFetch for " + from + "-" + to + " succeeded");
            //success, dont try blocked calling
            return true;
        }

        /// <summary>
        /// Gets input by filled inDto, saves input, calls GetRemo WS and saves output
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override List<EurotaxOutDto> GetRemo(EurotaxInDto inDto)
        {
            // Ticket#2012010910000253 — Hotfix 5785-RW: Anpassung Webservices 
            List<EurotaxOutDto> outDtoList = new List<EurotaxOutDto>();
            //R10
            ObtypDataRestwertDto input = null;
            if (inDto.sysobtyp > 0)
            {
                try
                {
                    input = this.obtypDao.getObTypData(inDto.sysobtyp);
                    inDto.NationalVehicleCode = input.Schwacke;
                }
                catch (Exception)
                {
                    input = new ObtypDataRestwertDto();
                    input.sysobtyp = inDto.sysobtyp;
                }
                
            }
            else
            {
                //BR10
                input = this.eurotaxDBDao.getObTypDataByNVCByString(inDto.NationalVehicleCode.ToString());
                if (input.sysobtyp > 0)
                {
                    input.Schwacke = inDto.NationalVehicleCode;
                }

                inDto.sysobtyp = input.sysobtyp;
            }
            if (inDto.prodKontext == null)
                inDto.prodKontext = new prKontextDto();
            BankNowCalculator.autoAssignPrhgroup(inDto.prodKontext, obtypDao, PrismaDaoFactory.getInstance().getPrismaDao());
            RestWertSettingsDto rwSettings = eurotaxDBDao.getSysVGForVGREFType(VGRefType.RW, input.sysobtyp, inDto.prodKontext.sysbrand, inDto.prodKontext.sysprhgroup, inDto.prodKontext.perDate);
            inDto.prodKontext.sysbrand = rwSettings.sysbrand; //autoassignment of brand if not given
            if (rwSettings.External)//no vgreftable found
                rwSettings = eurotaxDBDao.getRestwertSettings(input.sysobtyp, inDto.prodKontext.perDate);
            _log.Debug("GetRemo: sysobtyp:" + input.sysobtyp + " using sysvg:" + rwSettings.sysvgrw);
            outDtoList = MyGetForecastForRemo(rwSettings, inDto, input.sysobtyp);
            if (outDtoList.Count() == 0)
            {
                rwSettings = eurotaxDBDao.getRestwertSettings2(input.sysobtyp, inDto.prodKontext.perDate);
                if (rwSettings.sysvgrw != 0)
                {
                    outDtoList = MyGetForecastForRemo(rwSettings, inDto, input.sysobtyp);
                }
                else
                {
                    throw new System.ApplicationException("No Default Value for OBTYP " + input.sysobtyp);
                }
            }
            return outDtoList;
        }

        /// <summary>
        /// Gets input by sysAuskunft, calls GetForecast WS and saves output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto GetForecast(long sysAuskunft)
        {
            long code = codeTechExc;

            request1 = new GetForecastRequest1();

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                EurotaxInDto inDto = eurotaxDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.EurotaxInDto = inDto;

                request1.ETGHeader = MyGetHeaderType(inDto);
                request1.Settings = MyGetSettingType(inDto);
                request1.Vehicle = MyGetVehicleType(inDto);
                code = codeSerAufExc;
                eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(inDto.sysobtyp, "EurotaxGetForecast"));
                eurotaxWSDao.getForecast(ref request1, inDto.sysobtyp);
                code = codeTechExc;
                EurotaxOutDto outDto = MyMapVehicleToOutDto(request1.Vehicle, request1.ETGHeader);
                auskunftdto.EurotaxOutDto = outDto;

                // Save EurotaxOut
                eurotaxDBDao.SaveEurotaxForecastOutput(sysAuskunft, outDto);

                if (outDto.ErrorCode != 0)
                {
                    code = (long)outDto.ErrorCode;
                }

                // Update Auskunft
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);

                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in Eurotax-GetForecast!");
                throw new ApplicationException("Unexpected Exception in Eurotax-GetForecast!", e);
            }
        }
        /// <summary>
        /// HCE always uses eurotax and fallback to rwtab if error+available
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        private EurotaxOutDto GetValuationHCE(EurotaxInDto inDto)
        {
            double Neupreis = 0;
            double TradeValuePercent = 0;
            
            ObtypDataRestwertDto input = null;
            input = this.eurotaxDBDao.getObTypDataByNVCByString(inDto.NationalVehicleCode.ToString());
            input.Schwacke = inDto.NationalVehicleCode;

            inDto.sysobtyp = input.sysobtyp;

            RestWertSettingsDto rwSettings = eurotaxDBDao.getRestwertSettings(input.sysobtyp,inDto.prodKontext != null ? inDto.prodKontext.perDate : CfgDate.verifyPerDate(null)); //Ticket#2012010910000253 — Hotfix 5785-RW: Anpassung Webservices 
            request = new DAO.Auskunft.EurotaxValuationRef.GetValuationRequest();

            request.ETGHeader = MyGetHeaderTypeForValuation(inDto);
            request.Settings = MyGetSettingTypeForValuation(inDto);
            request.Valuation = MyGetValuationType(inDto);
            bool useRWTAB = false;
            EurotaxSource source = EurotaxSource.EurotaxValuation;
            try
            {
                eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(inDto.sysobtyp, "EurotaxGetValuation"));
                eurotaxWSDao.getValuation(ref request.ETGHeader, ref request.Settings, ref request.Valuation, inDto.sysobtyp);
                if (request.ETGHeader.Response != null && request.ETGHeader.Response.Item != null)
                {
                    FailureType failure = request.ETGHeader.Response.Item as FailureType;
                    if (failure != null && failure.ETGError != null)
                    {
                        useRWTAB = true;
                        _log.Warn("Eurotax for " + inDto.NationalVehicleCode + " failed: " + failure.ETGError.Description);
                        _log.Info("Will use RWTAB " + rwSettings.sysvgrw + " if >0");
                    }
                }
                
            }catch(Exception e)
            {
                useRWTAB = true;
                _log.Warn("Eurotax for " + inDto.NationalVehicleCode + " failed: " + e.Message);
            }
            // Send request away
            if (useRWTAB && rwSettings.sysvgrw > 0)
            {
                _log.Info("Using rwtab for " + inDto.NationalVehicleCode);
                try
                {
                    Neupreis = evaluateNeupreis(input.sysobtyp);
                }
                catch (Exception ex)
                {
                    //sonderfall: wenn sysobtyp von ebene 1(vc_obtyp1), dann fehler ignorieren (neupreis=0, sonst werfen
                    if (!isLevelOne(input.sysobtyp))
                    {
                        throw ex;
                    }
                }

                RestwertRequestDto Data = new RestwertRequestDto();

                DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                long Years = aktuell.Year - inDto.RegistrationDate.Year;
                long Months = aktuell.Month - inDto.RegistrationDate.Month;
                Data.Laufzeit = (Years * 12 + Months).ToString();
                Data.Laufleistung = inDto.Mileage;
                Data.perDate = aktuell;
                Data.sysobtyp = input.sysobtyp;
                TradeValuePercent = evaluateRestwert(Data, rwSettings.sysvgrw);
                request.Valuation.BasicResidualValue = new DAO.Auskunft.EurotaxValuationRef.ValuationAmountType();
                {
                    request.Valuation.BasicResidualValue.B2Bamount = 0;
                    request.Valuation.BasicResidualValue.RetailAmount = Math.Round(Neupreis * (TradeValuePercent / 100), 2);
                    request.Valuation.BasicResidualValue.TradeAmount = Math.Round(Neupreis * (TradeValuePercent / 100), 2);
                }
                source =  EurotaxSource.InternalTableRW;
            }

            EurotaxOutDto outDto = MyMapValuationToOutDtoHCE(request.Valuation, request.ETGHeader);
            outDto.source = source;

            return outDto;
        }
        /// <summary>
        /// Gets input by filled inDto, saves input, calls GetValuation WS and saves output
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override EurotaxOutDto GetValuation(EurotaxInDto inDto)
        {
            if (inDto.ISOCountryCodeValuation == DAO.Auskunft.EurotaxValuationRef.ISOcountryType.DE)
            {
                return GetValuationHCE(inDto);
            }

            double Neupreis = 0;
            double TradeValuePercent = 0;
            //BR10 /BNRZEHN-1669
            ObtypDataRestwertDto input = null;
            if (inDto.sysobtyp > 0)
            {
                input = this.obtypDao.getObTypData(inDto.sysobtyp);
                inDto.NationalVehicleCode = input.Schwacke;
            }
            else
            {
                //BR10
                input = this.eurotaxDBDao.getObTypDataByNVCByString(inDto.NationalVehicleCode.ToString());
                if (input.sysobtyp > 0)
                {
                    input.Schwacke = inDto.NationalVehicleCode;
                }

                inDto.sysobtyp = input.sysobtyp;
            }
            EurotaxSource internalSource = EurotaxSource.InternalTableVGREF_RW;
            if (inDto.prodKontext == null)
                inDto.prodKontext = new prKontextDto();
            BankNowCalculator.autoAssignPrhgroup(inDto.prodKontext, obtypDao, PrismaDaoFactory.getInstance().getPrismaDao());
            RestWertSettingsDto rwSettings = eurotaxDBDao.getSysVGForVGREFType(VGRefType.RW, input.sysobtyp, inDto.prodKontext.sysbrand, inDto.prodKontext.sysprhgroup, inDto.prodKontext.perDate);
            if (rwSettings.External)//no vgreftable found
            {
                internalSource = EurotaxSource.InternalTableRW;
                rwSettings = eurotaxDBDao.getRestwertSettings(input.sysobtyp,inDto.prodKontext!=null?inDto.prodKontext.perDate: CfgDate.verifyPerDate(null)); //Ticket#2012010910000253 — Hotfix 5785-RW: Anpassung Webservices 
                if (rwSettings.External)//no vgreftable found
                    internalSource = EurotaxSource.EurotaxValuation;
            }

            request = new DAO.Auskunft.EurotaxValuationRef.GetValuationRequest();

            request.ETGHeader = MyGetHeaderTypeForValuation(inDto);
            request.Settings = MyGetSettingTypeForValuation(inDto);
            request.Valuation = MyGetValuationType(inDto);

            // Save Auskunft
            //long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.EurotaxGetValuation);

            // Save EurotaxInput
            //eurotaxDBDao.SaveEurotaxValuationInput(sysAuskunft, inDto);

            // Send request away
            if (!rwSettings.External)
            {
                try
                {
                    Neupreis = evaluateNeupreis(input.sysobtyp);
                }
                catch (Exception ex)
                {
                    //sonderfall: wenn sysobtyp von ebene 1(vc_obtyp1), dann fehler ignorieren (neupreis=0, sonst werfen
                    if (!isLevelOne(input.sysobtyp))
                    {
                        throw ex;
                    }
                }

                RestwertRequestDto Data = new RestwertRequestDto();

                DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                long Years = aktuell.Year - inDto.RegistrationDate.Year;
                long Months = aktuell.Month - inDto.RegistrationDate.Month;
                Data.Laufzeit = (Years * 12 + Months).ToString();
                Data.Laufleistung = inDto.Mileage;
                Data.perDate = aktuell;
                Data.sysobtyp = input.sysobtyp;
                _log.Debug("GetValuation: sysobtyp:" + input.sysobtyp + " using sysvg:" + rwSettings.sysvgrw + " Laufzeit:" + Data.Laufzeit+" Laufleistung:"+Data.Laufleistung);
                TradeValuePercent = evaluateRestwert(Data, rwSettings.sysvgrw);
                request.Valuation.BasicResidualValue = new DAO.Auskunft.EurotaxValuationRef.ValuationAmountType();
                {
                    request.Valuation.BasicResidualValue.B2Bamount = 0;
                    request.Valuation.BasicResidualValue.RetailAmount = Math.Round(Neupreis * (TradeValuePercent / 100), 2);
                    request.Valuation.BasicResidualValue.TradeAmount = Math.Round(Neupreis * (TradeValuePercent / 100), 2);
                }
            }
            else
            {
                eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(inDto.sysobtyp, "EurotaxGetValuation"));
                eurotaxWSDao.getValuation(ref request.ETGHeader, ref request.Settings, ref request.Valuation, inDto.sysobtyp);
            }

            EurotaxOutDto outDto = MyMapValuationToOutDto(request.Valuation, request.ETGHeader);
            outDto.source = internalSource;

         

            return outDto;
        }

        /// <summary>
        /// Gets input by sysAuskunft, calls GetValuation WS and saves output  
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto GetValuation(long sysAuskunft)
        {
            long code = codeTechExc;
            try
            {
                request = new DAO.Auskunft.EurotaxValuationRef.GetValuationRequest();

                // Get AuskunftDto
                AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);

                EurotaxInDto inDto = eurotaxDBDao.FindValuationInputBySysId(auskunftdto.sysAuskunft);
                auskunftdto.EurotaxInDto = inDto;

                request.ETGHeader = MyGetHeaderTypeForValuation(inDto);
                request.Settings = MyGetSettingTypeForValuation(inDto);
                request.Valuation = MyGetValuationType(inDto);
                code = codeSerAufExc;
                eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(inDto.sysobtyp, "EurotaxGetValuation"));
                eurotaxWSDao.getValuation(ref request.ETGHeader, ref request.Settings, ref request.Valuation, inDto.sysobtyp);
                code = codeTechExc;
                EurotaxOutDto outDto = MyMapValuationToOutDto(request.Valuation, request.ETGHeader);
                auskunftdto.EurotaxOutDto = outDto;

                // Save EurotaxOut
                eurotaxDBDao.SaveEurotaxValuationOutput(sysAuskunft, outDto);

                if (outDto.ErrorCode != 0)
                {
                    code = (long)outDto.ErrorCode;
                }

                // Update Auskunft
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, outDto.ErrorCode);

                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in Eurotax-GetValuation!", e);
            }
        }

        /// <summary>
        /// Ermitteln des Restwertes eines Fahrzeugs
        /// </summary>
        /// <param name="input">Eingabedaten</param>
        /// <param name="sysvg">Wertegruppen ID</param>
        /// <returns>Restwert in Prozent</returns>
        public override double evaluateRestwert(RestwertRequestDto input, long sysvg)
        {


            return vgDao.getVGValue(sysvg, Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null), input.Laufzeit, input.Laufleistung, VGInterpolationMode.LINEAR, plSQLVersion.V2);
        }

        /// <summary>
        /// Ermittlen des Neuwerts eines Fahrzeugs.
        /// </summary>
        /// <param name="sysobtyp">Objekttyp</param>
        /// <returns>Netto Neupreis Betrag</returns>
        public override double evaluateNeupreis(long sysobtyp)
        {
            return eurotaxDBDao.evaluateFzNeupreis(sysobtyp);
        }

        /// <summary>
        /// returns true, if the obtyp is a root-vehicle-tree id
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public bool isLevelOne(long sysobtyp)
        {
            return eurotaxDBDao.isLevelOne(sysobtyp);
        }




       

        /// <summary>
        /// getLGDP
        /// </summary>
        /// <param name="eLIndto"></param>
        /// <param name="buchwerte">buchwerte</param>
        /// <param name="marktwerte">marktwerte</param>
        /// <returns></returns>
        public double getLGDP(ELInDto eLIndto, double buchwerte, double marktwerte)
        {

            double bkp_faktor = 0;
            double lz_faktor = 0;
            double pbkp_faktor = 0;
            double plz_faktor = 0;
            double alter_faktor = 0;
            double palter_faktor = 0;
            double BKPFaktor = 0;
            double LZFaktor = 0;

            if  (eLIndto.sysvg == 0)
            {
                eLIndto.sysvg = 201;
            }

            // BKP Faktor 	(<<B>>/(Anzahlung/Barkaufspreis) 
            bkp_faktor = (double)vgDao.getVGValue(eLIndto.sysvg, DateTime.Now, eLIndto.scorebezeichnung, "BKP_FAKTOR", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor für den Barkaufpreis
            pbkp_faktor = (double)vgDao.getVGValue(eLIndto.sysvg, DateTime.Now, eLIndto.scorebezeichnung, "P_BKP", VGInterpolationMode.NONE, plSQLVersion.V1); //Potenz zum Faktor Barkaufpreis

            if (eLIndto.vgName == "STANDARD")

            {
                if (eLIndto.anzahlung == 0 || (eLIndto.anzahlung / eLIndto.barkaufpreis) <= 0.05)
                {
                    BKPFaktor = 2;
                }
                else
                {
                    BKPFaktor = Math.Pow((double)bkp_faktor / (eLIndto.anzahlung / eLIndto.barkaufpreis), (double)pbkp_faktor);
                }
            }
            else
            {
                BKPFaktor = 1;
            }

            // LZ Faktor  	(Laufzeit/<<A>>)
            lz_faktor = (double)vgDao.getVGValue(eLIndto.sysvg, DateTime.Now, eLIndto.scorebezeichnung, "LZ_FAKTOR", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor für die Laufzeit
            plz_faktor = (double)vgDao.getVGValue(eLIndto.sysvg, DateTime.Now, eLIndto.scorebezeichnung, "P_LZ", VGInterpolationMode.NONE, plSQLVersion.V1); //Potenz zum Faktor Laufzeit

            LZFaktor = Math.Pow((eLIndto.laufzeit / lz_faktor), plz_faktor);


            //  Alter Faktor	(Alter/<<C>>)
            alter_faktor = (double)vgDao.getVGValue(eLIndto.sysvg, DateTime.Now, eLIndto.scorebezeichnung, "ALTER_FAKTOR", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor für das Alter
            palter_faktor = (double)vgDao.getVGValue(eLIndto.sysvg, DateTime.Now, eLIndto.scorebezeichnung, "P_ALT", VGInterpolationMode.NONE, plSQLVersion.V1); //Potenz zum Faktor Alter
            double ALTERFaktor = 0;
            if (alter_faktor > 0)
            {
                ALTERFaktor = Math.Pow((eLIndto.alter_Fhz_Mt / alter_faktor), palter_faktor);
            }
            if (ALTERFaktor < 0.5)
            {
                ALTERFaktor = 1;
            }

            double Kalibrierungswert = LZFaktor * BKPFaktor * ALTERFaktor;

            /*LGD in %:
            (((Summe aller Buchwerte * LZ Faktor * BKP Faktor * Alter Faktor* (1+MwSt))
            - (Summe aller Marktwerte * 0.9))
            / (Laufzeit-2))/ Barkaufpreis-Anzahlung */

            double lgdCH = (buchwerte * Kalibrierungswert - (marktwerte * 0.9)) / (eLIndto.laufzeit - 2);
            double lgdp = (lgdCH / (eLIndto.barkaufpreis - eLIndto.anzahlung)) * 100;

          
            

            return lgdp;
        }


        /// <summary>
        /// getBuchwerte
        /// </summary>
        /// <param name="eLIndto"></param>
        /// <returns></returns>
        public double getBuchwerte(ELInDto eLIndto)
        {
            IMwStBo mwstBo = BOFactory.getInstance().createMwstBo();
            double ust = 0;
            if (eLIndto.mwst == null)
            {
                ust = mwstBo.getMehrwertSteuer(1, eLIndto.sysvart, DateTime.Now);
            }
            else
            {
                ust = (double)eLIndto.mwst;
            }

            double barkaufpreis = eLIndto.barkaufpreis;
            double anzahlung = eLIndto.anzahlung;
          
            double barwert = -(barkaufpreis - anzahlung);
            double restwert = eLIndto.restwert;
            double zins_nom = eLIndto.zins / 100;
            double zinsa;
            double tilgunga = anzahlung;
            double rate1 = anzahlung;

            double restschuld = barkaufpreis;
            double rate = (double)eLIndto.rate;
            restschuld -= rate1;
            zinsa = restschuld * zins_nom * 30 / 360;
            double tilgung1 = rate1 - zinsa;

            tilgunga = rate - zinsa;
            double tempTilgung = tilgunga;

            double[] bwarray = new double[eLIndto.laufzeit];
            double[] bwarrayround = new double[eLIndto.laufzeit];
            double[] tilgung = new double[eLIndto.laufzeit];
            double[] tilgunground = new double[eLIndto.laufzeit];
            tilgung[0] = tilgung1;

            double bwnet = (barkaufpreis - tilgung1) / 1.076;
            bwarray[0] = Math.Round(bwnet, 2);
            double sum = 0;
            
            sum = bwarray[0];

            for (int i = 0; i < eLIndto.laufzeit - 1; i++)
            {
                restschuld -= tilgunga;
                zinsa = restschuld * zins_nom * 30 / 360;
                tilgunga = rate - zinsa;
                tilgung[i + 1] = tilgunga;

                bwarray[i + 1] = Math.Round(bwarray[i] - (tilgung[i + 1]/1.076), 2);
                sum += Math.Round(bwarray[i + 1],2);


            }
            return sum;

        }



        /// <summary>
        /// getAusfallwahrscheinlichkeitP
        /// </summary>
        /// <param name="sysvg"></param>
        /// <param name="scorebezeichnung"></param>
        /// <param name="scorewert"></param>
        /// <returns></returns>
        private double getAusfallwahrscheinlichkeitP(double sysvg, string scorebezeichnung, double scorewert)
        {

            // Ausfallwahrscheinlichkeit = 1/(1+EXP(-(Scorekartenabhängiger_Kalibrierungswert+ Scorekartenabhängiger_Kalibrierungsfaktor*Scorewert)))//
            double kalibrierungswert = (double)vgDao.getVGValue((long)sysvg, DateTime.Now, scorebezeichnung, "INT", VGInterpolationMode.NONE, plSQLVersion.V1);
            double kalibrierungsfaktor = (double)vgDao.getVGValue((long)sysvg, DateTime.Now, scorebezeichnung, "SLOPE", VGInterpolationMode.NONE, plSQLVersion.V1);
            double temp = 1 + (double)Math.Pow(2.7182818285, (-(kalibrierungswert + kalibrierungsfaktor * scorewert)));
            double awp = (1 / temp)*100;
            return awp;
        }




       /// <summary>
       /// 
       /// </summary>
       /// <param name="sysobtyp"></param>
       /// <param name="laufzeit"></param>
       /// <param name="neupreis"></param>
       /// <param name="neupreisDefault"></param>
       /// <param name="neupreisIW"></param>
       /// <param name="kmStand"></param>
       /// <param name="zubehoer"></param>
       /// <param name="erstzulassung"></param>
       /// <param name="schwacke"></param>
       /// <param name="jahresKm"></param>
       /// <returns></returns>
        public override List<EurotaxOutDto> getEurotaxOutList(long sysobtyp, int laufzeit, double neupreis, double neupreisDefault, double neupreisIW, double neupreisVGREF, double  kmStand, double zubehoer, DateTime? erstzulassung, string schwacke, long jahresKm)
        {
            //BNRSIZE - 1145 neupreisDefault LP wird neu immer aus der gleichen Quelle kommen wie die Restwertverläufe.
            EurotaxInDto inDto = new EurotaxInDto();
            if (laufzeit >= 3)
            {
                inDto.ForecastPeriodFrom = "3"; //Einrichtung in Eurotax auf From=3
            }
            else
            {
                inDto.ForecastPeriodFrom = "1";
            }
            inDto.ForecastPeriodUntil = laufzeit.ToString();
            inDto.CurrentMileageValue = (UInt32)kmStand;
            //inDto.TotalListPriceOfEquipment = zubehoer;
            inDto.TotalListPriceOfEquipment = 0; //wegen Alte Logik auf 0 gesetzt anspr. Stas
            inDto.RegistrationDate = (erstzulassung != null) ? (DateTime)erstzulassung : DateTime.Now;
            try
            {
                inDto.NationalVehicleCode = Convert.ToInt64(schwacke);
            }
            catch (Exception) {

            }
            inDto.EstimatedAnnualMileageValue = (UInt32)jahresKm;
            inDto.ISOCountryCode = (DAO.Auskunft.EurotaxRef.ISOcountryType)Enum.Parse(typeof(DAO.Auskunft.EurotaxRef.ISOcountryType), "CH");
            inDto.ISOCurrencyCode = (DAO.Auskunft.EurotaxRef.ISOcurrencyType)Enum.Parse(typeof(DAO.Auskunft.EurotaxRef.ISOcurrencyType), "CHF");
            inDto.ISOLanguageCode = (DAO.Auskunft.EurotaxRef.ISOlanguageType)Enum.Parse(typeof(DAO.Auskunft.EurotaxRef.ISOlanguageType), "DE");

            List<EurotaxOutDto> outDtoList = new List<EurotaxOutDto>();

            if (inDto.prodKontext == null)
                inDto.prodKontext = new prKontextDto();
            BankNowCalculator.autoAssignPrhgroup(inDto.prodKontext, obtypDao, PrismaDaoFactory.getInstance().getPrismaDao());
            RestWertSettingsDto rwSettings = eurotaxDBDao.getSysVGForVGREFType(VGRefType.RW, sysobtyp, inDto.prodKontext.sysbrand, inDto.prodKontext.sysprhgroup, CfgDate.verifyPerDate(null));
            EurotaxSource source = EurotaxSource.InternalTableVGREF_RW;
            
            if (rwSettings.External)//no vgreftable found
            {
                source = EurotaxSource.InternalTableRW;
                rwSettings = eurotaxDBDao.getRestwertSettings(sysobtyp, CfgDate.verifyPerDate(null)); //Ticket#2012010910000253 — Hotfix 5785-RW: Anpassung Webservices 
                if(rwSettings.External)
                    source = EurotaxSource.EurotaxForecast;
            }
            
            if (!rwSettings.External)
            {
                if(source==EurotaxSource.InternalTableRW)
                    outDtoList = MyGetForecastForRemo(rwSettings, inDto, sysobtyp, neupreis, neupreisIW, source);
                else
                    outDtoList = MyGetForecastForRemo(rwSettings, inDto, sysobtyp, neupreis, neupreisVGREF, source);
            }
            if (rwSettings.External)
            {
                outDtoList = MyGetForecastForRemo(rwSettings, inDto, sysobtyp, neupreis, neupreis, EurotaxSource.EurotaxForecast);
            }
            if (outDtoList.Count() == 0)
            {
                rwSettings = eurotaxDBDao.getRestwertSettings2(sysobtyp, CfgDate.verifyPerDate(null));
                if (rwSettings.sysvgrw != 0)
                {

                    outDtoList = MyGetForecastForRemo(rwSettings, inDto, sysobtyp, neupreis,neupreisDefault, EurotaxSource.InternalTableRemo);
                }
                else
                {
                    throw new System.ApplicationException("No Default Value for OBTYP " + sysobtyp);
                }
            }
            if (outDtoList != null)
            {
                //zwei Elemente dazugefügt wegen die ForecastPeriodFrom = 3

                List<EurotaxOutDto> result = new List<EurotaxOutDto>();

                if (laufzeit >= 3)
                {
                    result.Add(outDtoList.ElementAt(0));
                    result.Add(outDtoList.ElementAt(0));
                }


                foreach (EurotaxOutDto elem in outDtoList)
                {
                    result.Add(elem);
                }
                return result;

            }

            return outDtoList;
        }
       
     
       
       
       
     


        /// <summary>
        /// Gets input by sysAuskunft, calls GetForecast WS and saves output
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override  EurotaxVinOutDto GetVinDecode(EurotaxVinInDto inDto)
        {
            long code = codeTechExc;
            EurotaxVinOutDto outDto = new EurotaxVinOutDto();

            Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.VinDecodeRequest request1 = new  Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.VinDecodeRequest();

            Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto output = new Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto();
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.EurotaxVinDecode);

            try
            {
               
                request1.ETGHeader = MyGetHeaderType(inDto);
                Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.VinDecodeInputType request = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.VinDecodeInputType();
                request.Settings = MyGetSettingType(inDto);
                request.VinCode = inDto.vinCode;
                request.ExtendedOutput = true;
                request.ServiceId = MyGetServiceId(inDto);
                request1.Request = request;
                code = codeSerAufExc;

                //For report
                eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotaxVin(sysAuskunft, AuskunfttypDao.EurotaxVinDecode));

                Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.VinDecodeOutputType response = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.VinDecodeOutputType();
                eurotaxWSDao.getVinDecode(ref request1, ref response);

                code = codeTechExc;
                outDto = MyMapVinDecodeOutput(response);

                code = outDto.statusCode;
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                auskunftDao.setAuskunftWfuser(sysAuskunft, inDto.syswfuser);

                if (!string.IsNullOrEmpty(inDto.area) || inDto.sysid != 0)
                {
                    auskunftDao.setAuskunfAreaUndId(sysAuskunft, inDto.area, inDto.sysid); //BNRZW-1724
                }

                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.requestXML = eurotaxWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = eurotaxWSDao.getSoapXMLDto().responseXML;
                return outDto;
            }
           
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                auskunftDao.setAuskunftWfuser(sysAuskunft, inDto.syswfuser);
                if (!string.IsNullOrEmpty(inDto.area) || inDto.sysid != 0)
                {
                    auskunftDao.setAuskunfAreaUndId(sysAuskunft, inDto.area, inDto.sysid); //BNRZW-1724
                }
                _log.Error("Exception in VinDecode");
                throw new ApplicationException("E_00011_Exception_VINDECODE", e);
            }
        }

        private EurotaxVinOutDto MyMapVinDecodeOutput(DAO.Auskunft.EurotaxVinRef.VinDecodeOutputType response)
        {

            EurotaxVinOutDto outDto = new EurotaxVinOutDto();
            outDto = Mapper.Map<DAO.Auskunft.EurotaxVinRef.VinDecodeOutputType, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxVinOutDto>(response);
            return outDto;

        }

        public override string MyGetServiceId(EurotaxVinInDto inDto)
        {
            EurotaxLoginDataDto accessData = eurotaxDBDao.GetEurotaxAccessData(OpenOne.Common.DAO.Auskunft.AuskunftCfgDao.EUROTAXVINDECODE);
            string serviceID = accessData.serviceId;

            if (serviceID == null || serviceID.Length == 0)//legacy-fallback
            { 
                string serviceIDDefault = "2c507b7850cb43aada4fd0e19fb6a51484ad0baf.15357675a";
                serviceID = new EaiparDao().getEaiParFileByCode("EurotaxVinServiceID", serviceIDDefault);
                _log.Warn("Vin serviceid not found in AUSKUNFTCFG.SYSEAIPAR for " + OpenOne.Common.DAO.Auskunft.AuskunftCfgDao.EUROTAXVINDECODE + " using id from EAIPAR with code EurotaxVinServiceID or static default 2c....");
            }
            
            return serviceID;
        }

        private object MyGetVinDecodeReq(EurotaxVinInDto inDto)
        {
            throw new NotImplementedException();
        }


        #region MyMethods
        /// <summary>
        /// Provides access data (username, password, signature)
        /// </summary>
        private ETGHeaderType MyGetHeaderType(EurotaxInDto inDto)
        {
            EurotaxLoginDataDto accessData = eurotaxDBDao.GetEurotaxAccessData(OpenOne.Common.DAO.Auskunft.AuskunftCfgDao.EUROTAXGETFORECAST);
            ETGHeaderType header = new ETGHeaderType();
            LoginDataType LoginData = new LoginDataType();
            LoginData.Name = accessData.name;
            LoginData.Password = accessData.password;
            header.Originator = new OriginatorType();
            header.Originator.Item = LoginData;
            header.Originator.Signature = accessData.signature;
            // (EurotaxRef.ISOcountryType)Enum.Parse(typeof(EurotaxRef.ISOcountryType), etgSettings.ISOCOUNTRYCODE);
            // header.VersionRequest = (VersionType)Enum.Parse(typeof(VersionType), inDto.Version);
            header.VersionRequest = VersionType.Item103;
            return header;
        }


        /// <summary>
        /// Provides access data (username, password, signature)
        /// </summary>
        public override Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ETGHeaderType MyGetHeaderType(EurotaxVinInDto inDto)
        {
            EurotaxLoginDataDto accessData= eurotaxDBDao.GetEurotaxAccessData(OpenOne.Common.DAO.Auskunft.AuskunftCfgDao.EUROTAXVINDECODE);
            Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ETGHeaderType header = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ETGHeaderType();
            Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.LoginDataType LoginData = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.LoginDataType();
            LoginData.Name = accessData.name;
            LoginData.Password = accessData.password;
            header.Originator = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.OriginatorType();
            header.Originator.LoginData = LoginData;
            header.Originator.Signature = accessData.signature;// "EGISonline";
            header.VersionRequest = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.VersionType.Item110;
            return header;
        }

        /// <summary>
        /// Fills Setting Type
        /// </summary>
        /// <param name="inDto"></param>
        private ETGsettingType MyGetSettingType(EurotaxInDto inDto)
        {
            ETGsettingType settingType = new ETGsettingType();
            settingType.ISOcountryCode = inDto.ISOCountryCode;
            settingType.ISOcurrencyCode = inDto.ISOCurrencyCode;
            settingType.ISOlanguageCode = inDto.ISOLanguageCode;
            //settingType.ISOcountryCode = (ISOcountryType)Enum.Parse(typeof(ISOcountryType), inDto.ISOcountryCode);
            //settingType.ISOcurrencyCode = (ISOcurrencyType)Enum.Parse(typeof(ISOcurrencyType), inDto.ISOcurrencyCode);
            //settingType.ISOlanguageCode = (ISOlanguageType)Enum.Parse(typeof(ISOlanguageType), inDto.ISOlanguageCode);
            return settingType;
        }


        /// <summary>
        /// Fills Setting Type
        /// </summary>
        /// <param name="inDto"></param>
        public override Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ETGsettingType MyGetSettingType(EurotaxVinInDto inDto)
        {
            Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ETGsettingType settingType = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ETGsettingType();
            settingType.ISOcountryCode = inDto.ISOCountryCode;
            settingType.ISOlanguageCode = inDto.ISOLanguageCode;
            return settingType;
        }


        /// <summary>
        /// Fills VehicleType with necessary data provided by inDto
        /// </summary>
        /// <param name="inDto"></param>
        private VehicleType MyGetVehicleType(EurotaxInDto inDto)
        {
            VehicleType vehicle = new VehicleType();
            // Fill EstimatedAnnualMileage (Pflicht)
            vehicle.EstimatedAnnualMileage = new ETGmileageType();
            vehicle.EstimatedAnnualMileage.Value = (uint)inDto.EstimatedAnnualMileageValue;
            vehicle.EstimatedAnnualMileage.Unit = DAO.Auskunft.EurotaxRef.ETGmileageUnitType.km;

            // Fill NationalVehicleCode (Pflicht)
            vehicle.Items = new object[1];
            vehicle.Items[0] = inDto.NationalVehicleCode;

            // Fill ForecastPeriod (Pflicht)
            vehicle.ForecastData = new ETGforecastDataType[1];
            vehicle.ForecastData[0] = new ETGforecastDataType();
            vehicle.ForecastData[0].Item = inDto.ForecastPeriodFrom;

            // Fill Equipment (Pflicht)
            vehicle.Items1 = new object[1];
            vehicle.Items1[0] = inDto.TotalListPriceOfEquipment;
            vehicle.Items1ElementName = new DAO.Auskunft.EurotaxRef.Items1ChoiceType[1];
            vehicle.Items1ElementName[0] = Items1ChoiceType.TotalListPriceOfEquipment;

            // Fill RegistrationDate (optional)
            if (inDto.RegistrationDate.Year > 1900)
            {
                vehicle.RegistrationDate = new ETGdateType();
                vehicle.RegistrationDate.Day = inDto.RegistrationDate.Day.ToString();
                vehicle.RegistrationDate.Month = inDto.RegistrationDate.Month.ToString();
                vehicle.RegistrationDate.Year = inDto.RegistrationDate.Year.ToString();
            }

            // Fill CurrentMileage (optional)
            if (inDto.CurrentMileageValue != null)
            {
                vehicle.CurrentMileage = new ETGmileageType();
                vehicle.CurrentMileage.Value = (uint)inDto.CurrentMileageValue;
                vehicle.CurrentMileage.Unit = DAO.Auskunft.EurotaxRef.ETGmileageUnitType.km;
            }
            return vehicle;
        }

        /// <summary>
        /// Fills VehicleType with necessary data provided by inDto
        /// </summary>
        /// <param name="inDto"></param>
        /// <param name="periodeVon"></param>
        /// <param name="periodeBis"></param>
        /// <returns></returns>
        private VehicleType MyGetVehicleType(EurotaxInDto inDto, int periodeVon, int periodeBis)
        {
            VehicleType vehicle = new VehicleType();
            // Fill EstimatedAnnualMileage (Pflicht)
            vehicle.EstimatedAnnualMileage = new ETGmileageType();
            vehicle.EstimatedAnnualMileage.Value = (uint)inDto.EstimatedAnnualMileageValue;
            vehicle.EstimatedAnnualMileage.Unit = DAO.Auskunft.EurotaxRef.ETGmileageUnitType.km;

            // Fill NationalVehicleCode (Pflicht)
            vehicle.Items = new object[1];
            vehicle.Items[0] = inDto.NationalVehicleCode;

            // Fill ForecastPeriod (Pflicht)
            int periods = periodeBis - periodeVon + 1;
            vehicle.ForecastData = new ETGforecastDataType[periods];
            int i = 0;
            for (Int32 counter = periodeVon; counter <= periodeBis; counter++)
            {
                vehicle.ForecastData[i] = new ETGforecastDataType();
                vehicle.ForecastData[i].Item = counter.ToString();
                i++;
            }

            // Fill Equipment (Pflicht)
            vehicle.Items1 = new object[1];
            vehicle.Items1[0] = inDto.TotalListPriceOfEquipment;
            vehicle.Items1ElementName = new DAO.Auskunft.EurotaxRef.Items1ChoiceType[1];
            vehicle.Items1ElementName[0] = Items1ChoiceType.TotalListPriceOfEquipment;

            // Fill RegistrationDate (optional)
            if (inDto.RegistrationDate.Year > 1900)
            {
                vehicle.RegistrationDate = new ETGdateType();
                vehicle.RegistrationDate.Day = inDto.RegistrationDate.Day.ToString();
                vehicle.RegistrationDate.Month = inDto.RegistrationDate.Month.ToString();
                vehicle.RegistrationDate.Year = inDto.RegistrationDate.Year.ToString();
            }

            // Fill CurrentMileage (optional)
            if (inDto.CurrentMileageValue != null)
            {
                vehicle.CurrentMileage = new ETGmileageType();
                vehicle.CurrentMileage.Value = (uint)inDto.CurrentMileageValue;
                vehicle.CurrentMileage.Unit = DAO.Auskunft.EurotaxRef.ETGmileageUnitType.km;
            }
            return vehicle;
        }

        /// <summary>
        /// Fills EurotaxOutDto with vehicleType or extendedVehicleType
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="header"></param>
        private EurotaxOutDto MyMapVehicleToOutDto(VehicleType vehicle, ETGHeaderType header)
        {
            EurotaxOutDto outDto = new EurotaxOutDto();
            if (header!=null && header.Response != null && header.Response.Item != null)
            {
                // if header.Response.Item is FailureType a fatal error has occurred during processing of the request
                if (header.Response.Item is FailureType)
                {
                    FailureType failure = header.Response.Item as FailureType;
                    if (failure.ETGError != null)
                    {
                        outDto.ErrorCode = failure.ETGError.Code;
                        outDto.ErrorDescription = failure.ETGError.Description;
                    }
                }
            }
            if (vehicle.ForecastData[0].ForecastedValue != null)
            {
                outDto.ForecastPeriod = long.Parse(vehicle.ForecastData[0].Item.ToString());
                if (vehicle.ForecastData[0].ForecastedValue.RetailAmountSpecified)
                {
                    outDto.RetailAmount = vehicle.ForecastData[0].ForecastedValue.RetailAmount;
                }
                if (vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentageSpecified)
                {
                    outDto.RetailValueInPercentage = vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentage;
                }
                if (vehicle.ForecastData[0].ForecastedValue.TradeAmountSpecified)
                {
                    outDto.TradeAmount = vehicle.ForecastData[0].ForecastedValue.TradeAmount;
                }
                if (vehicle.ForecastData[0].ForecastedValue.TradeValueInPercentageSpecified)
                {
                    outDto.TradeValueInPercentage = vehicle.ForecastData[0].ForecastedValue.TradeValueInPercentage;
                }
               
            }
            return outDto;
        }
        /// <summary>
        /// Fills EurotaxOutDto with vehicleType or extendedVehicleType
        /// </summary>
        /// <param name="targetList"></param>
        /// <param name="vehicle"></param>
        /// <param name="header"></param>
        private int MyMapVehicleToOutDto(List<EurotaxOutDto> targetList, VehicleType vehicle, ETGHeaderType header)
        {
            int errCode = 0;
            for (int i = 0; i < vehicle.ForecastData.Length; i++)
            {
                EurotaxOutDto outDto = new EurotaxOutDto();
                if (header.Response != null && header.Response.Item != null)
                {
                    // if header.Response.Item is FailureType a fatal error has occurred during processing of the request
                    if (header.Response.Item is FailureType)
                    {
                        FailureType failure = header.Response.Item as FailureType;
                        if (failure.ETGError != null)
                        {
                            outDto.ErrorCode = failure.ETGError.Code;
                            outDto.ErrorDescription = failure.ETGError.Description;
                        }
                    }
                }
                if (vehicle.ForecastData[i].ForecastedValue != null)
                {
                    outDto.ForecastPeriod = long.Parse(vehicle.ForecastData[i].Item.ToString());
                    if (vehicle.ForecastData[i].ForecastedValue.RetailAmountSpecified)
                    {
                        outDto.RetailAmount = vehicle.ForecastData[i].ForecastedValue.RetailAmount;
                    }
                    if (vehicle.ForecastData[i].ForecastedValue.RetailValueInPercentageSpecified)
                    {
                        outDto.RetailValueInPercentage = vehicle.ForecastData[i].ForecastedValue.RetailValueInPercentage;
                    }
                    if (vehicle.ForecastData[i].ForecastedValue.TradeAmountSpecified)
                    {
                        outDto.TradeAmount = vehicle.ForecastData[i].ForecastedValue.TradeAmount;
                    }
                    if (vehicle.ForecastData[i].ForecastedValue.TradeValueInPercentageSpecified)
                    {
                        outDto.TradeValueInPercentage = vehicle.ForecastData[i].ForecastedValue.TradeValueInPercentage;
                    }
                }
                if (outDto.ErrorCode != 0)
                {
                    errCode = outDto.ErrorCode;
                }
                targetList.Add(outDto);
            }
            return errCode;
        }

        /// <summary>
        /// Maps Valuation to OutDto
        /// </summary>
        /// <param name="valuation"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        private EurotaxOutDto MyMapValuationToOutDto(DAO.Auskunft.EurotaxValuationRef.ValuationType valuation, DAO.Auskunft.EurotaxValuationRef.ETGHeaderType header)
        {
            EurotaxOutDto outDto = new EurotaxOutDto();
            if (header.Response != null && header.Response.Item != null)
            {
                FailureType failure = header.Response.Item as FailureType;
                if (failure != null && failure.ETGError != null)
                {
                    outDto.ErrorCode = failure.ETGError.Code;
                    outDto.ErrorDescription = failure.ETGError.Description;
                }
            }
            if (valuation.ActualNewPrice != null)
            {
                outDto.ActualNewPrice = valuation.ActualNewPrice.Amount;
                outDto.ActualNewPriceIndicator = valuation.ActualNewPrice.Indicator;
            }
            outDto.AverageMileage = valuation.AverageMileage;
            if (valuation.BasicResidualValue != null)
            {
                outDto.BasicResidualB2BAmount = valuation.BasicResidualValue.B2Bamount;
                outDto.BasicResidualRetailAmount = valuation.BasicResidualValue.RetailAmount;
                outDto.BasicResidualTradeAmount = valuation.BasicResidualValue.TradeAmount;
            }
            if (valuation.MonthlyAdjustmentValue != null)
            {
                outDto.MonthlyAdjustmentB2BAmount = valuation.MonthlyAdjustmentValue.B2Bamount;
                outDto.MonthlyAdjustmentRetailAmount = valuation.MonthlyAdjustmentValue.RetailAmount;
                outDto.MonthlyAdjustmentTradeAmount = valuation.MonthlyAdjustmentValue.TradeAmount;
            }
            if (valuation.MileageAdjustmentValue != null)
            {
                outDto.MileageAdjustmentB2BAmount = valuation.MileageAdjustmentValue.B2Bamount;
                outDto.MileageAdjustmentRetailAmount = valuation.MileageAdjustmentValue.RetailAmount;
                outDto.MileageAdjustmentTradeAmount = valuation.MileageAdjustmentValue.TradeAmount;
            }
            if (valuation.TotalValuation != null)
            {
                outDto.TotalValuationB2BAmount = valuation.TotalValuation.B2Bamount;
                outDto.TotalValuationRetailAmount = valuation.TotalValuation.RetailAmount;
                outDto.TotalValuationTradeAmount = valuation.TotalValuation.TradeAmount;
            }
            return outDto;
        }


        /// <summary>
        /// Maps Valuation to OutDto
        /// </summary>
        /// <param name="valuation"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        private EurotaxOutDto MyMapValuationToOutDtoHCE(DAO.Auskunft.EurotaxValuationRef.ValuationType valuation, DAO.Auskunft.EurotaxValuationRef.ETGHeaderType header)
        {
            EurotaxOutDto outDto = new EurotaxOutDto();
            if (header.Response != null && header.Response.Item != null)
            {
                FailureType failure = header.Response.Item as FailureType;
                if (failure != null && failure.ETGError != null)
                {
                    outDto.ErrorCode = failure.ETGError.Code;
                    outDto.ErrorDescription = failure.ETGError.Description;
                }
            }
            if (valuation.ActualNewPrice != null)
            {
                outDto.ActualNewPrice = valuation.ActualNewPrice.Amount;
                outDto.ActualNewPriceIndicator = valuation.ActualNewPrice.Indicator;
            }
            outDto.AverageMileage = valuation.AverageMileage;
            if (valuation.BasicResidualValue != null)
            {
                outDto.BasicResidualB2BAmount = valuation.BasicResidualValue.B2Bamount;
                outDto.BasicResidualRetailAmount = valuation.BasicResidualValue.RetailAmount;
                outDto.BasicResidualTradeAmount = valuation.BasicResidualValue.TradeAmount;
            }
            if (valuation.MonthlyAdjustmentValue != null)
            {
                outDto.MonthlyAdjustmentB2BAmount = valuation.MonthlyAdjustmentValue.B2Bamount;
                outDto.MonthlyAdjustmentRetailAmount = valuation.MonthlyAdjustmentValue.RetailAmount;
                outDto.MonthlyAdjustmentTradeAmount = valuation.MonthlyAdjustmentValue.TradeAmount;
            }
            if (valuation.MileageAdjustmentValue != null)
            {
                outDto.MileageAdjustmentB2BAmount = valuation.MileageAdjustmentValue.B2Bamount;
                outDto.MileageAdjustmentRetailAmount = valuation.MileageAdjustmentValue.RetailAmount;
                outDto.MileageAdjustmentTradeAmount = valuation.MileageAdjustmentValue.TradeAmount;
            }
            //HCE Change for HCEB-2011 Der GetValuationService gibt derzeit nur das Array BasicResidualValue zurück. Für die Übergabe an das MUW werden jedoch die Werte aus dem TradeValuation Array benötigt
            if (valuation.TotalValuation != null)
            {
                outDto.BasicResidualB2BAmount = valuation.TotalValuation.B2Bamount;
                outDto.BasicResidualRetailAmount = valuation.TotalValuation.RetailAmount;
                outDto.BasicResidualTradeAmount = valuation.TotalValuation.TradeAmount;
            }
            if (valuation.TotalValuation != null)
            {
                outDto.TotalValuationB2BAmount = valuation.TotalValuation.B2Bamount;
                outDto.TotalValuationRetailAmount = valuation.TotalValuation.RetailAmount;
                outDto.TotalValuationTradeAmount = valuation.TotalValuation.TradeAmount;
            }
            return outDto;
        }

        /// <summary>
        /// Gets username and password from db and fills header
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override DAO.Auskunft.EurotaxValuationRef.ETGHeaderType MyGetHeaderTypeForValuation(EurotaxInDto inDto)
        {
            EurotaxLoginDataDto accessData = eurotaxDBDao.GetEurotaxAccessData(OpenOne.Common.DAO.Auskunft.AuskunftCfgDao.EUROTAXGETVALUATION);
            DAO.Auskunft.EurotaxValuationRef.ETGHeaderType header = new DAO.Auskunft.EurotaxValuationRef.ETGHeaderType();
            DAO.Auskunft.EurotaxValuationRef.LoginDataType LoginData = new DAO.Auskunft.EurotaxValuationRef.LoginDataType();
            LoginData.Name = accessData.name;
            LoginData.Password = accessData.password;
            header.Originator = new DAO.Auskunft.EurotaxValuationRef.OriginatorType();
            header.Originator.Item = LoginData;
            header.Originator.Signature = accessData.signature;
            // header.VersionRequest = (DAO.Auskunft.EurotaxValuationRef.VersionType)Enum.Parse(typeof(DAO.Auskunft.EurotaxValuationRef.VersionType), inDto.Version);
            header.VersionRequest = DAO.Auskunft.EurotaxValuationRef.VersionType.Item114;
            return header;
        }

        /// <summary>
        /// Gets setting for Valuation
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override DAO.Auskunft.EurotaxValuationRef.ETGsettingType MyGetSettingTypeForValuation(EurotaxInDto inDto)
        {
            DAO.Auskunft.EurotaxValuationRef.ETGsettingType settingType = new DAO.Auskunft.EurotaxValuationRef.ETGsettingType();
            settingType.ISOcountryCode = inDto.ISOCountryCodeValuation;
            settingType.ISOcurrencyCode = inDto.ISOCurrencyCodeValuation;
            settingType.ISOlanguageCode = inDto.ISOLanguageCodeValuation;
            return settingType;
        }

        /// <summary>
        /// Gets ValuationType
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        private DAO.Auskunft.EurotaxValuationRef.ValuationType MyGetValuationType(EurotaxInDto inDto)
        {
            DAO.Auskunft.EurotaxValuationRef.ValuationType valuation = new DAO.Auskunft.EurotaxValuationRef.ValuationType();
            valuation.NationalVehicleCode = inDto.NationalVehicleCode;
            valuation.Mileage = inDto.Mileage;
            DAO.Auskunft.EurotaxValuationRef.ETGdateType dateType = new DAO.Auskunft.EurotaxValuationRef.ETGdateType();
            dateType.Day = inDto.RegistrationDate.Day.ToString();
            dateType.Month = inDto.RegistrationDate.Month.ToString();
            dateType.Year = inDto.RegistrationDate.Year.ToString();
            valuation.RegistrationDate = dateType;
            return valuation;
        }

        private List<EurotaxOutDto> MyGetForecastForRemo(RestWertSettingsDto rwSettings, EurotaxInDto inDto, long sysobtyp)
        {
            double Neupreis = 0;
            double RestwertPercent = 0;

            Int32 periodeVon = 0;
            Int32 periodeBis = 0;
            Int32.TryParse(inDto.ForecastPeriodFrom, out periodeVon);
            Int32.TryParse(inDto.ForecastPeriodUntil, out periodeBis);

            // if only PeriodeVon is filled, then fetch only one Forecast 
            if (periodeVon > periodeBis)
            {
                periodeBis = periodeVon;
            }

            try
            {
                Neupreis = evaluateNeupreis(sysobtyp) + inDto.TotalListPriceOfEquipment;
            }
            catch (Exception ex)
            {
                //sonderfall: wenn sysobtyp von ebene 1(vc_obtyp1), dann fehler ignorieren (neupreis=0, sonst werfen
                if (!isLevelOne(sysobtyp))
                {
                    throw ex;
                }
            }

            DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
            long Years = aktuell.Year - inDto.RegistrationDate.Year;
            long Months = aktuell.Month - inDto.RegistrationDate.Month;
            long Laufzeit = (Years * 12 + Months);

            List<EurotaxOutDto> outDtoList = new List<EurotaxOutDto>();
            IKorrekturBo korr = BOFactory.getInstance().createKorrekturBo();
            List<long> obtypascendants = this.obtypDao.getObTypAscendants(sysobtyp);
            obtypascendants.Reverse();
            if (!rwSettings.External)
            {
                for (Int32 counter = periodeVon; counter <= periodeBis; counter++)
                {
                    inDto.ForecastPeriodFrom = counter.ToString();

                    request1 = new GetForecastRequest1();
                    request1.ETGHeader = MyGetHeaderType(inDto);
                    request1.Settings = MyGetSettingType(inDto);
                    request1.Vehicle = MyGetVehicleType(inDto);

                    RestwertRequestDto Data = new RestwertRequestDto();
                    Data.Laufleistung = ((int)(inDto.CurrentMileageValue + (inDto.EstimatedAnnualMileageValue * (counter / 12.0)))).ToString();
                    Data.Laufzeit = (counter + Laufzeit).ToString();
                    Data.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                    Data.sysobtyp = sysobtyp;
                    RestwertPercent = evaluateRestwert(Data, rwSettings.sysvgrw);

                    // Korrektur soll bei GetForecast greifen und bei GetRemo und GetValuation nicht
                    // RestwertPercent = korr.Correct(KORRTYP_RW, (decimal)RestwertPercent, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1]);

                    request1.Vehicle.ForecastData[0] = new ETGforecastDataType();
                    request1.Vehicle.ForecastData[0].Item = counter.ToString();
                    request1.Vehicle.ForecastData[0].ForecastedValue = new ETGforecastedValueType();
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailAmount = Math.Round(Neupreis * (RestwertPercent / 100), 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentage = Math.Round(RestwertPercent, 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeAmount = Math.Round(Neupreis * (RestwertPercent / 100), 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeValueInPercentage = Math.Round(RestwertPercent, 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailAmountSpecified = true;
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentageSpecified = true;
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeAmountSpecified = true;
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeValueInPercentageSpecified = true;

                    EurotaxOutDto outDto = MyMapVehicleToOutDto(request1.Vehicle, request1.ETGHeader);
                    if (outDto.ErrorCode != 0)
                    {
                        outDtoList.Clear();
                        return outDtoList;
                    }
                    else
                    {
                        outDto.source = EurotaxSource.InternalTableRemo;
                        outDtoList.Add(outDto);
                    }
                }
            }
            else
            {
                request1 = new GetForecastRequest1();
                request1.ETGHeader = MyGetHeaderType(inDto);
                request1.Settings = MyGetSettingType(inDto);

                if (isBlockedCall())
                {
                    request1.Vehicle = MyGetVehicleType(inDto, periodeVon, periodeBis);
                    try
                    {
                        eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(sysobtyp, "EurotaxGetForecast"));
                        eurotaxWSDao.getForecast(ref request1, inDto.sysobtyp);
                    }
                    catch (Exception)
                    {
                        outDtoList.Clear();
                        return outDtoList;
                    }
                    int errcode = MyMapVehicleToOutDto(outDtoList, request1.Vehicle, request1.ETGHeader);
                    if (errcode != 0)
                    {
                        outDtoList.Clear();
                        return outDtoList;
                    }
                }
                else
                {
                    for (Int32 counter = periodeVon; counter <= periodeBis; counter++)
                    {
                        inDto.ForecastPeriodFrom = counter.ToString();

                        request1 = new GetForecastRequest1();
                        request1.ETGHeader = MyGetHeaderType(inDto);
                        request1.Settings = MyGetSettingType(inDto);
                        request1.Vehicle = MyGetVehicleType(inDto);
                        try
                        {
                            eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(sysobtyp, "EurotaxGetForecast"));
                            eurotaxWSDao.getForecast(ref request1, inDto.sysobtyp);
                        }
                        catch (Exception)
                        {
                            outDtoList.Clear();
                            return outDtoList;
                        }

                        EurotaxOutDto outDto = MyMapVehicleToOutDto(request1.Vehicle, request1.ETGHeader);
                        if (outDto.ErrorCode != 0)
                        {
                            outDtoList.Clear();
                            return outDtoList;
                        }
                        else
                        {
                            outDtoList.Add(outDto);
                        }
                    }
                }

                //CR 24777
                foreach (EurotaxOutDto outDto in outDtoList)
                {
                    // Korrektur soll bei GetForecast greifen und bei GetRemo und GetValuation nicht
                    // outDto.RetailValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.RetailValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1]);
                    // outDto.TradeValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.TradeValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1]);
                    outDto.RetailAmount = Math.Round(Neupreis * (outDto.RetailValueInPercentage / 100), 2);
                    outDto.TradeAmount = Math.Round(Neupreis * (outDto.TradeValueInPercentage / 100), 2);
                    outDto.source = EurotaxSource.EurotaxForecast;
                }
            }
            return outDtoList;
        }

        /// <summary>
        /// Fetch the eurotax forecast value
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override EurotaxOutDto getEurotaxForecast(EurotaxInDto inDto)
        {
            
            GetForecastRequest1 request1 = new GetForecastRequest1();
            request1.ETGHeader = MyGetHeaderType(inDto);
            request1.Settings = MyGetSettingType(inDto);
            request1.Vehicle = MyGetVehicleType(inDto);
            try
            {
                eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(inDto.sysobtyp, "EurotaxGetForecast"));
                eurotaxWSDao.getForecast(ref request1, inDto.sysobtyp);
            }
            catch (Exception e)
            {
                throw e;// new Exception("Fatal error communicating with Forecast-Eurotax Service",e);
            }

            EurotaxOutDto outDto = MyMapVehicleToOutDto(request1.Vehicle, request1.ETGHeader);
            if (outDto.ErrorCode != 0)
            {
                throw new Exception("Failure fetching Forecast-Value from Eurotax - ErrorCode:" + outDto.ErrorCode);
            }
            else
            {
                return outDto;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rwSettings"></param>
        /// <param name="inDto"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="neupreis"></param>
        /// <param name="neupreisDefault"></param>
        /// <param name="eurotaxSource"></param>
        /// <returns></returns>
        private List<EurotaxOutDto> MyGetForecastForRemo(RestWertSettingsDto rwSettings, EurotaxInDto inDto, long sysobtyp, double neupreis,double neupreisDefault, EurotaxSource eurotaxSource)
        {
            double Neupreis = neupreis;
            double RestwertPercent = 0;
            
            Int32 periodeVon = 0;
            Int32 periodeBis = 0;
            Int32.TryParse(inDto.ForecastPeriodFrom, out periodeVon);
            Int32.TryParse(inDto.ForecastPeriodUntil, out periodeBis);

            // if only PeriodeVon is filled, then fetch only one Forecast 
            if (periodeVon > periodeBis)
            {
                periodeBis = periodeVon;
            }

            DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
         
            long Years = aktuell.Year - inDto.RegistrationDate.Year;
            long Months = aktuell.Month - inDto.RegistrationDate.Month;
            long daysInMonat = (aktuell.Day > inDto.RegistrationDate.Day)?1:0;
            //BNRSIZE - 1145 Richtig FZ_Alter wie in Sql (ceil) LP wird neu immer aus der gleichen Quelle kommen wie die Restwertverläufe.
            long Laufzeit = (Years * 12 + Months) + daysInMonat;
            

            List<EurotaxOutDto> outDtoList = new List<EurotaxOutDto>();
            IKorrekturBo korr = BOFactory.getInstance().createKorrekturBo();
            List<long> obtypascendants = this.obtypDao.getObTypAscendants(sysobtyp);
            obtypascendants.Reverse();

            if (!rwSettings.External)
            {
                //BNRSIZE - 1145  LP wird neu immer aus der gleichen Quelle kommen wie die Restwertverläufe.
                Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.ETGHeaderType ETGHeader = MyGetHeaderType(inDto);
                Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.ETGsettingType Settings = MyGetSettingType(inDto);
                Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.VehicleType Vehicle = MyGetVehicleType(inDto);
                neupreis = neupreisDefault;
                for (Int32 counter = periodeVon; counter <= periodeBis; counter++)
                {
                    inDto.ForecastPeriodFrom = counter.ToString();

                    request1 = new GetForecastRequest1();
                    request1.ETGHeader = ETGHeader;
                    request1.Settings = Settings;
                    request1.Vehicle = Vehicle;

                    RestwertRequestDto Data = new RestwertRequestDto();
                    Data.Laufleistung = ((int)(inDto.CurrentMileageValue + (inDto.EstimatedAnnualMileageValue * (counter / 12.0)))).ToString();
                    Data.Laufzeit = (counter + Laufzeit).ToString();
                    Data.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                    Data.sysobtyp = sysobtyp;
                 

                    RestwertPercent = evaluateRestwert(Data, rwSettings.sysvgrw);
              

                    // Korrektur soll bei GetForecast greifen und bei GetRemo und GetValuation nicht
                    // RestwertPercent = korr.Correct(KORRTYP_RW, (decimal)RestwertPercent, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1]);

                    request1.Vehicle.ForecastData[0] = new ETGforecastDataType();
                    request1.Vehicle.ForecastData[0].Item = counter.ToString();
                    request1.Vehicle.ForecastData[0].ForecastedValue = new ETGforecastedValueType();
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentage = Math.Round(RestwertPercent, 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailAmount = Math.Round(Neupreis * (request1.Vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentage / 100), 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeAmount = Math.Round(Neupreis * (request1.Vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentage / 100), 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeValueInPercentage  = Math.Round(RestwertPercent, 2);
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailAmountSpecified = true;
                    request1.Vehicle.ForecastData[0].ForecastedValue.RetailValueInPercentageSpecified = true;
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeAmountSpecified = true;
                    request1.Vehicle.ForecastData[0].ForecastedValue.TradeValueInPercentageSpecified = true;

                    EurotaxOutDto outDto = MyMapVehicleToOutDto(request1.Vehicle, request1.ETGHeader);

                    if (outDto.ErrorCode != 0)
                    {
                        outDtoList.Clear();
                        return outDtoList;
                    }
                    else
                    {
                        outDto.source = eurotaxSource;
                        outDtoList.Add(outDto);
                    }
                }
            }
            else
            {
                request1 = new GetForecastRequest1();
                request1.ETGHeader = MyGetHeaderType(inDto);
                request1.Settings = MyGetSettingType(inDto);

                if (isBlockedCall())
                {
                    request1.Vehicle = MyGetVehicleType(inDto, periodeVon, periodeBis);
                    try
                    {
                        eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(sysobtyp, "EurotaxGetForecast"));
                        eurotaxWSDao.getForecast(ref request1, inDto.sysobtyp);
                    }
                    catch (Exception)
                    {
                        outDtoList.Clear();
                        return outDtoList;
                    }
                    int errcode = MyMapVehicleToOutDto(outDtoList, request1.Vehicle, request1.ETGHeader);
                    if (errcode != 0)
                    {
                        outDtoList.Clear();
                        return outDtoList;
                    }
                }
                else
                {
                    Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.ETGHeaderType ETGHeader = MyGetHeaderType(inDto);
                    Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.ETGsettingType Settings = MyGetSettingType(inDto);
                    Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.VehicleType Vehicle = MyGetVehicleType(inDto);
                    for (Int32 counter = periodeVon; counter <= periodeBis; counter++)
                    {
                        inDto.ForecastPeriodFrom = counter.ToString();

                        request1 = new GetForecastRequest1();
                        request1.ETGHeader = ETGHeader;
                        request1.Settings = Settings;
                        request1.Vehicle = Vehicle;
                        try
                        {
                            eurotaxWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLogEurotax(sysobtyp, "EurotaxGetForecast"));
                            eurotaxWSDao.getForecast(ref request1, inDto.sysobtyp);
                        }
                        catch (Exception)
                        {
                            outDtoList.Clear();
                            return outDtoList;
                        }

                        EurotaxOutDto outDto = MyMapVehicleToOutDto(request1.Vehicle, request1.ETGHeader);
                        if (outDto.ErrorCode != 0)
                        {
                            outDtoList.Clear();
                            return outDtoList;
                        }
                        else
                        {
                            outDto.source = EurotaxSource.EurotaxForecast;
                            outDtoList.Add(outDto);
                            
                        }
                    }

                }

                //CR 24777
                foreach (EurotaxOutDto outDto in outDtoList)
                {
                    // Korrektur soll bei GetForecast greifen und bei GetRemo und GetValuation nicht
                    // outDto.RetailValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.RetailValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1]);
                    // outDto.TradeValueInPercentage = korr.Correct(KORRTYP_RW, (decimal)outDto.TradeValueInPercentage, "*", aktuell, "" + obtypascendants[0], "" + obtypascendants[1]);
                    outDto.RetailAmount = Math.Round(Neupreis * (outDto.RetailValueInPercentage / 100), 2);
                    outDto.TradeAmount = Math.Round(Neupreis * (outDto.TradeValueInPercentage / 100), 2);

                }

            }
              return outDtoList;
        }

    
        #endregion

    }


   
}