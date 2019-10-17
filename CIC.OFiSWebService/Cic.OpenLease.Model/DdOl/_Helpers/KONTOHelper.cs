// OWNER MP, 01-03-2010

namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion


    public class KONTOHelper
    {
        #region Methods
        public static bool Contains(OlExtendedEntities olExtendedEntities, long sysKONTO)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.KONTO.Where(par => par.SYSKONTO == sysKONTO).Any<KONTO>();
        }

        public static bool Contains(OlExtendedEntities olExtendedEntities, string kontoNr)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.KONTO.Where(par => par.KONTONR == kontoNr).Any<KONTO>();
        }

        public static void Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, KONTO Konto)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Save
            olExtendedEntities.AddToKONTO(Konto);
            olExtendedEntities.SaveChanges();
        }

        public static void Update(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, KONTO KONTO)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            olExtendedEntities.SaveChanges();
        }
        #endregion
    }
}
