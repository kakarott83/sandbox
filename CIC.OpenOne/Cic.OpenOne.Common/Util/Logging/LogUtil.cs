using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Cic.OpenOne.Common.Model.DdCt;
using log4net.Appender;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOd;
using Cic.OpenOne.Common.Model.DdIc;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.Common.Model.DdOiqueue;
using System.Data.EntityClient;
using Devart.Data.Oracle;
using System.Linq;
using Cic.OpenOne.Common.DTO;
using System.Reflection;
using Cic.OpenOne.Common.Util.Config;
using System.Configuration;
using Cic.One.DTO;
using System.Data.Common;

namespace Cic.OpenOne.Common.Util.Logging
{
	/// <summary>
	/// LogUtil
	/// </summary>
	public class LogUtil
	{
		private static readonly ILog _log = Log.GetLogger (MethodBase.GetCurrentMethod ().DeclaringType);
		
		private static Dictionary<long, DateTime> dictLoggedIn = new Dictionary<long, DateTime> ();

		private static int sessionTimeoutMin = 60;
		private static long? appId;
		private static string hostID;
		private static string userSource;
		private static bool bInit = false;

		/// <summary>
		/// assert logging each minute only
		/// rh 20170203
		/// </summary>
		/// <param name="sysperole"></param>
		/// <param name="d"></param>
		/// <returns></returns>
		private static bool RecentlyLogged (long sysperole, DateTime d)
		{
			if (dictLoggedIn.ContainsKey (sysperole))				// See whether it contains this sysperole.
			{
				DateTime lastStamp = dictLoggedIn[sysperole].AddMinutes (1);	//each 1 minute only
				dictLoggedIn.TryGetValue (sysperole, out lastStamp);
				if (lastStamp.AddMinutes (1) > d)					// assert logging each minute only 
					return true;									// skip all if we logged less then 1 minute before
			}
			return false;											// NOT RecentlyLogged
		}

		/// <summary>
		/// Update on logging
		/// rh 20170203
		/// </summary>
		/// <param name="sysperole"></param>
		/// <param name="d"></param>
		private static void UpdatedictLoggedIn (long sysperole, DateTime d)
		{
			if (!dictLoggedIn.ContainsKey (sysperole))				// See whether it contains this sysperole.
			{
				dictLoggedIn.Add (sysperole, d);					// add if not
			}
			else
			{
				dictLoggedIn[sysperole] = d;						// Update timestamp
			}
			CleanUpLoggedInDict (d);								// check for cleanup ancient entries
		}

		/// <summary>
		/// cleanup entries older then 1 month
		/// rh 20170203
		/// </summary>
		/// <param name="d"></param>
		private static void CleanUpLoggedInDict (DateTime d)
		{
			foreach (KeyValuePair<long, DateTime> pair in dictLoggedIn)
			{
				if (pair.Value.AddMonths (1) < d)
					dictLoggedIn.Remove (pair.Key);
			}
		}

