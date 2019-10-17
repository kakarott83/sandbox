using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public interface IPruefungDao
    {
        /// <summary>
        /// Returns number of Available Products filtered by conditiontypes give
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="sysperole"></param>
        /// <param name="sysprjoker"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        int getOffeneJokerAnzahl(long sysprproduct, long sysperole, long sysprjoker, DateTime perDate);

        /// <summary>
        /// Returns number of Jokers [for Händler] 
        /// </summary>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns></returns>
        int getJokerAnzahl(long sysprproduct, long sysperole);

        /// <summary>
        /// Inserts Antrag in PRJOKER table and returns sysid of joker when  a available exists joker for Händler otherwise exception.
        /// </summary>
        /// <param name="sysantrag">sysantrag</param>
        /// <param name="sysperole">sysperole</param>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <param name="sysprjoker">sysprjoker</param>
        /// <returns></returns>
        long updatePrjoker(long sysantrag, long sysperole, long sysprproduct, long sysprjoker);

        /// <summary>
        /// Returns true when product is a JokerProduct
        /// </summary>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <returns></returns>
        bool isJokerProduct(long sysprproduct);

        /// <summary>
        /// Delete Antrag reference in joker table (Makes joker available)
        /// </summary>
        /// <param name="sysantrag">sysantrag</param>
        void setJokerFreiWithAntrag(long sysantrag);


        /// <summary>
        /// Returns true when Antrag is in PRJOKER table.
        /// </summary>
        /// <param name="sysprproduct">sysprproduct,</param>
        /// <param name="sysantrag">sysantrag</param>
        /// <returns></returns>
        bool isJokerWithAntrag(long sysprproduct, long sysantrag);


        /// <summary>
        /// Returns (Vermittler) = Person von Händler (Parent vom Verkäufer)
        /// </summary>
        /// <param name="sysperole">sysperole</param>
        /// <returns></returns>
        long getSysVM(long sysperole);

    }
}
