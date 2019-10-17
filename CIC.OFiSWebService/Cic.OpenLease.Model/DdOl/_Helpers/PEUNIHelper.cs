// OWNER MK, 09-06-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    using System.Collections.Generic;
    #endregion

    [System.CLSCompliant(true)]
    public class PEUNIHelper
    {
        #region Enums
        public enum Areas
        {
            PERSON,
            IT,
            VT,
            ANTRAG,
            ANGEBOT,
            MYCALC,
        }
        #endregion        

        #region Methods
      /*  public static bool IsInSightField(OlExtendedEntities context, long sysPEROLE, Areas area, long sysId)
        {
            List<long> SightFieldAngebots = Cic.OpenLease.Model.DdOl.PEUNIHelper.DeliverSightFieldIds(context, sysPEROLE, area);
            if (SightFieldAngebots.Contains(sysId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<long> DeliverSightFieldIds(OlExtendedEntities context, long sysPEROLE, Areas area)
        {
            string Area = area.ToString();

            // Get the sightfield
            List<long> SysPeroleList = new List<long>() { sysPEROLE };

            System.Linq.IQueryable<Cic.OpenLease.Model.DdOl.PEUNI> Query;

            // Create
            Query = context.CreateQuery<Cic.OpenLease.Model.DdOl.PEUNI>(Cic.Basic.Data.Objects.ContextHelper.GetQualifiedEntitySetName(context, typeof(Cic.OpenLease.Model.DdOl.PEUNI)));

            // Create quely: (SYSPEUNI = x) or (SYSPEUNI = y) or (SYSPEUNI = z) or ...
            Query = Query.Where(Cic.Basic.Data.Objects.EntityFrameworkHelper.BuildContainsExpression<PEUNI, long>(peuni => peuni.PEROLE.SYSPEROLE, SysPeroleList));
            Query = Query.Where(peuni => peuni.AREA.ToUpper() == Area);
            Query = Query.Where(peuni => peuni.SYSID != null);

            // Get only the ids
            List<long> SysList = Query.Select(peuni => peuni.SYSID.Value).ToList<long>();

            string s = ((System.Data.Objects.ObjectQuery)Query).ToTraceString();

            return SysList;
        }*/

        public static void ConnectNodes(OlExtendedEntities context, Areas areas, long sysId, long sysPEROLE)
        {
            DateTime DateTimeNow = DateTime.Now;
            PEUNI PeUni = new PEUNI();
            
            // Set
            PeUni.AREA = areas.ToString();
            PeUni.SYSID = sysId;
            PeUni.UNIDATE = DateTimeNow;
            PeUni.PEROLEReference.EntityKey = context.getEntityKey(typeof(PEROLE), sysPEROLE);

            // Add
            context.AddToPEUNI(PeUni);
        }        
        #endregion
    }
}
