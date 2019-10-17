using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Model.DdOw;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenLease.Service
{
    public class LogHelper
    {
        /// <summary>
        /// logToDatabase
        /// </summary>
        /// <param name="soaptext">soaptext</param>
        /// <param name="entryCode">entryCode</param>
        /// <param name="entity">entity</param>
        /// <param name="id">id</param>
        public static void logToDatabase(String title, String message, String entity, long id)
        {
            if (id != 0 && title!=null && entity!=null && !title.Equals("") && !entity.Equals(""))
            {
                using (DdOwExtended context = new DdOwExtended())
                {
                    DateTime currentDate = DateTime.Now;
                    LOGDUMP logdump = new LOGDUMP();
                    context.LOGDUMP.Add(logdump);
                    logdump.DESCRIPTION = title;
                    logdump.DUMPVALUE = message;
                    logdump.DUMPDATE = currentDate;
                    logdump.INPUTFLAG = 0;
                    logdump.DUMPTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(currentDate);
                    logdump.AREA = entity;
                    logdump.SYSID = id;

                    context.SaveChanges();
                }
            }
        }
    }
}