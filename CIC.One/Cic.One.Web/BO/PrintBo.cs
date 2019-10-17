using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pechkin.Synchronized;
using Pechkin;
using System.Drawing.Printing;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.One.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.One.Web.DAO;

namespace Cic.One.Web.BO
{
    public enum EaiHotStatusConstants
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 0,
        /// <summary>
        /// Working
        /// </summary>
        Working = 1,
        /// <summary>
        /// Bereit
        /// </summary>
        Ready = 2
    }

    public class PrintBo : AbstractPrintBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private SynchronizedPechkin sc = null;
        private const int PRINTTIMEOUT = 60;

        private void initPechkin()
        {
            if (sc == null)
            {
                sc = new SynchronizedPechkin(new GlobalConfig().SetMargins(new Margins(100, 100, 100, 100))
               .SetCopyCount(1).SetImageQuality(50)
               .SetLosslessCompression(true).SetMaxImageDpi(20).SetOutlineGeneration(true).SetOutputDpi(1200).SetPaperOrientation(false)

               .SetPaperSize(PaperKind.A4));
            }
        }

        /// <summary>
        /// Converts the html to a PDF byte array
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        override public byte[] htmlToPdf(String html)
        {
            initPechkin();
            return sc.Convert(html);
        }

        /// <summary>
        /// Converts the given url to a pdf
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        override public byte[] urlToPdf(Uri url)
        {
            initPechkin();
            return sc.Convert(url);
        }

        /// <summary>
        /// Fills the html-template with the data and converts to PDF
        /// </summary>
        /// <param name="data"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        override public byte[] templateToPdf(object data, string template)
        {
            HtmlReportBo htmlReportBo = new HtmlReportBo(new StringHtmlTemplateDao(template));
            String pdfTxt = htmlReportBo.CreateHtmlReport(data, true, false, true);

            initPechkin();
            return sc.Convert(new ObjectConfig(), pdfTxt);
        }

        /// <summary>
        /// Prints a document via EAI
        /// </summary>
        /// <param name="urliprintparam>
        /// <returns></returns>
        override public EaihotDto printDocument(iprintDocumentDto iprint, long syswfuser)
        {
          

            EAIHOT eaihotInput = new EAIHOT();
            eaihotInput.CODE = iprint.code;
            eaihotInput.OLTABLE = iprint.area;
            eaihotInput.SYSOLTABLE = iprint.sysid;
            eaihotInput.PROZESSSTATUS = (int)EaiHotStatusConstants.Pending;
            eaihotInput.SYSWFUSER = syswfuser;
            eaihotInput.EVE = 1;
            eaihotInput.HOSTCOMPUTER = "*";
            eaihotInput.CLIENTART = 1;
            eaihotInput.INPUTPARAMETER1 = "" + syswfuser;
            eaihotInput.INPUTPARAMETER2 = iprint.INPUTPARAMETER2;
            eaihotInput.INPUTPARAMETER3 = iprint.INPUTPARAMETER3;
            eaihotInput.INPUTPARAMETER4 = iprint.INPUTPARAMETER4;
            eaihotInput.INPUTPARAMETER5 = iprint.INPUTPARAMETER5;
            DateTime d = DateTime.Now;
            eaihotInput.SUBMITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(d);
            eaihotInput.SUBMITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(d);



            using (DdOwExtended owCtx = new DdOwExtended())
            {

                
                eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
                                      where EaiArt.CODE.Equals(iprint.eaiartCode)
                                      select EaiArt).FirstOrDefault();

                owCtx.AddToEAIHOT(eaihotInput);
                owCtx.SaveChanges();





                /* DropListDto[] languageCode = dictionaryListDao.deliverCTLANG();
                 int? sprachid = new int();

                 foreach (DokumenteDto dokument in dokumente)
                 {
                     foreach (DropListDto spracheAusListe in languageCode)
                     {
                         if (spracheAusListe.code == dokument.DefaultSprache)
                         {
                             sprachid = (int)spracheAusListe.sysID;
                         }
                     }
                     if (sprachid == null)
                     {
                         throw new ArgumentException("Keine gültige Sprache gefunden für Dokumentensprache: " + dokument.DefaultSprache + " vom Dokument: " + dokument.DokumentenID);
                     }
                     eaihotDao.createEaiqin(new EaiqinDto()
                     {
                         sysEaihot = eaihot.SYSEAIHOT,
                         F20 = dokument.sysEaiquo.ToString(),
                         F01 = dokument.DokumentenID.ToString(),
                         F02 = sprachid.ToString(),
                         F03 = dokument.KundenExemplar.ToString(),
                         F04 = dokument.VertriebspatnerExemplar.ToString(),
                         F05 = dokument.BnowMitarbeiterExemplar.ToString(),
                         F06 = dokument.Druck.ToString(),
                     });
                 }
                 eaihot.EVE = 1;
                 eaihot = eaihotDao.updateEaihot(eaihot);
             */
                DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                TimeSpan timeOut = new TimeSpan(0, 0, 0, MyGetTimeOutValue("PRINTTIMEOUT", PRINTTIMEOUT));
                int? pStatus = eaihotInput.PROZESSSTATUS;
                while (pStatus != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                {
                    pStatus = (from hot in owCtx.EAIHOT
                               where hot.SYSEAIHOT == eaihotInput.SYSEAIHOT
                               select hot.PROZESSSTATUS).FirstOrDefault();

                    System.Threading.Thread.Sleep(500);
                }
                if (pStatus == (int)EaiHotStatusConstants.Ready)
                {
                    EaihotDto file = DAOFactoryFactory.getInstance().getEntityDao().getEaihotDetails(eaihotInput.SYSEAIHOT);
                    return file;
                }
               
            }

            throw new ApplicationException("Could not get dokumentfile (timeout).");
        }

        /// <summary>
        /// Prüfung auf Timeout
        /// </summary>
        /// <param name="oldDate">Alter Zeitstempel</param>
        /// <param name="timeOut">NeuerZeitstempel</param>
        /// <returns>Timeout true= Timeout/false = kein Timeout</returns>
        public static bool isTimeOut(DateTime oldDate, TimeSpan timeOut)
        {
            TimeSpan ts = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) - oldDate;

            if (ts > timeOut) return true;

            return false;
        }

        /// <summary>
        /// Holt den Timeout-Parameter für den Print-Service aus der CFG
        /// </summary>
        /// <returns>Timeout-Wert</returns>
        private static int MyGetTimeOutValue(String timeoutKey, int defaultValue)
        {
            int retValue = 0;
            String cfgParam = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry("WEBSERVICES", timeoutKey, defaultValue.ToString(), "SETUP.NET");
            Int32.TryParse(cfgParam, out retValue);

            return retValue;
        }

        /// <summary>
        /// Returns a list of documents for the document Area Code
        /// </summary>
        /// <param name="docCode"></param>
        /// <returns></returns>
        override public List<PrintDocumentDto> getDocumentList(string docCode)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = docCode });
                return ctx.ExecuteStoreQuery<PrintDocumentDto>("select cfgvar.code code, cfgvar.bezeichnung title,cfgsec.code area from cfgvar,cfgsec,cfg where cfgvar.syscfgsec=cfgsec.syscfgsec and cfgsec.code=:code and cfgsec.syscfg=cfg.syscfg and cfg.code='DOKUMENTE' order by cfgvar.wert", pars.ToArray()).ToList();
            }

        }

        /// <summary>
        /// Returns a list of documentsareas
        /// </summary>
        /// <returns></returns>
        override public List<String> getDocumentAreas()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {


                return ctx.ExecuteStoreQuery<String>("select cfgsec.code area from cfgsec,cfg where cfgsec.syscfg=cfg.syscfg and cfg.code='DOKUMENTE' order by cfgsec.code", null).ToList();
            }

        }
    }
}
