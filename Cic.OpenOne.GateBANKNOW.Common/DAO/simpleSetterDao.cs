using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Data access object for setting simple fields
    /// </summary>
    public class SimpleSetterDao : ISimpleSetterDao
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// set Geschaeftsart
        /// </summary>
        public string setGeschaeftsart(long sysprchannel)
        {
            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                string name = null;
                    BCHANNEL channel = (from chnl in olCtx.BCHANNEL
                                        where chnl.SYSBCHANNEL == sysprchannel
                                        select chnl).FirstOrDefault();
                    if (channel != null)
                    {
                        name = channel.NAME + " vermittelt";
                    }
                    return name;
            }
        }

        /// <summary>
        /// set Farbe
        /// </summary>
        public void setFarbe(long sysID, string Farbe)
        {
            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                ANTOB antob = (from ant in olCtx.ANTOB
                                       where ant.SYSANTRAG == sysID
                                       select ant).FirstOrDefault();
                _log.Debug("Set Antrag Farbe");
                if (antob != null)
                {
                    antob.FARBEA = Farbe;
                    olCtx.SaveChanges();
                }
            }
        }

        /// <summary>
        /// set Kontrollschild
        /// </summary>
        public void setKontrollschild(long sysID, string Kontrollschild)
        {
            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                ANTOB antob = (from ant in olCtx.ANTOB
                               where ant.SYSANTRAG == sysID
                                 select ant).FirstOrDefault();
                _log.Debug("Set Antrag Kontrollschild");
                if (antob != null)
                {
                    antob.KENNZEICHEN = Kontrollschild;
                    olCtx.SaveChanges();
                }
            }
        }

        /// <summary>
        /// set Stammnummer
        /// </summary>
        public void setStammnummer(long sysID, string Stammnummer)
        {
            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                ANTOB antob = (from ant in olCtx.ANTOB
                               where ant.SYSANTRAG == sysID
                                 select ant).FirstOrDefault();
                if (antob != null)
                {
                    ANTOBBRIEF brief = (from antb in olCtx.ANTOBBRIEF
                                        where antb.SYSANTOBBRIEF == antob.SYSOB
                                        select antb).FirstOrDefault();
                    if (brief != null)
                    {
                        _log.Debug("Set Antrag Chassisnummer");
                        brief.STAMMNUMMER = Stammnummer;
                        olCtx.SaveChanges();
                    }
                    else
                    {
                        _log.Debug("Add new Antobbrief and set Antrag Stammnummer");
                        ANTOBBRIEF newBrief = new ANTOBBRIEF();
                        olCtx.ANTOBBRIEF.Add(newBrief);
                        newBrief.STAMMNUMMER = Stammnummer;
                        newBrief.ANTOB = antob;
                        olCtx.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// set Chassisnummer
        /// </summary>
        public void setChassisnummer(long sysID, string Chassisnummer)
        {
            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                ANTOB antob = (from ant in olCtx.ANTOB
                               where ant.SYSANTRAG == sysID
                                 select ant).FirstOrDefault();
                if (antob != null)
                {
                    ANTOBBRIEF brief = (from antb in olCtx.ANTOBBRIEF
                                        where antb.SYSANTOBBRIEF == antob.SYSOB
                                        select antb).FirstOrDefault();

                    if (brief != null)
                    {
                        _log.Debug("Set Antrag Chassisnummer");
                        brief.FIDENT = Chassisnummer;
                        olCtx.SaveChanges();
                    }
                    else
                    {
                        _log.Debug("Add new Antobbrief and set Antrag Chassisnummer");
                        ANTOBBRIEF newBrief = new ANTOBBRIEF();
                        olCtx.ANTOBBRIEF.Add(newBrief);
                        newBrief.FIDENT = Chassisnummer;
                        newBrief.ANTOB = antob;
                        olCtx.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// set Ablieferdatum
        /// </summary>
        public void setAblieferdatum(long sysID, DateTime Ablieferdatum)
        {
            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                ANTOB antob = (from ant in olCtx.ANTOB
                               where ant.SYSANTRAG == sysID
                                 select ant).FirstOrDefault();
                _log.Debug("Set Antrag Ablieferdatum");
                if (antob != null)
                {
                    antob.LIEFER = Ablieferdatum;
                    olCtx.SaveChanges();
                }
            }
        }
    }
}