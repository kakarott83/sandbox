using System;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.Common.DAO.Auskunft
{
    /// <summary>
    /// SoapLogDumpDao-Klasse
    /// </summary>
    public class SoapLogDumpDao
    {

        const int MAXDESCRIPTIONLENGTH = 119;
        /// <summary>
        /// Default empty Constructor
        /// </summary>
        public SoapLogDumpDao()
        {
        }

        /// <summary>
        /// CreateUpdateLogDump
        /// </summary>
        /// <param name="soaptext">soaptext</param>
        /// <param name="entryCode">entryCode</param>
        /// <param name="entity">entity</param>
        /// <param name="id">id</param>
        public void CreateUpdateLogDump(String soaptext, String entryCode, String entity, long id)
        {
            if (id != 0 && !entryCode.Equals("") && !entity.Equals(""))
            {
                using (DdOwExtended context = new DdOwExtended())
                {
                    DateTime currentDate = DateTime.Now;
                    LOGDUMP logdump = new LOGDUMP();
                    context.AddToLOGDUMP(logdump);
                    string description = entity.ToUpper() + "_" + id + "_" + entryCode.ToUpper();
                    description = description.Length < MAXDESCRIPTIONLENGTH ? description : description.Substring(0, MAXDESCRIPTIONLENGTH);
                    logdump.DESCRIPTION = description;
                    logdump.DUMPVALUE = soaptext;
                    logdump.DUMPDATE = currentDate;
                    logdump.INPUTFLAG = 0;
                    logdump.DUMPTIME = DateTimeHelper.DateTimeToClarionTime(currentDate);
                    logdump.AREA = entity;
                    logdump.SYSID = id;

                    context.SaveChanges();
                }
            }
        }

        
    }
}