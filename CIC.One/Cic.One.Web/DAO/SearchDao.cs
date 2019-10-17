using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.One.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.One.Web.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.One.Web.BO;
using System.Globalization;
using AutoMapper;
using System.Text.RegularExpressions;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.BO;
using System.Data.Objects;
using System.Data.Common;
using System.Data.EntityClient;
using Dapper;
using Devart.Data.Oracle;
using System.Threading;
using System.Threading.Tasks;
using Cic.OpenOne.Common.Util.Config;
using Cic.One.Web.BO.Search;
using Cic.OpenOne.Common.Model.Prisma;

namespace Cic.One.Web.Service.DAO
{
    /// <summary>
    /// Suche Data Access Obejct
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class SearchDao<R> : ISearchDao<R>
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private long TIMEOUT = 0;//seconds until a query timeout occurs
        private static long SLOWDURATION = 2000;//log query when slower than
        private bool async = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchDao()
        {
            TIMEOUT =  AppConfig.Instance.GetCfgEntry("SETUP.NET","SEARCH", "TIMEOUT", 10);
            if (TIMEOUT == 0)
                async = false;
        }

        /// <summary>
        /// returns a description of all result fields
        /// </summary>
        /// <returns></returns>
        public List<Viewfield> getFields()
        {
            return null;
        }

