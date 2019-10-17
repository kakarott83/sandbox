// OWNER MK, 03-06-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public class RNHelper
    {
        public static int DeliverRNCount(OlExtendedEntities context, long sysPerson, RNSearchArtConstants? rNSearchArtConstant, RNSearchTypeConstants? rNSearchTypeConstant, DateTime? minFaelligkeitsdatum, DateTime? maxFaelligkeitsdatum, RNPaidConstants? rNPaidConstants)
        {
            if (sysPerson <= 0)
            {
                throw new System.Exception("sysPerson");
            }

            IQueryable<RN> Query = MyDeliverSearchQuery(context, sysPerson, rNSearchArtConstant, rNSearchTypeConstant, minFaelligkeitsdatum, maxFaelligkeitsdatum, rNPaidConstants);

            string s = ((System.Data.Objects.ObjectQuery)Query).ToTraceString();

            return Query.Count<RN>();
        }

        public static System.Collections.Generic.List<RN> DeliverRN(OlExtendedEntities context, long sysPerson, RNSearchArtConstants? rNSearchArtConstant, RNSearchTypeConstants? rNSearchTypeConstant, DateTime? minFaelligkeitsdatum, DateTime? maxFaelligkeitsdatum, RNPaidConstants? rNPaidConstants, int skip, int top)
        {
            if (sysPerson <= 0)
            {
                throw new System.Exception("sysPerson");
            }

            IQueryable<RN> Query = MyDeliverSearchQuery(context, sysPerson, rNSearchArtConstant, rNSearchTypeConstant, minFaelligkeitsdatum, maxFaelligkeitsdatum, rNPaidConstants);

            // Order by
            Query = Query.OrderBy(par => par.VALUTADATUM).ThenBy(par => par.ART).ThenBy(par => par.BELEG);

            // Set skipt top
            Query = Query.Skip<Cic.OpenLease.Model.DdOl.RN>(skip);

            Query = Query.Take<Cic.OpenLease.Model.DdOl.RN>(top);

            string s = ((System.Data.Objects.ObjectQuery)Query).ToTraceString();

            return Query.ToList<RN>();
        }

        public static double? DeliverOp(OlExtendedEntities context, long sysRN)
        {
            System.Data.EntityClient.EntityConnection EntityConnection;
            double? Op = null;

            EntityConnection = context.Connection as System.Data.EntityClient.EntityConnection;

            if (EntityConnection != null)
            {
                // Get SQL part from cicconf
                string SqlPart = Cic.OpenLease.Model.DdOl.CICCONFHelper.DeliverOpSql(context);

                string Sql = "SELECT " + SqlPart + " FROM CIC.RN WHERE CIC.RN.SysID = " + sysRN.ToString();

                System.Collections.Generic.List<System.Collections.Generic.List<string>> StringList = PassThroughQueryHelper.GetBySql(EntityConnection.StoreConnection, Sql, false);

                if (StringList.Count > 0 && StringList[0].Count > 0)
                {
                    try
                    {
                        Op = double.Parse(StringList[0][0]);
                    }
                    catch
                    {
                        // ignore
                    }
                }

            }

            return Op;
        }


        private static IQueryable<RN> MyDeliverSearchQuery(OlExtendedEntities context, long sysPerson, RNSearchArtConstants? rNSearchArtConstant, RNSearchTypeConstants? rNSearchTypeConstant, DateTime? minFaelligkeitsdatum, DateTime? maxFaelligkeitsdatum, RNPaidConstants? rNPaidConstants)
        {
            IQueryable<RN> Query;

            // Set Query
            Query = context.RN;
            // Search for code
            Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.PERSON.SYSPERSON == sysPerson);
			Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.VALUTADATUM != null);

            // Filter out RANG = 1000
            // TODO MK 0 MK, Q&D According to AS
            Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.KREIS != 10000);
            

            // input output or all
            if (rNSearchArtConstant.HasValue && rNSearchArtConstant.Value == RNSearchArtConstants.IncomingMandantorOutgoingCustomer)
            {
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.ART == 1);
            }

            if (rNSearchArtConstant.HasValue && rNSearchArtConstant.Value == RNSearchArtConstants.OutgoingMandantorIncomingCustomer)
            {
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.ART == 0);
            }

            // reference to vt or ob
            if (rNSearchTypeConstant.HasValue && rNSearchTypeConstant.Value == RNSearchTypeConstants.WithoutReference)
            {
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.SYSOB == null || par.SYSOB.Value <= 0);
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par =>  par.VT.SYSID <= 0);
            }

            if (rNSearchTypeConstant.HasValue && rNSearchTypeConstant.Value == RNSearchTypeConstants.OnlyContractReference)
            {
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.VT != null || par.VT.SYSID > 0);
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.SYSOB == null || par.SYSOB.Value <= 0);
            }

            if (rNSearchTypeConstant.HasValue && rNSearchTypeConstant.Value == RNSearchTypeConstants.ContractAndObjectReferences)
            {
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.SYSOB != null && par.SYSOB.Value > 0);
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.VT != null || par.VT.SYSID > 0);
            }

            // paid not paid
            if (rNPaidConstants.HasValue && rNPaidConstants.Value == RNPaidConstants.Paid)
            {
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.BEZAHLT == 1);
            }

            if (rNPaidConstants.HasValue && rNPaidConstants.Value == RNPaidConstants.NotPaid)
            {
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.BEZAHLT == null || par.BEZAHLT == 0);
            }

            // take date into consideration
            if (minFaelligkeitsdatum.HasValue)
            {
                DateTime dt = minFaelligkeitsdatum.Value;
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.VALUTADATUM >= dt);
            }

            if (maxFaelligkeitsdatum.HasValue)
            {
                DateTime dt = maxFaelligkeitsdatum.Value;
                Query = Query.Where<Cic.OpenLease.Model.DdOl.RN>(par => par.VALUTADATUM <= dt);
            }
            return Query;
        }
    }
}
