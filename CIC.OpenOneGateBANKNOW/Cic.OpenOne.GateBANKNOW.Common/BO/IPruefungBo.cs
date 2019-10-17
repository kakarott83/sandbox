using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Joker-Produkt Prüpfung und antragsreferenz in der Joker-Tabelle angelegt.
    /// True = keine Joker-Produkt oder Joker-Produkt und Antrag in PRJOKER tabelle eingetragen.
    /// False = Keine offene Joker für das Produkt verfügbar, Antrag Verweis Eintrag nicht möglicht 
    /// </summary>
    public interface IPruefungBo
    {
        /// <summary>
        /// Returns a Map containing informations about the joker-produkts in the products list
        /// </summary>
        /// <param name="products">products</param>
        /// <param name="context">context</param>
        /// <returns></returns>
        JokerPruefungDto analyzeJokerProducts(AvailableProduktDto[] products, prKontextDto context);

        /// <summary>
        /// Inserts Antrag in PRJOKER table and returns sysid of joker when product is a jokerproduct and exists a available joker for Händler otherwise 0.
        /// </summary>
        /// <param name="sysantrag">antrag</param>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <param name="name">name</param>
        /// <param name="sysperole">sysperole</param>
        /// <param name="sysprjoker">sysprjoker</param>
        /// <returns></returns>
        long jokerPruefung(long sysantrag, long sysprproduct, string name, long sysperole, long sysprjoker);

        /// <summary>
        /// Returns true when product is a JokerProduct
        /// </summary>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <returns></returns>
        bool isJokerProduct(long sysprproduct);


        /// <summary>
        /// Returns true when Joker is available for Product
        /// </summary>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <param name="sysperole">sysperole</param>
        /// <param name="sysprjoker">sysprjoker</param>
        /// <param name="perDate">perDate</param>
        /// <returns></returns>
        bool isJokerFree(long sysprproduct, long sysperole, long sysprjoker, DateTime perDate);

        /// <summary>
        /// Returns true when Antrag is in PRJOKER table.
        /// </summary>
        /// <param name="sysprproduct">sysprproduct,</param>
        /// <param name="sysantrag">sysantrag</param>
        /// <returns></returns>
        bool isJokerWithAntrag(long sysprproduct, long sysantrag);
       
    }
}