        /// <summary>
        /// Returns the first or default data set for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public R getFirstOrDefault(string query, object param)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                return con.Query<R>(query, param).FirstOrDefault();
            }
        }

        /// <summary>
        /// Search Function
        /// </summary>
        /// <param name="query">Query</param>
        /// <param name="param">Parameters</param>
        /// <returns></returns>
        public List<R> search(string query, object[] param)
        {
            if (async)
            {
                Task<List<R>> taskResult = SearchWithTimeout(query, param);
                Task.FromResult(taskResult);
                
                if (taskResult.Exception != null)
                    throw taskResult.Exception;
                
                return taskResult.Result;              
            }
            using (DdOlExtended ctx = new DdOlExtended())
            {
                try
                {


                    DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                    long start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                    if (param != null && param.Length == 0)
                        param = null;
                    var dbArgs = new DynamicParameters();
                    if(param!=null)
                        foreach (OracleParameter pair in param) dbArgs.Add(pair.ParameterName, pair.Value);


                   /* List<R> rval = (await con.QueryAsync<R>(
                        new CommandDefinition(query,dbArgs, cancellationToken: tokenSource.Token)
                    )).ToList();*/

                    List<R> rval = con.Query<R>(query, dbArgs).ToList();
                    
                    long duration = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds - start);
                    if (duration > SLOWDURATION)
                    {
                        _log.Warn("Long Query(" + duration + "ms): " + query + " params: " + (param!=null?param.AsList().ToString():""));
                    }
                    return rval;
                }
                catch (Exception ex)
                {
                    _log.Warn("Search with query failed: " + query + " " + ex.Message);
                    if (_log.IsDebugEnabled)
                    {
                        _log.Debug("Search with query failed: " + query);
                        if (param != null)
                            _log.Debug(getParams(param));
                    }
                    throw ex;
                }
            }
        }


        async Task<List<R>> SearchWithTimeout(string query, object[] param)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(TIMEOUT));
            using (DdOlExtended ctx = new DdOlExtended())
            {
                try
                {


                    DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                    long start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                    if (param != null && param.Length == 0)
                        param = null;
                    var dbArgs = new DynamicParameters();
                    if (param != null)
                        foreach (OracleParameter pair in param) dbArgs.Add(pair.ParameterName, pair.Value);


                    List<R> rval = (await con.QueryAsync<R>(
                        new CommandDefinition(query, dbArgs, cancellationToken: tokenSource.Token)
                    )).ToList();

                    //List<R> rval = con.Query<R>(query, dbArgs).ToList();

                    long duration = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds - start);
                    if (duration > SLOWDURATION)
                    {
                        _log.Warn("Long Query(" + duration + "ms): " + query + " params: " + (param != null ? param.AsList().ToString() : ""));
                    }
                    return rval;
                }
                catch (Exception ex)
                {
                    _log.Warn("Search with query failed: " + query+" "+ex.Message);
                    if (_log.IsDebugEnabled)
                    {
                        _log.Debug("Search with query failed: " + query);
                        if (param != null)
                            _log.Debug(getParams(param));
                    }
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Search Function 
        /// </summary>
        /// <param name="query">Query</param>
        /// <param name="param">Parameters</param>
        /// <returns></returns>
        public IEnumerable<R> search(ObjectContext ctx, string query, object[] param)
        {
            try
            {
                long start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                IQueryable<R> rval = ctx.ExecuteStoreQuery<R>(query, param).AsQueryable();
                long duration = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds - start);
                if (duration > SLOWDURATION)
                {
                    _log.Info("Long Query(" + duration + "ms): " + query + " params: " + param);
                }
                return rval;
            }
            catch (Exception ex)
            {
                if (_log.IsDebugEnabled)
                {
                    _log.Debug("Search with query failed: " + query);
                    if (param != null)
                        _log.Debug(getParams(param));
                }
                throw ex;
            }
          

        }

        private static String getParams(object[] par)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(": ");
            if (par != null)
                foreach (object o in par)
                {
                    sb.Append(((Devart.Data.Oracle.OracleParameter)o).ParameterName);
                    sb.Append("=");
                    sb.Append(((Devart.Data.Oracle.OracleParameter)o).Value.ToString());
                    sb.Append(";");
                }
            return sb.ToString();
        }

        /// <summary>
        /// Bereitet die Ergebnisse auf.
        /// Dadurch können noch mehr Felder geladen werden oder auch besser gemappt werden.
        /// </summary>
        /// <param name="found"></param>
        /// <param name="syswfuser"></param>
		/// TODO area
		public void PostPrepare (oSearchDto<R> found, long syswfuser, QueryInfoData infoData)
        {
            if (found.results == null || found.results.Count() == 0)
                return;

			//see gviewsearchdao for generic postprocessing from wfvconfig-configuration entry in viewmeta

            if (typeof(R).Equals(typeof(Cic.One.DTO.AccountDto)))
            {
               /* ICASBo bo = BOFactoryFactory.getInstance().getCASBo();
                if (bo != null)
                {
                    iCASEvaluateDto ieval = new iCASEvaluateDto();
                    ieval.area = "PERSON";
                    ieval.expression = new String[] { CASBo.OP, CASBo.OBLIGO, CASBo.OBLIGOKWG };
                    ieval.sysID = found.results.Cast<AccountDto>().Select(a => a.entityId).ToArray();
                    CASEvaluateResult[] er = bo.getEvaluation(ieval, syswfuser);
                    int i = 0;
                    foreach (AccountDto acc in found.results.Cast<AccountDto>())
                    {
                        try
                        {
                            acc.op = 0;
                            acc.obligo = 0;
                            acc.obligokwg = 0;
                            if (er[i].clarionResult[0] != null && !"".Equals(er[i].clarionResult[0]))
                                acc.op = Double.Parse(er[i].clarionResult[0], CultureInfo.InvariantCulture);
                            if (er[i].clarionResult[1] != null && !"".Equals(er[i].clarionResult[1]))
                                acc.obligo = Double.Parse(er[i].clarionResult[1], CultureInfo.InvariantCulture);
                            if (er[i].clarionResult[2] != null && !"".Equals(er[i].clarionResult[2]))
                                acc.obligokwg = Double.Parse(er[i].clarionResult[2], CultureInfo.InvariantCulture);
                        }
                        catch (Exception e)
                        {
                            _log.Warn("CAS-Evaluation failed for Account op/obligo", e);
                        }
                        i++;
                    }


                }*/
            }
            // Wird über die Infodata gemacht
            //else if (typeof(R).Equals(typeof(Cic.One.DTO.ItemcatmDto)))
            //{
            //    using (DdOwExtended ctx = new DdOwExtended())
            //    {
            //        foreach (ItemcatmDto itemcatm in found.results.Cast<ItemcatmDto>())
            //        {
            //            ITEMCAT itemcat = (from item in ctx.ITEMCAT
            //                              where item.SYSITEMCAT == itemcatm.sysItemCat
            //                              select item).FirstOrDefault();
            //            if (itemcat != null)
            //            {
            //                itemcatm.name = itemcat.NAME;
            //                itemcatm.description = itemcat.DESCRIPTION;
            //            }
            //        }
            //    }
            //}
            else if (typeof(R).Equals(typeof(Cic.One.DTO.MailmsgDto)))
            {
                foreach (MailmsgDto mailmsg in found.results.Cast<MailmsgDto>())
                {
                    if (string.IsNullOrEmpty(mailmsg.itemId))
                        mailmsg.IsDraft = 1;
                    if (!string.IsNullOrEmpty(mailmsg.content))
                    {
                        mailmsg.content = Regex.Replace(mailmsg.content, "<([^/][A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4})>", "$1", RegexOptions.IgnoreCase);
                        mailmsg.content = Regex.Replace(mailmsg.content, "<([/][A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4})>", "", RegexOptions.IgnoreCase);
                        //mailmsg.content = Regex.Replace(mailmsg.content, "(<)(.*?)(>)", new MatchEvaluator((a) => a.Groups[1].Value + a.Groups[2].Value.Replace("&quot;", "\'") + a.Groups[3].Value), RegexOptions.IgnoreCase);
                    }
                }
            }
            else if (typeof(R).Equals(typeof(Cic.One.DTO.PartnerDto)))
            {
                fillPartnerRelationInfos(found.results.Cast<PartnerDto>().ToList());

            }
            else if (typeof(R).Equals(typeof(Cic.One.DTO.ContactDto)))
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    foreach (ContactDto contact in found.results.Cast<ContactDto>())
                    {
                        long? sysptrelate = (from c in ctx.PTRELATE where c.SYSPERSON1 == contact.sysPerson && c.SYSPERSON2 == contact.sysPartner select c.SYSPTRELATE).FirstOrDefault();
                        contact.sysPtrelate = sysptrelate.HasValue ? sysptrelate.Value : 0;
                    }
                }
            }
            else if (typeof(R).Equals(typeof(Cic.One.DTO.PrkgroupmDto)))
            {
                //var items = found.results.Cast<PrkgroupmDto>();
                //var ids = items.Select((a) => a.entityId);

                //using (DdOlExtended ctx = new DdOlExtended())
                //{
                //    var results = from c in ctx.PERSON where ids.Contains(c.SYSPERSON) select c;

                //    foreach (PERSON p in results)
                //    {
                //        get
                //    }
                //}
            }

            if (!EntityBo.INDICATOR_DISABLED && typeof(R).IsSubclassOf(typeof(EntityDto)))
            {

                EntityBo.assignIndicatorDefault<R>(found.results.Cast<EntityDto>().ToList());
                
            }
        }

        /// <summary>
        /// Kann die Ergebnisse noch Konvertieren in andere Objekte
        /// </summary>
        /// <typeparam name="T">Typ welches als Ergebniss raus kommen soll</typeparam>
        /// <param name="found">Gefundene Elemente</param>
        /// <param name="permissionId"></param>
        /// <returns>Ergebnis</returns>
        public oSearchDto<T> PostConvert<T>(oSearchDto<R> found, long permissionId)
        {
            if (false)
            {
                //Hier können weitere spezielle Möglichkeiten eingefügt werden
                //Ansonsten wird der Automapper dafür verwendet.
            }
            else
            {
                //TODO eventuell alle Möglichkeiten, die PostConvert verwenden in das MappingProfil eintragen.
                Mapper.CreateMap<oSearchDto<R>, oSearchDto<T>>();
                return Mapper.Map(found, new oSearchDto<T>());
            }
        }

        public static void fillPartnerRelationInfos(List<PartnerDto> partners)
        {
            DictionaryListsDao dld = new DictionaryListsDao();
            DropListDto[] functions = dld.findByDDLKPPOSCode(DDLKPPOSType.PTRELATE_FUNCCODE, null);
            DropListDto[] typcodes = dld.findByDDLKPPOSCode(DDLKPPOSType.PTRELATE_TYPCODE, null);
            DropListDto[] addinfos = dld.findByDDLKPPOSCode(DDLKPPOSType.PTRELATE_ADDINFO, null);

            foreach (PartnerDto partner in partners)
            {
                if (partner.sysptrelate > 0)
                {
                    try
                    {
                        partner.beziehungstyp = typcodes.Where(f => f.code.Equals(partner.typCode)).FirstOrDefault().bezeichnung;
                        partner.beziehungsfunktion = functions.Where(f => f.code == partner.funcCode).FirstOrDefault().bezeichnung;
                        partner.beziehung = addinfos.Where(f => f.code == partner.additionalInfo).FirstOrDefault().bezeichnung;
                    }
                    catch (Exception) { }
                }
            }
        }


    }
}