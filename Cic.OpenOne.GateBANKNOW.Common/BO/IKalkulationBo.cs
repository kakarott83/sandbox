using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Kalkulations BO Schnittstelle
    /// </summary>
    public interface IKalkulationBo
    {

        /// <summary>
        /// Neue Kalkulation erzeugen oder bestehende öffnen
        /// </summary>
        /// <param name="angVar">Updatestruktur, Wenn Primärschlüssel der Variante = 0 => neues erzeugen</param>
        /// <returns>Neues oder geöffnetes KalkulationDto</returns>
        AngAntVarDto createOrUpdateKalkulation(AngAntVarDto angVar);

        /// <summary>
        /// Neues Kalkulation Objekt erzeugen
        /// </summary>
        /// <returns>Neu Kalkulation</returns>
        AngAntVarDto createKalkulation(long sysID);
        
        /// <summary>
        /// Betehende Kalkulation holen
        /// </summary>
        /// <param name="sysVar">SysID der Variante</param>
        /// <returns></returns>
        AngAntVarDto getKalkulation(long sysVar);

        /// <summary>
        /// Updaten eines bestehenden Kalkulation Objekts
        /// </summary>
        /// <param name="kalkulation">Zu speichernde Kalkulation</param>
        /// <returns>Gespeicherte Kalkulation</returns>
        AngAntVarDto updateKalkulation(AngAntVarDto kalkulation);

        /// <summary>
        /// Kopieren einer Kalkulation
        /// </summary>
        /// <param name="kalkulation">Quellenkalkulation</param>
        /// <returns>Zielkalklulation</returns>
        AngAntVarDto copyKalkulation(AngAntVarDto kalkulation);

        /// <summary>
        /// Speichern einer Kalkulation
        /// </summary>
        /// <param name="kalkulation">Quellenkalkulation</param>
        /// <returns>Zielkalklulation</returns>
        void saveKalkulation(AngAntVarDto kalkulation);

        /// <summary>
        /// Löschen einer Kalkulation
        /// </summary>
        /// <param name="sysVar">Primaärschlüssel der Variante</param>
        /// <returns>Zielkalklulation</returns>
        void deleteKalkulation(long sysVar);

        /// <summary>
        /// calculates the calculation
        /// </summary>
        /// <param name="kalkulation">Kalkulationsdaten</param>
        /// <param name="prodCtx">Produktions-Kontext</param>
        /// <param name="kalkCtx">Kalkulations-Kontext</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="rateError">Fehler bei Ratenberechnung</param>
        /// <returns>kalkulationsdaten</returns>
        KalkulationDto calculate(KalkulationDto kalkulation, prKontextDto prodCtx, kalkKontext kalkCtx, string isoCode, ref byte rateError);

        /// <summary>
        /// returns a "virtual" Prisma Product Parameter for the RAP Zins
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        Cic.OpenOne.Common.DTO.Prisma.ParamDto getRap(long sysprproduct);

        /// <summary>
        /// calculates the request calculation
        /// </summary>
        /// <param name="membershipInfo">service context</param>
        /// <param name="antrag">request to be calculated</param>
        AntragDto calculateAntrag(MembershipUserValidationInfo membershipInfo, AntragDto antrag);

        /// <summary>
        /// Analyzes the calculation Errors and throws the corresponding Exception, if any error occured
        /// </summary>
        /// <param name="rateError">error code</param>
        void throwErrorMessages(byte rateError);



        /// <summary>
        /// Calculates Provisions for Expected Loss Calculations
        /// uses a minimum required input interfaces
        /// </summary>
        /// <param name="prodCtx"></param>
        /// <param name="kundenScore"></param>
        /// <param name="finanzierungsbetrag"></param>
        /// <param name="zinsertrag"></param>
        /// <returns></returns>
        List<AngAntProvDto> calculateProvisionsDirect(prKontextDto prodCtx, String kundenScore, double finanzierungsbetrag, double zinsertrag);
    }
}
