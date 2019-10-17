using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.One.DTO;
using Cic.One.Web.BO;
using Cic.One.Web.BO.Search;
using Cic.One.Web.DAO;
using System.Reflection;
using Cic.One.Web.Service.DAO;
using AutoMapper;
using Cic.One.Web.Contract;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.DTO;
using CIC.ASS.SearchService;
using System.ServiceModel.Web;
using CIC.ASS.SearchService.DTO;
using CIC.ASS.SearchService.BO;
using Cic.OpenOne.Common.Model.Prisma;


namespace Cic.One.Web.Service
{
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class luceneService : ILuceneSearch
    {
        

        
        /// <summary>
        /// Searches for the given value in all supported entities and uses the perole
        /// </summary>
        /// <param name="value"></param>
        /// <param name="perole"></param>
        /// <returns></returns>
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "search?value={value}&perole={perole}")]
        public SearchResult[] search(String value, String perole)
        {
            return new SearchBO().search(value, perole);

        }

        /// <summary>
        /// Search for the query-String
        /// </summary>
        /// <param name="query"></param>
        /// <param name="perole"></param>
        /// <param name="entities">comma sparated entities to search in</param>
        /// <param name="additionalQuery">query suffix</param>
        /// <returns></returns>
        [WebInvoke(Method = "GET",
                  ResponseFormat = WebMessageFormat.Json,
                  UriTemplate = "searchEntities?query={query}&perole={perole}&entities={entities}&additionalQuery={additionalQuery}")]
        public SearchEntityResult[] searchEntities(String query, String perole, String entities, String additionalQuery)
        {
            return LuceneBO.getInstance().search(query,perole,entities,additionalQuery,null,null);
            
        }

        /// <summary>
        /// Get all suggestions for the given search term
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "suggest?term={term}")]
        public String[] suggest(String term)
        {
            return  LuceneBO.getInstance().suggest(term);
        }
    

    }
}
