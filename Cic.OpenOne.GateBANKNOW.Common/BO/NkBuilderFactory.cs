using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.BO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Factory for Nummernkreis Builder
    /// </summary>
    public class NkBuilderFactory
    {
        private static String AREA = "B2B";
        private static String CFGSEC = "NK";
        private static String CFG = "SETUP.NET";

        private static NkBuilder antragNkBuilder;
        private static NkBuilder angebotNkBuilder;
        private static NkBuilder personNkBuilder;
        private static NkBuilder itNkBuilder;
        private static NkBuilder angobNkBuilder;
        private static NkBuilder antobNkBuilder;

        /// <summary>
        /// Angebot Nummernkreis Builder erzeugen
        /// </summary>
        /// <returns>Nummernkreis builder</returns>
        public static NkBuilder createAngebotNkBuilder()
        {
            if (angebotNkBuilder == null)
            {
                angebotNkBuilder = new NkBuilder(AppConfig.Instance.GetEntry(CFGSEC, "ANGEBOT_TYP", "ANGEBOT", CFG), AppConfig.Instance.GetEntry(CFGSEC, "ANGEBOT_BEREICH", AREA, CFG));
            }
            return angebotNkBuilder;
        }

        /// <summary>
        /// Antrag Nummernkreis erzeugen
        /// </summary>
        /// <returns>Nummernkreis builder</returns>
        public static NkBuilder createAntragNkBuilder()
        {
            if (antragNkBuilder == null)
            {
                antragNkBuilder = new NkBuilder(AppConfig.Instance.GetEntry(CFGSEC, "ANTRAG_TYP", "ANTRAG", CFG), AppConfig.Instance.GetEntry(CFGSEC, "ANTRAG_BEREICH", AREA, CFG));
            }
            return antragNkBuilder;
        }
        /// <summary>
        /// Person Nummernkreis erzeugen
        /// </summary>
        /// <returns>Nummernkreis builder</returns>
        public static NkBuilder createPersonNkBuilder()
        {
            if (personNkBuilder == null)
            {
                personNkBuilder = new NkBuilder(AppConfig.Instance.GetEntry(CFGSEC, "PERSON_TYP", "PERSON", CFG), AppConfig.Instance.GetEntry(CFGSEC, "PERSON_BEREICH", AREA, CFG));
            }
            return personNkBuilder;
        }

        /// <summary>
        /// Interessent Nummernkreis erzeugen
        /// </summary>
        /// <returns>Nummernkreis builder</returns>
        public static NkBuilder createItNkBuilder()
        {
            if (itNkBuilder == null)
            {
                itNkBuilder = new NkBuilder(AppConfig.Instance.GetEntry(CFGSEC, "IT_TYP", "IT", CFG), AppConfig.Instance.GetEntry(CFGSEC, "IT_BEREICH", AREA, CFG));
            }
            return itNkBuilder;
        }
        /// <summary>
        /// Angebot Objekt Nummernkreis erzeugen
        /// </summary>
        /// <returns>Nummernkreis builder</returns>
        public static NkBuilder createAngobNkBuilder()
        {
            if (angobNkBuilder == null)
            {
                angobNkBuilder = new NkBuilder(AppConfig.Instance.GetEntry(CFGSEC, "ANGOB_TYP", "ANGOB", CFG), AppConfig.Instance.GetEntry(CFGSEC, "ANGOB_BEREICH", AREA, CFG));
            }
            return angobNkBuilder;
        }
        /// <summary>
        /// Antrag Objekt Nummernkreis erzeugen
        /// </summary>
        /// <returns>Nummernkreis builder</returns>
        public static NkBuilder createAntobNkBuilder()
        {
            if (antobNkBuilder == null)
            {
                antobNkBuilder = new NkBuilder(AppConfig.Instance.GetEntry(CFGSEC, "ANTOB_TYP", "ANTOB", CFG), AppConfig.Instance.GetEntry(CFGSEC, "ANTOB_BEREICH", AREA, CFG));
            }
            return antobNkBuilder;
        }
    }
}
