using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Enums;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Vertrags BO
    /// </summary>
    public class VertragBo : AbstractVertragBo
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="vtdao">Vertrags DAO</param>
        /// <param name="eaihotDao">EaiHot DAO</param>
        /// <param name="obTypDao">ObTyp DAO</param>
        public VertragBo(IVertragDao vtdao, IEaihotDao eaihotDao, IObTypDao obTypDao) : base(vtdao, eaihotDao, obTypDao) { }

        /// <summary>
        /// Vertrag via ID holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns></returns>
        public override VertragDto getVertrag(long sysid)
        {
            return vtdao.getVertragDetails(sysid);
        }


        /// <summary>
        /// bool isRREChangeAllowed(long sysid, long sysperole);
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public override bool isRREChangeAllowed(long sysid, long sysperole)
        {
            VertragDao vertragDao = new VertragDao();
            VertragDto vertrag = new VertragDto();
            long syshaendler = obTypDao.getHaendlerByEmployee(sysperole);
            long sysperson = obTypDao.getPersonIDByPEROLE(syshaendler);

            return vertragDao.isRREChangeAllowed(sysid, sysperson);
        }

        /// <summary>
        /// performKaufofferte
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override operformKaufofferte performKaufofferte(iperformKaufofferteDto input)
        {
            try
            {
                operformKaufofferte rval = new operformKaufofferte();
                string empfaenger = "";
                string mitarbeiter = "";
                long vertragid = vtdao.getVertragSysId(input.sysid);

                /*if (input.isHaendler)
                {
                    long? sysrwga = vtdao.getRwga(input.sysid);
                    if (sysrwga != null)
                        empfaenger = sysrwga.ToString();
                }
                else
                {
                    long? syskd = vtdao.getKunde(input.sysid);
                    if (syskd != null)
                        empfaenger = syskd.ToString();
                }*/

                if (input.isHaendler)
                {
                    empfaenger = "HD";
                }
                else
                {
                    empfaenger = "KD";
                }

                mitarbeiter = obTypDao.getPersonIDByPEROLE(input.sysPerole).ToString();
                

                EaihotDto eaiOutput = new EaihotDto();
                eaiOutput = new EaihotDto()
                {
                    CODE = "SOAP_DOK_KAUFOFFERTE",
                    OLTABLE = "VT",
                    SYSOLTABLE = vertragid,
                    SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    EVE = 1,
                    INPUTPARAMETER1 = "OFFERTE",
                    INPUTPARAMETER2 = empfaenger,
                    INPUTPARAMETER3 = mitarbeiter,
                    INPUTPARAMETER4 = input.herkunft,
                    INPUTPARAMETER5 = input.perDatumCode,

                    PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                    HOSTCOMPUTER = "*",

                };
                eaiOutput = eaihotDao.createEaihot(eaiOutput);



                DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                TimeSpan timeOut = new TimeSpan(0, 0, 0, 120);
                while (eaiOutput.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                {
                    eaiOutput = eaihotDao.getEaihot(eaiOutput.SYSEAIHOT);
                    System.Threading.Thread.Sleep(500);

                }


                if (eaiOutput.PROZESSSTATUS == (int)EaiHotStatusConstants.Ready)
                {

                    if (eaiOutput.OUTPUTPARAMETER1 != null)
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.message = eaiOutput.OUTPUTPARAMETER1;
                        if (eaiOutput.OUTPUTPARAMETER1.Equals("OK"))
                        {
                            rval.frontid = "BID_ORDER_SENT_TEXT";
                        }
                        if (eaiOutput.OUTPUTPARAMETER1.Equals("NOK"))
                        {
                            rval.frontid = "BID_ORDER_NOT_SENT_ERROR";
                        }


                    }
                }
                return rval;

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Could not get Kaufofferte. EAIHOT", ex);
            }

        }

        /// <summary>
        /// changeRRReceiver / Änderung des Empfängers für die Restwertrechnung
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        public override ochangeRRReceiver changeRRReceiver(long sysid)
        {
            try
            {
                ochangeRRReceiver retVal = new ochangeRRReceiver();
                string kunde = "";
                long vertragid = vtdao.getVertragSysId(sysid);
                long? syskd = vtdao.getKunde(sysid);
                if (syskd != null)
                {
                    kunde = syskd.ToString();
                }

                EaihotDto eaihotOutput = new EaihotDto();
                eaihotOutput = new EaihotDto()
                {
                    CODE = "SOAP_CHG_VT_KAEUFER",
                    OLTABLE = "VT",
                    SYSOLTABLE = vertragid,
                    SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    EVE = 1,
                    INPUTPARAMETER1 = kunde,
                   
                    PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                    HOSTCOMPUTER = "*"

                };
                eaihotOutput = eaihotDao.createEaihot(eaihotOutput);
                DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                TimeSpan timeOut = new TimeSpan(0, 0, 0, 120);
                while (eaihotOutput.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                {
                    eaihotOutput = eaihotDao.getEaihot(eaihotOutput.SYSEAIHOT);
                    System.Threading.Thread.Sleep(500);

                }
              

                if (eaihotOutput.PROZESSSTATUS == (int)EaiHotStatusConstants.Ready)
                {
                    
                    if (eaihotOutput.OUTPUTPARAMETER2 != null)
                    {
                        retVal.message = new OpenOne.Common.DTO.Message();
                        retVal.message.message = eaihotOutput.OUTPUTPARAMETER2;
                        if (eaihotOutput.OUTPUTPARAMETER2.Equals("SEND"))
                        {
                            retVal.frontid = "CHG_CUSTOMER_SEND";
                        }
                        if (eaihotOutput.OUTPUTPARAMETER2.Equals("EXIST"))
                        {
                            retVal.frontid = "CHG_CUSTOMER_EXIST";
                        }
                        if (eaihotOutput.OUTPUTPARAMETER2.Equals("OK"))
                        {
                            retVal.frontid = "CHG_CUSTOMER_SUCCESSFUL";
                        }

                        
                    }

                }
                else
                 {
                        // Throw an exception
                     throw new ArgumentException("Could not change RRReciver. EAIHOT timeout");
                    }
            return retVal;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Could not change RRReciver. EAIHOT timeout", ex);
            }
        }

        /// <summary>
        /// Prüfung auf Timeout
        /// </summary>
        /// <param name="oldDate">Alter Zeitstempel</param>
        /// <param name="timeOut">NeuerZeitstempel</param>
        /// <returns>Timeout true= Timeout/false = kein Timeout</returns>
        public static bool isTimeOut(DateTime oldDate, TimeSpan timeOut)
        {
            TimeSpan ts = DateTime.Now - oldDate;

            if (ts > timeOut)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Prüfung auf Restwertrechnungsempfänger möglich, pendente Auflösung, Restwertrechnung verschickt
        /// </summary>
        /// <param name="result"></param>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public override oSearchDto<VertragDto> zustandPruefung(oSearchDto<VertragDto> result, long sysperole)
        {
            // Änderung des Restwertrechnungsempfänger möglich, ist vt pendente Auflösung oder Restwertrechnung verschickt cr 29

            long syshaendler = obTypDao.getHaendlerByEmployee(sysperole);
            long haendlerPerson = obTypDao.getPersonIDByPEROLE(syshaendler);
            bool chgkaeufer = false;
            if (result != null && result.results!=null)
            {
                foreach (VertragDto v in result.results)
                {

                    v.isPendenteAufloesung = (v.vtruekZustand != null && v.vtruekZustand == "ERSTELLT") ? true : false;
                    v.isRwReVerschickt = (v.wfzustSyscode != null && v.wfzustSyscode.Contains("D_RWRE") && v.vtendekz == 0) ? true : false;
                    chgkaeufer = (v.vtvertrag == null) ? false : v.vtvertrag.Contains('V');
                    v.isRREChangeAllowed = (v.sysvart == 1 && v.vtendekz == 0 && !v.wfzustSyscode.Contains("D_RWRE") && (v.vtsysrwga == haendlerPerson || v.vtsysrwga == 0 || v.vtsysrwga == null)) && !chgkaeufer ? true : false;

                }



            }


            return result;
        }

        /// <summary>
        /// Returns true when order purchase offer in ePOS-now allowed
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        public override bool isPerformKaufofferteAllowed(long sysid)
        {
                VertragDao vertragDao = new VertragDao();
                
                return vertragDao.isPerformKaufofferteAllowed(sysid);
        }


        /// <summary>
        /// gets contract details by its id.
        /// function created so that we can change the query and thus decide what data is given in what field, customizable.
        /// </summary>
        /// <param name="sysvt">contract id</param>
        /// <returns>contract details</returns>
        public override VertragDto getVertragForExtension(long sysvt)
        {
            return vtdao.getVertragForExtension(sysvt);
        }


        /// <summary>
        /// get MWST by sysvt
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public override double getMWST(long sysvt, DateTime perDate)
        {
            return vtdao.getMWST(sysvt, perDate);

        }
    }
}