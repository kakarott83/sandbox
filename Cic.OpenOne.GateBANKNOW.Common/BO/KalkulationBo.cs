using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Klasse für die Behandlung von Kalkulation Dto's
    /// Beinhaltet kalkulieren, erstellen, laden und löschen von Kalkulationen
    /// </summary>
    [System.CLSCompliant(false)]
    public class KalkulationBo : AbstractKalkulationBo
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private String isoCode;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao"></param>
        /// <param name="obTypDao"></param>
        /// <param name="zinsDao"></param>
        /// <param name="prismaDao"></param>
        /// <param name="angAntDao"></param>
        /// <param name="vgDao"></param>
        /// <param name="etWsDao"></param>
        /// <param name="etDbDao"></param>
        /// <param name="auskDao"></param>
        /// <param name="kunDao"></param>
        /// <param name="provisionDao"></param>
        /// <param name="subventionDao"></param>
        /// <param name="versDao"></param>
        /// <param name="mwstDao">Mehrwertsteuer-Ermittlugns DAO</param>
        /// <param name="quoteDao">Quote DAO</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="prismaServiceDao">prismaServiceDao</param>
        public KalkulationBo(IKalkulationDao pDao, IObTypDao obTypDao, IZinsDao zinsDao, IPrismaDao prismaDao, IAngAntDao angAntDao, IVGDao vgDao, IEurotaxWSDao etWsDao, IEurotaxDBDao etDbDao, IAuskunftDao auskDao, IKundeDao kunDao, IProvisionDao provisionDao, ISubventionDao subventionDao, IInsuranceDao versDao, IMwStDao mwstDao, IQuoteDao quoteDao, String isoCode, IPrismaServiceDao prismaServiceDao)
            : base(pDao, obTypDao, zinsDao, prismaDao, angAntDao, vgDao, etWsDao, etDbDao, auskDao, kunDao, provisionDao, subventionDao, versDao, mwstDao, quoteDao, prismaServiceDao)
        {
            this.isoCode = isoCode;
        }


        /// <summary>
        /// Neue Kalkulation erzeugen oder bestehende öffnen
        /// </summary>
        /// <param name="angVar">Updatestruktur, Wenn Primärschlüssel der Variante = 0 => neues erzeugen</param>
        /// <returns>Neues oder geöffnetes KalkulationDto</returns>
        override
        public AngAntVarDto createOrUpdateKalkulation(AngAntVarDto angVar)
        {
            if (angVar.sysangvar == 0)
            {
                AngAntVarDto Kalkulation = createKalkulation(angVar.sysangebot);
                return Kalkulation;

            }
            else
            {
                AngAntVarDto Kalkulation = updateKalkulation(angVar);
                return Kalkulation;
            }
        }

        /// <summary>
        /// Create Calculation
        /// </summary>
        /// <param name="SysID">Angebot Primary Key</param>
        /// <returns>New Offer Data</returns>
        override
        public AngAntVarDto createKalkulation(long SysID)
        {
            AngAntVarDto Kalkulation = pDao.createKalkulation(SysID);
            return Kalkulation;
        }

        /// <summary>
        /// Update Calculation
        /// </summary>
        /// <param name="sysVar">Variant ID</param>
        /// <returns>New Offer Data</returns>
        override
        public AngAntVarDto getKalkulation(long sysVar)
        {
            AngAntVarDto Kalkulation = pDao.getKalkulation(sysVar);
            return Kalkulation;

        }

        /// <summary>
        /// Updaten eines bestehenden Kalkulation Objekts
        /// </summary>
        /// <param name="kalkulation">Zu speichernde Kalkulation</param>
        /// <returns>Gespeicherte Kalkulation</returns>#
        override
        public AngAntVarDto updateKalkulation(AngAntVarDto kalkulation)
        {
            return pDao.updateKalkulation(kalkulation);
        }

        /// <summary>
        /// Kopieren einer Kalkulation
        /// </summary>
        /// <param name="kalkulation">Quellenkalkulation</param>
        /// <returns>Zielkalklulation</returns>
        override
        public AngAntVarDto copyKalkulation(AngAntVarDto kalkulation)
        {
            if (kalkulation.sysangvar == 0)
            {
                throw new Exception("Keine Kalkulation zum Kopieren angegeben.");
            }
            AngAntVarDto copied = this.getKalkulation(kalkulation.sysangvar);
            copied.sysangvar = 0;
            return copied;
        }

        /// <summary>
        /// Löschen einer Kalkulation
        /// </summary>
        /// <param name="kalkulation">Quellenkalkulation</param>
        /// <returns>Zielkalklulation</returns>
        override
        public void saveKalkulation(AngAntVarDto kalkulation)
        {
            pDao.updateKalkulation(kalkulation);
        }

        /// <summary>
        /// Löschen einer Kalkulation
        /// </summary>
        /// <param name="sysID">Primary Key</param>
        /// <returns>Zielkalklulation</returns>
        override
        public void deleteKalkulation(long sysID)
        {
            pDao.deleteKalkulation(sysID);
        }

      

        /// <summary>
        /// calculates the calculation
        /// </summary>
        /// <param name="kalkulation">kalkulations Daten</param>
        /// <param name="prodCtx">Produktions-Kontext</param>
        /// <param name="kalkCtx">KalkulationsKontext</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="rateError">Fehler bei Ratenberechnung</param>
        /// <returns>Kalkulationsdaten</returns>
        override
        public KalkulationDto calculate(KalkulationDto kalkulation, prKontextDto prodCtx, kalkKontext kalkCtx, string isoCode, ref byte rateError)
        {
            IBankNowCalculator calculator = new BankNowCalculator(pObTypDao, pZinsDao, pPrismaDao, pVGDao, pProvisionDao, pSubventionDao, pInsuranceDao, pMehrWertDao, quoteDao, isoCode, pDao, prismaServiceDao);
            
            return calculator.calculate(kalkulation, prodCtx, kalkCtx, ref rateError);
        }


        /// <summary>
        /// Calculates Provisions for Expected Loss Calculations
        /// uses a minimum required input interfaces
        /// </summary>
        /// <param name="prodCtx"></param>
        /// <param name="kundenScore"></param>
        /// <param name="finanzierungsbetrag"></param>
        /// <param name="zinsertrag"></param>
        /// <returns></returns>
        public override List<AngAntProvDto> calculateProvisionsDirect(prKontextDto prodCtx, string kundenScore, double finanzierungsbetrag, double zinsertrag)
        {
            IBankNowCalculator calculator = new BankNowCalculator(pObTypDao, pZinsDao, pPrismaDao, pVGDao, pProvisionDao, pSubventionDao, pInsuranceDao, pMehrWertDao, quoteDao, isoCode, pDao, prismaServiceDao);

            return calculator.calculateProvisionsDirect(prodCtx, kundenScore, finanzierungsbetrag, zinsertrag);
        }

        /// <summary>
        /// returns a "virtual" Prisma Product Parameter for the RAP Zins
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        override
            public Cic.OpenOne.Common.DTO.Prisma.ParamDto getRap(long sysprproduct)
        {
            IZinsBo zinsBo = new ZinsBo(pZinsDao, pPrismaDao, pObTypDao, ZinsBo.CONDITIONS_BANKNOW, isoCode, pVGDao);

            IntsDto step = null;

            List<IntstrctDto> intstrct = pZinsDao.getIntstrct();
            var q = from s in intstrct
                    where s.sysprproduct == sysprproduct
                    && s.validFrom <= DateTime.Now
                    orderby s.validFrom descending
                    select s;

            IntstrctDto strct = q.FirstOrDefault();
            if (strct != null)
            {
                if (strct.method != 3)
                {
                    List<IntsDto> rate = pZinsDao.getIntsrate();
                    step = (from i in rate
                            where i.sysintsdate == strct.sysintsdate
                            select i).FirstOrDefault();
                }
                else
                {
                    List<IntsDto> rate = pZinsDao.getIntsband();
                    step = (from i in rate
                            where i.sysintsdate == strct.sysintsdate
                            select i).FirstOrDefault();
                }
            }

            CIC.Database.PRISMA.EF6.Model.VTTYP vt = pPrismaDao.getVttyp(sysprproduct);
            if (step != null && vt != null && vt.CODE != null && vt.CODE.ToLower().IndexOf("casa") > -1)
            {
                //have casa-product
                String defaultZins = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", "B2C", "CASA_DEFAULT_ZINS", "5.9");
                step.intrate = Double.Parse(defaultZins, System.Globalization.CultureInfo.InvariantCulture);
            }

            Cic.OpenOne.Common.DTO.Prisma.ParamDto rval = new Cic.OpenOne.Common.DTO.Prisma.ParamDto();
            rval.meta = "ZINS_RAP";
            rval.disabled = false;
            rval.name = "Rap-Zins";
            rval.minvaln = 0;
            rval.maxvaln = 0;
            if (step != null)//needed for min max slider 
            {
                rval.minvaln = step.minrate;
                rval.maxvaln = step.maxrate;
                rval.defvaln = step.intrate;
            }
            else
            {
                rval.defvaln = 0;
            }

            return rval;
        }

        /// <summary>
        /// calculates the request calculation
        /// </summary>
        /// <param name="membershipInfo">service context</param>
        /// <param name="antrag">request to be calculated</param>
        override public AntragDto calculateAntrag(MembershipUserValidationInfo membershipInfo, AntragDto antrag)
        {
            string joker = "Joker";
            bool pruefungJokerneeded = true;
            byte rateError = 0;

            AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, membershipInfo.sysPUSER, antrag.sysid, DateTime.Now);
            AuthenticationBo.validateActivePerole(membershipInfo.sysPEROLE);

            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            IPruefungBo pruefungbo = BOFactory.getInstance().createPruefungBo();
            //recalc upon save!
            if (antrag.kalkulation != null)
            {
                prKontextDto pKontext = MyCreateProductKontext(membershipInfo.sysPEROLE, antrag);
                _log.Debug("Calculating on Antrag Update with context: " + _log.dumpObject(pKontext));

                if (antrag.kalkulation.angAntAblDto != null)
                {
                    foreach (AngAntAblDto abl in antrag.kalkulation.angAntAblDto)
                    {
                        if (abl.sysabltyp <= 0)
                        {
                            throw new ArgumentException("Einen zugelassenen Wert im Dropdown Art der Finanzierung auszuwählen", "sysabltyp");

                        }
                    }
                }

                if (pruefungbo.isJokerProduct(pKontext.sysprproduct))
                {
                    if (pruefungbo.isJokerWithAntrag(pKontext.sysprproduct, antrag.sysid) != true)//noch nicht als Joker gespeichet
                    {
                        if (!pruefungbo.isJokerFree(pKontext.sysprproduct, membershipInfo.sysPEROLE, antrag.sysprjoker, DateTime.Now))//aktuell Joker verfügbar?
                        {
                            throw new ApplicationException("Joker steht nicht mehr zur Verfügung. Bitte ein anderes Produkt auswählen");
                        }
                    }
                    else
                    {
                        pruefungJokerneeded = false;
                    }
                }

                kalkKontext kKontext = new kalkKontext();
                if (antrag.angAntObDto != null)
                {
                    kKontext.grundBrutto = antrag.angAntObDto.grundBrutto;
                    kKontext.zubehoerBrutto = antrag.angAntObDto.zubehoerBrutto;
                    kKontext.ubnahmeKm = antrag.angAntObDto.ubnahmeKm;
                    kKontext.erstzulassung = antrag.angAntObDto.erstzulassung;
                }
                KalkulationDto kalk = calculate(antrag.kalkulation, pKontext, kKontext, isoCode, ref rateError);
                antrag.kalkulation = kalk;
                if (antrag.angAntObDto != null)
                {
                    antrag.angAntObDto.satzmehrKm = kalk.angAntKalkDto.ob_mark_satzmehrkm;
                }
            }
            _log.Debug("Duration Kalkulation: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

            IKundeBo kundeBo = BOFactory.getInstance().createKundeBo();
            if (antrag.kunde != null)
            {
                if (antrag.kunde.kontos != null)
                {
                    foreach (KontoDto kto in antrag.kunde.kontos)
                    {
                        kto.sysantrag = antrag.sysid;
                    }
                }

                antrag.kunde = kundeBo.createOrUpdateKunde(antrag.kunde, membershipInfo.sysPEROLE);
                pKunDao.altePKZLeerenBysysAntrag(antrag.sysid, antrag.kunde.sysit);
            }
            _log.Debug("Duration Kunde: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if (antrag.mitantragsteller != null)
            {
                if (antrag.mitantragsteller.kontos != null)
                {
                    foreach (KontoDto kto in antrag.mitantragsteller.kontos)
                    {
                        kto.sysantrag = antrag.sysid;
                    }
                }
                antrag.mitantragsteller = kundeBo.createOrUpdateKunde(antrag.mitantragsteller, membershipInfo.sysPEROLE);
            }
            _log.Debug("Duration MA: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

            IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
            antrag = bo.createOrUpdateAntrag(antrag, membershipInfo.sysPEROLE);
            _log.Debug("Duration Antrag: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

            if (antrag.kalkulation != null && antrag.kalkulation.angAntKalkDto != null && pruefungJokerneeded)
            {
                long jokerpruefung = pruefungbo.jokerPruefung(antrag.sysid, antrag.kalkulation.angAntKalkDto.sysprproduct, joker, membershipInfo.sysPEROLE, antrag.sysprjoker);
                antrag.sysprjoker = jokerpruefung;
            }
            throwErrorMessages(rateError);
            return antrag;
        }


        /// <summary>
        /// Analyzes the calculation Errors and throws the corresponding Exception, if any error occured
        /// </summary>
        /// <param name="rateError">error code</param>
        override public void throwErrorMessages(byte rateError)
        {
            if (rateError == 0)
            {
                return;
            }

            String errorMessage = ExceptionMessages.F_00005_NegativeRateException;
            String errorPosition = "";
            if ((rateError & 1) == 1)
            {
                errorPosition += "Basis Ratenberechnung";
            }
            if ((rateError & 2) == 2)
            {
                if (errorPosition.Length > 0)
                {
                    errorPosition += ", ";
                }
                errorPosition += "Differenzleasing Ratenberechnung";
            }
            if ((rateError & 4) == 4)
            {
                if (errorPosition.Length > 0)
                {
                    errorPosition += ", ";
                }
                errorPosition += "RAP Min Ratenberechnung";
            }
            if ((rateError & 8) == 8)
            {
                if (errorPosition.Length > 0)
                {
                    errorPosition += ", ";
                }
                errorPosition += "RAP Max Ratenberechnung";
            }
            if (errorPosition.Length > 0)
            {
                errorMessage += errorPosition;
                throw new CalculatorException("F_00005_NegativeRateException", errorMessage, OpenOne.Common.DTO.MessageType.Info);

            }
            if ((rateError & 16) == 16)
            {
                throw new CalculatorException("F_00006_SZSmallerRateException", ExceptionMessages.F_00006_SZSmallerRateException, OpenOne.Common.DTO.MessageType.Info);

            }
            if ((rateError & 32) == 32)
            {
                throw new CalculatorException("F_00007_SZRWGreatPurchPExc", ExceptionMessages.F_00007_SZRWGreatPurchPExc, OpenOne.Common.DTO.MessageType.Info);

            }
        }

        #region Private Methods

        private static OpenOne.Common.DTO.Prisma.prKontextDto MyCreateProductKontext(long sysPEROLE, DTO.AntragDto antrag)
        {
            OpenOne.Common.DTO.Prisma.prKontextDto pKontext = new OpenOne.Common.DTO.Prisma.prKontextDto();

            pKontext.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
            pKontext.sysperole = sysPEROLE;
            if (antrag.kalkulation == null || antrag.kalkulation.angAntKalkDto == null)
            {
                pKontext.sysprproduct = 0;
            }
            else
            {
                pKontext.sysprproduct = antrag.kalkulation.angAntKalkDto.sysprproduct;
                pKontext.sysprusetype = antrag.kalkulation.angAntKalkDto.sysobusetype;
            }
            pKontext.sysbrand = 0;// antrag.sysbrand;
            if (antrag.kunde != null)
            {
                pKontext.syskdtyp = antrag.kunde.syskdtyp;
            }
            if (antrag.angAntObDto != null)
            {
                pKontext.sysobart = antrag.angAntObDto.sysobart;
                pKontext.sysobtyp = antrag.angAntObDto.sysobtyp;
            }
            pKontext.sysprchannel = antrag.sysprchannel == null ? 0 : antrag.sysprchannel.Value;
            pKontext.sysprhgroup = antrag.sysprhgroup == null ? 0 : antrag.sysprhgroup.Value;

            return pKontext;
        }



        #endregion Private Methods
    }


}
