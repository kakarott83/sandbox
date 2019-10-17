// OWNER MP, 11-03-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using directives
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    public class KNEHelper
    {
        #region Methods
        public static List<Cic.OpenLease.Model.DdOl.KNE> SelectBySysOber(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, long SysPerson)
        {
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOl.KNE> Query;
            List<Cic.OpenLease.Model.DdOl.KNE> KNEList;

            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }
            // Set Query
            Query = olExtendedEntities.KNE;
            // Search for code
            Query = Query.Where<Cic.OpenLease.Model.DdOl.KNE>(par => par.SYSOBER == SysPerson);

            try
            {
                // Select
                KNEList = Query.ToList();
            }
            catch
            {
                throw;
            }
            return KNEList;
        }

        public static void Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, KNE Kne)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }
            // Save
            olExtendedEntities.AddToKNE(Kne);
            olExtendedEntities.SaveChanges();
        }

        public static void Delete(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, KNE Kne)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }
            //Delete
            olExtendedEntities.DeleteObject(Kne);
        }


        #endregion
    }
}
