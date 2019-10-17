// OWNER WB, 11-03-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using System.Linq;
    using CIC.Database.OW.EF6.Model;


    using System;
    using Cic.OpenOne.Common.Model.DdOw;
    using Cic.OpenOne.Common.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    #endregion

    [System.CLSCompliant(true)]
    public static class ANGOBHelper
    {
        #region Methods


        public static  ANGOB GetAngobFromAngebot(DdOlExtended context, long? SYSANGEBOT)
        {
            ANGKALK kalk = ANGEBOTHelper.GetAngkalkFromAngebot(context, SYSANGEBOT);
            if (kalk != null) //ANGKALK is loaded
            {
                if (kalk.ANGOB == null)
                    context.Entry(kalk).Reference(f => f.ANGOB).Load();
                
                return kalk.ANGOB;
            }
            return null;
        }

        public static  ANGOBINI GetAngobiniFromAngob(DdOlExtended context, long? SYSANGOB)
        {
            var query = from angobini in context.ANGOBINI
                        where angobini.SYSOBINI == SYSANGOB
                        select angobini;
            return query.FirstOrDefault<ANGOBINI>();
        }

        public static  ANGOBAUST[] GetAngobaustFromAngob(DdOlExtended context, long? SYSANGOB)
        {
            var query = from angobaust in context.ANGOBAUST
                        where angobaust.SYSANGOB == SYSANGOB
                        select angobaust;
            return query.ToArray<ANGOBAUST>();
        }

        public static  ANGOBAUST GetAngobAust(DdOlExtended context, long? SYSANGOB, string snr)
        {
            var query = from angobaust in context.ANGOBAUST
                        where angobaust.SNR == snr && angobaust.SYSANGOB == SYSANGOB
                        select angobaust;
            return query.FirstOrDefault<ANGOBAUST>();
        }
        #endregion
    }
}