using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Defines the Query Pattern for the three Search-Modes
    /// </summary>
    public class QueryStructure
    {
         public String countQuery {get;set;}
        public String partialQuery {get;set;}
        public String completeQuery {get;set;}
    }
}