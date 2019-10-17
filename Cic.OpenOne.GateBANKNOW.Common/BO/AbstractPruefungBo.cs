using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public abstract class AbstractPruefungBo : IPruefungBo
    {
         /// <summary>
        /// Prisma Data Access Object
        /// </summary>
        protected IPrismaDao pDao;

        /// <summary>
        /// Object Type Data Access Object
        /// </summary>
        protected IObTypDao obDao;

        /// <summary>
        /// Object Type Data Access Object
        /// </summary>
        protected ITranslateDao translateDao;

        /// <summary>
        /// Object Type Data Access Object
        /// </summary>
        protected IPruefungDao pruefungDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao">pDao</param>
        /// <param name="obDao">obDao</param>
        /// <param name="translateDao">translateDao</param>
        /// <param name="pruefungDao">pruefungDao</param>
        public AbstractPruefungBo(IPrismaDao pDao, IObTypDao obDao, ITranslateDao translateDao, IPruefungDao pruefungDao)
        {
            this.pDao = pDao;
            this.obDao = obDao;
            this.translateDao = translateDao;
            this.pruefungDao = pruefungDao;

        }

        /// <summary>
        /// Returns a Map containing informations about the joker-produkts in the products list
        /// </summary>
        /// <param name="products">products</param>
        /// <param name="context">context</param>
        /// <returns></returns>
        public abstract JokerPruefungDto analyzeJokerProducts(AvailableProduktDto[] products, prKontextDto context);

        /// <summary>
        /// Inserts Antrag in PRJOKER table and returns sysid of joker when product is a jokerproduct and exists a available joker for Händler otherwise 0.
        /// </summary>
        /// <param name="sysantrag">antrag</param>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <param name="name">name</param>
        /// <param name="sysperole">sysperole</param>
        /// <param name="sysprjoker">sysprjoker</param>
        /// <returns></returns>
        public abstract long jokerPruefung(long sysantrag, long sysprproduct, string name, long sysperole, long sysprjoker);


        /// <summary>
        /// Returns true when product is a JokerProduct
        /// </summary>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <returns></returns>
        public abstract bool isJokerProduct(long sysprproduct);


        /// <summary>
        /// Returns true when Joker is available for Product
        /// </summary>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <param name="sysperole">sysperole</param>
        /// <param name="sysprjoker">sysprjoker</param>
        /// <param name="perDate">perDate</param>
        /// <returns></returns>
        public abstract bool isJokerFree(long sysprproduct, long sysperole, long sysprjoker, DateTime perDate);

        /// <summary>
        /// Returns true when Antrag is in PRJOKER table.
        /// </summary>
        /// <param name="sysprproduct">sysprproduct,</param>
        /// <param name="sysantrag">sysantrag</param>
        /// <returns></returns>
        public abstract bool isJokerWithAntrag(long sysprproduct, long sysantrag);
    }
}
