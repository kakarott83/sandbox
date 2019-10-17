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

    public static class ANGOBSICHHelper
    {
        public static System.Collections.Generic.List<ANGOBSICH> GetAngobisch(DdOlExtended context, long sysangebot)
        {
            System.Collections.Generic.List<ANGOBSICH> ANGOBSICHList = null;
            
            // Get angobsl
            // Ticket#2012121910000071 — ANGEBOT Mitantragssteller ist weg
            var Query = from angobsich in context.ANGOBSICH.Include("ANGEBOT").Include("IT")
                        where (angobsich.SYSVT == sysangebot) && (angobsich.SICHTYP.RANG==10 || angobsich.SICHTYP.RANG==80)
                        select angobsich;

            //Create list
            ANGOBSICHList = Query.ToList();

            return ANGOBSICHList;
        }

        public static ANGOBSICH GetAngobsischById(DdOlExtended context, long sysId)
        {
            //Get angobsl
            var AngobSich = (from angobsich in context.ANGOBSICH
                             where angobsich.SYSID == sysId
                             select angobsich).FirstOrDefault();
            return AngobSich;
        }
        public static ANGOBSICH GetAngobsischByRang(DdOlExtended context, long sysangebot, int rang)
        {
            // Get angobsl
            // Ticket#2012121910000071 — ANGEBOT Mitantragssteller ist weg
            // Die Verknüpfung zwischen angobsich und angebot lautet in AIDA: angobsich.sysvt == angebot.sysid.
            var AngobSich = (from angobsich in context.ANGOBSICH
                             where angobsich.RANG == rang && angobsich.SYSVT == sysangebot
                             select angobsich).FirstOrDefault();
            return AngobSich;
        }
       
        public static void DeleteAngobsisch(DdOlExtended context, long sysId)
        {
            // Get the specified endorser
            ANGOBSICH Angobsisch = GetAngobsischById(context, sysId);

            // Check if the endorser was found
            if (Angobsisch == null)
            {
                // Throw an exception
                throw new ApplicationException("Specified angobsisch could not be found.");
            }

            // Delete the endorser
            context.DeleteObject(Angobsisch);

            // Save the changes
            context.SaveChanges();
        }
    }
}
