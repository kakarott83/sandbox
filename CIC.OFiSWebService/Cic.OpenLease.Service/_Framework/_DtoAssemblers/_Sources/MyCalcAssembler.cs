// OWNER MK, 23-11-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using System;
    using Cic.OpenLease.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    #endregion

    [System.CLSCompliant(true)]
    public class MyCalcAssembler : IDtoAssembler<Cic.OpenLease.ServiceAccess.MyCalcDto, MYCALC>
    {
        #region IDtoAssembler<ITDto,IT> Members (Methods)
        public bool IsValid(Cic.OpenLease.ServiceAccess.MyCalcDto myCalcDto)
        {
            if (myCalcDto == null)
            {
                throw new ArgumentException("myCalcDto");
            }

            // Otymistic
            bool IsValid = true;

            // TODO JJ 10 JJ, Implement
            // Validate

            return IsValid;
        }

        public Cic.OpenLease.Model.DdOl.MYCALC Create(Cic.OpenLease.ServiceAccess.MyCalcDto myCalcDto)
        {
            if (myCalcDto == null)
            {
                throw new ArgumentException("myCalcDto");
            }

            Cic.OpenLease.Model.DdOl.MYCALC MyCalc = new Cic.OpenLease.Model.DdOl.MYCALC();

            // Map
            MyMap(myCalcDto, MyCalc);

            // TODO JJ 10 JJ, Implement
            return null;
        }

        public Cic.OpenLease.Model.DdOl.MYCALC Update(Cic.OpenLease.ServiceAccess.MyCalcDto myCalcDto)
        {
            Cic.OpenLease.Model.DdOl.MYCALC MyCalc = null;

            if (myCalcDto == null)
            {
                throw new ArgumentException("myCalcDto");
            }

            if (myCalcDto.SYSMYCALC <= 0)
            {
                throw new Exception("myCaclDto.SYSMYCALC");
            }

            using (Cic.OpenLease.Model.DdOl.OlExtendedEntities context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {
                MyCalc = Cic.OpenLease.Model.DdOl.MYCALCHelper.DeliverMyCalc(context, myCalcDto.SYSMYCALC);

                if (MyCalc != null)
                {
                    MyMap(myCalcDto, MyCalc);
                }
                else
                {
                    throw new ArgumentException("MYCALC");
                }
            }

            // TODO JJ 10 JJ, Implement
            return null;
        }

        public Cic.OpenLease.ServiceAccess.MyCalcDto ConvertToDto(MYCALC myCalc)
        {
            if (myCalc == null)
            {
                throw new ArgumentException("myCalc");
            }

            Cic.OpenLease.ServiceAccess.MyCalcDto MyCalcDto = new Cic.OpenLease.ServiceAccess.MyCalcDto();
            MyCalcDto.ANREDE = myCalc.ANREDE;
            MyCalcDto.BEMERKUNG1 = myCalc.BEMERKUNG1;
            MyCalcDto.BEMERKUNG2 = myCalc.BEMERKUNG2;
            MyCalcDto.BGEXTERN = (decimal)myCalc.BGEXTERN;
            MyCalcDto.EMAIL = myCalc.EMAIL;
            MyCalcDto.ERSTELLTAM = myCalc.ERSTELLTAM.GetValueOrDefault();
            MyCalcDto.HANDY = myCalc.HANDY;
            MyCalcDto.JAHRESKM = myCalc.JAHRESKM.GetValueOrDefault();
            MyCalcDto.LZ = myCalc.LZ.GetValueOrDefault();
            MyCalcDto.MATCHCODE = myCalc.MATCHCODE;
            MyCalcDto.NAME = myCalc.NAME;
            MyCalcDto.NOTIZEN = myCalc.NOTIZEN;
            MyCalcDto.OBJEKT = myCalc.OBJEKT;
            MyCalcDto.PPY = myCalc.PPY.GetValueOrDefault();
            MyCalcDto.RATE = (decimal)myCalc.RATE;
            MyCalcDto.RW = (decimal)myCalc.RW;
            MyCalcDto.RWP = (decimal)myCalc.RWP;
            MyCalcDto.SZ = (decimal)myCalc.SZ;
            MyCalcDto.SZP = (decimal)myCalc.SZP;
            MyCalcDto.TELEFON = myCalc.TELEFON;
            MyCalcDto.VORNAME = myCalc.VORNAME;
            MyCalcDto.ZINS = (decimal)myCalc.ZINS;
            MyCalcDto.ZINSEFF = (decimal)myCalc.ZINSEFF;

            return MyCalcDto;
        }

        public MYCALC ConvertToDomain(Cic.OpenLease.ServiceAccess.MyCalcDto myCalcDto)
        {
            // TODO JJ 10 JJ, Implement
            throw new System.NotImplementedException();
        }
        #endregion

        #region IDtoAssembler<ITDto,IT> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get
            {
                // TODO JJ 10 JJ, Implement
                throw new System.NotImplementedException();
            }
        }
        #endregion

        #region My methods
        private static void MyMap(Cic.OpenLease.ServiceAccess.MyCalcDto fromMyCalcDto, Cic.OpenLease.Model.DdOl.MYCALC toMyCalc)
        {
            toMyCalc.ANREDE = fromMyCalcDto.ANREDE;
            toMyCalc.BEMERKUNG1 = fromMyCalcDto.BEMERKUNG1;
            toMyCalc.BEMERKUNG2 = fromMyCalcDto.BEMERKUNG2;
            toMyCalc.BGEXTERN = fromMyCalcDto.BGEXTERN;
            toMyCalc.EMAIL = fromMyCalcDto.EMAIL;
            toMyCalc.ERSTELLTAM = fromMyCalcDto.ERSTELLTAM;
            toMyCalc.HANDY = fromMyCalcDto.HANDY;
            toMyCalc.JAHRESKM = fromMyCalcDto.JAHRESKM;
            toMyCalc.LZ = fromMyCalcDto.LZ;
            toMyCalc.MATCHCODE = fromMyCalcDto.MATCHCODE;
            toMyCalc.NAME = fromMyCalcDto.NAME;
            toMyCalc.NOTIZEN = fromMyCalcDto.NOTIZEN;
            toMyCalc.OBJEKT = fromMyCalcDto.OBJEKT;
            toMyCalc.PPY = fromMyCalcDto.PPY;
            toMyCalc.RATE = (double)fromMyCalcDto.RATE;
            toMyCalc.RW = (double)fromMyCalcDto.RW;
            toMyCalc.RWP = (double)fromMyCalcDto.RWP;
            toMyCalc.SZ = fromMyCalcDto.SZ;
            toMyCalc.SZP = fromMyCalcDto.SZP;
            toMyCalc.TELEFON = fromMyCalcDto.TELEFON;
            toMyCalc.VORNAME = fromMyCalcDto.VORNAME;
            toMyCalc.ZINS = (double)fromMyCalcDto.ZINS;
            toMyCalc.ZINSEFF = (double)fromMyCalcDto.ZINSEFF;
        }
        #endregion
    }
}