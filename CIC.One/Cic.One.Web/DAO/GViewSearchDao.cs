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
using Cic.OpenOne.Common.Model.Prisma;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data;
using Cic.OpenOne.Common.Util.Config;
using System.Threading.Tasks;
using System.Threading;
using Cic.One.Web.BO.Search;

namespace Cic.One.Web.Service.DAO
{
	/// <summary>
	/// Suche Data Access Obejct
	/// </summary>
	/// <typeparam name="R"></typeparam>
	public class GViewSearchDao : ISearchDao<GviewDto>
	{
		private static readonly ILog _log = Log.GetLogger (MethodBase.GetCurrentMethod ().DeclaringType);
		private long TIMEOUT = 0;//seconds until a query timeout occurs
		private static long SLOWDURATION = 2000;//log query when slower than
		private bool async = true;
		private WfvEntry config;

		/// <summary>
		/// Constructor
		/// </summary>
		public GViewSearchDao (String queryId)
		{
			config = DAOFactoryFactory.getInstance ().getWorkflowDao ().getWfvEntry (queryId);
			TIMEOUT = AppConfig.Instance.GetCfgEntry ("SETUP.NET", "SEARCH", "TIMEOUT2", 30);
			if (TIMEOUT == 0)
				async = false;
		}

		/// <summary>
		/// returns a description of all result fields
		/// </summary>
		/// <returns></returns>
		public List<Viewfield> getFields ()
		{
			return config.customentry.viewmeta.fields;
		}

		/// <summary>
		/// Search Function
		/// </summary>
		/// <param name="query">Query</param>
		/// <param name="param">Parameters</param>
		/// <returns></returns>
		public List<GviewDto> search (string query, object[] param)
		{
			if (async)
			{
				Task<List<GviewDto>> taskResult = SearchWithTimeout (query, param);
				Task.FromResult (taskResult);

				if (taskResult.Exception != null)
					throw taskResult.Exception;

				return taskResult.Result;
			}
			List<GviewDto> rval = new List<GviewDto> ();
			using (PrismaExtended ctx = new PrismaExtended ())
			{
				try
				{
					long start = (long) (DateTime.Now.TimeOfDay.TotalMilliseconds);
					DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;

					con.Open ();
					DbCommand cmd = con.CreateCommand ();
					cmd.CommandText = query;
					if (param != null && param.Length > 0)
						cmd.Parameters.AddRange (param);

					DbDataReader reader = cmd.ExecuteReader ();
					Dictionary<String, int> fieldMap = new Dictionary<string, int> ();
					for (int i = 0; i < reader.FieldCount; i++)
					{
						fieldMap[reader.GetName (i)] = i;
					}
					int rn = 0;
					while (reader.Read ())//for every line
					{
						GviewDto row = new GviewDto ();
						rval.Add (row);
						row.fields = new List<Viewfield> ();
						object ukval = null;
						if (fieldMap.ContainsKey ("I_UKEY"))
							ukval = reader.GetValue (fieldMap["I_UKEY"]);
						if (ukval == null)
							ukval = -1 * rn;
						row.sysId = Convert.ToInt64 (ukval);
						foreach (Viewfield vf in config.customentry.viewmeta.fields)
						{
							if (fieldMap.ContainsKey (vf.attr.field.ToUpper ()))
							{
								row.fields.Add (Viewfield.createFieldFromDataReader (vf, reader, fieldMap[vf.attr.field.ToUpper ()]));
							}
						}
						rn++;
					}

					long duration = (long) (DateTime.Now.TimeOfDay.TotalMilliseconds - start);
					if (duration > SLOWDURATION)
					{
						_log.Warn ("Long GView Query(" + duration + "ms): " + query + " params: " + param);
					}
					return rval;
				}
				catch (Exception ex)
				{
					if (_log.IsDebugEnabled)
					{
						_log.Debug ("Search with query failed: " + query);
						if (param != null)
							_log.Debug (getParams (param));
					}
					throw ex;
				}
			}

		}

