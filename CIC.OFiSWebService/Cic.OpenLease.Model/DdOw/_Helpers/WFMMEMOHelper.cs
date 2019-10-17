//OWNER WB 28.06.2010
namespace Cic.OpenLease.Model.DdOw
{
    #region Using
    using System.Linq;
    
    using System.Collections.Generic;
    using Cic.Basic.Data.Objects;
    using Cic.OpenLease.Model.DdOw;
    using System;
    using Cic.OpenOne.Common.Util;
    #endregion

    public static class WFMMEMOHelper
    {
       

/*
        public static WFMMEMO DeliverWfmmemoFromAngebot(DdOw.OwExtendedEntities context, long sysId)
        {
            WFMMKAT WFMMKAT;
            WFMMEMO WFMMEMO = null;
            try
            {

                WFMMKAT = WFMKATHelper.DeliverWfmkatForAngebot(context);

                long sysWftable = WFTABLEHelper.DeliverSyswftableForAngebot(context);

                if (sysWftable != 0)
                {
                    var WfmmemoQuery = from wfmmemo in context.WFMMEMO
                                       where wfmmemo.SYSWFMTABLE == sysWftable && wfmmemo.SYSLEASE == sysId && wfmmemo.WFMMKAT.SYSWFMMKAT == WFMMKAT.SYSWFMMKAT
                                       orderby wfmmemo.SYSWFMTABLE descending
                                       select wfmmemo;
                    
                    WFMMEMO = WfmmemoQuery.FirstOrDefault();
                }

            }
            catch(Exception exception)
            {
                throw new Cic.Basic.BasicException("Error during getting WFMMEMO", exception);
            }

            return WFMMEMO;
        }*/
        public static WFMMEMO DeliverWfmmemoFromAngebot(DdOw.OwExtendedEntities context, long sysId, String kategorieBezeichnung)
        {
            //WFMMKAT WFMMKAT;
            WFMMEMO WFMMEMO = null;
            try
            {

                //WFMMKAT = WFMKATHelper.DeliverWfmkatForAngebot(context);

                long sysWftable = WFTABLEHelper.DeliverSyswftableForAngebot(context);

                if (sysWftable != 0)
                {
                    var WfmmemoQuery = from wfmmemo in context.WFMMEMO
                                       where wfmmemo.SYSWFMTABLE == sysWftable && wfmmemo.SYSLEASE == sysId && wfmmemo.WFMMKAT.BESCHREIBUNG == kategorieBezeichnung
                                       orderby wfmmemo.SYSWFMTABLE descending
                                       select wfmmemo;

                    WFMMEMO = WfmmemoQuery.FirstOrDefault();
                    
                }

            }
            catch (Exception exception)
            {
                throw new Exception("Error during getting WFMMEMO for " + kategorieBezeichnung, exception);
            }

            return WFMMEMO;
        }


        public static WFMMEMO UpdateWFMMEMOFields(OwExtendedEntities context, long syslease, WFMMEMO wfmmemo, String kategorieBezeichnung, String text, long _SysPERSON)
        {

            if (wfmmemo == null)
            {
                wfmmemo = new WFMMEMO();
                wfmmemo.CREATEDATE = DateTime.Now;
                wfmmemo.CREATETIME = DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                wfmmemo.CREATEUSER = _SysPERSON;
                wfmmemo.SYSLEASE = syslease;
                wfmmemo.SYSWFMTABLE = WFTABLEHelper.DeliverSyswftableForAngebot(context);
                wfmmemo.WFMMKAT = WFMKATHelper.DeliverWfmkat(context, kategorieBezeichnung);
                wfmmemo.NOTIZMEMO = StringConversionHelper.StringToClarionByte(text);
                //wfmmemo.NOTIZMEMO = Cic.Basic.OpenLease.StringConversionHelper.StringToByte(text);
                context.AddToWFMMEMO(wfmmemo);
                
            }
            else
            {
                wfmmemo.EDITDATE = DateTime.Now;
                wfmmemo.EDITTIME = DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                wfmmemo.EDITUSER = _SysPERSON;
                //wfmmemo.NOTIZMEMO = Cic.Basic.OpenLease.StringConversionHelper.StringToByte(text);
                wfmmemo.NOTIZMEMO = StringConversionHelper.StringToClarionByte(text);
            }
            context.SaveChanges();
            return wfmmemo;
        }
    }
}
