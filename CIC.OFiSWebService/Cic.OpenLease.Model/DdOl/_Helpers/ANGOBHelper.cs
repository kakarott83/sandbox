// OWNER WB, 11-03-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class ANGOBHelper
    {
        #region Methods


        public static Cic.OpenLease.Model.DdOl.ANGOB GetAngobFromAngebot(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long? SYSANGEBOT)
        {
            ANGKALK kalk = ANGEBOTHelper.GetAngkalkFromAngebot(context, SYSANGEBOT);
            if (kalk != null) //ANGKALK is loaded
            {
                if (!kalk.ANGOBReference.IsLoaded)
                {
                    kalk.ANGOBReference.Load();
                }
                return kalk.ANGOB;
            }
            return null;
        }

        public static Cic.OpenLease.Model.DdOl.ANGOBINI GetAngobiniFromAngob(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long? SYSANGOB)
        {
            var query = from angobini in context.ANGOBINI
                        where angobini.SYSOBINI == SYSANGOB
                        select angobini;
            return query.FirstOrDefault<Cic.OpenLease.Model.DdOl.ANGOBINI>();
        }

        public static Cic.OpenLease.Model.DdOl.ANGOBAUST[] GetAngobaustFromAngob(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long? SYSANGOB)
        {
            var query = from angobaust in context.ANGOBAUST
                        where angobaust.SYSANGOB == SYSANGOB
                        select angobaust;
            return query.ToArray<ANGOBAUST>();
        }

        public static Cic.OpenLease.Model.DdOl.ANGOBAUST GetAngobAust(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long? SYSANGOB, string snr)
        {
            var query = from angobaust in context.ANGOBAUST
                        where angobaust.SNR == snr && angobaust.SYSANGOB == SYSANGOB
                        select angobaust;
            return query.FirstOrDefault<ANGOBAUST>();
        }
        #endregion
    }
}