using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public class PruefungDao : IPruefungDao
    {

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static string VALIDATION = " and (validuntil is null or validuntil>=:currentdate or " +
                                           "validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) and (validfrom is null or validfrom <= :currentdate or validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and activeflag = 1";
        private static string QUERYOFFENENPRJOKERANZ = "select count(*) from prjoker where sysprproduct = :psysprproduct and sysperole = :psysperole and " +
                                                    "(usedflag = 0 or (usedflag = 1  and sysprjoker = :psysprjoker)) " + VALIDATION;
        private static string QUERYPRJOKERANZ = "select count(*) from prjoker where sysprproduct = :psysprproduct and sysperole = :psysperole " + VALIDATION;
        private static string QUERYPRJOKERANTRAG = "select * from prjoker where sysperole = :psysperole and sysantrag = :psysantrag " + VALIDATION;
        private static string QUERYOFFENENPRJOKER = "select * from prjoker where sysprproduct = :psysprproduct and sysperole = :psysperole and " +
                                                    "(usedflag = 0 or (usedflag = 1  and sysprjoker = :psysprjoker)) " + VALIDATION;
        private static string QUERYUPDATEPRJOKER = "UPDATE PRJOKER set sysantrag = :psysantrag, usedflag = 1, useddate = :currentdate, usedtime= :currenttime where sysprjoker = :psysprjoker";
        private static string QUERYPRPRODUCTTYP = "select count(*) from prclprprodtype,prprodtype where prprodtype.sysprprodtype=prclprprodtype.sysprprodtype and prclprprodtype.activeflag=1 and sysprproduct=:psysprproduct and prprodtype.code='JOKER'";



        /// <summary>
        /// getOffenenJockerAnzahl
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="sysperole"></param>
        /// <param name="sysprjoker"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public int getOffeneJokerAnzahl(long sysprproduct, long sysperole, long sysprjoker, DateTime perDate)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "psysprproduct", Value = sysprproduct },
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "psysperole", Value = sysperole },
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "psysprjoker", Value = sysprjoker },
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "currentdate", Value = perDate}};

                return ctx.ExecuteStoreQuery<int>(QUERYOFFENENPRJOKERANZ, pars).FirstOrDefault();
                //return 3;
            }


        }

        /// <summary>
        /// Returns number of Jokers [for Händler] 
        /// </summary>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns></returns>
        public  int getJokerAnzahl(long sysprproduct, long sysperole)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "psysprproduct", Value = sysprproduct },
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "psysperole", Value = sysperole },
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "currentdate", Value = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(DateTime.Now)}};


                return ctx.ExecuteStoreQuery<int>(QUERYPRJOKERANZ, pars).FirstOrDefault();
                //return 4
            }


        }

        /// <summary>
        /// Returns true when product is a JokerProduct
        /// </summary>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <returns></returns>
        public bool isJokerProduct(long sysprproduct)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
             
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "psysprproduct", Value = sysprproduct } };
                long count = ctx.ExecuteStoreQuery<long>(QUERYPRPRODUCTTYP, pars).FirstOrDefault();
                return count > 0;

            }

        }



        /// <summary>
        /// Inserts Antrag in PRJOKER table and returns sysid of joker when  a available exists joker for Händler otherwise exception.
        /// </summary>
        /// <param name="sysantrag">sysantrag</param>
        /// <param name="sysperole">sysperole</param>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <param name="sysprjoker">sysprjoker</param>
        /// <returns></returns>
        public long updatePrjoker(long sysantrag, long sysperole, long sysprproduct, long sysprjoker)
        {


            DateTime? crtDate = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));

            using (DdOlExtended ctx = new DdOlExtended())
            {

                PRJOKER prjoker;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();

                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysprproduct", Value = sysprproduct });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysperole", Value = sysperole });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysprjoker", Value = sysprjoker });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "currentdate", Value = crtDate });

                prjoker = ctx.ExecuteStoreQuery<PRJOKER>(QUERYOFFENENPRJOKER, parameters.ToArray()).FirstOrDefault();


                if (prjoker != null)
                {
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysprjoker", Value = prjoker.SYSPRJOKER });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysantrag", Value = sysantrag });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "currentdate", Value = crtDate });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "currenttime", Value = (long?)Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)) });
                    ctx.ExecuteStoreCommand(QUERYUPDATEPRJOKER, parameters.ToArray());
                    _log.Debug("updatePrjoker : sysprjoker " + prjoker.SYSPRJOKER + " mit Antrag : " + sysantrag);

                    return prjoker.SYSPRJOKER;
                }
                else
                    throw new ApplicationException("Joker steht nicht mehr zur Verfügung. Bitte ein anderes Produkt auswählen");

            }

        }

        /// <summary>
        /// Delete Antrag reference in joker table (Makes joker available)
        /// </summary>
        /// <param name="sysantrag">sysantrag</param>
        public void setJokerFreiWithAntrag(long sysantrag)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {



                DateTime? crtDate = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));



                PRJOKER prjokerWithAntrag = (from pr in ctx.PRJOKER
                                             where pr.ANTRAG.SYSID == sysantrag
                                             select pr).FirstOrDefault();

                if (prjokerWithAntrag != null)
                {
                    prjokerWithAntrag.SYSANTRAG = null;
                    prjokerWithAntrag.USEDFLAG = 0;
                    prjokerWithAntrag.USEDTIME = 0;
                    prjokerWithAntrag.USEDDATE = null;
                    ctx.SaveChanges();
                }
            }
        }



        /// <summary>
        /// Returns true when Antrag is in PRJOKER table.
        /// </summary>
        /// <param name="sysprproduct">sysprproduct,</param>
        /// <param name="sysantrag">sysantrag</param>
        /// <returns></returns>
        public bool isJokerWithAntrag(long sysprproduct, long sysantrag)
        {

            using (DdOlExtended ctx = new DdOlExtended())
            {



                PRJOKER prjokerWithAntrag = (from pr in ctx.PRJOKER
                                             where pr.ANTRAG.SYSID == sysantrag && pr.SYSPRPRODUCT == sysprproduct
                                             select pr).FirstOrDefault();

                if (prjokerWithAntrag != null) return true;
                else return false;
            }
        }

        /// <summary>
        /// Returns (Vermittler) = Person von Händler (Parent vom Verkäufer)
        /// </summary>
        /// <param name="sysperole">sysperole</param>
        /// <returns></returns>
        public long  getSysVM(long sysperole)
        {

            using (DdOlExtended ctx = new DdOlExtended())
            {

                var sysvm = (from parentPeRole in ctx.PEROLE
                             join perole in ctx.PEROLE on parentPeRole.SYSPARENT equals perole.SYSPEROLE
                             where parentPeRole.SYSPEROLE == sysperole
                             select perole.SYSPEROLE).FirstOrDefault();

                return  (long)sysvm;

            }
        }
    }
}
