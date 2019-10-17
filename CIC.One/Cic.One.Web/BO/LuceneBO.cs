using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CIC.ASS.Common.DTO;
using Cic.OpenOne.Common.Util;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Serialization;
using CIC.ASS.Common.BO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using CIC.ASS.SearchService.BO;
using CIC.ASS.SearchService.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using CIC.ASS.SearchService;
using Cic.One.Web.DAO;

namespace Cic.One.Web.BO
{
    /// <summary>
    /// Lucene BO for managing indexes
    /// </summary>
    public class LuceneBO
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IndexedTable[] indexedTables;
        private static volatile LuceneBO _self = null;
        private static readonly object InstanceLocker = new Object();
        private bool clFailure = false;
        private Dictionary<long, String> months = new System.Collections.Generic.Dictionary<long, string>();
        private luceneConfigServiceReference.IluceneConfigServiceClient client;
        private Indexer indexer;

        private LuceneBO()
        {
            //Indexer.debug = true;

            months.Add(1, "jan");
            months.Add(2, "feb");
            months.Add(3, "mar");
            months.Add(4, "apr");
            months.Add(5, "mai");
            months.Add(6, "jun");
            months.Add(7, "jul");
            months.Add(8, "aug");
            months.Add(9, "sep");
            months.Add(10, "okt");
            months.Add(11, "nov");
            months.Add(12, "dez");

            indexedTables = new IndexedTable[]{};
            /*
                            new IndexedTable("OPPO","select oppo.sysoppo id,oppo.name title,oppo.description description1,OPPOTP.NAME description2, PERSON.NAME description3, (SELECT rtrim(xmlagg(xmlelement(e,sysparent,',').extract('//text()') order by sysparent).GetClobVal(),',') FROM peuni, perolecache WHERE area = 'OPPO' AND peuni.sysperole = perolecache.syschild AND sysid=oppo.sysoppo) peuni, (SELECT listagg (sysidchild, ' ') WITHIN GROUP ( ORDER BY sysidchild) FROM crmnm WHERE crmnm.parentarea='OPPO' AND crmnm.childarea   ='PERSON' AND crmnm.sysidparent  =oppo.sysoppo ) sysidchildoppo from CIC.OPPO OPPO, CIC.OPPOTP OPPOTP, CIC.PERSON PERSON where  OPPO.SYSOPPOTP = OPPOTP.SYSOPPOTP(+) and OPPO.SYSPERSON = PERSON.SYSPERSON (+) ","sysoppo"),
                            new IndexedTable("MAILMSG","select  mailmsg.sysmailmsg id,mailmsg.subject title,mailmsg.recvfrom||' '||mailmsg.sentto description1,person.name description2,mailmsg.content description3, (SELECT rtrim(xmlagg(xmlelement(e,sysparent,',').extract('//text()') order by sysparent).GetClobVal(),',') FROM peuni, perolecache WHERE area = 'MAILMSG' AND peuni.sysperole = perolecache.syschild AND sysid=mailmsg.sysmailmsg) peuni from CIC.MAILMSG MAILMSG, PERSON PERSON where  MAILMSG.SYSPERSON = PERSON.SYSPERSON (+) ","sysmailmsg"),
                            new IndexedTable("PTASK", "select ptask.sysptask id,ptask.subject title,person.name description1,'' description2,ptask.content description3, (SELECT rtrim(xmlagg(xmlelement(e,sysparent,',').extract('//text()') order by sysparent).GetClobVal(),',') FROM peuni, perolecache WHERE area = 'PTASK' AND peuni.sysperole = perolecache.syschild AND sysid=ptask.sysptask) peuni from CIC.PTASK PTASK, PERSON PERSON where  PTASK.SYSPERSON = PERSON.SYSPERSON (+) ","sysptask"),
                            new IndexedTable("APPTMT","select apptmt.sysapptmt id,apptmt.subject title,person.name description1,apptmt.startdate description2,apptmt.content description3, (SELECT rtrim(xmlagg(xmlelement(e,sysparent,',').extract('//text()') order by sysparent).GetClobVal(),',') FROM peuni, perolecache WHERE area = 'APPTMT' AND peuni.sysperole = perolecache.syschild AND sysid=apptmt.sysapptmt) peuni from CIC.APPTMT APPTMT, PERSON PERSON where  APPTMT.SYSPERSON = PERSON.SYSPERSON (+) ","sysapptmt"),
                            new IndexedTable("PERSON","select  sysperson id,name||' '||vorname title,strasse description1, plz||' '||ort description2, telefon description3, '' peuni from person where 1=1 ","sysperson"),
                            //select sysperson id,name||' '||vorname title,strasse description1, plz||' '||ort description2, telefon description3, (SELECT rtrim(xmlagg(xmlelement(e,sysparent,',').extract('//text()') order by sysparent).GetClobVal(),',') FROM peuni, perolecache WHERE area = 'PERSON' AND peuni.sysperole = perolecache.syschild AND sysid=person.sysperson) peuni, (SELECT listagg (sysperson1, ' ') WITHIN GROUP (ORDER BY sysptrelate) FROM ptrelate WHERE sysperson2=person.sysperson) sysperson1, (SELECT listagg (sysidparent, ' ') WITHIN GROUP ( ORDER BY sysidparent) FROM crmnm WHERE crmnm.parentarea='OPPO' and crmnm.childarea='PERSON' and crmnm.sysidchild=person.sysperson ) sysidparentoppo from person where 1=1
                            new IndexedTable("ANGEBOT", "select  angebot.sysid id,angebot.angebot title,angebot.objektvt description1, angebot.zustand description2, kd.vorname || kd.name description3,'' peuni from angebot, CIC.PERSON KD, CIC.IT IT, CIC.LSADD LSADD, CIC.RVT RAHMEN where ANGEBOT.SYSKD=KD.SYSPERSON(+) and ANGEBOT.SYSIT=IT.SYSIT(+) and ANGEBOT.SYSLS=LSADD.SYSLSADD(+) and ANGEBOT.SYSRVT=RAHMEN.SYSRVT and angebot.sysls=3 ","sysid"),
                            new IndexedTable("VERTRAG","select  vt.sysid id,vt.vertrag title,vt.fabrikat description1, vt.vart ||' '||vt.zustand description2, kd.vorname || kd.name description3,'' peuni from vt, CIC.PERSON KD where VT.SYSKD=KD.SYSPERSON(+) and syskd>0 and vt.sysls=3","sysid"),
                            new IndexedTable("INT","select  sysit id,name||' '||vorname title,strasse description1, plz||' '||ort description2, telefon description3, '' peuni from it where 1=1","sysit")
                    };*/

            loadConfig();
        }

