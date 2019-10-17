using AutoMapper;
using Cic.One.DTO;
using Cic.One.Web.Service;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WfvXmlConfigurator.BO.GUI;
using WfvXmlConfigurator.DTO;


namespace WfvXmlConfigurator.DAO
{
    /// <summary>
    /// Datamase connection manager
    /// </summary>
    internal class DatabaseManager : IDataManager
    {
        private static readonly ILog log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal DatabaseManager()
        {
            Mapper.CreateMap<WfvEntry, WFV>();
            Mapper.CreateMap<WfvConfigEntry, WFV>();
        }

        /// <summary>
        /// SVN revision number for version control
        /// </summary>
        /// <returns>version for database parameter</returns>
        private static string GetVersion()
        {
            Assembly versionAssembly = Assembly.GetAssembly(typeof(VersionHandle));
            Version versionFull = versionAssembly.GetName().Version;
            int version = versionFull.Revision;
            if (version == 0)
                return ConstantsDto.MAX_VERSION;
            else
                return version.ToString();
        }

        /// <summary>
        /// Read data from database
        /// </summary>
        /// <returns>data from database</returns>
        public WfvConfig ReadData()
        {
            WfvConfig GuiDefinition = new WfvConfig();
            using (PrismaExtended ctx = new PrismaExtended())
            {
                try
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vers", Value = GetVersion() });

                    List<WfvConfigEntry> entries = ctx.ExecuteStoreQuery<WfvConfigEntry>(ConstantsDto.QUERY_SELECT_WFV_WFVCOMMAND, pars.ToArray()).ToList();
                    entries.Sort((entry1, entry2) => entry1.syscode.CompareTo(entry2.syscode));

                    GuiDefinition.entries = new List<WfvEntry>();
                    GuiDefinition.configentries = entries;
                }
                catch (Exception e)
                {
                    string errorinfo = "Fehler beim Lesen der GUI Definition aus der Datenbank";
                    log.Error(errorinfo, e);
                    throw new Exception(errorinfo, e);
                }
            }

            return GuiDefinition;
        }

        /// <summary>
        /// Write data to database
        /// </summary>
        /// <param name="data">edited data</param>
        public void SaveData(WfvConfig data)
        {
                
            HashSet<String> saved = new HashSet<String>();
            using (DdOwExtended ctx = new DdOwExtended())
            {


                foreach (WfvConfigEntry entry in data.configentries)
                {
                    WFV te = (from p in ctx.WFV
                                where p.SYSCODE.Equals(entry.syscode)
                                select p).FirstOrDefault();
                    //wfventry nicht in db -> 
                    //wfventry anlegen
                    if (te == null)
                    {
                        if (saved.Contains(entry.syscode))
                        {
                            log.Debug("SYSCODE MULTIPLE TIMES IN wfvconfig: " + entry.syscode);
                            continue;
                        }
                        WFV wfv = Mapper.Map<WfvConfigEntry, WFV>(entry);
                        wfv.KURZBEZ = ConstantsDto.SHORT_DESC_FOR_DATABASE_INSERTION;
                        wfv.TYP = 1;
                        ctx.AddToWFV(wfv);
                        saved.Add(entry.syscode);
                    }
                    else//update wfventry
                    {
                        Mapper.Map<WfvConfigEntry, WFV>(entry, te);
                    }
                }


                //static wfv 
                foreach (WfvEntry entry in data.entries)
                {
                    WFV te = (from p in ctx.WFV
                                where p.SYSCODE.Equals(entry.syscode)
                                select p).FirstOrDefault();
                    //wfventry nicht in db -> 
                    //wfventry anlegen
                    if (te == null)
                    {
                        if (saved.Contains(entry.syscode))
                        {
                            log.Debug("SYSCODE MULTIPLE TIMES IN wfvconfig: " + entry.syscode);
                            continue;
                        }
                        WFV wfv = Mapper.Map<WfvEntry, WFV>(entry);
                        if (entry.customentry != null)
                        {
                            wfv.BESCHREIBUNG = entry.customentry.title;
                        }
                        wfv.ART = "P";
                        XmlObjectConverter<WfvEntry> converter = new XmlObjectConverter<WfvEntry>();
                        wfv.EINRICHTUNG = (string)converter.ConvertTo(entry, typeof(string));
                        wfv.KURZBEZ = ConstantsDto.SHORT_DESC_FOR_DATABASE_INSERTION;
                        wfv.TYP = 1;
                        ctx.AddToWFV(wfv);
                        saved.Add(entry.syscode);
                    }
                    else//update wfventry
                    {
                        Mapper.Map<WfvEntry, WFV>(entry, te);
                        if (entry.customentry != null)
                        {
                            te.BESCHREIBUNG = entry.customentry.title;
                        }
                        XmlObjectConverter<WfvEntry> converter = new XmlObjectConverter<WfvEntry>();
                        te.EINRICHTUNG = (string)converter.ConvertTo(entry, typeof(string));
                    }



                }
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// clean stuff
        /// </summary>
        public void Dispose()
        {
        }
    }
}
