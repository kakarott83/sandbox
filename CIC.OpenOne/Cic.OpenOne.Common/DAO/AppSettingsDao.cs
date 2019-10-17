using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Model.DdOl;
using System.Text.RegularExpressions;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;


namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// DAO für REG; REGSEC; REGVAR- Zugriff
    /// 
    /// </summary>
    public class AppSettingsDao : IAppSettingsDao
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const String QUERYREGVARSEC = "SELECT  regsec.sysreg, regsec.sysregsec sysregsec, CONNECT_BY_ROOT regsec.bezeichnung rootvariable, LEVEL pathlen, reg.bezeichnung  || SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/') path,  regvar.*, reg.bezeichnung   FROM reg, regsec LEFT OUTER JOIN regvar on regsec.sysregsec = regvar.sysregsec WHERE reg.sysreg = regsec.sysreg and LEVEL > 0 and regsec.sysreg in (select sysreg from reg where syswfuser = :psyswfuser)and regvar.sysregsec = :psysregsec and sysregvar is not null CONNECT BY PRIOR regsec.sysregsec = regsec.sysregsecparent start with regsec.sysregsecparent is null  ORDER BY  regvar.chgdate desc, regsec.sysregsec, rootvariable, pathlen, path";
        private const String QUERYREGVAR = "SELECT regsec.sysregsec sysregsec, CONNECT_BY_ROOT regsec.bezeichnung rootvariable, LEVEL pathlen, SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/') path,  regvar.*  FROM regsec LEFT OUTER JOIN regvar on regsec.sysregsec = regvar.sysregsec WHERE LEVEL > 0 and regsec.sysreg in (select sysreg from reg where syswfuser = :syswfuser) and sysregvar =:sysregval CONNECT BY PRIOR regsec.sysregsec = regsec.sysregsecparent start with regsec.bezeichnung in (select reg.bezeichnung from reg where syswfuser = :syswfuser)  ORDER BY  regvar.chgdate desc,regsec.sysregsec, rootvariable, pathlen, path";
        private const String QUERYREGVARREGCOMP = "SELECT regsec.sysregsec sysregsec, CONNECT_BY_ROOT regsec.bezeichnung rootvariable, LEVEL pathlen, SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/') path,  connect_by_isleaf leaf, regvar.*  FROM regsec LEFT OUTER JOIN regvar on regsec.sysregsec = regvar.sysregsec WHERE  (regvar.syswfuser is null or regvar.syswfuser= :syswfuser) and LEVEL > 0 and  connect_by_isleaf=1 and regsec.sysreg in (select sysreg from reg where syswfuser = :syswfuser and reg.bezeichnung = :regbezeichnung) and sysregvar is not null CONNECT BY PRIOR regsec.sysregsec = regsec.sysregsecparent start with regsec.bezeichnung = :bezeichnung ORDER BY  regvar.chgdate desc, regsec.sysregsec, rootvariable, pathlen, path";
        private const String QUERYREGVARREGCOMPAREA = "SELECT regsec.sysregsec sysregsec, CONNECT_BY_ROOT regsec.bezeichnung rootvariable, LEVEL pathlen, SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/') path,  connect_by_isleaf leaf, regvar.*  FROM regsec LEFT OUTER JOIN regvar on regsec.sysregsec = regvar.sysregsec WHERE  regvar.area=:area and (regvar.syswfuser is null or regvar.syswfuser= :syswfuser) and LEVEL > 0 and  connect_by_isleaf=1 and regsec.sysreg in (select sysreg from reg where syswfuser = :syswfuser and reg.bezeichnung = :regbezeichnung) and sysregvar is not null CONNECT BY PRIOR regsec.sysregsec = regsec.sysregsecparent start with regsec.bezeichnung = :bezeichnung ORDER BY  regvar.chgdate desc, regsec.sysregsec, rootvariable, pathlen, path";
        private const String QUERYREGVARREGCOMPAREASYSID = "SELECT regsec.sysregsec sysregsec, CONNECT_BY_ROOT regsec.bezeichnung rootvariable, LEVEL pathlen, SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/') path,  connect_by_isleaf leaf, regvar.*  FROM regsec LEFT OUTER JOIN regvar on regsec.sysregsec = regvar.sysregsec WHERE  regvar.area=:area and regvar.sysid=:areasysid and  (regvar.syswfuser is null or regvar.syswfuser= :syswfuser) and LEVEL > 0 and  connect_by_isleaf=1 and regsec.sysreg in (select sysreg from reg where syswfuser = :syswfuser and reg.bezeichnung = :regbezeichnung) and sysregvar is not null CONNECT BY PRIOR regsec.sysregsec = regsec.sysregsecparent start with regsec.bezeichnung = :bezeichnung ORDER BY  regvar.chgdate desc, regsec.sysregsec, rootvariable, pathlen, path";
        private const String QUERYCFG = "select 0 sysid, cfgvar.syscfgvar sysregvar, cfgvar.syscfgsec sysregsec, cfgvar.code code, cfgvar.wert wert, cfgvar.bezeichnung, '/'||cfg.code||'/'||cfgsec.code||'/'||cfgvar.code completepath from cfgvar,cfg, cfgsec where cfgvar.syscfgsec=cfgsec.syscfgsec and cfg.code='SETUP.NET' and cfgsec.syscfg=cfg.syscfg and cfgsec.code=:cfgsec";

        private const String QUERYREGVARWFUSER = "SELECT regsec.sysregsec sysregsec,  LEVEL pathlen, CONCAT('/',CONCAT(CONCAT(reg.bezeichnung,'/'),SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/'))) path,  regvar.*  FROM regsec LEFT OUTER JOIN regvar on regsec.sysregsec = regvar.sysregsec left outer join reg on reg.sysreg = regsec.sysreg WHERE LEVEL > 0 and regsec.sysreg in (select sysreg from reg where syswfuser = :syswfuser) and sysregvar is not null CONNECT BY PRIOR regsec.sysregsec = regsec.sysregsecparent start with regsec.sysregsecparent is null ORDER BY  regvar.chgdate desc,regsec.sysregsec,  pathlen, path";
        private const String QUERYREGVARREGPARTIAL = "SELECT regsec.sysregsec sysregsec,  LEVEL pathlen, SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/') path,  regvar.*  FROM regsec LEFT OUTER JOIN regvar on regsec.sysregsec = regvar.sysregsec WHERE LEVEL > 0 and regsec.sysreg in (select sysreg from reg where syswfuser = :syswfuser and reg.bezeichnung = :regbezeichnung) and sysregvar is not null CONNECT BY PRIOR regsec.sysregsec = regsec.sysregsecparent start with regsec.sysregsecparent is null ORDER BY  regvar.chgdate desc,regsec.sysregsec,  pathlen, path";
        private const String QUERYREGVARUPDATE = "UPDATE cic.regvar set cic.regvar.wert = :pwert, cic.regvar.chgdate=:pchgdate, cic.regvar.blobwert=:pblobwert where cic.regvar.sysregvar =:psysregvar";
        private const String QUERYREGVARDELETE = "DELETE FROM cic.regvar where cic.regvar.sysregvar =";

        private const String QUERYREGSEC = "SELECT regsec.sysregsec sysregsec, regsec.sysreg, CONNECT_BY_ROOT regsec.bezeichnung rootvariable, LEVEL pathlen, SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/') path FROM regsec  WHERE LEVEL > 0 and regsec.sysreg in (select sysreg from reg where syswfuser = :syswfuser) CONNECT BY PRIOR regsec.sysregsec = regsec.sysregsecparent start with regsec.bezeichnung = :bezeichnung  ORDER BY regsec.sysregsec, rootvariable, pathlen, path";
        private const String QUERYREGSECREG = "SELECT regsec.sysregsec sysregsec, regsec.sysreg, CONNECT_BY_ROOT regsec.bezeichnung rootvariable, LEVEL pathlen, SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/') path FROM regsec  WHERE LEVEL > 0 and regsec.sysreg in (select sysreg from reg where syswfuser = :syswfuser and reg.bezeichnung = :regbezeichnung) CONNECT BY PRIOR regsec.sysregsec = regsec.sysregsecparent start with regsec.bezeichnung = :bezeichnung  ORDER BY regsec.sysregsec, rootvariable, pathlen, path";

        private const String QUERYUPDATEREGSEC = "UPDATE cic.regsec set sysregsecparent=:psysregsecparent, regsec.sysreg =:psysreg where cic.regsec.sysregsec = :psysregsec";
        private const String QUERYREGSELECT = "SELECT * from cic.reg WHERE syswfuser = :psyswfuser and bezeichnung = :pregbezeichnung";
        
        private const String QUERYCREATEREG = "INSERT INTO cic.reg (bezeichnung,syswfuser,code) values (:pregbezeichnung , :psyswfuser, :pregbezeichnung)";
        private const String QUERYCREATEREGSEC = "INSERT INTO cic.regsec (bezeichnung,sysreg,code,sysregsecparent) values (:pbezeichnung , :psysreg, :pcode, :psysregsecparent)";
        private const String QUERYCREATEREGVAR = "INSERT INTO cic.regvar (sysregsec, code,wert,bezeichnung,werttyp,area,sysid,syswfuser,chgdate,blobwert) values (:sysregsec, :code,:wert,:bezeichnung,:werttyp,:area,:sysid, :syswfuser, :chgdate, :blobwert) returning sysregvar  into :myOutputParameter";


        private const String QUERYSECPATH = "select regvar.sysregsec,sec.pathlen,sec.path,regvar.sysregvar,sec.bezeichnung,regvar.* from (select reg.bezeichnung regbez, regsec.sysregsec, level pathlen, reg.bezeichnung || SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/') path,reg.bezeichnung from regsec left outer join reg on reg.sysreg=regsec.sysreg where regsec.code is not null and syswfuser=:syswfuser and reg.bezeichnung=:regbezeichnung CONNECT BY PRIOR regsec.sysregsec  = regsec.sysregsecparent start with regsec.code=:secpath) sec, regvar where regvar.sysregsec=sec.sysregsec";
            //"SELECT  regsec.sysreg, regsec.sysregsec sysregsec, CONNECT_BY_ROOT regsec.bezeichnung rootvariable, LEVEL pathlen, reg.bezeichnung  || SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/') path,  regvar.*, reg.bezeichnung   FROM reg, regsec LEFT OUTER JOIN regvar on regsec.sysregsec = regvar.sysregsec WHERE reg.sysreg = regsec.sysreg and LEVEL > 0 and regsec.code='USRCACHE'  and regvar.sysregsec in (select sysregsec from (SELECT regsec.sysregsec, SYS_CONNECT_BY_PATH(regsec.bezeichnung, '/') path  FROM regsec  WHERE LEVEL > 0  and regsec.sysreg in (select sysreg from reg where syswfuser = :syswfuser and reg.bezeichnung = :regbezeichnung) CONNECT BY PRIOR regsec.sysregsec = regsec.sysregsecparent start with regsec.sysregsecparent is null) paths where paths.path like :secpath ) and sysregvar is not null CONNECT BY PRIOR regsec.sysregsec = regsec.sysregsecparent start with regsec.sysregsecparent is null  ORDER BY  regvar.chgdate desc, regsec.sysregsec, rootvariable, pathlen, path";

        private const String QUERYREGVARPATH = "with choose_root_here as (" +
                                        "select bezeichnung  as root from reg where sysreg in (select regsec.sysreg  from regsec left outer join regvar on regsec.sysregsec = regvar.sysregsec where sysregvar = :psysregvar)" +
                                        ") select choose_root_here.root || path as bezeichnung from " +
                                        "(select level lvl, sys_connect_by_path(bezeichnung, '/') path, connect_by_isleaf leaf, cp.sysregsecparent,  cp.sysregsec " +
                                        " from regsec cp connect by nocycle prior cp.sysregsec= cp.sysregsecparent start with cp.sysregsecparent is null)" +
                                        "cross join choose_root_here where leaf = 1 AND sysregsec in (select sysregsec from regvar where sysregvar = :psysregvar)";

        private const String QUERYREGVARSYS = "select * from cic.regvar where sysregvar= :psysregvar";

        private const String QUERYREG = "select sysreg from cic.reg where syswfuser= :psyswfuser and bezeichnung = :pbezeichnung";


        private const String QUERYSYSREGSEC = "SELECT sysregsec from REGSEC where bezeichnung = :pbezeichnung and sysreg = :psysreg and code = :pcode and sysregsecparent = :psysregsecparent";
        private const String QUERYSYSREGSECPARENTNULL = "SELECT sysregsec from REGSEC where bezeichnung = :pbezeichnung and sysreg = :psysreg and code = :pcode and sysregsecparent is NULL";

        /// <summary>
        /// AppSettingsItems laden
        /// 
        /// Beispiel:
        ///  AppSettingsDao ad = new AppSettingsDao();
        ///  igetAppSettingsItemsDto input = new igetAppSettingsItemsDto();
        ///  input.bezeichnung = "/GLOBAL/LUCENE/MAXIDX";//Pfad bis Var, beginnender / wichtig!
        ///  input.area = area;//optional
        ///  input.syswfuser = -1;//Required
        ///  RegVarDto[] rvars = ad.deliverAppSettingsItems(input);
        /// </summary>
        /// <returns>array ofAppRegisterDtos</returns>
        public RegVarDto[] deliverAppSettingsItems(igetAppSettingsItemsDto input)
        {
            List<RegVarDto> values = null;
            String prefix = "";

            using (DdOwExtended ctx = new DdOwExtended())
            {
                REGVAR reg = new REGVAR();
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                //direkte suche über regvarkey
                if (input.sysregvar != 0)
                {

                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysregvar", Value = input.sysregvar });
                    String regestrypath = ctx.ExecuteStoreQuery<String>(QUERYREGVARPATH, parameters.ToArray()).FirstOrDefault();
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysregvar", Value = input.sysregvar });
                    values = ctx.ExecuteStoreQuery<RegVarDto>(QUERYREGVARSYS, parameters.ToArray()).ToList();

                    foreach (RegVarDto regvar in values)
                    {
                        regvar.completePath = regestrypath + "/" + regvar.code;
                    }


                }//suche über regsec
                else if (input.sysregsec != 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psyswfuser", Value = input.syswfuser });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysregsec", Value = input.sysregsec });
                    values = ctx.ExecuteStoreQuery<RegVarDto>(QUERYREGVARSEC, parameters.ToArray()).ToList();
                    foreach (RegVarDto regvar in values)
                    {
                        regvar.completePath = prefix + "/" + regvar.code;
                    }
                }//suche über pfad
                else if (input.bezeichnung != null && input.bezeichnung != "" && input.syswfuser != 0)
                {
                    //Wenn Pfad
                    if (input.bezeichnung.Contains("/"))
                    {
                        String ibez = input.bezeichnung;
                        ibez = ibez.TrimEnd('/');

                        string[] bezeichnungArray = ibez.Split('/');
                        
                        if (bezeichnungArray.Count()>0 && bezeichnungArray[1].Equals("SETUP.NET"))
                            values = findCfgByPath(input, ctx);

                        //Wenn Pfad NICHT bis REGVAR-Ebene, hole alle für die der pfad wie folgt beginnt
                        else if (bezeichnungArray.Count() <= 3)
                        {
                            String secpath = "%" + ibez.Substring(ibez.IndexOf('/', 1)) + "%";
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = input.syswfuser });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "regbezeichnung", Value = bezeichnungArray[1] });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "secpath", Value = bezeichnungArray[2] });
                            values = ctx.ExecuteStoreQuery<RegVarDto>(QUERYSECPATH, parameters.ToArray()).ToList();

                            foreach (RegVarDto regvartemp in values)
                            {
                                regvartemp.completePath = "/" + regvartemp.path.TrimEnd('/') + "/" + regvartemp.code;
                            }
                        }
                        else//Wenn Pfad bis REGVAR-Ebene
                        {
                            if (bezeichnungArray.Count() > 3)
                            {
                                
                                values = findRegVarByPath(input, ctx);
                            }
                        }
                    }
                    else
                    {
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = input.syswfuser });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "regbezeichnung", Value = input.bezeichnung });
                        values = ctx.ExecuteStoreQuery<RegVarDto>(QUERYREGSEC, parameters.ToArray()).ToList();
                        prefix = "/" + input.bezeichnung;
                        foreach (RegVarDto regvartemp in values)
                        {
                            regvartemp.completePath = prefix + regvartemp.path + "/" + regvartemp.code;
                        }
                    }
                }
                else if (input.syswfuser != 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = input.syswfuser });
                    values = ctx.ExecuteStoreQuery<RegVarDto>(QUERYREGVARWFUSER, parameters.ToArray()).ToList();
                    foreach (RegVarDto regvartemp in values)
                    {
                        regvartemp.completePath = regvartemp.path + "/" + regvartemp.code;
                    }
                }
                else
                {
                    throw new Exception("Type not supported for Register");
                }


                return values.ToArray();
            }
        }

        /// <summary>
        /// Finds the CFG-Entry by Path, internal use only
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private List<RegVarDto> findCfgByPath(igetAppSettingsItemsDto input, DdOwExtended ctx)
        {
            String ibez = input.bezeichnung;
            ibez = ibez.TrimEnd('/');

            string[] bezeichnungArray = ibez.Split('/');
            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "cfgsec", Value = bezeichnungArray[2] });


            List<RegVarDto> valuesregvartemp = ctx.ExecuteStoreQuery<RegVarDto>(QUERYCFG, parameters.ToArray()).ToList();
            
            List<RegVarDto> values = new List<RegVarDto>();
           
            foreach (RegVarDto regvartemp in valuesregvartemp)
            {
                if (regvartemp.completePath.Contains(ibez))
                {
                    values.Add(regvartemp);
                }
            }
          
            return values;
        }

        /// <summary>
        /// Searches all Regvars for a path, internal use only
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private List<RegVarDto> findRegVarByPath(igetAppSettingsItemsDto input, DdOwExtended ctx)
        {
            string prefix = "";
            String ibez = input.bezeichnung;
            ibez = ibez.TrimEnd('/');

            string[] bezeichnungArray = ibez.Split('/');
            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = input.syswfuser });
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "regbezeichnung", Value = bezeichnungArray[1] });
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bezeichnung", Value = bezeichnungArray[2] });

            String q = QUERYREGVARREGCOMP;
           
            
            if (input.sysid>0)
            {
                q = QUERYREGVARREGCOMPAREASYSID;
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "areasysid", Value = input.sysid });
            }
            else if (input.area != null)
            {
                q = QUERYREGVARREGCOMPAREA;
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = input.area });
            }

            List<RegVarDto> valuesregvartemp = ctx.ExecuteStoreQuery<RegVarDto>(q, parameters.ToArray()).ToList();
            string temp = "";
            List<RegVarDto> values = new List<RegVarDto>();
            if (valuesregvartemp.Count() == 0)
            {
                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = input.syswfuser });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "regbezeichnung", Value = bezeichnungArray[1] });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bezeichnung", Value = bezeichnungArray[2] });
                valuesregvartemp = ctx.ExecuteStoreQuery<RegVarDto>(QUERYREGSECREG, parameters.ToArray()).ToList();

                foreach (RegVarDto regvartemp in valuesregvartemp)
                {
                    prefix = "/" + bezeichnungArray[1];
                    temp = prefix + regvartemp.path;
                    if ((ibez).Contains(regvartemp.path) || ibez.Equals(regvartemp.path))
                    {
                        regvartemp.completePath = prefix + regvartemp.path + "/" + regvartemp.code;
                        values.Add(regvartemp);

                    }

                }

            }
            else
            {
                foreach (RegVarDto regvartemp in valuesregvartemp)
                {
                    prefix = "/" + bezeichnungArray[1];
                    temp = prefix + regvartemp.path;
                    if (temp.Contains(ibez) || ibez.Equals(temp + "/" + regvartemp.code))
                    {

                        regvartemp.completePath = prefix + regvartemp.path + "/" + regvartemp.code;
                        values.Add(regvartemp);
                    }

                }
            }
            return values;
        }

        /// <summary>
        /// AppSettingsItems erzeugen oder aktualisieren
        /// see createOrUpdateAppSettingsItem for description
        /// </summary>
        /// <returns>array ofAppRegisterDtos</returns>
        /// 
        public RegVarDto[] createOrUpdateAppSettingsItems(icreateOrUpdateAppSettingsItemsDto input)
        {
            List<RegVarDto> rval = new List<RegVarDto>();

            foreach (RegVarDto regvar in input.regVars)
            {
                rval.AddRange(createOrUpdateAppSettingsItem(input.sysWfuser, regvar));
            }
            return rval.ToArray();
        }

        /// <summary>
        /// AppSettingsItems erzeugen oder aktualisieren
        /// 
        /// Beispiel:
        ///  AppSettingsDao ad = new AppSettingsDao();
        ///   icreateOrUpdateAppSettingsItemDto input = new icreateOrUpdateAppSettingsItemDto();
        ///   input.regVar = new RegVarDto();
        ///   input.regVar.area = area; //optional, genauso wie .sysid
        ///   input.regVar.completePath = "/GLOBAL/LUCENE/MAXIDX"; //WICHTIG der Slash am Anfang!
        ///   input.regVar.code = "MAXIDX"; //Dieser Code muss am Ende des completePaths stehen!
        ///   input.regVar.wert = ""+idx;
        ///   input.regVar.syswfuser = -1; //WICHTIG ist der wfuser in regvar und input-Struktur!
        ///   input.sysWfuser = -1;
        ///   ad.createOrUpdateAppSettingsItem(input);
        /// </summary>
        /// <returns>array ofAppRegisterDtos</returns>
        public RegVarDto[] createOrUpdateAppSettingsItem(icreateOrUpdateAppSettingsItemDto input)
        {

            return createOrUpdateAppSettingsItem(input.sysWfuser, input.regVar);
        }

        /// <summary>
        /// Updates a list of already saved regvars
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RegVarDto[] updateAppSettingsItems(RegVarDto[] input)
        {
            List<RegVarDto> rval = new List<RegVarDto>();
            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            using (DdOwExtended ctx = new DdOwExtended())
            {
                foreach (RegVarDto regVar in input)
                {
                    

                    //löschen wenn wert leer oder "" oder false
                    if (regVar.blobWert == null && (regVar.wert == null || regVar.wert.Equals("") || regVar.wert.ToLower().Equals("false")))
                    {
                        ctx.ExecuteStoreCommand(QUERYREGVARDELETE + regVar.sysRegVar);
                        
                    }
                    else
                    {
                        parameters.Clear();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysregvar", Value = regVar.sysRegVar });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pwert", Value = regVar.wert });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pchgdate", Value = DateTime.Today });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pblobwert", Value = regVar.blobWert });
                        ctx.ExecuteStoreQuery<RegVarDto>(QUERYREGVARUPDATE, parameters.ToArray()).ToList();
                        regVar.chgdate = DateTime.Today;
                        rval.Add(regVar);
                    }
                }
                ctx.SaveChanges();
            }
            return rval.ToArray();
        }

        /// <summary>
        /// legt übergebene vars neu an, inkl. section/reg falls benötigt
        /// </summary>
        /// <param name="sysWfuser"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public RegVarDto[] createAppSettingsItems(long sysWfuser, RegVarDto[] input)
        {
            List<RegVarDto> rval = new List<RegVarDto>();
            //Wenn Pfad bis REGVAR-Ebene
            using (DdOwExtended ctx = new DdOwExtended())
            {
                foreach (RegVarDto regVar in input)
                {

                    String regbez = regVar.completePath;
                    if (regbez == null)
                        continue;
                    string[] bezeichnungArray = regbez.Split('/');
                    if (bezeichnungArray.Count() <= 3) continue;
                   
                    List<RegSecTemp> valuesregsectemp = null;

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psyswfuser", Value = sysWfuser });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pregbezeichnung", Value = bezeichnungArray[1] });
                    REG reg = ctx.ExecuteStoreQuery<REG>(QUERYREGSELECT, parameters.ToArray()).FirstOrDefault();

                    //1. ebene reg anlegen falls noch nicht vorhanden
                    if (reg == null)
                    {
                        AddReg(ctx, bezeichnungArray[1], sysWfuser);
                    }                                    

                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = sysWfuser });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bezeichnung", Value = bezeichnungArray[2] });
                    valuesregsectemp = ctx.ExecuteStoreQuery<RegSecTemp>(QUERYREGSEC, parameters.ToArray()).ToList();

                    RegSecTemp regsec = new RegSecTemp();
                    igetAppSettingsItemsDto item = new igetAppSettingsItemsDto();
                    item.bezeichnung = regbez.Remove(regbez.Length - (bezeichnungArray[bezeichnungArray.Count() - 1].Length + 1), bezeichnungArray[bezeichnungArray.Count() - 1].Length + 1);
                    item.syswfuser = sysWfuser;

                    //wenn section gefunden, dann verwenden
                    if (valuesregsectemp.Count > 0)
                    {
                        item.sysreg = valuesregsectemp[0].sysreg;
                    }
                    else
                    {
                        parameters.Clear();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psyswfuser", Value = sysWfuser });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pbezeichnung", Value = bezeichnungArray[1] });
                        item.sysreg = ctx.ExecuteStoreQuery<long>(QUERYREG, parameters.ToArray()).FirstOrDefault();
                    }

                    //regsec anlegen oder zurückgeben falls schon vorhanden
                    regsec = createregsec(ctx, item, valuesregsectemp);

                    String orgBez = regbez;
                    regVar.completePath = bezeichnungArray[bezeichnungArray.Count() - 1];
                    regVar.sysRegSec = regsec.sysregsec.Value;

                    //regVar anlegen
                    long sysregvar = AddRegVar(ctx,regVar);
                    regVar.sysRegVar = sysregvar;
                    rval.Add(regVar);
                   
                 
                }
                ctx.SaveChanges();
            }
            return rval.ToArray();

        }
        /// <summary>
        /// AppSettingsItems erzeugen oder aktualisieren
        /// 
        /// Beispiel:
        ///  AppSettingsDao ad = new AppSettingsDao();
        ///  RegVarDto input = new RegVarDto();
        ///   input.area = area; //optional, genauso wie .sysid
        ///   input.completePath = "/GLOBAL/LUCENE/MAXIDX"; //WICHTIG der Slash am Anfang!
        ///   input.code = "MAXIDX"; //Dieser Code muss am Ende des completePaths stehen!
        ///   input.wert = ""+idx;
        ///   input.syswfuser = -1; //WICHTIG ist der wfuser in regvar und input-Struktur!
        ///   ad.createOrUpdateAppSettingsItem(-1, input);
        /// </summary>
        /// <returns>array ofAppRegisterDtos</returns>
        private RegVarDto[] createOrUpdateAppSettingsItem(long sysWfuser, RegVarDto regVar)
        {

            String regbez = regVar.completePath;
            if (regbez == null)
                return null;
            string[] bezeichnungArray = regbez.Split('/');
            if (bezeichnungArray.Count() <= 3) return null;

            //Wenn Pfad bis REGVAR-Ebene
            using (DdOwExtended ctx = new DdOwExtended())
            {


                List<RegVarDto> valuesregvartemp = null;
                List<RegSecTemp> valuesregsectemp = null;

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psyswfuser", Value = sysWfuser });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pregbezeichnung", Value = bezeichnungArray[1] });
                REG reg = ctx.ExecuteStoreQuery<REG>(QUERYREGSELECT, parameters.ToArray()).FirstOrDefault();

                //1. ebene reg anlegen falls noch nicht vorhanden
                if (reg == null)
                {
                    AddReg(ctx, bezeichnungArray[1], sysWfuser);
                }

                parameters.Clear();
                int len = regbez.LastIndexOf('/') - regbez.IndexOf('/', 1);
                String secpath = "%" + regbez.Substring(regbez.IndexOf('/', 1), len) + "%";

                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = regVar.syswfuser });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "regbezeichnung", Value = bezeichnungArray[1] });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "secpath", Value = bezeichnungArray[2] });
                valuesregvartemp = ctx.ExecuteStoreQuery<RegVarDto>(QUERYSECPATH, parameters.ToArray()).ToList();
                
                //gefundene regvars löschen oder aktualisieren
                foreach (RegVarDto regvartemp in valuesregvartemp)
                {
                    String curPath = "/" + regvartemp.path + "/" + regvartemp.code;
                    //suche nach vorhandener regvar über bezeichnung und area/areaid
                    if (regbez.Equals(curPath) && ((regVar.area == null && regvartemp.area == null) || (regVar.area.Equals(regvartemp.area) && regVar.sysid == regvartemp.sysid)))
                    {
                        parameters.Clear();
                        //löschen wenn wert leer oder "" oder false
                        if (regVar.blobWert == null && (regVar.wert == null || regVar.wert.Equals("") || regVar.wert.ToLower().Equals("false")))
                        {
                            ctx.ExecuteStoreCommand(QUERYREGVARDELETE+regvartemp.sysRegVar);
                            return new RegVarDto[0];
                        }
                        else //regvar aktualisieren
                        {
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysregvar", Value = regvartemp.sysRegVar });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pwert", Value = regVar.wert });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pchgdate", Value = DateTime.Today });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pblobwert", Value = regVar.blobWert });
                            valuesregvartemp = ctx.ExecuteStoreQuery<RegVarDto>(QUERYREGVARUPDATE, parameters.ToArray()).ToList();
                            igetAppSettingsItemsDto inputappsetting = new igetAppSettingsItemsDto()
                            {
                                syswfuser = sysWfuser,
                                bezeichnung = regbez,
                                sysregvar = regvartemp.sysRegVar,

                            };


                            //return deliverAppSettingsItems(inputappsetting);
                            regVar.syswfuser = sysWfuser;
                            regVar.bezeichnung = regbez;
                            regVar.sysRegVar = regvartemp.sysRegVar;

                            return new RegVarDto[] { regVar };
                        }
                    }


                }

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = sysWfuser });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bezeichnung", Value = bezeichnungArray[2] });
                valuesregsectemp = ctx.ExecuteStoreQuery<RegSecTemp>(QUERYREGSEC, parameters.ToArray()).ToList();

                RegSecTemp regsec = new RegSecTemp();
                igetAppSettingsItemsDto item = new igetAppSettingsItemsDto();
                item.bezeichnung = regbez.Remove(regbez.Length - (bezeichnungArray[bezeichnungArray.Count() - 1].Length + 1), bezeichnungArray[bezeichnungArray.Count() - 1].Length + 1);
                item.syswfuser = sysWfuser;

                if (valuesregsectemp.Count > 0)
                {
                    item.sysreg = valuesregsectemp[0].sysreg;
                }
                else
                {
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psyswfuser", Value = sysWfuser });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pbezeichnung", Value = bezeichnungArray[1] });
                    item.sysreg = ctx.ExecuteStoreQuery<long>(QUERYREG, parameters.ToArray()).FirstOrDefault();
                }


                regsec = createregsec(ctx, item, valuesregsectemp);

                String orgBez = regbez;
                regVar.completePath = bezeichnungArray[bezeichnungArray.Count() - 1];
                regVar.sysRegSec = regsec.sysregsec.Value;
                AddRegVar(ctx, regVar);

                ctx.SaveChanges();

                igetAppSettingsItemsDto inputappsetting2 = new igetAppSettingsItemsDto()
                        {
                            syswfuser = sysWfuser,
                            bezeichnung = orgBez,

                        };


                return deliverAppSettingsItems(inputappsetting2);




            }

        }


        /// <summary>
        /// Registry Section hinzufügen, intern benötigt
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="input"></param>
        /// <param name="valuesregsectemp"></param>
        /// <returns></returns>
        private RegSecTemp createregsec(DdOwExtended ctx, igetAppSettingsItemsDto input, List<RegSecTemp> valuesregsectemp)
        {
            string[] bezeichnungArray;
            RegSecTemp res = null;
            if (input.bezeichnung != "")
            {
                bezeichnungArray = input.bezeichnung.Split('/');

                if (bezeichnungArray.Count() == 2)
                {
                    res = new RegSecTemp()
                    {
                        sysregsec = null,
                        sysreg = input.sysreg
                    };
                    return res;
                }
            }

            if (input.bezeichnung == "")
            {

                res = new RegSecTemp()
                {
                    sysregsec = null,
                    sysreg = input.sysreg
                };
                return res;

            }

            

                string temp = "";
                bezeichnungArray = input.bezeichnung.Split('/');
                string prefix = "/" + bezeichnungArray[1];
                foreach (RegSecTemp regsectemp in valuesregsectemp)
                {
                    temp = prefix + regsectemp.path;
                    if (temp.Equals(input.bezeichnung))
                    {
                        res = new RegSecTemp()
                        {
                            sysregsec = (long)regsectemp.sysregsec,
                            code = regsectemp.code,
                            wert = regsectemp.wert,
                            bezeichnung = regsectemp.bezeichnung,
                            sysreg = (long)regsectemp.sysreg
                        };
                        return res;
                    }
                }

                string neuebezeichnung = input.bezeichnung.Remove(input.bezeichnung.Length - (bezeichnungArray[bezeichnungArray.Count() - 1].Length + 1), bezeichnungArray[bezeichnungArray.Count() - 1].Length + 1);



                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                input.bezeichnung = neuebezeichnung;

                long? sysregsecparent = createregsec(ctx,input, valuesregsectemp).sysregsec;
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pbezeichnung", Value = bezeichnungArray[bezeichnungArray.Count() - 1] });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcode", Value = bezeichnungArray[bezeichnungArray.Count() - 1] });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysreg", Value = input.sysreg });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysregsecparent", Value = sysregsecparent });
                AddRegsec(parameters);
                parameters.Clear();

                if (sysregsecparent == null)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pbezeichnung", Value = bezeichnungArray[bezeichnungArray.Count() - 1] });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcode", Value = bezeichnungArray[bezeichnungArray.Count() - 1] });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysreg", Value = input.sysreg });
                    res = new RegSecTemp() { sysregsec = ctx.ExecuteStoreQuery<long>(QUERYSYSREGSECPARENTNULL, parameters.ToArray()).FirstOrDefault() };
                    parameters.Clear();
                }
                else
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pbezeichnung", Value = bezeichnungArray[bezeichnungArray.Count() - 1] });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcode", Value = bezeichnungArray[bezeichnungArray.Count() - 1] });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysreg", Value = input.sysreg });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysregsecparent", Value = sysregsecparent });
                    res = new RegSecTemp() { sysregsec = ctx.ExecuteStoreQuery<long>(QUERYSYSREGSEC, parameters.ToArray()).FirstOrDefault() };
                    parameters.Clear();
                }



                return res;
            

        }


        /// <summary>
        /// Root Reg erzeugen, intern benötigt
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="bezeichnung"></param>
        /// <param name="sysWfuser"></param>
        private void AddReg(DdOwExtended ctx, String bezeichnung, long sysWfuser)
        {
            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psyswfuser", Value = sysWfuser });
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pregbezeichnung", Value = bezeichnung });
            ctx.ExecuteStoreCommand(QUERYCREATEREG, parameters.ToArray());


        }

        /// <summary>
        /// RegVar erzeugen, intern benötigt
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="rvarDto"></param>
        private long AddRegVar(DdOwExtended ctx, RegVarDto rvarDto)
        {
            try
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();

                Devart.Data.Oracle.OracleParameter output = new Devart.Data.Oracle.OracleParameter { ParameterName = "myOutputParameter", OracleDbType = Devart.Data.Oracle.OracleDbType.Long, Direction = System.Data.ParameterDirection.ReturnValue };
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysregsec", Value = rvarDto.sysRegSec });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = rvarDto.code });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "wert", Value = rvarDto.wert });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bezeichnung", Value = rvarDto.bezeichnung });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "werttyp", Value = rvarDto.wertTyp });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = rvarDto.area });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = rvarDto.sysid });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = rvarDto.syswfuser });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "blobwert", Value = rvarDto.blobWert });
                    parameters.Add(output);
                    //parameters.Add(new OracleParameter("myOutputParameter", OracleDbType.Long, System.Data.ParameterDirection.ReturnValue));
                    if (rvarDto.chgdate == null || rvarDto.chgdate.Year < 1900)
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "chgdate", Value = DateTime.Today });
                    else
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "chgdate", Value = rvarDto.chgdate });
                    ctx.ExecuteStoreCommand(QUERYCREATEREGVAR, parameters.ToArray());
                    return Convert.ToInt64(output.Value);
                
            }
            catch (Exception e)
            {
                _Log.Error("Error inserting Regvar " + _Log.dumpObject(rvarDto), e);
                return 0;
            }

        }


        /// <summary>
        /// Add Section-Entity (because of mission REGSEC-EDMX)
        /// </summary>
        /// <param name="parameters"></param>
        private void AddRegsec(List<Devart.Data.Oracle.OracleParameter> parameters)
        {

            using (DdOwExtended ctx = new DdOwExtended())
            {

                ctx.ExecuteStoreCommand(QUERYCREATEREGSEC, parameters.ToArray());


            }

        }



        #region Declaration of temporary RegSec for internal usage

        /// <summary>
        /// Temporary Registry Data
        /// </summary>
        private class RegSecTemp
        {
            public long sysreg { get; set; }
            public string path { get; set; }
            public string code { get; set; }
            public string wert { get; set; }
            public string bezeichnung { get; set; }
            public long? sysregsec { get; set; }


        }
        #endregion
    }
}