using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.IO;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// S1 Business Object
    /// </summary>
    class RISKEWBS1Bo : AbstractRISKEWBS1Bo
    {

        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto sendData(long sysAuskunft)
        {
            try
            {
                AuskunftDto outDto = new AuskunftDto();
                outDto.sysAuskunft = sysAuskunft;

                s1DBDao = new RISKEWBS1DBDao(sysAuskunft);


                long _ret = 0;
                // send data to SimpleServices
                try
                {


                    Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.CicServiceClient Client = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.CicServiceClient();

                    Client.CalculateWB(sysAuskunft, new AsyncLoadedDataStream(delegate (BlockingCollection<byte> queue)
                    {

                        try
                        {
                            int i = 0;
                            using (DdOlExtended context = new DdOlExtended())
                            {
                                String queryString =
                                    "SELECT 1 as CallNumber, 1 as RandomNumber, deenvinp.processcode DecisionProcessCode, InquiryCode, OrganizationCode, ProcessVersion, InquiryDate, InquiryTime, " +
                                        "Str01 I_C_VARCHAR_F01, Str02 I_C_VARCHAR_F02, Str03 I_C_VARCHAR_F03, Str04 I_C_VARCHAR_F04, Str05 I_C_VARCHAR_F05, Str06 I_C_VARCHAR_F06, Str07 I_C_VARCHAR_F07, Str08 I_C_VARCHAR_F08, Str09 I_C_VARCHAR_F09, Str10 I_C_VARCHAR_F10, Str11 I_C_VARCHAR_F11, Str12 I_C_VARCHAR_F12, Str13 I_C_VARCHAR_F13, Str14 I_C_VARCHAR_F14, Str15 I_C_VARCHAR_F15, " +
                                        "Dat01 I_C_DATE_F16, Dat02 I_C_DATE_F17, Dat03 I_C_DATE_F18, Dat04 I_C_DATE_F19, Dat05 I_C_DATE_F20, Dat06 I_C_DATE_F21, Dat07 I_C_DATE_F22, Dat08 I_C_DATE_F23, Dat09 I_C_DATE_F24, Dat10 I_C_DATE_F25, Dat11 I_C_DATE_F26, Dat12 I_C_DATE_F27, Dat13 I_C_DATE_F28, Dat14 I_C_DATE_F29, Dat15 I_C_DATE_F30, Dat16 I_C_DATE_F31, Dat17 I_C_DATE_F32, Dat18 I_C_DATE_F33, Dat19 I_C_DATE_F34, Dat20 I_C_DATE_F35, " +
                                        "Pdec1501 I_C_NUMBER_152_F36, Pdec1502 I_C_NUMBER_152_F37, Pdec1503 I_C_NUMBER_152_F38, Pdec1504 I_C_NUMBER_152_F39, Pdec1505 I_C_NUMBER_152_F40, Pdec1506 I_C_NUMBER_152_F41, Pdec1507 I_C_NUMBER_152_F42, Pdec1508 I_C_NUMBER_152_F43, Pdec1509 I_C_NUMBER_152_F44, Pdec1510 I_C_NUMBER_152_F45, Pdec1511 I_C_NUMBER_152_F46, Pdec1512 I_C_NUMBER_152_F47, Pdec1513 I_C_NUMBER_152_F48, Pdec1514 I_C_NUMBER_152_F49, Pdec1515 I_C_NUMBER_152_F50, Pdec1516 I_C_NUMBER_152_F51, Pdec1517 I_C_NUMBER_152_F52, Pdec1518 I_C_NUMBER_152_F53, Pdec1519 I_C_NUMBER_152_F54, Pdec1520 I_C_NUMBER_152_F55, Pdec1521 I_C_NUMBER_152_F56, Pdec1522 I_C_NUMBER_152_F57, Pdec1523 I_C_NUMBER_152_F58, Pdec1524 I_C_NUMBER_152_F59, Pdec1525 I_C_NUMBER_152_F60, " +
                                        "Int01 I_C_NUMBER_120_F61, Int02 I_C_NUMBER_120_F62, Int03 I_C_NUMBER_120_F63, Int04 I_C_NUMBER_120_F64, Int05 I_C_NUMBER_120_F65, Int06 I_C_NUMBER_120_F66, Int07 I_C_NUMBER_120_F67, Int08 I_C_NUMBER_120_F68, Int09 I_C_NUMBER_120_F69, Int10 I_C_NUMBER_120_F70, " +
                                        "Flag01 I_C_NUMBER_030_F71, Flag02 I_C_NUMBER_030_F72, Flag03 I_C_NUMBER_030_F73, Flag04 I_C_NUMBER_030_F74, Flag05 I_C_NUMBER_030_F75, Flag06 I_C_NUMBER_030_F76, Flag07 I_C_NUMBER_030_F77, Flag08 I_C_NUMBER_030_F78, Flag09 I_C_NUMBER_030_F79, Flag10 I_C_NUMBER_030_F80 " +
                                    "FROM cic.dewbin, cic.deenvinp " +
                                    "WHERE cic.dewbin.sysdewbin = cic.deenvinp.sysdewbin AND cic.dewbin.sysdeinpexec IN (SELECT sysdeinpexec FROM cic.deinpexec WHERE sysauskunft = :pSysAuskunft)";

                                Cic.OpenOne.CarConfigurator.DAO.EaiparDao eaiParDao = new Cic.OpenOne.CarConfigurator.DAO.EaiparDao();
                                queryString = eaiParDao.getEaiParFileByCode("RISKEWB_DB2S1", queryString);
                                _log.Debug("sendData from Query " + queryString + " with sysAukunft=" + sysAuskunft + " to S1...");

                                List<object> parameters = new List<object>();
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysAuskunft", Value = sysAuskunft });
                                IQueryable<RISKEWBS1DataDto> results = context.ExecuteStoreQuery<RISKEWBS1DataDto>(queryString, parameters.ToArray()).AsQueryable<RISKEWBS1DataDto>();

                                foreach (RISKEWBS1DataDto dto in results)
                                {

                                    byte[] mybytes = UTF8Encoding.Default.GetBytes(dto.ToString());
                                    foreach (byte b in mybytes)
                                    {
                                        queue.Add(b);
                                    }

                                    i++;
                                    if (i % 1000 == 0)
                                    {
                                        this._log.Debug("sendData to S1: " + i + " contracts processed");
                                    }
                                }

                            }

                            //_log.Debug("sendData to S1: " + i + " contracts processed and waiting 30minutes...");
                            //System.Threading.Thread.Sleep(1000*60*30);
                            _log.Debug("sendData to S1: " + i + " contracts processed and finished!");
                        }
                        catch (Exception ex)
                        {
                            _ret = 2;
                            _log.Error("sendData to S1 failed: " + ex.Message, ex);
                        }



                    }));



                }
                catch (Exception e)
                {
                    s1DBDao.SaveAuskunftStatus(AuskunftStatus.ErrorSimpleService, "");
                    throw new ApplicationException("Unexpected Exception in S1 Webservice!", e);
                }

                if (_ret == 0)
                {
                    s1DBDao.SaveAuskunftStatus(AuskunftStatus.OKSendToSS, "");
                    outDto.Fehlercode = "";
                }
                else if (_ret == 2)
                {
                    s1DBDao.SaveAuskunftStatus(AuskunftStatus.ErrorSimpleService, "");
                    outDto.Fehlercode = "Error by RISKEWBS1DBDao.GetDataFromDB";
                }
                else
                {
                    s1DBDao.SaveAuskunftStatus(AuskunftStatus.OKSendToSSErrorInData, "");
                    outDto.Fehlercode = "Error in structure of the input data";
                }

                return outDto;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Unexpected Exception in S1 Webservice!", e);
            }
        }

        /// <summary>
        /// make a one big string with all data
        /// </summary>
        /// <param name="listS1Dto"></param>
        /// <returns></returns>
        public string myPrepareS1String(List<RISKEWBS1DataDto> listS1Dto)
        {
            string outStr = string.Empty;

            foreach (RISKEWBS1DataDto _s1Dto in listS1Dto)
            {
                outStr = outStr + _s1Dto.ToString() + "\v";
            }

            return outStr;
        }


    }
}