        public static LuceneBO getInstance()
        {
            if (_self == null)
            {
                lock (InstanceLocker)
                {
                    if (_self == null)
                        _self = new LuceneBO();
                }
            }
            return _self;
        }

        /// <summary>
        /// Starts the indexer
        /// </summary>
        public void startIndexer()
        {
            try
            {
                getClient().startIndexer(indexedTables);//this may take a very long time and produce a timeout exception, but the indexer will nonetheless run
            }
            catch (Exception)
            {
                if (getClient() == null)//if null we really have no endpoint
                {
                    indexer = new Indexer(indexedTables,false);//Start Lucene Indexer, do not recreate the index now
                }
            }
        }

        /// <summary>
        /// reinitializes the lucene config service endpoint
        /// </summary>
        /// <returns></returns>
        private luceneConfigServiceReference.IluceneConfigService getClient()
        {
            if (clFailure)
                return null;

            if (client == null)
            {
                try
                {
                    client = new luceneConfigServiceReference.IluceneConfigServiceClient();
                    _log.Info("Using Lucene Service.");
                    clFailure = false;
                }
                catch (Exception e)
                {
                    clFailure = true;
                    _log.Warn("Lucene Service not found, using local instance. Reason: "+e.Message);
                }
            }
            else
            {
                if (client.State != System.ServiceModel.CommunicationState.Opened && client.State != System.ServiceModel.CommunicationState.Created && client.State != System.ServiceModel.CommunicationState.Opening)
                {
                    try
                    {
                        client = new luceneConfigServiceReference.IluceneConfigServiceClient();
                        clFailure = false;
                    }
                    catch (Exception)
                    {
                        clFailure = true;
                        _log.Warn("Lucene Service not found, using local instance.");
                    }
                }
            }
            return client;
        }