		async Task<List<GviewDto>> SearchWithTimeout (string query, object[] param)
		{
			CancellationTokenSource tokenSource = new CancellationTokenSource ();
			tokenSource.CancelAfter (TimeSpan.FromSeconds (TIMEOUT));
			List<GviewDto> rval = new List<GviewDto> ();
			using (PrismaExtended ctx = new PrismaExtended ())
			{
				try
				{
					long start = (long) (DateTime.Now.TimeOfDay.TotalMilliseconds);
					DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;

					con.Open ();
					DbCommand cmd = con.CreateCommand ();
					cmd.CommandText = query;
					if (param != null && param.Length > 0)
						cmd.Parameters.AddRange (param);

					DbDataReader reader = (await cmd.ExecuteReaderAsync (tokenSource.Token));
					//DbDataReader reader = cmd.ExecuteReader();

					Dictionary<String, int> fieldMap = new Dictionary<string, int> ();
					for (int i = 0; i < reader.FieldCount; i++)
					{
						fieldMap[reader.GetName (i)] = i;
					}
					int rn = 0;
					while (reader.Read ())//for every line
					{
						GviewDto row = new GviewDto ();
						rval.Add (row);
						row.fields = new List<Viewfield> ();
						object ukval = null;
						if (fieldMap.ContainsKey ("I_UKEY"))
							ukval = reader.GetValue (fieldMap["I_UKEY"]);
                        if (ukval == null && config.customentry.viewmeta.query!=null && config.customentry.viewmeta.query.pkey !=null && fieldMap.ContainsKey(config.customentry.viewmeta.query.pkey))
                            ukval = reader.GetValue(fieldMap[config.customentry.viewmeta.query.pkey]);

                        if (ukval == null)
							ukval = -1 * rn;
						row.sysId = Convert.ToInt64 (ukval);
						foreach (Viewfield vf in config.customentry.viewmeta.fields)
						{
							if (fieldMap.ContainsKey (vf.attr.field.ToUpper ()))
							{
								row.fields.Add (Viewfield.createFieldFromDataReader (vf, reader, fieldMap[vf.attr.field.ToUpper ()]));
							}
						}
						rn++;
					}

					long duration = (long) (DateTime.Now.TimeOfDay.TotalMilliseconds - start);
					if (duration > SLOWDURATION)
					{
						_log.Warn ("Long GView Query(" + duration + "ms): " + query + " params: " + param);
					}
					return rval;
				}
				catch (Exception ex)
				{
					if (_log.IsDebugEnabled)
					{
						_log.Debug ("Search with query failed: " + query);
						if (param != null)
							_log.Debug (getParams (param));
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
		public IEnumerable<GviewDto> search (ObjectContext ctx, string query, object[] param)
		{

			long start = (long) (DateTime.Now.TimeOfDay.TotalMilliseconds);

			DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;

			con.Open ();
			DbCommand cmd = con.CreateCommand ();
			cmd.CommandText = query;
			cmd.Parameters.AddRange (param);

			DbDataReader reader = cmd.ExecuteReader ();
			Dictionary<String, int> fieldMap = new Dictionary<string, int> ();
			for (int i = 0; i < reader.FieldCount; i++)
			{
				fieldMap[reader.GetName (i)] = i;
			}

			while (reader.Read ())//for every line
			{
				GviewDto row = new GviewDto ();
				// rval.Add(row);
				foreach (Viewfield vf in config.customentry.viewmeta.fields)
				{
					if (fieldMap.ContainsKey (vf.attr.field.ToUpper ()))
					{
						row.fields.Add (Viewfield.createFieldFromDataReader (vf, reader, fieldMap[vf.attr.field.ToUpper ()]));
					}
				}
				yield return row;
			}


			//IQueryable<GviewDto> rval = ctx.ExecuteStoreQuery<GviewDto>(query, param).AsQueryable();

			long duration = (long) (DateTime.Now.TimeOfDay.TotalMilliseconds - start);
			if (duration > 250)
			{
				_log.Warn ("Long Query(" + duration + "ms): " + query + " params: " + param);
			}
			// return rval;

			/* catch (Exception ex)
			 {
				 if (_log.IsDebugEnabled)
				 {
					 _log.Debug("Search with query failed: " + query);
					 if (param != null)
						 _log.Debug(getParams(param));
				 }
				 throw ex;
			 }*/


		}

		private static String getParams (object[] par)
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append (": ");
			if (par != null)
				foreach (object o in par)
				{
					sb.Append (((Devart.Data.Oracle.OracleParameter) o).ParameterName);
					sb.Append ("=");
					sb.Append (((Devart.Data.Oracle.OracleParameter) o).Value.ToString ());
					sb.Append (";");
				}
			return sb.ToString ();
		}

		/// <summary>
		/// Bereitet die Ergebnisse auf.
		/// Dadurch können noch mehr Felder geladen werden oder auch besser gemappt werden.
		/// </summary>
		/// <param name="found"></param>
		/// <param name="syswfuser"></param>
		public void PostPrepare (oSearchDto<GviewDto> found, long syswfuser, QueryInfoData infoData)
		{
			if (infoData != null && infoData.GetType () == typeof (QueryInfoDataType5))
			{
				QueryInfoDataType5 info = (QueryInfoDataType5) infoData;
                if (info.getQueryConfig() == null || info.getQueryConfig().query == null || info.getQueryConfig().query.postprocess == null) return;

				foreach (PostprocessCommand pp in info.getQueryConfig ().query.postprocess)
				{
					if (pp != null && "CAS".Equals (pp.type))
					{
						ICASBo bo = BOFactoryFactory.getInstance ().getCASBo ();
						if (bo != null)
						{
							iCASEvaluateDto ieval = new iCASEvaluateDto ();
							ieval.area = pp.area;
							ieval.expression = new String[] { pp.command };
							ieval.sysID = found.results.Cast<GviewDto> ().Select (a => a.entityId).ToArray ();
							CASEvaluateResult[] er = bo.getEvaluation (ieval, syswfuser);


							ViewFieldAttributes atts = (from f in info.getQueryConfig ().fields
														where f.id.Equals (pp.field)
														select f.attr).FirstOrDefault ();
							int i = 0;
							foreach (GviewDto row in found.results.Cast<GviewDto> ())
							{
								try
								{
									var t = (from f in row.fields
											 where f.id.Equals (pp.field)
											 select f).FirstOrDefault ();
									t.attr = atts;//temporary assignment for value-conversion info
									t.fillFromObject(Double.Parse (er[i].clarionResult[0], CultureInfo.InvariantCulture));
									t.attr = null;
								}
								catch (Exception e)
								{
									_log.Warn ("CAS-Evaluation failed for "+pp.command+" into "+pp.field, e);
								}
								i++;
							}

						}
					}
				} 
			}
		}

		/// <summary>
		/// Kann die Ergebnisse noch Konvertieren in andere Objekte
		/// </summary>
		/// <typeparam name="T">Typ welches als Ergebniss raus kommen soll</typeparam>
		/// <param name="found">Gefundene Elemente</param>
		/// <param name="permissionId"></param>
		/// <returns>Ergebnis</returns>
		public oSearchDto<T> PostConvert<T> (oSearchDto<GviewDto> found, long permissionId)
		{

			if (false)
			{
				//Hier können weitere spezielle Möglichkeiten eingefügt werden
				//Ansonsten wird der Automapper dafür verwendet.
			}
			else
			{
				//TODO eventuell alle Möglichkeiten, die PostConvert verwenden in das MappingProfil eintragen.
				Mapper.CreateMap<oSearchDto<GviewDto>, oSearchDto<T>> ();
				return Mapper.Map (found, new oSearchDto<T> ());
			}
		}




	}
}