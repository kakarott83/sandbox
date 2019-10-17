namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenOne.Common.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public static bool IsInSightField(DdOlExtended context, long sysPUSER, Areas area, long sysId)
        {
            //string query = "select cic.CIC_PEROLE_UTILS.ChkObjInPEUNI("+sysPUSER+", '"+area+"',"+sysId+",sysdate) test from dual";
            String query = "select 1 from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + sysPUSER + ",  '" + area + "',sysdate)) t where t.sysid=" + sysId;
            long testvalue =  context.ExecuteStoreQuery<long>(query).FirstOrDefault();

            return testvalue > 0;
        
        }

        public static List<long> DeliverSightFieldIds(DdOlExtended context, long sysPUSER, Areas area)
        {
            string Area = area.ToString();


            string query = "select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + sysPUSER + ", '" + Area + "',sysdate)) ";
            return context.ExecuteStoreQuery<long>(query).ToList<long>();

        }

        #endregion

        public static void ConnectNodes(DdOlExtended context, Areas areas, long sysId, long sysPEROLE)
        {
            DateTime DateTimeNow = DateTime.Now;
            PEUNI PeUni = new PEUNI();

            // Set
            PeUni.AREA = areas.ToString();
            PeUni.SYSID = sysId;
            PeUni.UNIDATE = DateTimeNow;
            PeUni.SYSPEROLE= sysPEROLE;

            // Add
            context.PEUNI.Add(PeUni);
        }
    }
}
