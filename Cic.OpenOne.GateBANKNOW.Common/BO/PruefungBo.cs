using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public class PruefungBo : AbstractPruefungBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public PruefungBo(IPrismaDao pDao, IObTypDao obDao, ITranslateDao transDao, IPruefungDao pruefungDao)
            : base(pDao, obDao, transDao, pruefungDao)
        {
           
        }


        /// <summary>
        /// Returns a Map containing informations about the joker-produkts in the products list
        /// </summary>
        /// <param name="products">products</param>
        /// <param name="context">context</param>
        /// <returns></returns>
        override public JokerPruefungDto analyzeJokerProducts(AvailableProduktDto[] products, prKontextDto context)
        {
            List<JokerProductMap> resultmap = new List<JokerProductMap>();
            List<long> jokerProducts = pDao.getAllJokerProducts();
            JokerPruefungDto resultPruefung = new JokerPruefungDto();
            List<AvailableProduktDto> resultProducts = new List<AvailableProduktDto>();
            


         
            int offenenjokerAnzahl = 0;
            int jokerAnzahl = 0; ;
            foreach (AvailableProduktDto p in products)
            {

                if (jokerProducts.Contains(p.sysID))
                {
                    offenenjokerAnzahl = pruefungDao.getOffeneJokerAnzahl(p.sysID, context.sysperole, context.sysprjoker, Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(context.perDate));
                    if (offenenjokerAnzahl > 0)
                    {
                        _log.Debug("Offene Jokeranzahl > 0 für sysprproduct: " + p.sysID);
                        JokerProductMap map = new JokerProductMap();
                        //Anzahl der offenen Joker für das jeweilige Produkt und die maximale Anzahl der Joker
                        jokerAnzahl = pruefungDao.getJokerAnzahl(p.sysID, context.sysperole);
                        map.name = " (" + offenenjokerAnzahl.ToString() + "/" + jokerAnzahl + ")";
                        map.sysprproduct = p.sysID;
                        resultmap.Add(map);
                        p.bezeichnung += map.name;
                        _log.Debug("PRPRODTYPE Joker :" + p.bezeichnung);
                        resultProducts.Add(p);
                    }
                }
                else
                {
                    resultProducts.Add(p);
                }
            }

            resultPruefung.jokerproductsmap = resultmap;
            resultPruefung.products = resultProducts;
            return resultPruefung;
        }



        /// <summary>
        /// Inserts Antrag in PRJOKER table and returns sysid of joker when product is a jokerproduct and exists a available joker for Händler otherwise 0.
        /// </summary>
        /// <param name="sysantrag">antrag</param>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <param name="name">name</param>
        /// <param name="sysperole">sysperole</param>
        /// <param name="sysprjoker">sysprjoker</param>
        /// <returns></returns>
        override public long jokerPruefung(long sysantrag, long sysprproduct, string name, long sysperole, long sysprjoker)
        {
            sysperole = pruefungDao.getSysVM(sysperole);

            if (pruefungDao.isJokerProduct(sysprproduct))
            {
                pruefungDao.setJokerFreiWithAntrag(sysantrag);
                return pruefungDao.updatePrjoker(sysantrag, sysperole, sysprproduct, sysprjoker);
            }
            else
            {
                pruefungDao.setJokerFreiWithAntrag(sysantrag);
                return 0;
            }

        }

        /// <summary>
        /// Returns true when product is a JokerProduct
        /// </summary>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <returns></returns>
        override public bool isJokerProduct(long sysprproduct)
        {
            return pruefungDao.isJokerProduct(sysprproduct);

        }

        /// <summary>
        /// Returns true when Antrag is in PRJOKER table.
        /// </summary>
        /// <param name="sysprproduct">sysprproduct,</param>
        /// <param name="sysantrag">sysantrag</param>
        /// <returns></returns>
        override public bool isJokerWithAntrag(long sysprproduct, long sysantrag)
        {
            return pruefungDao.isJokerWithAntrag(sysprproduct, sysantrag);

        }

        /// <summary>
        /// Returns true when Joker is available for Product
        /// </summary>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <param name="sysperole">sysperole</param>
        /// <param name="sysprjoker">sysprjoker</param>
        /// <param name="perDate">perDate</param>
        /// <returns></returns>
        override public bool isJokerFree(long sysprproduct, long sysperole, long sysprjoker, DateTime perDate)
        {
            sysperole = pruefungDao.getSysVM(sysperole);

            return pruefungDao.getOffeneJokerAnzahl(sysprproduct, sysperole, sysprjoker, perDate) >0;

        }
    }
}
