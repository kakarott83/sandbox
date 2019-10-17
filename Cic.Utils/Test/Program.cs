using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.Utils.BO;
using Cic.OpenOne.Common.Util.Security;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Blowfish bf = new Blowfish("C.I.C.-S0ftwareGmbH1987Muenchen0");//"C.I.C.-S0ftwareGmbH1987B2B2011_0");
            String decoded = bf.Decode("AC3A0FFAA5C1E9DF61455628D40E0420");
            //"Êûà=Êõ©4rÀ\v("
            //"Êûà=Êõ©4rœÀ\v("
            String dec = "MçÙÝiXûN?ÂD?é\\\"&y";
            DateTime start = new DateTime(2014, 04,01);
            DateTime end = new DateTime(2014, 05,01);
           // DateTime start = new DateTime(2012, 10,01);
            //DateTime end = new DateTime(2012, 10,31);
            bool incStart = true;
            bool incEnd = false;
            int days = CalcUtil.calcZinsTage(start, end, incStart, incEnd, CalcMethod.M_30_360);
            Console.WriteLine(days);
            days = CalcUtil.calcZinsTage(start, end, incStart, incEnd, CalcMethod.M_ACT_360);
            Console.WriteLine(days);
            days = CalcUtil.calcZinsTage(start, end, incStart, incEnd, CalcMethod.M_ACT_365);
            Console.WriteLine(days);
            days = CalcUtil.calcZinsTage(start, end, incStart, incEnd, CalcMethod.M_ACT_ACT);
            Console.WriteLine(days);
            

            double faktor = CalcUtil.calcZinsTageFaktor(start, end, incStart, incEnd, CalcMethod.M_30_360);
            Console.WriteLine(faktor);
            faktor = CalcUtil.calcZinsTageFaktor(start, end, incStart, incEnd, CalcMethod.M_ACT_360);
            Console.WriteLine(faktor);
            faktor = CalcUtil.calcZinsTageFaktor(start, end, incStart, incEnd, CalcMethod.M_ACT_365);
            Console.WriteLine(faktor);
            faktor = CalcUtil.calcZinsTageFaktor(start, end, incStart, incEnd, CalcMethod.M_ACT_ACT);
            Console.WriteLine(faktor);


            Cic.One.Utils.BO.CalculatorFacade.main(args);

            CalculatorFacade cf = new CalculatorFacade();
            cf.beginn = new DateTime(2014,5,1);
            cf.valuta = new DateTime(2014,4,1);
            cf.zins = 2.82;
            cf.zahlweise = 1;
            cf.laufzeit = 58;
            cf.methode = CalcMethod.M_30_360;
            //cf.zinstage = 29;
            cf.letzteRate = 1450;
            cf.rate = 273.38;
            cf.barwert = 15834.782;
            
            cf.zahlmodus = true;
            cf.includeStart = false;
            cf.includeEnd = false;
            cf.checkBarwert();
            cf.calculate();
            StaffelKalkulator sk = new StaffelKalkulator(cf);
            double rw = sk.calcENDWERT(16000);

           // RangeSplitter.main(args);
            Console.ReadLine();
        }
    }
}