        /// <summary>
        /// loads lucene config or writes default config to disc
        /// </summary>
        private void loadConfig()
        {
            
            CustomerConfig cconfig = WFVDao.getCustomerConfig();
            if(cconfig==null)
            {
                _log.Warn("No Lucene Configuration found, check SETUP.NET/CONFIG/CUSTOMER");
                return;
            }
            List<IndexedTable> ctables = new List<IndexedTable>();
            foreach(IndexedTableDto itab in cconfig.luceneconfig.tables)
            {
                IndexedTable it = new IndexedTable();
                it.id = itab.id;
                it.areaField = itab.areaField;
                it.indexid = itab.indexid;
                it.keyField = itab.keyField;
                it.peroleFilter = itab.peroleFilter;
                it.peuniFilter = itab.peuniFilter;
                it.positiveId = itab.positiveId;
                it.query = itab.query;
                it.updatequery = itab.updatequery;
                ctables.Add(it);
            }
            indexedTables = ctables.ToArray();
            /*
            byte[] data = null;
            try
            {
                data = FileUtils.loadData(FileUtils.getCurrentPath() + "\\..\\luceneconfig.xml");
                String ENCODING = "UTF-8";
                LuceneConfig luceneConfig = XMLDeserializer.objectFromXml<LuceneConfig>(data, ENCODING);
                indexedTables = luceneConfig.tables;

            }
            catch (Exception)
            {
                _log.Error("LuceneConfig not found"); LuceneConfig cfgTmp = new LuceneConfig();
                cfgTmp.tables = indexedTables;
                FileUtils.saveFile(FileUtils.getCurrentPath() + "\\..\\luceneconfig.xml", System.Text.UTF8Encoding.Default.GetBytes(XMLSerializer.SerializeUTF8(cfgTmp)));
                _log.Debug("Using lucene config: " + XMLSerializer.SerializeUTF8(cfgTmp));
            }*/
        }

        /// <summary>
        /// returns a list of currently disabled perolefilter-entities
        /// </summary>
        /// <returns></returns>
        public String[] getDisabledPeroles()
        {
            return (from t in indexedTables
                    where t.peroleFilter == false
                    select t.id).ToArray();
        }
        /// <summary>
        /// Returns true when the given index has peuni filter active
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public String getPeuniFilter(String id)
        {
            IndexedTable it = (from t in indexedTables
                    where t.id.Equals(id)
                    select t).FirstOrDefault();
            if (it == null) return null;
            return it.peuniFilter;
        }
        public void queueForIndexUpdate(String area, long id)
        {

            try
            {
                getClient().queueForIndexUpdate(area, id);
            }
            catch (Exception)
            {
                Indexer.queueForIndexUpdate(area, id);
            }
        }
        /// <summary>
        /// rebuild the complete index
        /// </summary>
        public void rebuild()
        {
            if(indexer!=null)
                indexer.rebuildIndex();
        }
        public String[] suggest(String term)
        {
            try
            {
                return getClient().suggest(term);
            }
            catch (Exception)
            {
                return new AutoCompleteBO().Suggest(term);
            }
        }

