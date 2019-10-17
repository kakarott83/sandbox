using AutoMapper;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public class IncentivierungDao : IIncentivierungDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const String GETIDENTRULES = "select sysprkgroupident,prkgroup.sysprkgroup,sysprkgroupident.description,prkgroup.code, sysprkgroupident.exprident from prkgroupident,prkgroup where prkgroup.sysprkgroup=prkgroupident.sysprkgroup and prkgroupident.activeflag=1 and (prkgroupident.validfrom is null or prkgroupident.validfrom<=   :perDate  or prkgroupident.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and (prkgroupident.validuntil is null or prkgroupident.validuntil>=  :perDate  or prkgroupident.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy'))";
        private const String CONTRACTDATA = "select ANTKALK.BGEXTERNBRUTTO-ANTKALK.SZBRUTTO umsatz,  (select count(*) from antvs where antvs.sysantrag=vt.sysantrag and antvs.praemie>0) anzInsurance from ANTKALK, VT  where VT.sysantrag = ANTKALK.sysantrag and vt.sysid=:sysid";
        private const String SEGMENT = "select prkgroup.code from prkgroup,person,perole where kundengruppe=prkgroup.code and perole.sysperson=person.sysperson and perole.sysperole=:sysperole";
        private const String QUERYREPORT="select syswftx from wftx where document='REPORT_VK_INCENTIVES'";

        //basis für provision
        private const String GESAMTUMSATZPROVPLANVT = @"SELECT SUM(ANTKALK.BGEXTERNBRUTTO-ANTKALK.SZBRUTTO) GESAMTUMSATZ 
                            from ANTKALK, VT 
                            where VT.sysantrag = ANTKALK.sysantrag
                            and (
                                  vt.sysid = :sysvt 
                                  or 
                                  exists (select 1 from perole,prov where perole.sysperole=:sysperole and prov.area = 'PRPROVSET' and prov.syslease = :sysprprovset and prov.syspartner = perole.sysperson and  prov.sysvt =VT.sysid )
                                )";

        private const String AKTPAYOUTTOTAL = "select sum(auszahlung) from prov,perole where sysfi >0 and (STORNOKZ is null or stornokz=0) and perole.sysperole=:sysperole and prov.area = 'PRPROVSET' and prov.syslease = :sysprprovset and prov.syspartner = perole.sysperson";

        /// <summary>
        /// saves the incentive provisions and the traces for them
        /// </summary>
        /// <param name="provs"></param>
        /// <param name="tracing"></param>
        public void createIncentiveProvisions(List<AngAntProvDto> provs, List<ProvKalkDto> tracing)
        {
         

            using (DdOlExtended ctx = new DdOlExtended())
            {
                //Update/Insert changed ones
                if (provs != null && provs.Count > 0)
                { 
                    foreach (AngAntProvDto prov in provs)
                    {
                        CIC.Database.OL.EF6.Model.PROV vtprov = new CIC.Database.OL.EF6.Model.PROV();
                        ctx.PROV.Add(vtprov);
                        Mapper.Map<AngAntProvDto, CIC.Database.OL.EF6.Model.PROV>(prov, vtprov);
                        vtprov.SYSPRPROVTYPE = prov.sysprprovtype;
                        vtprov.SYSPARTNER= prov.syspartner;
                        vtprov.SYSVT  =prov.sysvt;
                        ctx.SaveChanges();
                        
                        prov.sysprov = vtprov.SYSPROV;
                        //CIC.Database.OL.EF6.Model.PROV Felder: BASIS, PROVISIONPRO, PROVISION, SYSVT(VTReference speichert nicht?)
                    
                    }
                }
                
                 
                if(tracing != null && tracing.Count>0)
                {
                    foreach (ProvKalkDto trace in tracing)
                    {
                        CIC.Database.OL.EF6.Model.PROVKALK pkalk = new CIC.Database.OL.EF6.Model.PROVKALK();
                        Mapper.Map<ProvKalkDto, CIC.Database.OL.EF6.Model.PROVKALK>(trace, pkalk);
                        pkalk.SYSPROVKALK = 0;
                        ctx.PROVKALK.Add(pkalk);
                        if (trace.prov != null && trace.prov.sysprov>0)
                            pkalk.SYSPROV=trace.prov.sysprov;
                        pkalk.SYSPRPROVTYPE=trace.sysprprovtype;
                        
                        
                        ctx.SaveChanges();
                    }
                }
                ctx.SaveChanges();
                        
            }
        }

        /// <summary>
        /// gets all ident rules for the given date
        /// </summary>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public List<PrkGroupIdentDto> getIdentRules(DateTime perDate)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                return ctx.ExecuteStoreQuery<PrkGroupIdentDto>(GETIDENTRULES, parameters.ToArray()).ToList();
            }
        }



        /// <summary>
        /// returns the segmentation  prkgroup code currently assigned to the sales person
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public String getSegmentation(long sysperole)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                return ctx.ExecuteStoreQuery<String>(SEGMENT, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// returns the segmentation prkgroup name currently assigned to the segmentation code
        /// </summary>
        /// <param name="segmentCode">code of the segment</param>
        /// <param name="perDate">date when the segment is valid (e.g. DateTime.Today)</param>
        /// <returns>nice name of the segment</returns>
        public String getSegmentationName(string segmentCode, DateTime perDate)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = segmentCode });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perdate", Value = perDate });
                string name = ctx.ExecuteStoreQuery<String>("SELECT name FROM prkgroup where :code=:code and to_date(validfrom) <= :perdate and :perdate <= to_date(validuntil)", parameters.ToArray()).FirstOrDefault();
                if (name == null || name.Length == 0)
                {
                    if (segmentCode.Length > 6)
                        name = segmentCode.Substring(6); //Präfix "HDGR._" wird abgeschnitten
                    else
                        name = segmentCode;
                }
                return name;
            }
        }

        /// <summary>
        /// get provision set sysvg by provision set id and field
        /// </summary>
        /// <param name="sysprprovset"></param>
        /// <param name="sysprfld"></param>
        /// <returns>sysvg</returns>
        public long getSysvg(long sysprprovset, long sysprfld)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                //sysvg
                string query = "select sysvg from prprovstep where sysprprovset=:sysprprovset and sysprfld=:sysprfld";
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprprovset", Value = sysprprovset });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprfld", Value = sysprfld });
                return ctx.ExecuteStoreQuery<long>(query, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// get value groups by sysvg
        /// </summary>
        /// <param name="sysvg"></param>
        /// <returns>value groups</returns>
        public List<VGValuesDto> getSegmentations(long sysvg)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                //
                //zeile aus akt segment:
                string query = "select * from  CIC.VGVALUES_V where sysvg=:sysvg order by linescale, to_num(x1),to_num(y1)";
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvg", Value = sysvg });
                return ctx.ExecuteStoreQuery<VGValuesDto>(query, parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// returns the segmentation code, identified by the ident rules for the given prkgroup, date by the given gesamtumsatz
        /// </summary>
        /// <param name="gesamtumsatz"></param>
        /// <param name="sysprkgroup"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public String getSegmentation(double gesamtumsatz, long sysprkgroup, DateTime perDate)
        {
            List<PrkGroupIdentDto> rules = getIdentRules(perDate);
            using (PrismaExtended ctx = new PrismaExtended())
            {
                rules = (from t in rules
                         where t.sysprkgroup == sysprkgroup
                         select t).ToList();
                foreach (PrkGroupIdentDto ident in rules)
                {
                    String query = "select 1 from dual where " + ident.exprident;

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = gesamtumsatz });
                    long isRule = ctx.ExecuteStoreQuery<long>(query, parameters.ToArray()).FirstOrDefault();
                    if (isRule > 0)
                    {
                        return ident.code;
                    }
                }

            }
            return null;
        }

        /// <summary>
        /// Fetches needed data of contract for incentivation calculation
        /// </summary>
        /// <param name="sysvt"></param>
        /// <returns></returns>
        public VTIncentivierungDataDto getContractData(long sysvt)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysvt });
                return ctx.ExecuteStoreQuery<VTIncentivierungDataDto>(CONTRACTDATA, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// returns the actual payed money for the perole and provplan
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysprprovset"></param>
        /// <returns></returns>
        public double getAktpayoutTotal(long sysperole, long sysprprovset)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprprovset", Value = sysprprovset });
                return ctx.ExecuteStoreQuery<double>(AKTPAYOUTTOTAL, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// gets the current umsatz for the given provplan for the given role
        /// sysvt allows to include the not yet provisioned contract
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysprprovset"></param>
        /// <param name="sysvt"></param>
        /// <returns></returns>
        public double getProvPlanUmsatz(long sysperole, long sysprprovset, long sysvt)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = sysvt });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprprovset", Value = sysprprovset });
                return ctx.ExecuteStoreQuery<double>(GESAMTUMSATZPROVPLANVT, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// get the document id
        /// </summary>
        /// <returns>syswftx</returns>
        public long getDocumentId()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                
                return ctx.ExecuteStoreQuery<long>(QUERYREPORT,null).FirstOrDefault();
            }
        }
    }
}
