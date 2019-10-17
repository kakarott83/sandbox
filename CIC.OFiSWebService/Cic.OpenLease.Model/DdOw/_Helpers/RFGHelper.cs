// OWNER MK, 06-07-2009
namespace Cic.OpenLease.Model.DdOw
{
    #region Using
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class RFGHelper
    {
        #region Properties
        public static RFG DeliverRFG(Cic.OpenLease.Model.DdOw.OwExtendedEntities context, long sysWFUSER, long sysRFN)
        {
            return MyDeliverRFG(context, sysWFUSER, sysRFN);
        }

        public static RFG DeliverRFG(Cic.OpenLease.Model.DdOw.OwExtendedEntities context, long sysWFUSER, string rightFunctionName)
        {
            IQueryable<long> RfnQuery = from rfn in context.RFN
                                        where rfn.NAME == rightFunctionName
                                        select rfn.SYSRFN;

            long SysRfn = RfnQuery.FirstOrDefault<long>();

            return MyDeliverRFG(context, sysWFUSER, SysRfn);
        }

        public static bool HasRightToSee(Cic.OpenLease.Model.DdOw.OwExtendedEntities context, long sysWFUSER, long? sysRFN)
        {
            // Optimistic
            bool HasRights = true;

            // optimistic
            if (sysRFN.GetValueOrDefault() != 0)
            {
                RFG Rfg = MyDeliverRFG(context, sysWFUSER, sysRFN.Value);

                if (Rfg.SEHEN == 0)
                {
                    HasRights = false;
                }
            }

            return HasRights;
        }

        public static bool HasRightToExecute(Cic.OpenLease.Model.DdOw.OwExtendedEntities context, long sysWFUSER, long? sysRFN)
        {
            // Optimistic
            bool HasRights = true;

            // optimistic
            if (sysRFN.GetValueOrDefault() != 0)
            {
                RFG Rfg = MyDeliverRFG(context, sysWFUSER, sysRFN.Value);

                if (Rfg.AUSFUEHREN == 0)
                {
                    HasRights = false;
                }
            }

            return HasRights;
        }

        public static bool HasRightToModify(Cic.OpenLease.Model.DdOw.OwExtendedEntities context, long sysWFUSER, long? sysRFN)
        {
            // Optimistic
            bool HasRights = true;

            // optimistic
            if (sysRFN.GetValueOrDefault() != 0)
            {
                RFG Rfg = MyDeliverRFG(context, sysWFUSER, sysRFN.Value);

                if (Rfg.BEARBEITEN == 0)
                {
                    HasRights = false;
                }
            }

            return HasRights;
        }

        public static bool HasRightToInsert(Cic.OpenLease.Model.DdOw.OwExtendedEntities context, long sysWFUSER, long? sysRFN)
        {
            // Optimistic
            bool HasRights = true;

            // optimistic
            if (sysRFN.GetValueOrDefault() != 0)
            {
                RFG Rfg = MyDeliverRFG(context, sysWFUSER, sysRFN.Value);

                if (Rfg.EINFUEGEN == 0)
                {
                    HasRights = false;
                }
            }

            return HasRights;
        }

        public static bool HasRightToDelete(Cic.OpenLease.Model.DdOw.OwExtendedEntities context, long sysWFUSER, long? sysRFN)
        {
            // Optimistic
            bool HasRights = true;

            // optimistic
            if (sysRFN.GetValueOrDefault() != 0)
            {
                RFG Rfg = MyDeliverRFG(context, sysWFUSER, sysRFN.Value);

                if (Rfg.LOESCHEN == 0)
                {
                    HasRights = false;
                }
            }

            return HasRights;
        }
        #endregion

        #region My methods
        private static RFG MyDeliverRFG(Cic.OpenLease.Model.DdOw.OwExtendedEntities context, long sysWFUSER, long sysRFN)
        {
            RFG Rfg = new RFG();
            Rfg.AUSFUEHREN = 0;
            Rfg.BEARBEITEN = 0;
            Rfg.BEIAUFRUF = 0;
            Rfg.EINFUEGEN = 0;
            Rfg.LOESCHEN = 0;
            Rfg.SEHEN = 0;

            var RgmQuery = from rgm in context.RGM
                           where rgm.SYSWFUSER == sysWFUSER
                           select rgm.RGR.SYSRGR;

            System.Collections.Generic.List<long> SysRgrList = RgmQuery.ToList<long>();

            if (SysRgrList.Count > 0)
            {
                if (sysRFN != 0)
                {

                    //IQueryable<RFG> Query;

                    var Query = from p in context.RFG
                         where
                              p.RFN.SYSRFN==sysRFN &&
                           SysRgrList.Contains(p.RGR.SYSRGR)
                          select p;
                

                    /*Query = context.CreateQuery<Cic.OpenLease.Model.DdOw.RFG>(Cic.Basic.Data.Objects.ContextHelper.GetQualifiedEntitySetName(context, typeof(Cic.OpenLease.Model.DdOw.RFG)));

                    Query = Query.Where(Cic.Basic.Data.Objects.EntityFrameworkHelper.BuildContainsExpression<RFG, long>(rfg => rfg.RGR.SYSRGR, SysRgrList.ToList<long>()));

                    Query = Query.Where(rfg => rfg.RFN.SYSRFN == sysRFN);
                    */

                    System.Collections.Generic.List<RFG> RfgList = Query.ToList<RFG>();

                    foreach (RFG RfgLoop in RfgList)
                    {
                        if (RfgLoop.AUSFUEHREN.GetValueOrDefault(0) == 1) Rfg.AUSFUEHREN = 1;
                        if (RfgLoop.BEARBEITEN.GetValueOrDefault(0) == 1) Rfg.BEARBEITEN = 1;
                        if (RfgLoop.BEIAUFRUF.GetValueOrDefault(0) == 1) Rfg.BEIAUFRUF = 1;
                        if (RfgLoop.EINFUEGEN.GetValueOrDefault(0) == 1) Rfg.EINFUEGEN = 1;
                        if (RfgLoop.LOESCHEN.GetValueOrDefault(0) == 1) Rfg.LOESCHEN = 1;
                        if (RfgLoop.SEHEN.GetValueOrDefault(0) == 1) Rfg.SEHEN = 1;
                    }
                }
            }

            return Rfg;
        }

        #endregion
    }
}
