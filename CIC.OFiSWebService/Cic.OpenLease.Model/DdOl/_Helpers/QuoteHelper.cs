using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    
    #endregion

    /// <summary>
    /// @Deprecated
    /// </summary>
    [System.CLSCompliant(true)]
    public static class QuoteHelper
    {
/*
        public static decimal MyDeliverProzentFromQuote(Cic.OpenLease.Model.DdOl.OlExtendedEntities Context, string Bezeichnung)
        {
            double? Prozent;

            var Query = from quotedat in Context.QUOTEDAT
                        where quotedat.QUOTEAlias.BEZEICHNUNG == Bezeichnung
                        orderby quotedat.GUELTIGAB descending
                        select quotedat.PROZENT;

            Prozent = Query.FirstOrDefault();

            if(Prozent == null)
            {
                throw new System.Exception("Prozent from Quote is null exception");
            }

            
            return (decimal)Prozent.GetValueOrDefault();
        }*/
    }
}