		/// <summary>
		/// ListLoggedIn according to incoming searchPattern
		/// rh 20170203
		/// </summary>
		/// <param name="searchPattern"></param>
		/// <returns></returns>
		public static List<CicLogDto> ListLoggedIn (CicLogDto searchPattern)
		{
			try
			{
				// Feature: if searchPattern.cicbenutzer IS NULL then SEARCH ALL Users
				using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended ())
				{
					List<CicLogDto> CicLogList = null;
					// Feature: ON null searchPattern GET ALL logged in NOW 
					if (searchPattern == null)
					{
						// SET "SELECT * FROM CICLOG WHERE LOGINDATE <= SYSDATE AND LOGOUTDATE >= SYSDATE;"
						CicLogList = ctx.ExecuteStoreQuery<CicLogDto> ("SELECT * FROM CICLOG WHERE LOGINDATE <= SYSDATE AND LOGOUTDATE >= SYSDATE", null).ToList ();
					}
					else
					{
						// Feature: SELECT Now ON empty MANDATORY logindate
						searchPattern.logindate = searchPattern.logindate == null ? DateTime.Now : searchPattern.logindate;

						//———————————————————————————————————————————————————————————
						// GET entry FROM search pattern definition 
						//———————————————————————————————————————————————————————————
						List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter> ();
						string strSelect = "SELECT * FROM CICLOG WHERE ";
						strSelect += GetParamAndQueryString (searchPattern.cicbenutzer, "CICBENUTZER", parameters);
						strSelect += GetParamAndQueryString (searchPattern.maschine, "MASCHINE", parameters);
						strSelect += GetParamAndQueryString (searchPattern.source, "SOURCE", parameters);
						strSelect += GetParamAndQueryString (searchPattern.orabenutzer, "ORABENUTZER", parameters);
						strSelect += GetParamAndQueryString (searchPattern.id, "ID", parameters);
						strSelect += GetParamAndQueryString (searchPattern.sysciclog, "SYSCICLOG", parameters);
						strSelect += GetParamAndQueryString (searchPattern.logoutdate, "LOGOUTDATE", parameters);
						// MANDATORY LogInDate, SET ANYWAY from Feature: SELECT Now ON empty logindate 
						strSelect += GetParamAndQueryString (searchPattern.logindate, "LOGINDATE", parameters);

						CicLogList = ctx.ExecuteStoreQuery<CicLogDto> (strSelect, parameters.ToArray ()).ToList ();
					}
					return CicLogList;
				}
			}
			catch (Exception e)
			{
				_log.Warn ("ListLoggedIn-Problem: " + e.Message, e);
			}

			return null;

		}

