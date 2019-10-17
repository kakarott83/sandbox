using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abstraktes 
    /// </summary>
    [System.CLSCompliant(false)]
    public abstract class AbstractKalkulationBo : IKalkulationBo
    {
        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IKalkulationDao pDao;
        /// <summary>
        /// Object type DAO
        /// </summary>
        protected IObTypDao pObTypDao;
        /// <summary>
        /// Zins DAO
        /// </summary>
        protected IZinsDao pZinsDao;
        /// <summary>
        /// Prisma DAO
        /// </summary>
        protected IPrismaDao pPrismaDao;
        /// <summary>
        /// AngebotAntrag DAO
        /// </summary>
        protected IAngAntDao pAngAntDao;
        /// <summary>
        /// VG DAO
        /// </summary>
        protected IVGDao pVGDao;
        /// <summary>
        /// Eurotax Webservice DAO
        /// </summary>
        protected IEurotaxWSDao pEtWsDao;
        /// <summary>
        /// Eurotax Datenbank DAO
        /// </summary>
        protected IEurotaxDBDao pEtDbDao;
        /// <summary>
        /// Auskunft DAO
        /// </summary>
        protected IAuskunftDao pAuskDao;
        /// <summary>
        /// Kunde DAO
        /// </summary>
        protected IKundeDao pKunDao;

        /// <summary>
        /// Provisions DAO
        /// </summary>
        protected IProvisionDao pProvisionDao;

        /// <summary>
        /// Provisions DAO
        /// </summary>
        protected ISubventionDao pSubventionDao;

        /// <summary>
        /// Provisions DAO
        /// </summary>
        protected IInsuranceDao pInsuranceDao;

        /// <summary>
        /// Mehrwertsteuer-Ermittlungs DAO
        /// </summary>
        protected IMwStDao pMehrWertDao;
        
        /// <summary>
        /// Mehrwertsteuer-Ermittlungs DAO
        /// </summary>
        protected IQuoteDao quoteDao;

        /// <summary>
        /// Prisma Services dao
        /// </summary>
        protected IPrismaServiceDao prismaServiceDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="KalkulationDao">Kalkulation Data Access Object</param>
        /// <param name="obTypDao">Object type DAO</param>
        /// <param name="zinsDao">Zins Dao</param>
        /// <param name="prismaDao">Prisma DAO</param>
        /// <param name="angAntDao">Angebot Antrag DAO</param>
        /// <param name="vgDao">VG DAO</param>
        /// <param name="etWsDao">Eurotax WebService DAO</param>
        /// <param name="etDbDao">Eurotax DB DAO</param>
        /// <param name="auskDao">Auskunft DAO</param>
        /// <param name="kunDao">Kunde DAO</param>
        /// <param name="provisionDao">Provisions DAO</param>
        /// <param name="pSubventionDao">Subventions DAO</param>
        /// <param name="pInsuranceDao">Versicherungs DAO</param>
        /// <param name="pMwStDao">Mehrwertsteuer Ermittlungs DAO</param>
        /// <param name="quoteDao">Quote DAO</param>
        /// <param name="prismaServiceDao">prismaServiceDao</param>
        public AbstractKalkulationBo(IKalkulationDao KalkulationDao, IObTypDao obTypDao, IZinsDao zinsDao, IPrismaDao prismaDao, IAngAntDao angAntDao, IVGDao vgDao, IEurotaxWSDao etWsDao, IEurotaxDBDao etDbDao, IAuskunftDao auskDao, IKundeDao kunDao, IProvisionDao provisionDao, ISubventionDao pSubventionDao, IInsuranceDao pInsuranceDao, IMwStDao pMwStDao, IQuoteDao quoteDao, IPrismaServiceDao prismaServiceDao)
        {
            this.pDao = KalkulationDao;
            this.pObTypDao = obTypDao;
            this.pZinsDao = zinsDao;
            this.pPrismaDao = prismaDao;
            this.pAngAntDao = angAntDao;
            this.pVGDao = vgDao;
            this.pEtWsDao = etWsDao;
            this.pEtDbDao = etDbDao;
            this.pAuskDao = auskDao;
            this.pKunDao = kunDao;
            this.pProvisionDao = provisionDao;
            this.pSubventionDao = pSubventionDao;
            this.pInsuranceDao = pInsuranceDao;
            this.pMehrWertDao = pMwStDao;
            this.quoteDao = quoteDao;
            this.prismaServiceDao = prismaServiceDao;

        }

        /// <summary>
        /// Neue Kalkulation erzeugen oder bestehende öffnen
        /// </summary>
        /// <param name="angVar">Updatestruktur, Wenn Primärschlüssel der Variante = 0 => neues erzeugen</param>
        /// <returns>Neues oder geöffnetes Kalkulation Data Object</returns>
        public abstract AngAntVarDto createOrUpdateKalkulation(AngAntVarDto angVar);

        /// <summary>
        /// Neue Kalkulation erzeugen
        /// </summary>
        /// <returns>Neues Kalkulation Data Object</returns>
        public abstract AngAntVarDto createKalkulation(long sysID);

        /// <summary>
        /// Bestehende Kalkulation laden
        /// </summary>
        /// <returns>Geöffnetes Kalkulation Data Object</returns>
        public abstract AngAntVarDto getKalkulation(long sysVar);

        /// <summary>
        /// Updaten eines bestehenden Kalkulation Objekts
        /// </summary>
        /// <param name="kalkulation">Zu speichernde Kalkulation</param>
        /// <returns>Gespeicherte Kalkulation</returns>
        public abstract AngAntVarDto updateKalkulation(AngAntVarDto kalkulation);

        /// <summary>
        /// Kopieren einer Kalkulation
        /// </summary>
        /// <param name="kalkulation">Quellenkalkulation</param>
        /// <returns>Zielkalklulation</returns>
        public abstract AngAntVarDto copyKalkulation(AngAntVarDto kalkulation);

        /// <summary>
        /// Speichern einer Kalkulation
        /// </summary>
        /// <param name="kalkulation">Quellenkalkulation</param>
        /// <returns>Zielkalklulation</returns>
        public abstract void saveKalkulation(AngAntVarDto kalkulation);

        /// <summary>
        /// Löschen einer Kalkulation
        /// </summary>
        /// <param name="sysID">ID der zu löschenden Kalkulation</param>
        /// <returns>Zielkalklulation</returns>
        public abstract void deleteKalkulation(long sysID);

        /// <summary>
        /// calculates the calculation
        /// </summary>
        /// <param name="kalkulation">Kalkulationsdaten</param>
        /// <param name="prodCtx">Produktions-Kontext</param>
        /// <param name="kalkCtx">Kalkulations-Kontext</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="rateError">Fehler bei Ratenberechnung</param>
        /// <returns></returns>
        public abstract KalkulationDto calculate(KalkulationDto kalkulation, prKontextDto prodCtx, kalkKontext kalkCtx, string isoCode, ref byte rateError);


        /// <summary>
        /// Calculates Provisions for Expected Loss Calculations
        /// uses a minimum required input interfaces
        /// </summary>
        /// <param name="prodCtx"></param>
        /// <param name="kundenScore"></param>
        /// <param name="finanzierungsbetrag"></param>
        /// <param name="zinsertrag"></param>
        /// <returns></returns>
        abstract public List<AngAntProvDto> calculateProvisionsDirect(prKontextDto prodCtx, String kundenScore, double finanzierungsbetrag, double zinsertrag);

        /// <summary>
        /// returns a "virtual" Prisma Product Parameter for the RAP Zins
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        public abstract Cic.OpenOne.Common.DTO.Prisma.ParamDto getRap(long sysprproduct);

        /// <summary>
        /// calculates the request calculation
        /// </summary>
        /// <param name="membershipInfo">service context</param>
        /// <param name="antrag">request to be calculated</param>
        public abstract AntragDto calculateAntrag(MembershipUserValidationInfo membershipInfo, AntragDto antrag);

        /// <summary>
        /// Analyzes the calculation Errors and throws the corresponding Exception, if any error occured
        /// </summary>
        /// <param name="rateError">error code</param>
        public abstract void throwErrorMessages(byte rateError);
    }
}

