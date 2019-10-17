using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.DAO.Prisma
{
    /// <summary>
    /// Prisma Data Access Object
    /// </summary>
    public class PrismaServiceDao : IPrismaServiceDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string QUERYVSTYPPRODUCT = "select disabledgrp, neededgrp, neededpos, disabledpos, mitfin, setid, posid, rank, validfrom,validuntil,validfromgrp,validuntilgrp,   flagdefault,sysprproduct, vssysvsart sysvsart,vssysvstyp sysvstyp, vsperson sysperson,isrsv " +
  " from ( " +
  " SELECT v1.*, vsart.sysvsart vssysvsart,vtyp.sysvstyp vssysvstyp, vs.sysperson vsperson,vtyp.flagdefault flagdefault,vtyp.validfrom, vtyp.validuntil,0 isrsv    FROM  CIC.PRVSOB2_V  v1,vsart,vstyp vtyp,vs where vsart.sysvsart = v1.sysvsart AND v1.method=1 AND vsart.sysvsart = vtyp.sysvsart AND vs.sysperson = vtyp.sysvs " +
  " union all SELECT v1.*, vsart.sysvsart vssysvsart,vtyp.sysvstyp vssysvstyp, vs.sysperson  vsperson,vtyp.flagdefault flagdefault,vtyp.validfrom, vtyp.validuntil,1 isrsv from CIC.PRVSP2_V v1,vsart,vstyp vtyp,vs where vsart.sysvsart = v1.sysvsart AND v1.method=1 AND vsart.sysvsart = vtyp.sysvsart AND vs.sysperson = vtyp.sysvs) " +


   " union all select disabledgrp, neededgrp, neededpos, disabledpos, mitfin, setid, posid, rank, validfrom,validuntil,validfromgrp,validuntilgrp,flagdefault,sysprproduct,vssysvsart,vssysvstyp, vsperson,isrsv  " +
   " from ( " +
   "      SELECT v1.*, vsart.sysvsart vssysvsart,vtyp.sysvstyp vssysvstyp, vs.sysperson vsperson,vtyp.flagdefault flagdefault,vtyp.validfrom, vtyp.validuntil,0 isrsv  FROM  CIC.PRVSOB2_V  v1,vsart,vstyp vtyp,vs where vs.sysperson = v1.sysperson AND v1.method=2 AND vsart.sysvsart = vtyp.sysvsart AND vs.sysperson = vtyp.sysvs " +
   "      union all SELECT v1.*, vsart.sysvsart vssysvsart,vtyp.sysvstyp vssysvstyp, vs.sysperson  vsperson,vtyp.flagdefault flagdefault,vtyp.validfrom, vtyp.validuntil,1 isrsv from CIC.PRVSP2_V v1,vsart,vstyp vtyp,vs where vs.sysperson = v1.sysperson AND v1.method=2 AND vsart.sysvsart = vtyp.sysvsart AND vs.sysperson = vtyp.sysvs) " +

  " union all select disabledgrp, neededgrp, neededpos, disabledpos, mitfin, setid, posid, rank, validfrom,validuntil,validfromgrp,validuntilgrp,flagdefault,sysprproduct, vssysvsart,vssysvstyp, vsperson,isrsv " +
  " from ( " +
   "    SELECT v1.*, vsart.sysvsart vssysvsart,vtyp.sysvstyp vssysvstyp, vs.sysperson vsperson,vtyp.flagdefault flagdefault,vtyp.validfrom, vtyp.validuntil,0 isrsv  FROM  CIC.PRVSOB2_V   v1,vsart,vstyp vtyp,vs where vtyp.sysvstyp = v1.sysvstyp AND v1.method=3 AND vsart.sysvsart = vtyp.sysvsart AND vs.sysperson = vtyp.sysvs " +
  "     union all SELECT v1.*, vsart.sysvsart vssysvsart,vtyp.sysvstyp vssysvstyp, vs.sysperson  vsperson,vtyp.flagdefault flagdefault,vtyp.validfrom, vtyp.validuntil,1 isrsv  FROM  CIC.PRVSP2_V v1,vsart,vstyp vtyp,vs where vtyp.sysvstyp = v1.sysvstyp AND v1.method=3 AND vsart.sysvsart = vtyp.sysvsart AND vs.sysperson = vtyp.sysvs) ";


        /*   "select disabledgrp, neededgrp, neededpos, disabledpos, mitfin, setid, posid, rank, validfrom,validuntil,validfromgrp,validuntilgrp,   flagdefault,sysprproduct, vsart.sysvsart,vstyp.sysvstyp, vs.sysperson from (SELECT *  FROM  CIC.PRVSOB2_V  union all select * from CIC.PRVSP2_V) v1,vsart,vstyp,vs where vsart.sysvsart = v1.sysvsart AND v1.method=1 AND vsart.sysvsart = vstyp.sysvsart AND vs.sysperson = vstyp.sysvs " +
                                        " union all select disabledgrp, neededgrp, neededpos, disabledpos, mitfin, setid, posid, rank, validfrom,validuntil,validfromgrp,validuntilgrp,   flagdefault,sysprproduct, vsart.sysvsart,vstyp.sysvstyp, vs.sysperson from (SELECT *  FROM  CIC.PRVSOB2_V  union all select * from CIC.PRVSP2_V) v1,vsart,vstyp,vs where vs.sysperson = v1.sysperson AND v1.method=2 AND vsart.sysvsart = vstyp.sysvsart AND vs.sysperson = vstyp.sysvs  " +
                                        " union all select disabledgrp, neededgrp, neededpos, disabledpos, mitfin, setid, posid, rank, validfrom,validuntil,validfromgrp,validuntilgrp,   flagdefault,sysprproduct, vsart.sysvsart,vstyp.sysvstyp, vs.sysperson from (SELECT *  FROM  CIC.PRVSOB2_V  union all select * from CIC.PRVSP2_V) v1,vsart,vstyp,vs where vstyp.sysvstyp = v1.sysvstyp AND v1.method=3 AND vsart.sysvsart = vstyp.sysvsart AND vs.sysperson = vstyp.sysvs ";*/

        private const String QUERYRSVCODE = @"select prrsvcode.* from PRCLRSVSET,prrsvset,prrsvcode where prrsvset.sysprrsvset=prclrsvset.sysprrsvset and prrsvcode.sysprrsvset=prrsvset.sysprrsvset 
and (prrsvset.validfrom is null or prrsvset.validfrom<=:perdate or prrsvset.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy'))
and (prrsvset.validuntil is null or prrsvset.validuntil>=:perdate or prrsvset.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy'))
and (prrsvcode.validfrom is null or prrsvcode.validfrom<=:perdate or prrsvcode.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy'))
        and prclrsvset.sysprproduct=:sysprproduct and prrsvset.activeflag=1 order by prrsvcode.validfrom desc";
        
        private const string QUERYSERVICELINKS = "select * from {0} where ACTIVEFLAG=1";
        private const string QUERYVSTYP = "select * from vstyp where ACTIVEFLAG=1 ";
        private const string QUERYVSART = "select * from vsart where sysvsart=:sysvsart";

        private DateTime nullDate = new DateTime(1800, 1, 1);

        /// <summary>
        /// Standarad Constuctor
        /// Database access Object for Prisma Products and Parameters
        /// </summary>
        public PrismaServiceDao()
        {
        }

        /// <summary>
        /// returns all condition links for a service
        /// </summary>
        /// <param name="tableName">link typ table name</param>
        /// <returns></returns>
        virtual public List<ServiceConditionLink> getServiceConditionLinks(String tableName)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                // Security check: Aufruf nur mit Konstanten.
                return ctx.ExecuteStoreQuery<ServiceConditionLink>(String.Format(QUERYSERVICELINKS, tableName), null).ToList();
            }
        }

        /// <summary>
        /// Versicherungstypen holen
        /// </summary>
        /// <returns>Liste mit Versicherungstypen</returns>
        virtual public List<VSTYP> getVSTYP()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<VSTYP>(QUERYVSTYP, null).ToList();
            }
        }

        /// <summary>
        ///  returns all Link info between VSTYP and PRPRODUCT
        /// </summary>
        /// <returns>Parameter list</returns>
        virtual public List<PRVSDto> getVSTYPForProduct(DateTime perDate, long productID)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<PRVSDto> values = ctx.ExecuteStoreQuery<PRVSDto>(QUERYVSTYPPRODUCT, null).ToList();

                // Debug only
                //foreach (PRVSDto item in values)
                //{
                //    _log.Debug("Item: Beschr:" + item.BESCHREIBUNG + " Bez:" + item.BEZEICHNUNG + " code:" + item.CODE + " prprod:" + item.SYSPRPRODUCT);
                //}

                var q = from c in values
                        where c.SYSPRPRODUCT == productID && !(c.NEEDEDGRP == 0 && c.DISABLEDGRP == 1)
                        && (c.VALIDFROM == null || c.VALIDFROM <= perDate || c.VALIDFROM <= nullDate)
                        && (c.VALIDUNTIL == null || c.VALIDUNTIL >= perDate || c.VALIDUNTIL <= nullDate)
                        && (c.VALIDFROMGRP == null || c.VALIDFROMGRP <= perDate || c.VALIDFROMGRP <= nullDate)
                        && (c.VALIDUNTILGRP == null || c.VALIDUNTILGRP >= perDate || c.VALIDUNTILGRP <= nullDate)
                        select c;

                //group by sysvstyp and take the ones that are needed
                Dictionary<long, PRVSDto> groupMap = new Dictionary<long, PRVSDto>();

                foreach (PRVSDto row in q)
                {
                    Dictionary<long, PRVSDto> useMap = groupMap;
                    if (!useMap.ContainsKey(row.SYSVSTYP))
                    {
                        useMap[row.SYSVSTYP] = row;
                    }
                    else
                    {
                        PRVSDto lrow = useMap[row.SYSVSTYP];
                        if (row.FLAGDEFAULT > 0)
                            useMap[row.SYSVSTYP].FLAGDEFAULT = 1;

                        if (row.NEEDEDGRP > 0)
                            useMap[row.SYSVSTYP].NEEDEDGRP = 1;

                        if (row.DISABLEDGRP > 0)
                            useMap[row.SYSVSTYP].DISABLEDGRP = 1;

                        if (row.NEEDEDPOS > 0)
                            useMap[row.SYSVSTYP].NEEDEDPOS = 1;

                        if (row.DISABLEDPOS > 0)
                            useMap[row.SYSVSTYP].DISABLEDPOS = 1;
                    }
                }

                List<PRVSDto> rval = new List<PRVSDto>();
                foreach (long key in groupMap.Keys)
                {
                    rval.Add(groupMap[key]);
                }

                return rval;
            }
        }

        /// <summary>
        /// Returns PRRSVCODE for an PRPRODUKT
        /// </summary>
        /// <param name="perDate">perDate</param>
        /// <param name="sysprprodukt">sysprprodukt</param>
        /// <returns>prrsvcode</returns>
        virtual public PRRSVCODE getPrrsvCodeByPrProdukt(DateTime perDate, long sysprprodukt)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                
                  object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprproduct", Value = sysprprodukt },
                                   new Devart.Data.Oracle.OracleParameter { ParameterName = "perdate", Value = perDate }};
                  PRRSVCODE rval = ctx.ExecuteStoreQuery<PRRSVCODE>(QUERYRSVCODE, pars).FirstOrDefault();
                  if (rval == null)
                      rval = new PRRSVCODE();
                  return rval;
            }
        }

        /// <summary>
        /// returns the VSART
        /// </summary>
        /// <param name="sysvsart"></param>
        /// <returns></returns>
        virtual public VSART getVSART(long sysvsart)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvsart", Value = sysvsart } };
                return ctx.ExecuteStoreQuery<VSART>(QUERYVSART, pars).FirstOrDefault();
            }
        }
    }
}