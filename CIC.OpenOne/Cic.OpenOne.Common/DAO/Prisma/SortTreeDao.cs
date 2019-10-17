using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenOne.Common.DAO.Prisma
{
    class BezInfo
    {
        public String name {get;set;}
        public long id {get;set;}
    }
    /// <summary>
    /// SortTree Data Access Object
    /// </summary>
    public class SortTreeDao : ISortTreeDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const String TREEQUERY = "select sortpos.syssortpos, sortpos.syssortposp, wftable.syscode wftablecode, sortpos.sysid, sorttyp.bezeichnung sortTypBezeichnung,sorttyp.sichtbar visible, sortpos.rang rang, sortpos.bezeichnung bezeichnung  from sortpos, sorttyp left outer join wftable on sorttyp.syswftable=wftable.syswftable where sorttyp.syssorttyp=sortpos.syssorttyp  connect by prior sortpos.syssortpos=sortpos.syssortposp start with syssortposp is null order by rang";
        private const String PRODBEZQUERY = "select name, sysprproduct id from prproduct";
        private const String CHANNELBEZQUERY = "select name, sysbchannel id from bchannel";
        private const String VARTBEZQUERY = "select bezeichnung name, sysvart id from vart";

        
        private static CacheDictionary<String, ThreadSafeDictionary<long, String>> bezCache = CacheFactory<String, ThreadSafeDictionary<long, String>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, List<SortTreeNode>> nodeListCache = CacheFactory<String, List<SortTreeNode>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);



        /// <summary>
        /// Standarad Constuctor
        /// Database access Object for SortTree
        /// </summary>
        public SortTreeDao()
        {
        }

        /// <summary>
        /// Clones the sort-tree nodes
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private List<SortTreeNode> cloneList(List<SortTreeNode> source)
        {
            List<SortTreeNode> rval = new List<SortTreeNode>();
            foreach(SortTreeNode node in source)
            {
                rval.Add(new SortTreeNode(node));
            }
            return rval;
        }

        /// <summary>
        /// get SortTree Root for given WfTableCode
        ///  root nodes have a parent of null
        /// </summary>
        /// <returns>Root of the sort tree</returns>
        public SortTreeNode getSortTree()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {


                if (!nodeListCache.ContainsKey("NODELIST"))
                {
                    //this cannot be cached easyly, because every call to this method will work on the resulting nodes and alter them!
                    nodeListCache["NODELIST"] = ctx.ExecuteStoreQuery<SortTreeNode>(TREEQUERY, null).ToList();
                }
                //clone the original list to work on it
                List<SortTreeNode> nodeList = cloneList(nodeListCache["NODELIST"]);
                if (!bezCache.ContainsKey("PRPRODUCT"))
                {
                    
                    List<BezInfo> bezInfos = ctx.ExecuteStoreQuery<BezInfo>(PRODBEZQUERY, null).ToList();
                    ThreadSafeDictionary<long, String> bezmap = new ThreadSafeDictionary<long, string>();
                    bezCache["PRPRODUCT"] = bezmap;
                    foreach(BezInfo bi in bezInfos)
                        bezmap[bi.id] = bi.name;
                    bezInfos = ctx.ExecuteStoreQuery<BezInfo>(CHANNELBEZQUERY, null).ToList();
                    bezmap = new ThreadSafeDictionary<long, string>();
                    bezCache["BCHANNEL"] = bezmap;
                    foreach (BezInfo bi in bezInfos)
                        bezmap[bi.id] = bi.name;
                    bezInfos = ctx.ExecuteStoreQuery<BezInfo>(VARTBEZQUERY, null).ToList();
                    bezmap = new ThreadSafeDictionary<long, string>();
                    bezCache["VART"] = bezmap;
                    foreach (BezInfo bi in bezInfos)
                        bezmap[bi.id] = bi.name;
                }
              

                //create tree items list
                Dictionary<long, SortTreeNode> keyItemDict = new Dictionary<long, SortTreeNode>();
                
                keyItemDict[-1] = new SortTreeNode();//root
                foreach (SortTreeNode node in nodeList)
                {
                    if (node.syssortposp == 0)//multiple roots, have parent 0, redirect to -1
                        node.syssortposp = -1;
                    
                    if (node.wftablecode!=null && bezCache.ContainsKey(node.wftablecode) )
                    {
                        ThreadSafeDictionary<long, string> dict = bezCache[node.wftablecode];
                        if (dict.ContainsKey(node.sysid))
                            node.bezeichnung = dict[node.sysid];
                    }
                    keyItemDict[node.syssortpos] = node;
                }
                foreach (SortTreeNode node in nodeList)
                {
                    if (!keyItemDict.ContainsKey(node.syssortposp))
                    {
                        _log.Warn("Invalid root-entry for SortPos " + node.syssortpos + ": " + node.syssortposp+" - skipping SortPos");
                        continue;
                    }
                    keyItemDict[node.syssortposp].addChild(node);
                    node.setParent(keyItemDict[node.syssortposp]);
                }


                return keyItemDict[-1];
            }
        }
    }
}
