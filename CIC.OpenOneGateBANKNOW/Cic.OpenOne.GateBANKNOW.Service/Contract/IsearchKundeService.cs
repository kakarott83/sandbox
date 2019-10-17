using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    using DTO;

    /// <summary>
    /// Das Interface searchKundeService stellt die Methoden für die Kundensuche bereit
    /// </summary>
    [ServiceContract(Name = "IsearchKundeService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IsearchKundeService
    {
        /// <summary>
        /// Findet Händlerkunden anhand Filterbedingung und berücksichtigt Sortierung und Pagination
        /// </summary>
        /// <param name="input">isearchKundeDto</param>
        /// <returns>osearchKundeDto</returns>
        [OperationContract]
        DTO.osearchKundeDto searchKunde(DTO.isearchKundeDto input);

        /// <summary>
        /// Liefert alle relevanten Kundenstamm- und Zusatzdaten
        /// </summary>
        /// <param name="input">igetKundenDetailDto</param>
        /// <returns>ogetKundenDetailDto</returns>
        [OperationContract]
        DTO.ogetKundeDetailDto getKundeDetail(DTO.igetKundeDetailDto input);

        /// <summary>
        /// Liefert PKZ/UKZ aus dem letzten Antrag im Zustand 'Vertrag aktiviert' 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        DTO.ogetZusatzDatenAktivDto getZusatzDatenAktiv(DTO.igetZusatzDatenAktivDto input);

        /// <summary>
        /// Findet KundeExtern anhand Filterbedingung und berücksichtigt Sortierung und Pagination
        /// </summary>
        /// <param name="input">isearchKundeDto</param>
        /// <returns>osearchKundeDto</returns>
        [OperationContract]
        DTO.osearchKundeExternDto searchKundeExtern(DTO.isearchKundeExternDto input);

        /// <summary>
        /// Findet externe Kunden anhand von bestimmten Sucheingaben
        /// </summary>
        /// <param name="input">isearchKundeExternNonGenericDto</param>
        /// <returns>osearchKundeExternNonGenericDto</returns>
        [OperationContract]
        osearchKundeExternNonGenericDto searchKundeExternNonGeneric(isearchKundeExternNonGenericDto input);
    }
}
