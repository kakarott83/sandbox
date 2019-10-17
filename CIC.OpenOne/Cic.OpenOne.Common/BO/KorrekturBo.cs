
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DTO;
using System.Reflection;
using System.Dynamic;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Clarion;
using System.Globalization;
using CIC.Database.OL.EF4.Model;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// BO for Korrektur
    /// </summary>
    public class KorrekturBo : AbstractKorrekturBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private DateTime nullDate = new DateTime(1800, 1, 1);
        private IEvaluator ev;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dao">DAO</param>
        public KorrekturBo(IKorrekturDao dao)
            : base(dao)
        {
            ev = EvaluatorFactory.createEvaluator();
        }


        /// <summary>
        /// corresponds to the clarion correct method, selecting the type - parameters automatically
        /// </summary>
        /// <param name="korrtypName"></param>
        /// <param name="value"></param>
        /// <param name="op"></param>
        /// <param name="perDate"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        override public double Correct(string korrtypName, decimal value, string op, DateTime perDate, String p1, String p2)
        {
            return Correct(korrtypName, value, op, perDate, p1, p2, KorrekturType.TYPE_AUTO, KorrekturType.TYPE_AUTO);
        }

        /// <summary>
        /// corresponds to the clarion correct method
        /// </summary>
        /// <param name="korrtypName"></param>
        /// <param name="value"></param>
        /// <param name="op"></param>
        /// <param name="perDate"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        override public double Correct(string korrtypName, decimal value, string op, DateTime perDate, String p1, String p2, KorrekturType type1, KorrekturType type2)
        {
            List<KORRTYP> ktypen = dao.getKorrekturTypen();
            ktypen = ktypen.Where(k => k.NAME.Equals(korrtypName)).ToList();

            

            if (type1 == KorrekturType.TYPE_DECIMAL || type1 == KorrekturType.TYPE_AUTO)
            {
                try
                {
                    ev.Bind("loc_p1", Decimal.Parse(p1));
                }
                catch (Exception)
                {
                    if (type1 != KorrekturType.TYPE_DECIMAL)
                        ev.Bind("loc_p1", p1);
                    //if no number, dont use it
                }
            }
            else if (type1 == KorrekturType.TYPE_STRING)
            {

                ev.Bind("loc_p1", p1);

            }

            if (type2 == KorrekturType.TYPE_DECIMAL || type2 == KorrekturType.TYPE_AUTO)
            {
                try
                {
                    ev.Bind("loc_p2", Decimal.Parse(p2));
                }
                catch (Exception)
                {
                    if (type2 != KorrekturType.TYPE_DECIMAL)
                        //if no number, dont use it
                        ev.Bind("loc_p2", p2);
                }
            }
            else if (type2 == KorrekturType.TYPE_STRING)
            {

                ev.Bind("loc_p2", p2);

            }
            String cVal = value.ToString("G",System.Globalization.CultureInfo.InvariantCulture);


            foreach (KORRTYP ktloop in ktypen)
            {


                var Querykorrektur = from korrektur in dao.getKorrekturen(ktloop.SYSKORRTYP)
                                     where (korrektur.DISABLEDFLAG == 0 || korrektur.DISABLEDFLAG == null)
                                     && (korrektur.VALIDFROM == null || korrektur.VALIDFROM.Value.Date <= perDate.Date || korrektur.VALIDFROM <= nullDate)
                                     && (korrektur.VALIDUNTIL == null || korrektur.VALIDUNTIL.Value.Date >= perDate.Date || korrektur.VALIDUNTIL <= nullDate)
                                     orderby korrektur.POSITION ascending
                                     select korrektur;

                List<KORREKTUR> korrekturList = Querykorrektur.ToList<KORREKTUR>();
                foreach (KORREKTUR kloop in korrekturList)
                {

                    string fac = (kloop.FACTORVALUENET);
                    string expr = (kloop.EXPRNET);

                    if (ev.validate(expr))
                    {
                        if (kloop.ART == 0)
                        {
                            cVal = ev.evaluate(cVal + " " + op + " (" + fac + ")");
                        }
                        else
                            cVal = ev.evaluate(fac);
                        if (kloop.BREAKCONDITION == 1) break;
                    }
                    else
                    {
                        if (kloop.BREAKCONDITION == 2) break;
                        else continue;
                    }
                }
            }
            return double.Parse(cVal, System.Globalization.CultureInfo.InvariantCulture);
        }

    }

}
