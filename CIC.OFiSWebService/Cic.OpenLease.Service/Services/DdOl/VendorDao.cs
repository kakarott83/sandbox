using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Container Class to hold return values for the VGADJDao Method
    /// 
    /// </summary>
    [System.CLSCompliant(true)]
    public class VendorDao
    {
        #region Private variables
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DdOlExtended _context;

        #endregion

        #region Constructors
        public VendorDao(DdOlExtended context)
        {
            _context = context;
        }
        #endregion

        /// <summary>
        /// Returns all persons in the same or deeper level as the given
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public TransferVendorDto[] DeliverTransferVendorsProvision(long sysperson)
        {

            long sysperole = _context.ExecuteStoreQuery<long>("select sysperole from perole,roletype where roletype.typ=6 and roletype.sysroletype=perole.sysroletype connect by prior perole.sysparent=perole.sysperole start with perole.sysperson=" + sysperson, null).FirstOrDefault();

            string query = "select syswfuser,pr.name, pr.sysperole from puser, perole pr,person where puser.syspuser=person.syspuser and pr.sysperson=person.sysperson and pr.sysperole in (select sysperole from perole connect by prior perole.sysperole = perole.sysparent start with perole.sysperole=" + sysperole + "  ) order by pr.name";

            TransferVendorDto[] result = _context.ExecuteStoreQuery<TransferVendorDto>(query).ToArray<TransferVendorDto>();
            return result;
        }
        /// <summary>
        /// Returns all persons in the same or deeper level as the perole
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public TransferVendorDto[] DeliverTransferVendors(long sysperole)
        {
            PeroleDto perole = PeroleHelper.DeliverPeRole(_context, sysperole);
            string query = "select syswfuser,pr.name, pr.sysperole from puser, perole pr,person where puser.syspuser=person.syspuser and pr.sysperson=person.sysperson and pr.sysperole in (select sysperole from perole connect by prior perole.sysperole = perole.sysparent start with perole.sysperole=" + perole.sysparent + "  ) order by pr.name";

            TransferVendorDto[] result = _context.ExecuteStoreQuery<TransferVendorDto>(query).ToArray<TransferVendorDto>();
            return result;
        }

        public void transferOfferProposalsToVendor(long sysangebot, TransferVendorDto param, long syspuser)
        {
            String area = PEUNIHelper.Areas.ANTRAG.ToString();

            string query = "select sysid from antrag where sysangebot=" + sysangebot;


            List<long> ids = _context.ExecuteStoreQuery<long>(query, null).ToList<long>();
            foreach (long sysid in ids)
            {

                var test = from p in _context.PEUNI
                           where p.AREA == area && p.SYSID == sysid && p.PEROLE.SYSPEROLE == param.sysperole
                           select p;
                PEUNI puni = test.FirstOrDefault();
                if (puni == null)
                    PEUNIHelper.ConnectNodes(_context, PEUNIHelper.Areas.ANTRAG, sysid, param.sysperole);

                ANTRAG ant = (from c in _context.ANTRAG
                              where c.SYSID == sysid
                              select c).FirstOrDefault();// _context.SelectById<ANTRAG>(sysid);

                ant.SYSWFUSER = param.syswfuser;



                //PERSON pers = PEROLEHelper.DeliverPerson(_context, param.sysperole);
                //ant.SYSBERATADDB = pers.SYSPERSON;
            }
        }

        public long transferOfferToVendor(long sysid, TransferVendorDto param, long syspuser)
        {
            String area = PEUNIHelper.Areas.ANGEBOT.ToString();

            ANGEBOT ang = (from c in _context.ANGEBOT
                           where c.SYSID == sysid
                           select c).FirstOrDefault();// _context.SelectById<ANGEBOT>(sysid);
            //nur wenn am gleichen händler
            PEROLE vpperole = PeroleHelper.FindRootPEROLEObjByRoleTypeCODE(_context, param.sysperole, "HD");
            if (vpperole != null && vpperole.SYSPERSON.HasValue)
            {
                long vpsysperson = (long)vpperole.SYSPERSON.Value;
                if (ang.SYSVPFIL > 0 && ang.SYSVPFIL != vpsysperson)
                {
                    throw new Exception("Wechsel nur innerhalb des gleichen Händlers möglich.");
                }

                /*ang.SYSVP = vpsysperson;*/
                //ang.SYSVPFIL = vpsysperson;
            }





            var test = from p in _context.PEUNI
                       where p.AREA == area && p.SYSID == sysid && p.PEROLE.SYSPEROLE == param.sysperole
                       select p;
            PEUNI puni = test.FirstOrDefault();
            if (puni == null)
                PEUNIHelper.ConnectNodes(_context, PEUNIHelper.Areas.ANGEBOT, sysid, param.sysperole);

            long syspusern = _context.ExecuteStoreQuery<long>("select syspuser from puser, perole where perole.sysperson=puser.sysperson and perole.sysperole=" + param.sysperole, null).FirstOrDefault();
            if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(_context, syspusern, Cic.OpenLease.Service.PEUNIHelper.Areas.IT, (long)ang.SYSIT.Value))
            {
                PEUNIHelper.ConnectNodes(_context, PEUNIHelper.Areas.IT, ang.SYSIT.Value, param.sysperole);
            }

            String query = "select angobsich.sysit from angobsich, sichtyp,angebot where angobsich.sysvt=angebot.sysid and angobsich.syssichtyp=sichtyp.syssichtyp and sichtyp.rang in (10,80,140) and angebot.sysid=" + sysid;

            List<long> mas = _context.ExecuteStoreQuery<long>(query, null).ToList();
            foreach (long sysitma in mas)
            {
                if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(_context, syspusern, Cic.OpenLease.Service.PEUNIHelper.Areas.IT, sysitma))
                {
                    PEUNIHelper.ConnectNodes(_context, PEUNIHelper.Areas.IT, sysitma, param.sysperole);
                }
            }







            ang.SYSWFUSER = param.syswfuser;
            PersonDto pers = PeroleHelper.DeliverPerson(_context, param.sysperole);
            ang.SYSBERATADDB = pers.sysperson;
            return pers.sysperson;

        }
    }
}