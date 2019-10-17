using System;
using System.Data.Common;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Clarion;
using Cic.OpenOne.Common.Util.Extension;
using Cic.OpenOne.Common.Util.Logging;
using System.Collections.Generic;
using CIC.Database.OL.EF4.Model;

namespace Cic.OpenOne.Common.BO
{
    class BindInfo
    {
        public String name {get;set;}
        public object value { get; set; }
        public BindInfo(String name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }
    /// <summary>
    /// Nummernkreis Builder-Klasse
    /// </summary>
    public class NkBuilder : AbstractNkBuilder
    {
        private String type;
        private String area;
        private static object synchronize = new object();
        private List<BindInfo> binds = new List<BindInfo>();

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// creates a new number builder for the given type and area
        /// </summary>
        /// <param name="typ">eg ANGEBOT</param>
        /// <param name="area">eg B2B</param>
        public NkBuilder(String typ, String area)
        {
            this.type = typ;
            this.area = area;
        }

        public void addBind(String name, object value)
        {
            binds.Add(new BindInfo(name, value));
        }

        /// <summary>
        /// creates the next unique id 
        /// </summary>       
        /// <returns></returns>
        override public String getNextNumber()
        {
            lock (synchronize)
            {
                string retValue = "";

                using (DdOlExtended ctx = new DdOlExtended())
                {

                    var nks = from c in ctx.NK select c;

                    if (type != null)
                    {

                        if (area == null)
                        {
                            nks = nks.Where(c => c.TYP == type && c.DOTNETENABLED == 1 && c.BEREICH == null);
                        }
                        else
                        {
                            nks = nks.Where(c => c.TYP == type && c.DOTNETENABLED == 1 && c.BEREICH == area);
                        }

                        retValue = MyEval(ctx, nks.FirstOrDefault());
                    }
                }



                return retValue;
            }
        }

        private string MyEval(DdOlExtended context,NK nk)
        {
            string retValue = "";
            long lfdNr = 0;


            if (nk != null)
            {
                nk.NKNUMReference.Load();
                if (nk.NKNUM != null)
                {
                    lfdNr = getLfdnr(context, nk.NKNUM);
                    // HC: Load again
                    // nk.NKNUMReference.Load(System.Data.Objects.MergeOption.OverwriteChanges);
                    // Because of performance I choose the "light" way so the line above was commented out.
                    nk.NKNUM.LFDNR = lfdNr;
                    Evaluator eval = new Evaluator();
                    eval.Bind("nk", nk);
                    eval.Bind("nknum", nk.NKNUM);
                    foreach (BindInfo bi in binds)
                    {
                        eval.Bind(bi.name,bi.value);
                    }
                    retValue = eval.evaluate(nk.AUSDRUCK);
                }
                else
                {
                    // HC: tbd. Throw exception ?
                }
            }
            return retValue;
        }

        /// <summary>
        /// FUNCTION nextlfdnr(pSysNKNUM NUMBER) RETURN NUMBER;
        ///
        /// FUNCTION nextlfdnr(pSysNKNUM NUMBER) RETURN NUMBER IS
        /// rval                 NUMBER(12,0);
        /// BEGIN
        /// SELECT lfdnr INTO rval FROM nknum WHERE sysnknum = pSysNKNUM FOR UPDATE OF lfdnr;
        /// rval := rval + 1;
        /// UPDATE nknum SET lfdnr = rval WHERE sysnknum = pSysNKNUM;
        /// COMMIT;
        /// RETURN rval;
        /// END nextlfdnr;
        ///        
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nknum"></param>
        /// <returns></returns>
        private long getLfdnr(DdOlExtended context, NKNUM nknum)
        {
           
            try
            {
                
                DbParameter[] pars = 
                        { 
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysNKNUM", Value =  nknum.SYSNKNUM}
                                ,new Devart.Data.Oracle.OracleParameter{ OracleDbType = Devart.Data.Oracle.OracleDbType.Number, Direction = System.Data.ParameterDirection.ReturnValue} 
                        };


                object v = ((ObjectContext)context).ExecuteFunction("CIC.CIC_SYS.NEXT_LFDNR", pars);
                
                return Convert.ToInt64(v);
            }
            catch (Exception)
            {
                _log.Warn("CIC.CIC_SYS.NEXTLFDNR not found, using old NK-Method");
                long lfdnr = 0;
                context.Connection.Open();
                context.ExecuteStoreCommand("CALL CIC.CIC_SYS.SET_LFDNR(" + nknum.SYSNKNUM + ")");
                lfdnr = context.ExecuteStoreQuery<long>("SELECT CIC.CIC_SYS.GET_LFDNR() FROM DUAL").FirstOrDefault();
                context.Connection.Close();

                return lfdnr;
            }

        }
    }
}
