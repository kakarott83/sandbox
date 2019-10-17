using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.One.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using System.Globalization;
using Cic.One.Web.BO.Search;
using Cic.OpenOne.Common.BO.Search;
using Cic.OpenOne.Common.Util;

namespace Cic.One.GateWKT.BO
{
    public class WITBO
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string CFG_SEC = "EAIHOT";
        private const string CFG_VAR_CLIENTART = "CLIENTART";
        private const string CFG_VAR_HOSTCOMPUTER = "HOSTCOMPUTER";
        private const string CFG_VAR_EVE = "EVE";
        private const string CFG = "WIT_SERVICE";


        public void createOrUpdateFinanzierung(FinanzierungDto fin, int saveMode)
        {

            try
            {
                long rahmenid = fin.sysrvt;

                EAIHOT eaihotInput = new EAIHOT();
                eaihotInput.CODE = "WIT_ACCEPTED";
                eaihotInput.OLTABLE = "RVT";
                eaihotInput.SYSOLTABLE = rahmenid;
                eaihotInput.PROZESSSTATUS = 0;
                eaihotInput.HOSTCOMPUTER = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(CFG_SEC, CFG_VAR_HOSTCOMPUTER, "*", CFG);
                eaihotInput.CLIENTART = int.Parse(Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(CFG_SEC, CFG_VAR_CLIENTART, "0", CFG));
                eaihotInput.EVE = int.Parse(Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(CFG_SEC, CFG_VAR_EVE, "1", CFG));
                eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                using (DdOwExtended owCtx = new DdOwExtended())
                {
                    owCtx.AddToEAIHOT(eaihotInput);
                    eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
                                          where EaiArt.CODE == eaihotInput.CODE
                                          select EaiArt).FirstOrDefault();

                    owCtx.SaveChanges();




                    saveNKKParameters(owCtx, eaihotInput.SYSEAIHOT, fin);

                }
                SearchCache.entityChanged("NKK");
            }
            catch (Exception e)
            {
                _log.Error("Error processing NKK WIT HEK data ", e);

            }

        }
        private EAIQIN createEaiQin(System.Data.EntityKey syseaihot, string value, string data)
        {
            EAIQIN eaiqinInput = new EAIQIN();
            eaiqinInput.EAIHOTReference.EntityKey = syseaihot;
            eaiqinInput.F01 = value;
            eaiqinInput.F02 = data;
            return eaiqinInput;


        }

        private void saveNKKParameters(DdOwExtended owCtx, long syseaihot, FinanzierungDto data)
        {
            List<EAIQIN> eaiqins = new List<EAIQIN>();
            String dealerid = owCtx.ExecuteStoreQuery<String>("SELECT person.CODE from cic.person,rvt where rvt.sysperson=person.sysperson and rvt.sysrvt=" + data.sysrvt, null).FirstOrDefault();
            String rahmenWaehrung = owCtx.ExecuteStoreQuery<String>("select code from rvt,waehrung where waehrung.syswaehrung=rvt.syswaehrung and sysrvt=" + data.sysrvt, null).FirstOrDefault();
            String actiontype = owCtx.ExecuteStoreQuery<String>("select nameintern from prproduct where sysprproduct=" + data.sysprproduct, null).FirstOrDefault();
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "system type", "ZF4"));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "dealer ic", "CZ2020133753"));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "action currency", data.waehrung.code));//??korrekt
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "action type", actiontype));//sysprproduct.nameintern
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "action order", "0"));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "information reserve", data.ob.obbrief.fident));

            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "information date-issued", data.beginn.Value.ToString("yyyy-MM-dd")));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "information date-due", data.auszahlungam.Value.ToString("yyyy-MM-dd")));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "information proforma", "0"));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "information order", data.ob.obbrief.fident));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "importer id", data.ob.obbrief.impcode));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "dealer id", dealerid));//code von person von rvt sysperson
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car vin", data.ob.obbrief.fident));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car body-color", data.ob.farbea));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car doors", data.ob.anzahlTueren.ToString(CultureInfo.InvariantCulture)));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car seats", data.ob.anzahlSitze.ToString(CultureInfo.InvariantCulture)));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car weight", data.ob.obbrief.zulgew.ToString(CultureInfo.InvariantCulture)));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "engine number", data.ob.obbrief.motornummer));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "engine cubature", data.ob.obbrief.hubraum.ToString(CultureInfo.InvariantCulture)));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "engine power", data.ob.obbrief.kw.ToString(CultureInfo.InvariantCulture)));
            //NOT IN GUI eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "engine fuel", data.ob.obbrief.treibstoff));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "engine fuel", "G"));//???

            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "licence-number", data.ob.kennzeichen));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "price currency", data.waehrung.code));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "priced", "" + data.nominal));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "model year", data.ob.baujahr.Value.Year.ToString(CultureInfo.InvariantCulture)));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "base-type", data.ob.fabrikat));
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "version", data.ob.typ));
            String comment = "On the basis of the General Contract BMW WHOLESALE FINANCING SCHEME, the Dealer hereby asks UniCredit Leasing Corporation for the withdrawal of BWF.";// data.bezeichnung.Trim();
            if (comment.Length > 255)
            {
                comment = comment.Substring(0, 255);
            }
            eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "comment", comment));
            foreach (EAIQIN eaiqinInput in eaiqins)
            {
                owCtx.AddToEAIQIN(eaiqinInput);
            }
            owCtx.SaveChanges();

        }
    }
}