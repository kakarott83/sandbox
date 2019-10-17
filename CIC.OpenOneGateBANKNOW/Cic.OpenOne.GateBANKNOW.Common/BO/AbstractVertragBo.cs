using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abstrakte Vertrags Business Objekt Klasse
    /// </summary>
    public abstract class AbstractVertragBo : IVertragBo
    {
        /// <summary>
        /// Vertrags Dao
        /// </summary>
        protected IVertragDao vtdao;

        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IEaihotDao eaihotDao;

        /// <summary>
        /// 
        /// </summary>
        protected IObTypDao obTypDao;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="vtdao">Vertrags Dao</param>
        /// <param name="eaihotDao">Eaihot Dao</param>
        /// <param name="obTypDao">Obtyp Dao</param>
        public AbstractVertragBo(IVertragDao vtdao, IEaihotDao eaihotDao, IObTypDao obTypDao)
        { 
            this.vtdao = vtdao;
            this.eaihotDao = eaihotDao;
            this.obTypDao = obTypDao;
        }

        /// <summary>
        /// Vertrag via ID auslesen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>Vertragsdaten</returns>
        public abstract VertragDto getVertrag(long sysid);

        /// <summary>
        /// Returns true when change of restwert rechnung empfänger allowed
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns></returns>
        public abstract bool isRREChangeAllowed(long sysid, long sysperole);

        /// <summary>
        /// performKaufofferte
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        public abstract operformKaufofferte performKaufofferte(iperformKaufofferteDto input);

        /// <summary>
        /// changeRReceiver / Änderung des Restwertrechnungsempfänger
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        public abstract ochangeRRReceiver changeRRReceiver(long sysid);

        /// <summary>
        /// Returns Prüfung auf change Restwertrechnungsempfänger möglich, pendente Auflösung, Restwertrechnung verschickt
        /// </summary>
        /// <param name="result">result</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns></returns>
        public abstract oSearchDto<VertragDto> zustandPruefung(oSearchDto<VertragDto> result, long sysperole);

        /// <summary>
        /// Returns true when order purchase offer in ePOS-now allowed
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        public abstract bool isPerformKaufofferteAllowed(long sysid);


        /// <summary>
        /// gets contract details by its id.
        /// function created so that we can change the query and thus decide what data is given in what field, customizable.
        /// </summary>
        /// <param name="sysvt">contract id</param>
        /// <returns>contract details</returns>
        public abstract VertragDto getVertragForExtension(long sysvt);


        /// <summary>
        /// get MWST by sysvt
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public abstract double getMWST(long sysvt, DateTime perDate);
    }
}