		/// <summary>
		/// little helper (quick & dirty)
		/// rh 20170206
		/// </summary>
		/// <param name="attribute"></param>
		/// <param name="attributename"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		private static string GetParamAndQueryString (object attribute, string attributename, List<Devart.Data.Oracle.OracleParameter> parameters)
		{
			if (attribute == null)
				return "";		// shortcut

			switch (Type.GetTypeCode (attribute.GetType ()))
			{
				case TypeCode.Decimal:
					break;
				// ...
				case TypeCode.Int32:
				case TypeCode.Int64:
					if (attribute != null && (long) attribute != 0)		// HINT: BEWARE of NON NULLABLE VALUE
					{
						parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = attributename, Value = attribute });
						return attributename + " = :" + attributename + " AND ";
					}
					break;
				case TypeCode.DateTime:
					DateTime? dt = (DateTime) attribute;
					if (dt.HasValue)
					{
						string strToDate = GetOracleDateString ((DateTime) attribute);
						parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = attributename, Value = strToDate });
						return attributename + " " + (attributename.ToUpper () == "LOGOUTDATE" ? "<" : ">=")
							+ " TO_DATE (:" + attributename + ", 'yyyy.mm.dd hh24:mi:ss')"
							+ (attributename.ToUpper () == "LOGOUTDATE" ? " AND " : "");
					}
					break;
				case TypeCode.String:
				// break;  // continue as default
				default:
					if (!string.IsNullOrEmpty (attribute as string))
					{
						parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = attributename, Value = attribute });
						return attributename + " = :" + attributename + " AND ";
					}
					break;
			}
			return "";
		}



		/// <summary>
		/// return string in format 'yyyy.MM.dd HH:mm:ss'
		/// rh 20170203
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		private static string GetOracleDateString (DateTime date)
		{
			return date.ToString ("yyyy.MM.dd HH:mm:ss");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="SessionTimeoutMin"></param>
		/// <param name="AppId"></param>
		/// <param name="HostID"></param>
		/// <param name="UserSource"></param>
		public static void UpdateLoggedInLogProperties (iLoggedInLogPropertiesDto dto)
		{
			sessionTimeoutMin = dto.sessionTimeoutMin;
			appId = dto.appId;
			hostID = dto.hostID;
			userSource = dto.userSource;
		}


		/// <summary>
		/// rh 20170201: RegisterLoggedIn to CICLOG
		/// </summary>
		/// <param name="sysperole"></param>
		/// <param name="sessionTimeoutMin"></param>
		/// <param name="appId"></param>
		/// <param name="hostID"></param>
		/// <param name="userSource"></param>
		/// ALT: public static void RegisterLoggedIn (long sysperole, int sessionTimeoutMin, long appId, string hostID, string userSource)
		public static void RegisterLoggedIn (long sysperole)
		{
			try
			{
				if (sysperole > 0)
				{
					//if (!bInit)
					//{
					//	sessionTimeoutMin = 0;
					//	appId = (long?) 0;
					//	hostID = "N/A";
					//	userSource = "N/A";
					//	bInit = true;

					//	//int sessionTimeoutMin = 60;						// HINT: ENTER timeOut here, until we can GET it from DefaultMessageHeader  
					//	//long appId = 717171;							// HINT: ENTER AppID here, until we can GET it from DefaultMessageHeader  
					//	//string userSource = "sourceID";					// HINT: ENTER userSource here, until we can GET it from DefaultMessageHeader  
					//	//string hostName = System.Net.Dns.GetHostEntry ("LocalHost").HostName;
					//}

					DateTime dateNow = DateTime.Now;
					int timeoutMinutes = sessionTimeoutMin <= 0 ? 60 : sessionTimeoutMin;		// DEFAULT 1 h (60 min)		
					TimeSpan timeOut = new TimeSpan (0, 0, timeoutMinutes, 0);
					DateTime logoutDate = dateNow.Add (timeOut);
					DateTime minValidLoginDate = dateNow.Add (-timeOut);
					bool bLog = Cic.OpenOne.Common.Properties.Config.Default.LogLoggedInUser;	// SWITCH LoggedInUser-Logging


					// assert SWITCH LoggedInUser-Logging is ON 
					// AND logging each minute only for acceleration 
					if (bLog && !RecentlyLogged (sysperole, dateNow))				
					{
						using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended ())
						{
							string userCode = ctx.ExecuteStoreQuery<string> ("SELECT wfuser.code FROM perole, wfuser, puser " +
								"WHERE perole.sysperson = wfuser.sysperson(+) AND perole.sysperson = puser.sysperson(+) AND perole.sysperole = " + sysperole, null).FirstOrDefault ();

							if (string.IsNullOrEmpty (userCode))			// UNDETERMINED USER attempt  // ReCheck rh 20170202: are there further actions to be done?
							{
								userCode = "UNKNOWN USER[" + sysperole + "]";
								logoutDate = dateNow;						// ReSET logoutDate to dateNow, so every UNDETERMINED USER attempt will be logged as a separate CICLOG-entry
							}

							List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter> ();
							parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "logindate", Value = dateNow });
							parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "logoutdate", Value = logoutDate });
							parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "cicbenutzer", Value = userCode });
							parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "maschine", Value = hostID });
							parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = appId });
							parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "source", Value = userSource });

							//———————————————————————————————————————————————————————————
							// GET LATEST (!) Entry FROM User with userCode 
							//———————————————————————————————————————————————————————————
							List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter> ();
							par.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "userCode", Value = userCode });
							CicLogDto sysCicLog = ctx.ExecuteStoreQuery<CicLogDto> ("SELECT * FROM (SELECT * FROM CICLOG  WHERE CICBENUTZER = :userCode AND LOGOUTDATE IS NOT NULL ORDER BY LOGOUTDATE DESC) WHERE ROWNUM = 1", par.ToArray ()).FirstOrDefault ();
							
							// if login was within timeout-span AND logoutdate not yet reached
							if (sysCicLog != null && sysCicLog.logindate > minValidLoginDate && sysCicLog.logoutdate > dateNow)			// not necc: sysCicLog.logoutdate.HasValue &&
							{
								// UPDATE current entry: 
								// updatelogoutDate: Advance by timeoutMin minutes (SYSDATE + 1/24/60*timeoutMin)
								ctx.ExecuteStoreCommand ("UPDATE CICLOG SET LOGOUTDATE=SYSDATE + 1/24/60*" + timeoutMinutes + " WHERE SYSCICLOG = " + sysCicLog.sysciclog, null);
							}
							else
							{
								// INSERT NEW entry: if login OR Logout expired OR "NEVER LOGGED" USER
								int rowsAffected = ctx.ExecuteStoreCommand ("INSERT INTO CICLOG (SysCicLog, LOGINDATE, LOGOUTDATE, CICBENUTZER, MASCHINE, ID, SOURCE)	" +
                                    "VALUES (cic.ciclog_seq.nextval, :logindate, :logoutdate, :cicbenutzer, :maschine, :id, :source)", parameters.ToArray());
								if (rowsAffected <= 0)
								{
									userSource = "NON-successful CICLOG_USER_INSERT";
									// ToDo rh: ReCheck if needed: _log ("DUBIOUS_INSERT_dummy...");
								}
							}
							ctx.SaveChanges ();
							UpdatedictLoggedIn (sysperole, dateNow);		// Update last logging-timestamp
						}
					}
					else
					{
						// logging skipped for acceleration 
					}
				}
			}
			catch (Exception e)
			{
				_log.Warn ("LoggedInLog-Problem: " + e.Message, e);
			}
		}

		/// <summary>
		/// Adds an WFLog-Entry
		/// </summary>
		/// <param name="area"></param>
		/// <param name="message"></param>
		public static void addWFLog (String area, String message, int logtyp)
		{
			using (DdCtExtended ctx = new DdCtExtended ())
			{
				DateTime cts = DateTime.Now;
				object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area } ,
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "message", Value = message } ,
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "time", Value = DateTimeHelper.DateTimeToClarionTime(cts) },
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "logtyp", Value = logtyp }  
                                 };
				ctx.ExecuteStoreCommand ("INSERT INTO WFLOG(GRUNDLAGE,FEHLERTEXT,DATUM,UHRZEIT,LOGTYP) values(:area,:message,sysdate,:time,:logtyp)", pars);
			}
		}

		/// <summary>
		/// Logs an object serialized as xml into ciclog/cictlog/cictlogb for the area/id and user
		/// </summary>
		/// <param name="area"></param>
		/// <param name="sysid"></param>
		/// <param name="logdata"></param>
		/// <param name="syswfuser"></param>
		/// <returns>id of cictlog</returns>
		public static long addTLog (String area, long sysid, object logdata, long syswfuser)
		{
			using (DdOiQueueExtended ctx = new DdOiQueueExtended ())
			{
				EntityConnection EntityConnection = ctx.Connection as EntityConnection;
				OracleConnection OracleConnection = EntityConnection.StoreConnection as OracleConnection;
				String uid = System.Environment.UserName;
				String logcontent = XMLSerializer.Serialize (logdata, "UTF-8");
				String machine = System.Environment.MachineName;
				String cuser = "BOS";
				String wfuser = "BOS";
				if (syswfuser > 0)
					wfuser = ctx.ExecuteStoreQuery<String> ("select code from wfuser where syswfuser=" + syswfuser, null).FirstOrDefault ();

                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                con.Open();
                DbCommand cmd = con.CreateCommand();
                cmd.CommandText = "insert into ciclog(SYSCICLOG,LOGINDATE,logoutdate,ORABENUTZER,CICBENUTZER,MASCHINE,ID,source) values(cic.ciclog_seq.nextval,sysdate,null,:orauser,:clientuser,:machine, sys_context('USERENV','SID'),null ) returning SYSCICLOG  into :myOutputParameter";
                //TODO kunde name, vorname, plz,strasse, ort
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new OracleParameter("orauser", OracleConnection.UserId));
                cmd.Parameters.Add(new OracleParameter("clientuser", wfuser));
                cmd.Parameters.Add(new OracleParameter("machine", System.Environment.MachineName));
                cmd.Parameters.Add(new OracleParameter("myOutputParameter", OracleDbType.Long, System.Data.ParameterDirection.ReturnValue));
                cmd.ExecuteNonQuery();
                long sysciclog = Convert.ToInt32(cmd.Parameters["myOutputParameter"].Value);

			

				object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area } ,
                                    new Devart.Data.Oracle.OracleParameter { ParameterName = "syslease", Value = sysid } ,
                                    new Devart.Data.Oracle.OracleParameter { ParameterName = "cuser", Value=cuser } ,
                                    new Devart.Data.Oracle.OracleParameter { ParameterName = "maschine", Value=machine } ,
                                    new Devart.Data.Oracle.OracleParameter { ParameterName = "clientuser", Value=wfuser } ,
                                    new Devart.Data.Oracle.OracleParameter { ParameterName = "orauser", Value= OracleConnection.UserId } ,
                                    new Devart.Data.Oracle.OracleParameter { ParameterName = "sysciclog", Value=sysciclog } 
                                 };
				ctx.ExecuteStoreCommand ("INSERT INTO CICTLOG(OLAREA,SYSLEASE,CUSER,MASCHINE,CLIENTUSER,changedate,orauser,sysciclog) values(:area,:syslease,:cuser,:maschine,:clientuser,sysdate,:orauser,:sysciclog)", pars);

				long syscictlog = ctx.ExecuteStoreQuery<long> ("select syscictlog from cictlog where sysciclog=" + sysciclog, null).FirstOrDefault ();

				object[] pars2 = { new Devart.Data.Oracle.OracleParameter { ParameterName = "syscictlog", Value = syscictlog } ,
                                    new Devart.Data.Oracle.OracleParameter { ParameterName = "value", Value = logcontent }
                                 };
				ctx.ExecuteStoreCommand ("INSERT INTO CICTLOGB(SYSCICTLOG,NEWVALUEC) values(:syscictlog,:value)", pars2);
				return syscictlog;

			}
		}
		/// <summary>
		/// Update the cictlog from addTLog with the correct sysid
		/// </summary>
		/// <param name="syscictlog"></param>
		/// <param name="sysid"></param>
		public static void updateTLog (long syscictlog, long sysid)
		{
			using (DdOiQueueExtended ctx = new DdOiQueueExtended ())
			{
				ctx.ExecuteStoreCommand ("UPDATE cictlog set syslease=" + sysid + " where syscictlog=" + syscictlog);
			}
		}

		/// <summary>
		/// Returns the last CHUNKSIZE Bytes from the Logfiles written by the first found log4net fileAppender
		/// The files are assumed to have the [%processid] in the filename, as defined in the log4net.config.
		/// </summary>
		/// <param name="CHUNKSIZEfromCFG"></param>
		/// <returns></returns>
		public static String getLogFileEnd (int CHUNKSIZEfromCFG)
		{
			FileAppender appender = GetFileAppender ();
			string folder = "";
			string filePattern = "";
			if (appender != null)
			{
				int lastSlash = appender.File.LastIndexOf ('\\');
				folder = appender.File.Substring (0, lastSlash);
				filePattern = appender.File.Substring (lastSlash + 1);
				// Replace the [%processid]
				filePattern = Regex.Replace (filePattern, "(\\[.*\\])", "*");
			}

			DirectoryInfo dirInfo = new DirectoryInfo (folder);
			var allFiles = dirInfo.GetFiles (filePattern, SearchOption.TopDirectoryOnly);
			List<FileInfo> allfileList = new List<FileInfo> (allFiles);

			// Sort the list by LastWriteTime in ascending order (oldest first)
			allfileList.Sort ((x, y) => x.LastWriteTime.CompareTo (y.LastWriteTime));

			DateTime latestDate = allfileList[allfileList.Count - 1].LastWriteTime.Date;
			// Leave only the files which have the same LastWrite date as the latest one
			allfileList = allfileList.FindAll (x => x.LastWriteTime.Date.Equals (latestDate));

			String rval = "";

			foreach (var logFile in allfileList)
			{
				rval += "\n ---------------- " + logFile.Name + " ---------------- \n";

				Stream byteStream = new FileStream (logFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				StreamReader myStream = new StreamReader (byteStream);

				//seek file pointer to end
				myStream.BaseStream.Seek (0, SeekOrigin.End);

				int localCHUNKSIZE = CHUNKSIZEfromCFG;

				if (myStream.BaseStream.Position < localCHUNKSIZE)
					localCHUNKSIZE = (int) myStream.BaseStream.Position;
				char[] arr = new char[localCHUNKSIZE];
				arr.Initialize ();

				// read backwards
				if (myStream.BaseStream.Position > 0)
				{
					// Move back localCHUNKSIZE bytes (for better control)
					myStream.BaseStream.Seek (-localCHUNKSIZE, SeekOrigin.Current);

					// read localCHUNKSIZE bytes
					int bytesRead = myStream.Read (arr, 0, localCHUNKSIZE);

					rval += new String (arr, 0, bytesRead);
				}
				myStream.BaseStream.Flush ();
				byteStream.Flush ();

				myStream.Close ();
				byteStream.Close ();
			}
			return rval;
		}
		/// <summary>
		/// returns the last CHUNKSIZE Bytes of the first found log4net fileAppender
		/// </summary>
		/// <param name="CHUNKSIZE"></param>
		/// <param name="file"></param>
		/// <returns></returns>
		public static String getLogFileEnd (int CHUNKSIZE, String file)
		{

			String rval = "";
			if (file != null && File.Exists (file))
			{

				Stream byteStream = new FileStream (file, FileMode.Open, FileAccess.Read, FileShare.Read);
				StreamReader myStream = new StreamReader (byteStream);
				//seek file pointer to end
				myStream.BaseStream.Seek (0, SeekOrigin.End);

				if (myStream.BaseStream.Position < CHUNKSIZE)
					CHUNKSIZE = (int) myStream.BaseStream.Position;
				char[] arr = new char[CHUNKSIZE];
				arr.Initialize ();

				//loop now and read backwards
				if (myStream.BaseStream.Position > 0)
				{

					//Move back 1024 bytes (for better control)
					myStream.BaseStream.Seek (-CHUNKSIZE, SeekOrigin.Current);
					//read 1024 bytes.
					int bytesRead = myStream.Read (arr, 0, CHUNKSIZE);
					rval = new String (arr, 0, bytesRead);
				}
				myStream.Close ();
				byteStream.Close ();

			}
			return rval;
		}
		/// <summary>
		/// GetAllAppenders
		/// </summary>
		/// <returns></returns>
		public static log4net.Appender.IAppender[] GetAllAppenders ()
		{
			ArrayList appenders = new ArrayList ();
			log4net.Repository.Hierarchy.Hierarchy h = (log4net.Repository.Hierarchy.Hierarchy) log4net.LogManager.GetRepository ();
			appenders.AddRange (h.Root.Appenders);
			return (log4net.Appender.IAppender[]) appenders.ToArray (typeof (log4net.Appender.IAppender));
		}

		/// <summary>
		/// GetFileAppender
		/// </summary>
		/// <returns></returns>
		public static FileAppender GetFileAppender ()
		{
			IAppender[] appenderList = GetAllAppenders ();
			FileAppender fileAppender = null;
			foreach (IAppender appender in appenderList)
			{
				if (appender.GetType ().FullName.IndexOf ("FileAppender") > -1)
				{
					fileAppender = (FileAppender) appender;
					break;
				}
			}
			return fileAppender;
		}
		/// <summary>
		/// returns the file of the fileappender
		/// </summary>
		/// <returns></returns>
		public static String getFileAppenderFile ()
		{
			FileAppender fa = GetFileAppender ();
			if (fa == null)
				return null;

			return fa.File;
		}

		///// <summary>
		///// Switch Log loggedIn users ON / OFF
		///// rh 20170209
		///// </summary>
		///// <param name="bLog"></param>
		//public static void switchLoggedInLog (bool bLog)
		//{
		//	if (bLog != Properties.Config.Default.LogLoggedInUser)	// avoid useless switching
		//	{
		//		// ON User Scope: 
		//		//Properties.Config.Default.LogLoggedInUser = bLog;	 
		//		//Properties.Config.Default.Save ();					// persist value

		//		// ON App-Scope: 
		//		System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration (ConfigurationUserLevel.None);
		//		configuration.AppSettings.Settings["LogLoggedInUser"].Value = bLog ? "True" : "False";
		//		configuration.Save ();
		//		ConfigurationManager.RefreshSection ("appSettings");
		//	}
		//}
	}
}