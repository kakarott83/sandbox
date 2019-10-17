using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OW.EF6.Model;
using System;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service.Services.DdOl
{


    [System.CLSCompliant(true)]
    public class AngebotBinaryDao
    {
        #region Private variables
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DdOwExtended _context;

        #endregion

        #region Constructors
        public AngebotBinaryDao(DdOwExtended context)
        {
            _context = context;
        }
        #endregion

        public void saveOrUpdateAngebotData(long sysid, byte[] data)
        {

            // Open the connection
           // _context.Connection.Open();
            //TRANSACTIONS
            // Create a transaction
           // DbTransaction OlTransaction = _context.Connection.BeginTransaction();

            var test = from w in _context.WFDADOC
                       where w.SYSID == sysid
                       select w;
            WFDADOC d = test.FirstOrDefault();
            if (d == null)
                d = new WFDADOC();

            d.AREA = "ANGEBOT";
            d.DESCRIPTION = "SA3 Picture";
            d.DOCFILE = data;
            d.SYSID = sysid;

            _context.SaveChanges();
            //OlTransaction.Commit();


        }

        public void copyPictureData(long fromId, long toId)
        {
            try
            {
                byte[] data = loadDataFromAngebot(fromId);
                long syswfdadoc = saveAngebotData(data);
                linkToAngebot(syswfdadoc, toId);
            }
            catch (Exception )
            {
               // _log.Debug("Picture not copied",ex);
            }
        }

        public long? getPictureIdFromAngebot(long sysid)
        {
            var test = from w in _context.WFDADOC
                       where w.SYSID==sysid && w.AREA=="ANGEBOT" 
                       select w;
            WFDADOC d = test.FirstOrDefault();
            if (d != null)
                return d.SYSWFDADOC;
            return null;
        }

        public void linkToAngebot(long SYSWFDADOC, long sysid)
        {

            // Open the connection
           // _context.Connection.Open();
            //TRANSACTIONS
            // Create a transaction
           // DbTransaction OlTransaction = _context.Connection.BeginTransaction();

            var test = from w in _context.WFDADOC
                       where w.SYSWFDADOC == SYSWFDADOC
                       select w;
            WFDADOC d = test.FirstOrDefault();
            if (d == null)
                return;


            d.SYSID = sysid;

            _context.SaveChanges();
           // OlTransaction.Commit();


        }

        public long saveAngebotData(byte[] data)
        {
            //TRANSACTIONS
            // Open the connection
           // _context.Connection.Open();

            // Create a transaction
            //DbTransaction OlTransaction = _context.Connection.BeginTransaction();

            WFDADOC d = new WFDADOC();

            d.AREA = "ANGEBOT";
            d.DESCRIPTION = "Picture";
            d.DOCFILE = data;

            _context.WFDADOC.Add(d);
            _context.SaveChanges();


            //OlTransaction.Commit();
            return d.SYSWFDADOC;

        }

        public byte[] loadDataFromAngebot(long sysid)
        {
            var test = from w in _context.WFDADOC
                       where w.SYSID == sysid
                       select w;
            WFDADOC d = test.FirstOrDefault();
            if (d == null)
                throw new Exception("No Picture for Angebot " + sysid + " found!");
            return d.DOCFILE;
        }

        public byte[] loadDataById(long SYSWFDADOC)
        {
            var test = from w in _context.WFDADOC
                       where w.SYSWFDADOC == SYSWFDADOC
                       select w;
            WFDADOC d = test.FirstOrDefault();
            if (d == null)
                throw new Exception("No Picture " + SYSWFDADOC + " found!");
            return d.DOCFILE;
        }


    }
}