        /// <summary>
        /// Uses Lucene Search and post-processes the result 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="perole"></param>
        /// <param name="entities"></param>
        /// <param name="additionalQuery"></param>
        /// <param name="preproc"></param>
        /// <param name="sortInfo"></param>
        /// <returns></returns>
        public SearchEntityResult[] search(String query, String perole, String entities, String additionalQuery, QueryPreprocessorConfig preproc, SortSearchInfo sortInfo)
        {
            SearchEntityResult[] rval = null;
            try
            {
                rval = getClient().searchEntities(query, perole, entities, additionalQuery, preproc, LuceneBO.getInstance().getDisabledPeroles());
            }
            catch (Exception)
            {
                rval = new SearchBO().searchEntitiesConfigured(query, perole, entities, additionalQuery, preproc, sortInfo, LuceneBO.getInstance().getDisabledPeroles());
            }
            IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(null);
            using (PrismaExtended ctx = new PrismaExtended())
            {
                foreach (SearchEntityResult ser in rval)//for every searched entity (ANGEBOT,ANTRAG,VT for example) the array contains the results
                {

                    String peuniFilter = getPeuniFilter(ser.entity);
                    if(peuniFilter!=null)
                    {
                        String[] areas = (from s in ser.results
                                          select s.area).Distinct<String>().ToArray();
                        List<SearchResult> results = new List<SearchResult>();
                        
                        foreach (String area in areas)
                        {
                            try
                            {
                                //peuniFilter of luceneConfig.xml contains the result-area, so check in peuni
                                if (peuniFilter.IndexOf(area) > -1 && perole != null)
                                {
                                    long[] idArr = (from s in ser.results
                                                    where s.area.Equals(area)
                                                    select long.Parse(s.id)).ToArray();
                                    String ids = String.Join(",", idArr);
                                    List<long> permittedIds = ctx.ExecuteStoreQuery<long>("SELECT sysid FROM peuni, perolecache WHERE area = '" + area + "' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + perole + " and sysid in (" + ids + ")", null).ToList();

                                    results.AddRange((from s in ser.results
                                                      where s.area.Equals(area) && permittedIds.Contains(long.Parse(s.id))
                                                      select s).ToList());
                                }
                                else//not in luceneConfig, use unfiltered without peuni
                                {
                                    results.AddRange((from s in ser.results
                                                      where s.area.Equals(area)
                                                      select s).ToList());
                                }
                            }catch(Exception e)
                            {
                                _log.Error("Lucene Search error for area: " + area, e);
                            }
                        }
                        ser.results = results.ToArray();
                    }
                 
                
                    foreach (SearchResult sr in ser.results)
                    {
                        sr.url = "auto=true&entity=" + sr.entity + "&key=" + sr.id;
                    }
                    /*long[] idArr = (from s in ser.results
                                   select long.Parse(s.id)).ToArray();

                   String ids = String.Join(",", idArr);
                   String luceneEntity = ser.entity.ToLower();
                   String olArea = AreaEntityMapperBO.getInstance().getAreaFromLucene(ser.entity);

                   */
                    /*  if (olArea.Equals("mailmsg"))
                   {
                       foreach (SearchResult sr in ser.results)
                       {
                           sr.description3 = "";
                       }
                   }*/
                   /* if (ids.Length > 0)
                    {
                            if (olArea.Equals("PERSON") || olArea.Equals("IT"))
                            {

                                igetExpdefDto expdefInput = new igetExpdefDto();
                                expdefInput.area = olArea;
                                expdefInput.areaids = idArr;
                                List<ExpdefDto> defaults = bo.getExpdefDetails(expdefInput);
                                foreach (ExpdefDto defs in defaults)
                                {
                                    SearchResult sr = (from res in ser.results
                                                       where res.id == defs.areaid.ToString()
                                                       select res).FirstOrDefault();
                                    sr.indicator = defs.output;
                                }
                            }
                    }*/

                }
            }
            return rval;
        }
    }
}