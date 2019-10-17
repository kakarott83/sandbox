using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public class DMSDao : IDMSDao
    {
         

        private const String QUERY_ANTRAG = "select antrag.zustand,antrag.attribut,antrag.syskd SYSKD, antrag.antrag AKTEID, antrag.attribut STATUS, it.syskdtyp KUNTY, 'Interessent' KONTY, it.sysit ID, it.sysit ITID, null KNR, it.name NAME, it.vorname VORNA, it.zusatz ZUSATZ, it.strasse||' '||it.hsnr STRNR, it.plz PLZ, it.ort ORT, it.gebdatum GEB, it.mitarbeiterflag CSMA from antrag,it where antrag.sysit=it.sysit and antrag.sysid=:sysid";
        private const String QUERY_ANTRAG_P = "select antrag.zustand,antrag.attribut,antrag.syskd SYSKD, antrag.antrag AKTEID, antrag.attribut STATUS, person.syskdtyp KUNTY, 'Kunde' KONTY, person.sysperson ID,antrag.sysit ITID, person.sysreferenz KNR, person.name NAME, person.vorname VORNA, person.zusatz ZUSATZ, person.strasse||' '||person.hsnr STRNR, person.plz PLZ, person.ort ORT, person.gebdatum GEB, person.mitarbeiterflag CSMA from antrag,person where antrag.syskd=person.sysperson and antrag.sysid=:sysid";


        private const String QUERY_ANGEBOT = "select angebot.zustand, angebot.attribut,angebot.angebot AKTEID, angebot.attribut STATUS, it.syskdtyp KUNTY, 'Interessent' KONTY,it.sysit ID, it.sysit ITID, null KNR, it.name NAME, it.vorname VORNA, it.zusatz ZUSATZ, it.strasse||' '||it.hsnr STRNR, it.plz PLZ, it.ort ORT, it.gebdatum GEB, it.mitarbeiterflag CSMA from angebot, it  where angebot.sysit=it.sysit(+) and angebot.sysid=:sysid";
        private const String QUERY_ANGEBOT_P = "select angebot.zustand,angebot.attribut, angebot.angebot AKTEID, angebot.attribut STATUS, person.syskdtyp KUNTY, 'Kunde' KONTY, person.sysperson ID,angebot.sysit ITID, person.sysreferenz KNR, person.name NAME, person.vorname VORNA, person.zusatz ZUSATZ, person.strasse||' '||person.hsnr STRNR, person.plz PLZ, person.ort ORT, person.gebdatum GEB, person.mitarbeiterflag CSMA from angebot,person  where angebot.syskd=person.sysperson(+) and angebot.sysid=:sysid";

        private const String QUERY_VT = "select vt.zustand,vt.attribut,vt.vertrag AKTEID, vt.attribut STATUS, person.syskdtyp KUNTY, 'Kunde' KONTY, person.sysperson ID,VT.SYSIT ITID, person.sysreferenz KNR, person.name NAME, person.vorname VORNA, person.zusatz ZUSATZ, person.strasse||' '||person.hsnr STRNR, person.plz PLZ, person.ort ORT, person.gebdatum GEB, person.mitarbeiterflag CSMA from vt,person where vt.syskd=person.sysperson and vt.sysid=:sysid";
        private const String QUERY_PERSON = "select null AKTEID, null STATUS, person.syskdtyp KUNTY, 'Kunde' KONTY, person.sysperson ID,  person.sysreferenz KNR, person.name NAME, person.vorname VORNA, person.zusatz ZUSATZ, person.strasse||' '||person.hsnr STRNR, person.plz PLZ, person.ort ORT, person.gebdatum GEB, person.mitarbeiterflag CSMA from person where sysperson=:sysid";
		private const String QUERY_IT = "select null AKTEID, null STATUS, it.syskdtyp KUNTY, 'Interessent' KONTY, it.sysit ID, it.sysit ITID, null KNR, it.name NAME, it.vorname VORNA, it.zusatz ZUSATZ, it.strasse||' '||it.hsnr STRNR, it.plz PLZ, it.ort ORT, it.gebdatum GEB, it.mitarbeiterflag CSMA from it where sysit=:sysid";

		private const String QUERY_PEROLE = @"SELECT person.sysreferenz referenz,
												CASE WHEN roletype.typ = 6
												THEN 'bnowp.pa' 
												WHEN roletype.typ = 7
												THEN 'bnowp.ma' 
												ELSE '' 
												END ARCHIVE,
                                                person.sysperson partnernr,
                                                case when roletype.code='MT' then (select sysperson from perole pp where pp.sysperole=perole.sysparent) when roletype.code='HD' then person.sysperson  else 0 end HAENDLNR,
                                                case when roletype.code='MT' then 'MITARBEITER' when roletype.code='HD' then 'HÄNDLER'  else '' end TYPE,
                                                case when perole.inactiveflag=0 then 'AKTIV' else 'INAKTIV' end AKTIV,
                                                person.name NAME,
                                                person.vorname VORNA,
                                                person.ort ORT,
                                                person.plz PLZ
											FROM person, perole, roletype
											WHERE person.sysperson = perole.sysperson
												AND roletype.sysroletype = perole.sysroletype
												AND perole.sysperole = :sysid";

		private const String QUERY_DMSUPL = "select sysdmsupl from dmsupl where (referenz=:ref and referenz is not null) or (sysreferenz=:sysref and sysreferenz>0)";
        private const String QUERY_RETENTIONDATE = "select refdate from dateref where area=:area and sysid=:sysid order by sysdateref desc";
        private const String QUERY_RETENTIONDATE_VT = "select refdate from dateref,vt where dateref.area='ANTRAG' and dateref.sysid=vt.sysantrag and vt.sysid=:sysid and 'VT'=:area order by sysdateref desc";
        private const String QUERY_VORGNR = "select vorgang from dmsupl where sysdmsupl=:sysdmsupl and sysdmsupl>0";
        
        private const String QUERY_FILE_ATT = "select 'application/pdf' ATT_FILE_TYPE, to_char(dmsdoc.gedrucktam,'YYYYMMDD HH24MISS') ATT_SCAN_DATE, to_char(sysdate,'YYYYMMDD') ATT_DOC_DATE, 1 ATT_PAGE_COUNT, 0 ATT_CHANNEL_TYPE, 0 ATT_CHANNEL_INFO, 0 ATT_PRODUCT,'de' ATT_LANGUAGE, wftx.document ATT_DOC_TYPE  from dmsdoc,wftx,dmsdocarea where dmsdocarea.sysdmsdoc=dmsdoc.sysdmsdoc and wftx.syswftx=dmsdoc.syswftx and dmsdoc.sysdmsdoc=:sysid";

        private const String QUERY_FINDANGEBOT = "select 'ANGEBOT' AREA, angebot.sysid AREAID from angebot where angebot=:nummer";
        private const String QUERY_FINDANTRAG = "select 'ANTRAG' AREA, antrag.sysid AREAID from antrag where antrag=:nummer";
        private const String QUERY_FINDVT = "select 'VT' AREA, vt.sysid AREAID from vt where vertrag=:nummer";
        private const String QUERY_FINDPERSON = "select 'PERSON' AREA, person.sysperson AREAID from person where sysperson=:nummer";
        private const String QUERY_FINDUPLINST = "select sysdmsuplinst from dmsuplinst where area=:area and sysid=:sysid";

        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the dmsakte-instance
        /// </summary>
        /// <param name="sysdmsakte"></param>
        /// <returns></returns>
        public DMSAKTE getDMSAkte(long sysdmsakte)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                DMSAKTE rval = 
                (from s in ctx.DMSAKTE
                        where s.SYSDMSAKTE == sysdmsakte
                        select s).FirstOrDefault();
                if (rval != null)
                    ctx.Detach(rval);
                return rval;
            }
        }


        /// <summary>
        /// Saves the dmsakte back to database
        /// </summary>
        /// <param name="akte"></param>
        public void updateDMSAkte(DMSAKTE akte)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                
              
                ctx.Entry(akte).State = System.Data.Entity.EntityState.Modified;
                
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Saves the dmsupldetails back to database
        /// </summary>
        /// <param name="akte"></param>
        public void createOrUpdateDmsUpldetails(List<DMSUPLDETAIL> details, long sysdmsupl)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                foreach (DMSUPLDETAIL det in details)
                {
                    
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "att", Value = det.ATTRIBUTNAME });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysdmsupl", Value = sysdmsupl });
                    long sysdmsupldetail = ctx.ExecuteStoreQuery<long>("SELECT SYSDMSUPLDETAIL from CIC.DMSUPLDETAIL where ATTRIBUTNAME=:att and SYSDMSUPL=:sysdmsupl",parameters.ToArray()).FirstOrDefault();

                    if (sysdmsupldetail == 0)//new one
                    {
                        det.SYSDMSUPL=sysdmsupl;
                        ctx.DMSUPLDETAIL.Add(det);
                    }
                    else {//Update
                        try
                        {
                            det.SYSDMSUPL=sysdmsupl;

                            //det.EntityKey = ctx.getEntityKey(typeof(DMSUPLDETAIL), sysdmsupldetail);
                            det.SYSDMSUPLDETAIL = sysdmsupldetail;
                            ctx.Entry(det).State = System.Data.Entity.EntityState.Modified;
                        }catch(Exception exc)
                        {
                            _log.Error("Update of DMSUPLDETAIL Field failed for " + det.ATTRIBUTNAME + ": " + exc.Message);
                        }
                    }
                }
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Saves the dmsakte back to database
        /// </summary>
        /// <param name="akte"></param>
        public DMSUPL createOrUpdateDMSUPL(DMSUPL uploadData, long sysdmsuplinst)
        {
            if (uploadData.SYSDMSUPL > 0)
            {
                updateDMSUPL(uploadData);
                
                return uploadData;
            }
            using (DdOwExtended ctx = new DdOwExtended())
            {
                ctx.DMSUPL.Add(uploadData);
                
                ctx.SaveChanges();
                
                
            }
            return uploadData;
        }

        /// <summary>
        /// Updates DMSUPL
        /// </summary>
        /// <param name="akte"></param>
        public void updateDMSUPL(DMSUPL uploadData)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                
                
                ctx.Entry(uploadData).State = System.Data.Entity.EntityState.Modified;

                ctx.SaveChanges();
             
            }
        }

        /// <summary>
        /// Returns the file attribute data for DMS for the given sysdmsdoc
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        public DMSExportDataDto getFileDataForDMS(long sysdmsdoc)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {

                EaiparDao eaiParDao = new EaiparDao();
                String query = eaiParDao.getEaiParFileByCode("DMS_FILE_ATT", QUERY_FILE_ATT);

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysdmsdoc });
                DMSExportDataDto data = ctx.ExecuteStoreQuery<DMSExportDataDto>(query, parameters.ToArray()).FirstOrDefault();

                return data;
            }
        }
        /// <summary>
        /// Creates the DMSUPLINST for the given gebiet or updates its timestamp only
        /// </summary>
        /// <param name="gebietInfo"></param>
        /// <returns></returns>
        public long createOrUpdateDmsUplInst(GebietInfoDto gebietInfo)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = gebietInfo.area });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = gebietInfo.areaid });

                long sysdmsuplinst = ctx.ExecuteStoreQuery<long>(QUERY_FINDUPLINST, parameters.ToArray()).FirstOrDefault();
                if(sysdmsuplinst==0)
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = gebietInfo.area });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = gebietInfo.areaid });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "time", Value = DateTimeHelper.DateTimeToClarionTime(DateTime.Now) });
                    Devart.Data.Oracle.OracleParameter output = new Devart.Data.Oracle.OracleParameter { ParameterName = "myOutputParameter", OracleDbType = Devart.Data.Oracle.OracleDbType.Long, Direction = System.Data.ParameterDirection.ReturnValue };
                    parameters.Add(output);

                    ctx.ExecuteStoreCommand("INSERT INTO CIC.DMSUPLINST (AREA,SYSID,CREDATE,CRETIME) values(:area,:sysid,sysdate,:time) returning sysdmsuplinst  into :myOutputParameter", parameters.ToArray());
                    return Convert.ToInt64(output.Value);
                }
                else
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysdms", Value = sysdmsuplinst });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "time", Value = DateTimeHelper.DateTimeToClarionTime(DateTime.Now) });
                    ctx.ExecuteStoreCommand("Update CIC.DMSUPLINST set chgdate=sysdate, chgtime=:time where sysdmsuplinst=:sysdms", parameters.ToArray());
                    return sysdmsuplinst;
                }
                
            }
        }

        /// <summary>
        /// Determines the sysdmsupl for the uplinst and docid
        /// </summary>
        /// <param name="docid"></param>
        /// <param name="sysdmsuplinst"></param>
        /// <returns></returns>
        public long getSysDmsupl(long docid, long sysdmsuplinst)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "docid", Value = docid });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysdmsuplinst", Value = sysdmsuplinst });

                return ctx.ExecuteStoreQuery<long>("select sysdmsupl from cic.dmsupl where docid=:docid and sysdmsuplinst=:sysdmsuplinst", parameters.ToArray()).FirstOrDefault();
            }
        }
        /// <summary>
        /// Finds the vt, antrag, angebot for the given number or the person
        /// </summary>
        /// <param name="nummer"></param>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public GebietInfoDto getDMSTarget(String nummer, long sysperson)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "nummer", Value = nummer });
                if(nummer!=null)
                { 
                    GebietInfoDto data = ctx.ExecuteStoreQuery<GebietInfoDto>(QUERY_FINDVT, parameters.ToArray()).FirstOrDefault();
                    if (data!=null && data.areaid > 0) return data;
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "nummer", Value = nummer });
                    data = ctx.ExecuteStoreQuery<GebietInfoDto>(QUERY_FINDANTRAG, parameters.ToArray()).FirstOrDefault();
                    if (data != null && data.areaid > 0) return data;
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "nummer", Value = nummer });
                    data = ctx.ExecuteStoreQuery<GebietInfoDto>(QUERY_FINDANGEBOT, parameters.ToArray()).FirstOrDefault();
                    if (data != null && data.areaid > 0) return data;
                }
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "nummer", Value = sysperson });
                GebietInfoDto data2 = ctx.ExecuteStoreQuery<GebietInfoDto>(QUERY_FINDPERSON, parameters.ToArray()).FirstOrDefault();
                return data2;
            }
        }
        /// <summary>
        /// Returns the data for DMS for the given area/id
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public DMSExportDataDto getDataForDMS (String area, long sysid)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                EaiparDao eaiParDao = new EaiparDao();
                String query = "";
                //Fetch data from area for aconso-interface
                switch (area)
                {
                    case ("ANGEBOT"):
                        {
                            query = eaiParDao.getEaiParFileByCode("DMS_ANGEBOT", QUERY_ANGEBOT);
                            long sysperson = ctx.ExecuteStoreQuery<long>("SELECT SYSKD FROM ANGEBOT where sysid=" + sysid, null).FirstOrDefault();
                            if (sysperson > 0)
                                query = eaiParDao.getEaiParFileByCode("DMS_ANGEBOT_P", QUERY_ANGEBOT_P);
                            break;
                        }
                    case ("ANTRAG"):
                        {
                            query = QUERY_ANTRAG;
                            query = eaiParDao.getEaiParFileByCode("DMS_ANTRAG", QUERY_ANTRAG);
                            long sysperson = ctx.ExecuteStoreQuery<long>("SELECT SYSKD FROM ANTRAG where sysid=" + sysid, null).FirstOrDefault();
                            if (sysperson > 0)
                                query = query = eaiParDao.getEaiParFileByCode("DMS_ANTRAG_P", QUERY_ANTRAG_P); 
                            break;
                        }
                    case ("PERSON"): query = query = eaiParDao.getEaiParFileByCode("DMS_PERSON", QUERY_PERSON); break;
                    case ("VT"): query = eaiParDao.getEaiParFileByCode("DMS_VT", QUERY_VT); break;
                    case ("IT"): query = eaiParDao.getEaiParFileByCode("DMS_IT", QUERY_IT); break;
					case ("PEROLE"): query = eaiParDao.getEaiParFileByCode ("DMS_IT", QUERY_PEROLE); break;   // rh 20181015

					default:
                        return null;
                }


                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                DMSExportDataDto data = ctx.ExecuteStoreQuery <DMSExportDataDto> (query, parameters.ToArray()).FirstOrDefault();
                data.AREA = area;
                if (data == null) return null;


                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });

                if("VT".Equals(area))
                    data.RETENTIONDATE = ctx.ExecuteStoreQuery<DateTime>(QUERY_RETENTIONDATE_VT, parameters.ToArray()).FirstOrDefault();
                else
                    data.RETENTIONDATE = ctx.ExecuteStoreQuery<DateTime>(QUERY_RETENTIONDATE, parameters.ToArray()).FirstOrDefault();
                

                // GET vorgnr from dmsupload if available
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "ref", Value = data.AKTEID });
                parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "sysref", Value = data.KNR });
                
                data.sysdmsupl =  ctx.ExecuteStoreQuery<long> (QUERY_DMSUPL, parameters.ToArray()).FirstOrDefault();

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "sysdmsupl", Value = data.sysdmsupl });

                data.VORGNR = ctx.ExecuteStoreQuery <String> (QUERY_VORGNR, parameters.ToArray()).FirstOrDefault();

                return data;
            }
        }
    }
}
