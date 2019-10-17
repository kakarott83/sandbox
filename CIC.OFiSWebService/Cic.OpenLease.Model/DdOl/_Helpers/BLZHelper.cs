// OWNER MK, 02-02-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    using System.Collections.Generic;
    #endregion


    public class BLZHelper
    {
        #region Methods
        public static List<BLZ> SearchBankname(OlExtendedEntities context, string blz, string bic)
        {
            List<BLZ> BLZList = new List<BLZ>();

            // Set null if empty
            if (blz == null || blz.Trim() == string.Empty)
            {
                blz = null;
            }

            // Set null if empty
            if (bic == null || bic.Trim() == string.Empty)
            {
                bic = null;
            }

            var Query = from blzrow in context.BLZ
                        where ((blz != null ? blzrow.BLZ1.StartsWith(blz) : true) && (bic != null ? blzrow.BIC.StartsWith(bic) : true))
                        orderby blzrow.NAME                        
                        select blzrow;

            BLZList = Query.ToList();
            
            return BLZList;
        }

        public static bool Contains(OlExtendedEntities olExtendedEntities, string blz)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }
            return olExtendedEntities.BLZ.Where(par => par.BLZ1.Equals(blz)).Any();
        }

        public static BLZ SelectByBlz(OlExtendedEntities olExtendedEntities, string blz)
        {
            if (blz != null && blz.Trim() != string.Empty)
            {
                List<BLZ> BLZList = new List<BLZ>();
                char charToRemove = ' ';
                blz = blz.Replace(charToRemove.ToString(), "");
                string blzFirstCharacter = blz[0].ToString();

                var Query = from blzrow in olExtendedEntities.BLZ
                            where ((blz != null ? blzrow.BLZ1.StartsWith(blzFirstCharacter) : true))
                            orderby blzrow.BLZ1
                            select blzrow;

                BLZList = Query.ToList();

                foreach (BLZ BLZ in BLZList)
                {
                    if (BLZ.BLZ1 != null && BLZ.BLZ1 != string.Empty)
                    {
                        string BlzWithoutWhitespaces = BLZ.BLZ1.Replace(charToRemove.ToString(), "");
                        if (BlzWithoutWhitespaces.Equals(blz))
                        {
                            return BLZ;
                        }
                    }
                }
            }
            return null;
        }

        public static BLZ SelectByBic(OlExtendedEntities olExtendedEntities, string bic)
        {
            if (bic != null && bic.Trim() != string.Empty)
            {
                List<BLZ> BLZList = new List<BLZ>();
                char charToRemove = ' ';
                bic = bic.Replace(charToRemove.ToString(), "");
                string blzFirstCharacter = bic[0].ToString();

                var Query = from blzrow in olExtendedEntities.BLZ
                            where ((bic != null ? blzrow.BLZ1.StartsWith(blzFirstCharacter) : true))
                            orderby blzrow.BIC
                            select blzrow;

                BLZList = Query.ToList();

                foreach (BLZ BLZ in BLZList)
                {
                    if (BLZ.BIC != null && BLZ.BIC.Trim() != string.Empty)
                    {
                        string BlzWithoutWhitespaces = BLZ.BIC.Replace(charToRemove.ToString(), "");
                        if (BlzWithoutWhitespaces.Equals(bic))
                        {
                            return BLZ;
                        }
                    }
                }
            }
            return null;
        }

        public static BLZ SelectBySysBlz(OlExtendedEntities olExtendedEntities, long sysBlz)
        {
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOl.BLZ> Query;
            Cic.OpenLease.Model.DdOl.BLZ Blz;
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }
            // Set Query
            Query = olExtendedEntities.BLZ;
            // Search for code
            Query = Query.Where<Cic.OpenLease.Model.DdOl.BLZ>(par => par.SYSBLZ == sysBlz);

            try
            {
                // Select
                Blz = Query.FirstOrDefault<Cic.OpenLease.Model.DdOl.BLZ>();
            }
            catch
            {
                throw;
            }

            // Check object
            if (Blz == null)
            {
                throw new System.Exception(typeof(Cic.OpenLease.Model.DdOl.BLZ).ToString() + "." + Cic.OpenLease.Model.DdOl.BLZ.FieldNames.BLZ1.ToString() + " = " + Blz.ToString());
            }

            return Blz;
        }

        public static void Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, BLZ Blz)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Save
            olExtendedEntities.AddToBLZ(Blz);
            //olExtendedEntities.SaveChanges();
        }

        #endregion
    }
}
